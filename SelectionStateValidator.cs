using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text.RegularExpressions;

namespace ipsw
{
    public static class SelectionStateValidator
    {
        private static readonly Regex SafeTokenRegex = new Regex("^[A-Za-z0-9._,-]+$", RegexOptions.Compiled);

        public static SelectionValidationResult ParseFromQuery(NameValueCollection query, ISet<string> knownDeviceIdentifiers)
        {
            var state = new SelectionState();
            var result = new SelectionValidationResult(state);

            if (query == null)
            {
                return result;
            }

            var sourceRaw = query["source"];
            if (!string.IsNullOrWhiteSpace(sourceRaw))
            {
                FirmwareSource parsedSource;
                if (TryParseSource(sourceRaw, out parsedSource))
                {
                    state.Source = parsedSource;
                }
                else
                {
                    result.Warnings.Add("Ignored an unknown source from the URL.");
                }
            }

            var resultRaw = query["result"];
            if (!string.IsNullOrWhiteSpace(resultRaw))
            {
                ResultMode parsedResultMode;
                if (TryParseResultMode(resultRaw, out parsedResultMode))
                {
                    state.ResultMode = parsedResultMode;
                }
                else
                {
                    result.Warnings.Add("Ignored an unknown result view from the URL.");
                }
            }

            var deviceRaw = NormalizeToken(query["device"], 64);
            if (!string.IsNullOrWhiteSpace(deviceRaw))
            {
                if (knownDeviceIdentifiers != null && knownDeviceIdentifiers.Contains(deviceRaw))
                {
                    state.DeviceIdentifier = deviceRaw;
                }
                else
                {
                    result.Warnings.Add("Ignored an unknown device identifier from the URL.");
                }
            }

            var versionRaw = NormalizeToken(query["version"], 32);
            if (!string.IsNullOrWhiteSpace(versionRaw))
            {
                state.Version = versionRaw;
            }

            var uiRaw = query["ui"];
            if (!string.IsNullOrWhiteSpace(uiRaw))
            {
                state.IsUiNext = string.Equals(uiRaw.Trim(), "next", StringComparison.OrdinalIgnoreCase);
            }

            var hasVersionMismatch = state.Source.HasValue
                && (state.Source.Value == FirmwareSource.Version || state.Source.Value == FirmwareSource.VersionOta)
                && !string.IsNullOrWhiteSpace(state.Version)
                && !IsKnownVersion(state.Version, state.Source.Value);
            if (hasVersionMismatch)
            {
                result.Warnings.Add("Ignored an unknown version from the URL.");
                state.Version = string.Empty;
            }

            if (state.Source.HasValue)
            {
                var sourceValue = state.Source.Value;
                if (sourceValue == FirmwareSource.Version || sourceValue == FirmwareSource.VersionOta)
                {
                    state.DeviceIdentifier = string.Empty;
                }
                else
                {
                    state.Version = string.Empty;
                }
            }

            return result;
        }

        public static bool ValidateSelection(SelectionState state, ISet<string> knownDeviceIdentifiers, out string message)
        {
            if (state == null)
            {
                message = "Selection state is required.";
                return false;
            }

            if (!state.Source.HasValue)
            {
                message = "Choose a firmware source to continue.";
                return false;
            }

            if (state.Source.Value == FirmwareSource.Version || state.Source.Value == FirmwareSource.VersionOta)
            {
                if (string.IsNullOrWhiteSpace(state.Version))
                {
                    message = "Select a version before retrieving firmware results.";
                    return false;
                }

                if (!IsKnownVersion(state.Version, state.Source.Value))
                {
                    message = "The selected version is not supported.";
                    return false;
                }
            }
            else
            {
                if (string.IsNullOrWhiteSpace(state.DeviceIdentifier))
                {
                    message = "Select a device before retrieving firmware results.";
                    return false;
                }

                if (knownDeviceIdentifiers != null && !knownDeviceIdentifiers.Contains(state.DeviceIdentifier))
                {
                    message = "The selected device is not recognized.";
                    return false;
                }
            }

            message = string.Empty;
            return true;
        }

        public static bool TryParseSource(string raw, out FirmwareSource source)
        {
            source = FirmwareSource.Official;
            if (string.IsNullOrWhiteSpace(raw))
            {
                return false;
            }

            var normalized = raw.Trim().ToLowerInvariant();
            if (normalized == "official")
            {
                source = FirmwareSource.Official;
                return true;
            }

            if (normalized == "ota")
            {
                source = FirmwareSource.Ota;
                return true;
            }

            if (normalized == "version")
            {
                source = FirmwareSource.Version;
                return true;
            }

            if (normalized == "version_ota" || normalized == "version(ota)" || normalized == "version-ota")
            {
                source = FirmwareSource.VersionOta;
                return true;
            }

            return false;
        }

        public static bool TryParseResultMode(string raw, out ResultMode mode)
        {
            mode = ResultMode.Table;
            if (string.IsNullOrWhiteSpace(raw))
            {
                return false;
            }

            var normalized = raw.Trim().ToLowerInvariant();
            if (normalized == "table")
            {
                mode = ResultMode.Table;
                return true;
            }

            if (normalized == "links")
            {
                mode = ResultMode.Links;
                return true;
            }

            return false;
        }

        public static bool IsSafeToken(string value, int maxLength)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return false;
            }

            var trimmed = value.Trim();
            if (trimmed.Length > maxLength)
            {
                return false;
            }

            return SafeTokenRegex.IsMatch(trimmed);
        }

        public static string NormalizeToken(string value, int maxLength)
        {
            if (!IsSafeToken(value, maxLength))
            {
                return string.Empty;
            }

            return value.Trim();
        }

        public static bool IsKnownVersion(string version, FirmwareSource source)
        {
            if (string.IsNullOrWhiteSpace(version))
            {
                return false;
            }

            var list = source == FirmwareSource.VersionOta ? VersionData.OTAVersions : VersionData.iOSVersions;
            return list.Any(v => string.Equals(v, version, StringComparison.OrdinalIgnoreCase));
        }
    }
}
