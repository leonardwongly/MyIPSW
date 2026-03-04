using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace ipsw
{
    public interface IFirmwareQueryService
    {
        Task<IReadOnlyList<FirmwareRecord>> GetFirmwareRecordsAsync(SelectionState state);
    }

    public sealed class FirmwareQueryService : IFirmwareQueryService
    {
        public async Task<IReadOnlyList<FirmwareRecord>> GetFirmwareRecordsAsync(SelectionState state)
        {
            if (state == null)
            {
                throw new ArgumentNullException(nameof(state));
            }

            if (!state.Source.HasValue)
            {
                return Array.Empty<FirmwareRecord>();
            }

            switch (state.Source.Value)
            {
                case FirmwareSource.Version:
                {
                    var json = await IpswApiCache.GetIpswByVersionJsonAsync(state.Version).ConfigureAwait(false);
                    return ParseVersionRecords(json);
                }
                case FirmwareSource.VersionOta:
                {
                    var json = await IpswApiCache.GetOtaByVersionJsonAsync(state.Version).ConfigureAwait(false);
                    return ParseVersionRecords(json);
                }
                case FirmwareSource.Official:
                {
                    var json = await IpswApiCache.GetDeviceFirmwareJsonAsync(state.DeviceIdentifier, "ipsw").ConfigureAwait(false);
                    return ParseDeviceRecords(json, state.DeviceIdentifier);
                }
                case FirmwareSource.Ota:
                {
                    var json = await IpswApiCache.GetDeviceFirmwareJsonAsync(state.DeviceIdentifier, "ota").ConfigureAwait(false);
                    return ParseDeviceRecords(json, state.DeviceIdentifier);
                }
                default:
                    return Array.Empty<FirmwareRecord>();
            }
        }

        internal static IReadOnlyList<FirmwareRecord> ParseVersionRecords(string json)
        {
            if (string.IsNullOrWhiteSpace(json))
            {
                return Array.Empty<FirmwareRecord>();
            }

            JArray rows;
            try
            {
                rows = JArray.Parse(json);
            }
            catch
            {
                return Array.Empty<FirmwareRecord>();
            }

            var records = new List<FirmwareRecord>();
            var urls = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            foreach (var row in rows)
            {
                var url = ReadString(row, "url");
                if (string.IsNullOrWhiteSpace(url) || !urls.Add(url))
                {
                    continue;
                }

                records.Add(CreateRecord(row, ReadString(row, "identifier"), url));
            }

            return SortRecords(records);
        }

        internal static IReadOnlyList<FirmwareRecord> ParseDeviceRecords(string json, string fallbackIdentifier)
        {
            if (string.IsNullOrWhiteSpace(json))
            {
                return Array.Empty<FirmwareRecord>();
            }

            JObject root;
            try
            {
                root = JObject.Parse(json);
            }
            catch
            {
                return Array.Empty<FirmwareRecord>();
            }

            var firmwareRows = root["firmwares"] as JArray;
            if (firmwareRows == null)
            {
                return Array.Empty<FirmwareRecord>();
            }

            var records = new List<FirmwareRecord>();
            var urls = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            foreach (var row in firmwareRows)
            {
                var url = ReadString(row, "url");
                if (string.IsNullOrWhiteSpace(url) || !urls.Add(url))
                {
                    continue;
                }

                records.Add(CreateRecord(row, fallbackIdentifier, url));
            }

            return SortRecords(records);
        }

        private static FirmwareRecord CreateRecord(JToken row, string fallbackIdentifier, string url)
        {
            var record = new FirmwareRecord();
            record.Identifier = string.IsNullOrWhiteSpace(ReadString(row, "identifier"))
                ? fallbackIdentifier ?? string.Empty
                : ReadString(row, "identifier");
            record.BuildId = ReadString(row, "buildid");
            record.Url = url;
            record.FileName = IpswUtilities.GetFileNameFromUrl(url);
            record.ReleaseDateText = ReadReleaseDate(row);
            record.FileSizeBytes = ReadFileSize(row);
            record.IsSigned = ReadSigned(row);
            return record;
        }

        private static IReadOnlyList<FirmwareRecord> SortRecords(List<FirmwareRecord> records)
        {
            return records
                .OrderByDescending(r => ParseReleaseDate(r.ReleaseDateText))
                .ThenBy(r => r.FileName, StringComparer.OrdinalIgnoreCase)
                .ToList();
        }

        private static DateTime ParseReleaseDate(string releaseDate)
        {
            DateTime parsed;
            if (!DateTime.TryParse(releaseDate, CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal, out parsed)
                && !DateTime.TryParse(releaseDate, CultureInfo.CurrentCulture, DateTimeStyles.AssumeLocal, out parsed))
            {
                return DateTime.MinValue;
            }

            return parsed;
        }

        private static string ReadReleaseDate(JToken row)
        {
            var raw = ReadString(row, "releasedate");
            if (string.IsNullOrWhiteSpace(raw))
            {
                return "-";
            }

            DateTime date;
            if (!DateTime.TryParse(raw, CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal, out date)
                && !DateTime.TryParse(raw, CultureInfo.CurrentCulture, DateTimeStyles.AssumeLocal, out date))
            {
                return raw;
            }

            return date.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
        }

        private static double ReadFileSize(JToken row)
        {
            var raw = ReadString(row, "filesize");
            if (string.IsNullOrWhiteSpace(raw))
            {
                return 0d;
            }

            try
            {
                return IpswUtilities.ParseFileSizeBytes(raw);
            }
            catch
            {
                return 0d;
            }
        }

        private static bool? ReadSigned(JToken row)
        {
            var raw = ReadString(row, "signed");
            if (string.IsNullOrWhiteSpace(raw))
            {
                return null;
            }

            bool parsedBool;
            if (bool.TryParse(raw, out parsedBool))
            {
                return parsedBool;
            }

            return null;
        }

        private static string ReadString(JToken row, string propertyName)
        {
            if (row == null)
            {
                return string.Empty;
            }

            var token = row[propertyName];
            return token == null ? string.Empty : token.ToString();
        }
    }
}
