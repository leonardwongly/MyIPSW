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
            if (url == null)
            {
                throw new ArgumentNullException(nameof(url));
            }

            Url = url;
            FileSizeBytes = fileSizeBytes;
        }

        public string Url { get; }

        public long FileSizeBytes { get; }
    }

    public sealed class FirmwareAggregationResult
    {
        public FirmwareAggregationResult(IReadOnlyList<FirmwareLinkInfo> links)
        {
            if (links == null)
            {
                throw new ArgumentNullException(nameof(links));
            }

            Links = links;
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

            JToken token = JToken.Parse(json);
            IEnumerable<JToken> items = GetItems(token, mode);
            List<FirmwareLinkInfo> links = new List<FirmwareLinkInfo>();
            HashSet<string> seenUrls = new HashSet<string>(StringComparer.Ordinal);

            foreach (JToken item in items)
            {
                string url = item.Value<string>("url");
                if (string.IsNullOrWhiteSpace(url) || !seenUrls.Add(url))
                {
                    continue;
                }

                long fileSizeBytes = ParseFileSize(item["filesize"]);
                links.Add(new FirmwareLinkInfo(url, fileSizeBytes));
            }

            return new FirmwareAggregationResult(links);
        }

        private static IEnumerable<JToken> GetItems(JToken token, FirmwareAggregationMode mode)
        {
            switch (mode)
            {
                case FirmwareAggregationMode.VersionIpsw:
                case FirmwareAggregationMode.VersionOta:
                    JArray versionItems = token as JArray;
                    if (versionItems != null)
                    {
                        return versionItems;
                    }
                    break;
                case FirmwareAggregationMode.Device:
                    JArray deviceItems = token?["firmwares"] as JArray;
                    if (deviceItems != null)
                    {
                        return deviceItems;
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
                    long value;
                    if (long.TryParse(fileSizeToken.Value<string>(), NumberStyles.Number, CultureInfo.InvariantCulture, out value))
                    {
                        return value;
                    }
                    break;
            }

            return 0L;
        }
    }
}
