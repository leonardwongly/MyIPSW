using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace ipsw
{
    public enum UiTelemetryEventName
    {
        PageViewed,
        SourceSelected,
        StepCompleted,
        RetrieveClicked,
        ResultsRendered,
        ErrorShown,
        DownloadAllClicked,
        DownloadAllConfirmed,
        DownloadAllBlocked
    }

    public sealed class UiTelemetryEvent
    {
        private static readonly HashSet<string> AllowedFields = new HashSet<string>(StringComparer.Ordinal)
        {
            "eventName",
            "sessionId",
            "journeyId",
            "source",
            "resultMode",
            "step",
            "status",
            "durationMs",
            "errorCode"
        };

        private static readonly Regex OpaqueIdRegex = new Regex("^[A-Za-z0-9-_.]{6,64}$", RegexOptions.Compiled);
        private static readonly Regex SafeFieldRegex = new Regex("^[A-Za-z0-9_.:-]{1,64}$", RegexOptions.Compiled);

        public UiTelemetryEventName EventName { get; set; }

        public string SessionId { get; set; }

        public string JourneyId { get; set; }

        public string Source { get; set; }

        public string ResultMode { get; set; }

        public string Step { get; set; }

        public string Status { get; set; }

        public int? DurationMs { get; set; }

        public string ErrorCode { get; set; }

        public UiTelemetryEvent()
        {
            SessionId = string.Empty;
            JourneyId = string.Empty;
            Source = string.Empty;
            ResultMode = string.Empty;
            Step = string.Empty;
            Status = string.Empty;
            ErrorCode = string.Empty;
        }

        public static bool TryParse(JObject payload, out UiTelemetryEvent telemetryEvent, out string error)
        {
            telemetryEvent = null;
            error = string.Empty;

            if (payload == null)
            {
                error = "Invalid telemetry payload.";
                return false;
            }

            if (payload.Properties().Any(p => !AllowedFields.Contains(p.Name)))
            {
                error = "Unknown field detected.";
                return false;
            }

            var eventNameRaw = ReadTrimmed(payload, "eventName");
            UiTelemetryEventName parsedEventName;
            if (!TryParseEventName(eventNameRaw, out parsedEventName))
            {
                error = "Unsupported eventName.";
                return false;
            }

            var sessionId = ReadTrimmed(payload, "sessionId");
            if (!OpaqueIdRegex.IsMatch(sessionId))
            {
                error = "Invalid sessionId.";
                return false;
            }

            var journeyId = ReadTrimmed(payload, "journeyId");
            if (!OpaqueIdRegex.IsMatch(journeyId))
            {
                error = "Invalid journeyId.";
                return false;
            }

            var source = ReadTrimmed(payload, "source");
            FirmwareSource ignoredSource;
            if (!string.IsNullOrEmpty(source) && !SelectionStateValidator.TryParseSource(source, out ignoredSource))
            {
                error = "Invalid source.";
                return false;
            }

            var resultMode = ReadTrimmed(payload, "resultMode");
            ResultMode ignoredResultMode;
            if (!string.IsNullOrEmpty(resultMode) && !SelectionStateValidator.TryParseResultMode(resultMode, out ignoredResultMode))
            {
                error = "Invalid resultMode.";
                return false;
            }

            var step = ReadBounded(payload, "step", 64, out error);
            if (!string.IsNullOrEmpty(error))
            {
                return false;
            }

            var status = ReadBounded(payload, "status", 64, out error);
            if (!string.IsNullOrEmpty(error))
            {
                return false;
            }

            var errorCode = ReadBounded(payload, "errorCode", 64, out error);
            if (!string.IsNullOrEmpty(error))
            {
                return false;
            }

            int? durationMs = null;
            var durationToken = payload["durationMs"];
            if (durationToken != null && durationToken.Type != JTokenType.Null)
            {
                int parsedDuration;
                if (!int.TryParse(durationToken.ToString(), NumberStyles.Integer, CultureInfo.InvariantCulture, out parsedDuration)
                    || parsedDuration < 0
                    || parsedDuration > 600000)
                {
                    error = "Invalid durationMs.";
                    return false;
                }

                durationMs = parsedDuration;
            }

            telemetryEvent = new UiTelemetryEvent
            {
                EventName = parsedEventName,
                SessionId = sessionId,
                JourneyId = journeyId,
                Source = source,
                ResultMode = resultMode,
                Step = step,
                Status = status,
                DurationMs = durationMs,
                ErrorCode = errorCode
            };

            return true;
        }

        private static bool TryParseEventName(string raw, out UiTelemetryEventName eventName)
        {
            eventName = UiTelemetryEventName.PageViewed;
            if (string.IsNullOrWhiteSpace(raw))
            {
                return false;
            }

            UiTelemetryEventName parsed;
            if (!Enum.TryParse(raw, true, out parsed))
            {
                return false;
            }

            if (!Enum.IsDefined(typeof(UiTelemetryEventName), parsed))
            {
                return false;
            }

            eventName = parsed;
            return true;
        }

        private static string ReadTrimmed(JObject payload, string field)
        {
            var token = payload[field];
            if (token == null || token.Type == JTokenType.Null)
            {
                return string.Empty;
            }

            return token.ToString().Trim();
        }

        private static string ReadBounded(JObject payload, string field, int maxLength, out string error)
        {
            error = string.Empty;
            var value = ReadTrimmed(payload, field);
            if (string.IsNullOrEmpty(value))
            {
                return string.Empty;
            }

            if (value.Length > maxLength)
            {
                error = string.Format(CultureInfo.InvariantCulture, "{0} exceeds max length.", field);
                return string.Empty;
            }

            if (!SafeFieldRegex.IsMatch(value))
            {
                error = string.Format(CultureInfo.InvariantCulture, "{0} has invalid characters.", field);
                return string.Empty;
            }

            return value;
        }
    }
}
