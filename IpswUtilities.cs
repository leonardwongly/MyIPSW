using System;
using System.Globalization;

namespace ipsw
{
    internal static class IpswUtilities
    {
        internal static string GetFileNameFromUrl(string url)
        {
            if (string.IsNullOrEmpty(url))
            {
                return string.Empty;
            }

            var lastSlash = url.LastIndexOf('/');
            if (lastSlash < 0 || lastSlash == url.Length - 1)
            {
                return url;
            }

            return url.Substring(lastSlash + 1);
        }

        internal static double ParseFileSizeBytes(string fileSize)
        {
            if (!double.TryParse(fileSize, NumberStyles.Float, CultureInfo.InvariantCulture, out var value))
            {
                throw new FormatException("Invalid file size.");
            }

            return value;
        }

        internal static string FormatGigabytes(double bytes)
        {
            var gb = bytes / 1024d / 1024d / 1024d;
            return gb.ToString("0.##", CultureInfo.InvariantCulture) + " GB";
        }
    }
}
