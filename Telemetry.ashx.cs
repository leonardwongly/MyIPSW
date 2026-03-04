using Newtonsoft.Json.Linq;
using System;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Runtime.Caching;
using System.Text;
using System.Web;

namespace ipsw
{
    public class Telemetry : IHttpHandler
    {
        private static readonly object RateLock = new object();
        private static readonly MemoryCache Cache = MemoryCache.Default;

        public bool IsReusable
        {
            get { return false; }
        }

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            context.Response.Cache.SetCacheability(HttpCacheability.NoCache);

            if (!string.Equals(context.Request.HttpMethod, "POST", StringComparison.OrdinalIgnoreCase))
            {
                context.Response.StatusCode = 405;
                context.Response.Write("Method not allowed.");
                return;
            }

            if (!IsTelemetryEnabled())
            {
                context.Response.StatusCode = 204;
                return;
            }

            var maxPayloadBytes = ReadAppSettingInt("TelemetryMaxPayloadBytes", 2048, 512, 16384);
            if (context.Request.TotalBytes > maxPayloadBytes)
            {
                context.Response.StatusCode = 400;
                context.Response.Write("Payload too large.");
                return;
            }

            string requestBody;
            if (!TryReadRequestBodyWithinLimit(context.Request.InputStream, maxPayloadBytes, out requestBody))
            {
                context.Response.StatusCode = 400;
                context.Response.Write("Payload too large.");
                return;
            }

            JObject payload;
            try
            {
                payload = JObject.Parse(requestBody);
            }
            catch
            {
                context.Response.StatusCode = 400;
                context.Response.Write("Invalid JSON.");
                return;
            }

            UiTelemetryEvent telemetryEvent;
            string validationError;
            if (!UiTelemetryEvent.TryParse(payload, out telemetryEvent, out validationError))
            {
                context.Response.StatusCode = 400;
                context.Response.Write(validationError);
                return;
            }

            var maxEvents = ReadAppSettingInt("TelemetrySessionMaxEvents", 120, 20, 1000);
            var windowSeconds = ReadAppSettingInt("TelemetrySessionWindowSeconds", 300, 10, 3600);
            if (!TryRegisterEvent(telemetryEvent.SessionId, maxEvents, TimeSpan.FromSeconds(windowSeconds)))
            {
                context.Response.StatusCode = 429;
                context.Response.Write("Rate limit exceeded.");
                return;
            }

            System.Diagnostics.Trace.TraceInformation(
                "ui_telemetry event={0} source={1} resultMode={2} step={3} status={4} durationMs={5} errorCode={6}",
                telemetryEvent.EventName,
                telemetryEvent.Source,
                telemetryEvent.ResultMode,
                telemetryEvent.Step,
                telemetryEvent.Status,
                telemetryEvent.DurationMs.HasValue ? telemetryEvent.DurationMs.Value.ToString(CultureInfo.InvariantCulture) : string.Empty,
                telemetryEvent.ErrorCode);

            context.Response.StatusCode = 204;
        }

        private static bool IsTelemetryEnabled()
        {
            var setting = ConfigurationManager.AppSettings["TelemetryEnabled"];
            if (string.IsNullOrWhiteSpace(setting))
            {
                return true;
            }

            bool enabled;
            return !bool.TryParse(setting, out enabled) || enabled;
        }

        private static int ReadAppSettingInt(string key, int defaultValue, int min, int max)
        {
            var setting = ConfigurationManager.AppSettings[key];
            if (string.IsNullOrWhiteSpace(setting))
            {
                return defaultValue;
            }

            int parsed;
            if (!int.TryParse(setting, NumberStyles.Integer, CultureInfo.InvariantCulture, out parsed))
            {
                return defaultValue;
            }

            if (parsed < min)
            {
                return min;
            }

            if (parsed > max)
            {
                return max;
            }

            return parsed;
        }

        internal static bool TryReadRequestBodyWithinLimit(Stream inputStream, int maxPayloadBytes, out string requestBody)
        {
            requestBody = string.Empty;
            if (inputStream == null)
            {
                return true;
            }

            if (maxPayloadBytes <= 0)
            {
                return false;
            }

            var totalBytesRead = 0;
            var buffer = new byte[1024];

            using (var bufferStream = new MemoryStream())
            {
                int read;
                while ((read = inputStream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    totalBytesRead += read;
                    if (totalBytesRead > maxPayloadBytes)
                    {
                        return false;
                    }

                    bufferStream.Write(buffer, 0, read);
                }

                requestBody = Encoding.UTF8.GetString(bufferStream.ToArray());
            }

            return true;
        }

        private static bool TryRegisterEvent(string sessionId, int maxEvents, TimeSpan window)
        {
            var safeSessionId = string.IsNullOrWhiteSpace(sessionId) ? "anonymous" : sessionId;
            var cacheKey = "telemetry:rate:" + safeSessionId;

            lock (RateLock)
            {
                var existing = Cache.Get(cacheKey) as TelemetryRateWindow;
                if (existing == null)
                {
                    Cache.Set(cacheKey, new TelemetryRateWindow { Count = 1 }, new CacheItemPolicy
                    {
                        AbsoluteExpiration = DateTimeOffset.UtcNow.Add(window)
                    });
                    return true;
                }

                if (existing.Count >= maxEvents)
                {
                    return false;
                }

                existing.Count += 1;
                return true;
            }
        }

        private sealed class TelemetryRateWindow
        {
            public int Count { get; set; }
        }
    }
}
