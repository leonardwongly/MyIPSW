using System.Linq;
using ipsw;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ipsw.Tests
{
    [TestClass]
    public class FirmwareAggregationHelperTests
    {
        [TestMethod]
        public void AggregateVersionIpsw_DeduplicatesAndSums()
        {
            const string json = @"[
  { \"url\": \"https://example.com/fw1.ipsw\", \"filesize\": 1024 },
  { \"url\": \"https://example.com/fw2.ipsw\", \"filesize\": \"2048\" },
  { \"url\": \"https://example.com/fw1.ipsw\", \"filesize\": 8192 }
]";

            FirmwareAggregationResult result = FirmwareAggregationHelper.Aggregate(json, FirmwareAggregationMode.VersionIpsw);

            Assert.AreEqual(2, result.Count, "Duplicate URLs should be removed.");
            Assert.AreEqual(3072L, result.TotalSizeBytes, "Total size should sum unique entries only.");
            CollectionAssert.AreEqual(
                new[] { "https://example.com/fw1.ipsw", "https://example.com/fw2.ipsw" },
                result.Links.Select(link => link.Url).ToArray(),
                "Link order should follow the first occurrence of each unique URL.");
        }

        [TestMethod]
        public void AggregateDeviceFirmwares_PreservesOrderForFirstOccurrences()
        {
            const string json = @"{
  \"firmwares\": [
    { \"url\": \"https://example.com/a\", \"filesize\": 100 },
    { \"url\": \"https://example.com/b\", \"filesize\": 200 },
    { \"url\": \"https://example.com/a\", \"filesize\": 300 },
    { \"url\": \"https://example.com/c\", \"filesize\": 0 }
  ]
}";

            FirmwareAggregationResult result = FirmwareAggregationHelper.Aggregate(json, FirmwareAggregationMode.Device);

            CollectionAssert.AreEqual(
                new[]
                {
                    "https://example.com/a",
                    "https://example.com/b",
                    "https://example.com/c"
                },
                result.Links.Select(link => link.Url).ToArray(),
                "Device aggregation should preserve order of first occurrence while deduplicating.");
            Assert.AreEqual(300L, result.TotalSizeBytes, "Total size should include each unique firmware exactly once.");
        }

        [TestMethod]
        public void AggregateVersionOta_IgnoresEmptyEntries()
        {
            const string json = @"[
  { \"url\": \"https://example.com/ota1\", \"filesize\": 50 },
  { \"url\": \"\", \"filesize\": 100 },
  { \"filesize\": 150 },
  { \"url\": \"https://example.com/ota2\" }
]";

            FirmwareAggregationResult result = FirmwareAggregationHelper.Aggregate(json, FirmwareAggregationMode.VersionOta);

            Assert.AreEqual(2, result.Count, "Entries without URLs should be skipped.");
            Assert.AreEqual(50L, result.TotalSizeBytes, "Missing file sizes should be treated as zero.");
            CollectionAssert.AreEqual(
                new[] { "https://example.com/ota1", "https://example.com/ota2" },
                result.Links.Select(link => link.Url).ToArray(),
                "Link order should remain stable for valid entries.");
        }
    }
}
