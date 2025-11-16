using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace ipsw
{
    public enum FirmwareAggregationMode
    {
        VersionIpsw,
        VersionOta,
        Device
    }

    public sealed class FirmwareLinkInfo
    {
        public FirmwareLinkInfo(string url, long fileSizeBytes)
        {
            Url = url ?? throw new ArgumentNullException(nameof(url));
            FileSizeBytes = fileSizeBytes;
        }

        public string Url { get; }

        public long FileSizeBytes { get; }
    }

    public sealed class FirmwareAggregationResult
    {
        public FirmwareAggregationResult(IReadOnlyList<FirmwareLinkInfo> links)
        {
            Links = links ?? throw new ArgumentNullException(nameof(links));
            TotalSizeBytes = links.Sum(link => link.FileSizeBytes);
        }

        public IReadOnlyList<FirmwareLinkInfo> Links { get; }

        public long TotalSizeBytes { get; }

        public int Count => Links.Count;
    }

    public static class FirmwareAggregationHelper
    {
        public static FirmwareAggregationResult Aggregate(string json, FirmwareAggregationMode mode)
        {
            if (json == null)
            {
                throw new ArgumentNullException(nameof(json));
            }

            var token = JToken.Parse(json);
            IEnumerable<JToken> items = GetItems(token, mode);
            var links = new List<FirmwareLinkInfo>();
            var seen = new HashSet<string>(StringComparer.Ordinal);

            foreach (var item in items)
            {
                var url = item.Value<string>("url");
                if (string.IsNullOrWhiteSpace(url) || !seen.Add(url))
                {
                    continue;
                }

                long fileSize = ParseFileSize(item["filesize"]);
                links.Add(new FirmwareLinkInfo(url, fileSize));
            }

            return new FirmwareAggregationResult(links);
        }

        private static IEnumerable<JToken> GetItems(JToken token, FirmwareAggregationMode mode)
        {
            switch (mode)
            {
                case FirmwareAggregationMode.VersionIpsw:
                case FirmwareAggregationMode.VersionOta:
                    if (token is JArray array)
                    {
                        return array;
                    }
                    break;
                case FirmwareAggregationMode.Device:
                    var firmwares = token?["firmwares"] as JArray;
                    if (firmwares != null)
                    {
                        return firmwares;
                    }
                    break;
            }

            throw new ArgumentException("Unexpected JSON structure for mode " + mode, nameof(token));
        }

        private static long ParseFileSize(JToken fileSizeToken)
        {
            if (fileSizeToken == null || fileSizeToken.Type == JTokenType.Null)
            {
                return 0L;
            }

            switch (fileSizeToken.Type)
            {
                case JTokenType.Integer:
                    return fileSizeToken.Value<long>();
                case JTokenType.Float:
                    return Convert.ToInt64(fileSizeToken.Value<double>());
                case JTokenType.String:
                    if (long.TryParse(fileSizeToken.Value<string>(), NumberStyles.Number, CultureInfo.InvariantCulture, out var value))
                    {
                        return value;
                    }
                    break;
            }

            return 0L;
        }
    }
}
