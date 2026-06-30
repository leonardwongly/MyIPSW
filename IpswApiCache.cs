using System;
using System.Collections.Concurrent;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.Caching;
using System.Threading;
using System.Threading.Tasks;

namespace ipsw
{
    internal static class IpswApiCache
    {
        private static readonly TimeSpan DevicesTtl = TimeSpan.FromHours(6);
        private static readonly TimeSpan FirmwaresTtl = TimeSpan.FromMinutes(10);
        private static readonly MemoryCache Cache = MemoryCache.Default;
        private static readonly ConcurrentDictionary<string, Lazy<Task<string>>> Inflight =
            new ConcurrentDictionary<string, Lazy<Task<string>>>();
        private static readonly HttpClient Client = CreateClient();

        internal static Task<string> GetDevicesJsonAsync()
        {
            return GetJsonAsync("devices", DevicesTtl);
        }

        internal static Task<string> GetIpswByVersionJsonAsync(string version)
        {
            return GetJsonAsync($"ipsw/{EscapePathSegment(version)}", FirmwaresTtl);
        }

        internal static Task<string> GetOtaByVersionJsonAsync(string version)
        {
            return GetJsonAsync($"ota/{EscapePathSegment(version)}", FirmwaresTtl);
        }

        internal static Task<string> GetDeviceFirmwareJsonAsync(string identifier, string type)
        {
            var safeType = Uri.EscapeDataString(type ?? string.Empty);
            return GetJsonAsync($"device/{EscapePathSegment(identifier)}?type={safeType}", FirmwaresTtl);
        }

        private static async Task<string> GetJsonAsync(string relativePath, TimeSpan ttl)
        {
            if (string.IsNullOrWhiteSpace(relativePath))
            {
                throw new ArgumentException("Path must be provided.", nameof(relativePath));
            }

            var url = $"https://api.ipsw.me/v4/{relativePath}";
            var cacheKey = "ipsw:json:" + url;
            var cached = Cache.Get(cacheKey) as string;
            if (cached != null)
            {
                return cached;
            }

            var lazy = Inflight.GetOrAdd(
                cacheKey,
                _ => new Lazy<Task<string>>(() => Client.GetStringAsync(url), LazyThreadSafetyMode.ExecutionAndPublication));

            try
            {
                var json = await lazy.Value;
                Cache.Set(cacheKey, json, new CacheItemPolicy
                {
                    AbsoluteExpiration = DateTimeOffset.UtcNow.Add(ttl)
                });
                return json;
            }
            finally
            {
                Inflight.TryRemove(cacheKey, out _);
            }
        }

        private static string EscapePathSegment(string segment)
        {
            return Uri.EscapeDataString(segment ?? string.Empty);
        }

        private static HttpClient CreateClient()
        {
            var handler = new HttpClientHandler
            {
                AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
            };

            var client = new HttpClient(handler)
            {
                Timeout = TimeSpan.FromSeconds(10)
            };
            client.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("gzip"));
            client.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("deflate"));
            return client;
        }
    }
}
