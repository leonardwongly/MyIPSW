using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ipsw.Tests
{
    [TestClass]
    public class FirmwareQueryServiceTests
    {
        [TestMethod]
        public void ParseVersionRecords_DeduplicatesByUrl()
        {
            var json = "["
                + "{\"identifier\":\"iPhone16,2\",\"buildid\":\"22A335\",\"url\":\"https://example.com/a.ipsw\",\"filesize\":\"1073741824\",\"releasedate\":\"2025-01-01\",\"signed\":\"true\"},"
                + "{\"identifier\":\"iPhone16,2\",\"buildid\":\"22A335\",\"url\":\"https://example.com/a.ipsw\",\"filesize\":\"1073741824\",\"releasedate\":\"2025-01-01\",\"signed\":\"true\"},"
                + "{\"identifier\":\"iPhone16,2\",\"buildid\":\"22A336\",\"url\":\"https://example.com/b.ipsw\",\"filesize\":\"2147483648\",\"releasedate\":\"2025-01-02\",\"signed\":\"false\"}"
                + "]";

            var records = FirmwareQueryService.ParseVersionRecords(json);

            Assert.AreEqual(2, records.Count);
            Assert.AreEqual("b.ipsw", records[0].FileName);
            Assert.AreEqual("a.ipsw", records[1].FileName);
        }

        [TestMethod]
        public void ParseDeviceRecords_UsesFallbackIdentifierWhenMissing()
        {
            var json = "{\"firmwares\":[{\"buildid\":\"22A335\",\"url\":\"https://example.com/a.ipsw\",\"filesize\":\"1073741824\",\"releasedate\":\"2025-01-01\",\"signed\":\"true\"}]}";

            var records = FirmwareQueryService.ParseDeviceRecords(json, "iPhone16,2");

            Assert.AreEqual(1, records.Count);
            Assert.AreEqual("iPhone16,2", records[0].Identifier);
            Assert.AreEqual(true, records[0].IsSigned);
        }

        [TestMethod]
        public void ParseVersionRecords_InvalidJson_ReturnsEmpty()
        {
            var records = FirmwareQueryService.ParseVersionRecords("{");

            Assert.AreEqual(0, records.Count);
        }

        [TestMethod]
        public void ParseDeviceRecords_MissingArray_ReturnsEmpty()
        {
            var records = FirmwareQueryService.ParseDeviceRecords("{}", "iPhone16,2");

            Assert.AreEqual(0, records.Count);
        }
    }
}
