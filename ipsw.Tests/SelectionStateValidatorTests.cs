using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace ipsw.Tests
{
    [TestClass]
    public class SelectionStateValidatorTests
    {
        [DataTestMethod]
        [DataRow("official", FirmwareSource.Official)]
        [DataRow("ota", FirmwareSource.Ota)]
        [DataRow("version", FirmwareSource.Version)]
        [DataRow("version_ota", FirmwareSource.VersionOta)]
        public void TryParseSource_ParsesKnownValues(string raw, FirmwareSource expected)
        {
            var ok = SelectionStateValidator.TryParseSource(raw, out var actual);

            Assert.IsTrue(ok);
            Assert.AreEqual(expected, actual);
        }

        [DataTestMethod]
        [DataRow("table", ResultMode.Table)]
        [DataRow("links", ResultMode.Links)]
        public void TryParseResultMode_ParsesKnownValues(string raw, ResultMode expected)
        {
            var ok = SelectionStateValidator.TryParseResultMode(raw, out var actual);

            Assert.IsTrue(ok);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ParseFromQuery_AcceptsKnownDeviceAndMode()
        {
            var query = new NameValueCollection
            {
                { "source", "official" },
                { "device", "iPhone16,2" },
                { "result", "links" }
            };

            var knownDevices = new HashSet<string>(System.StringComparer.OrdinalIgnoreCase)
            {
                "iPhone16,2"
            };

            var result = SelectionStateValidator.ParseFromQuery(query, knownDevices);

            Assert.AreEqual(FirmwareSource.Official, result.State.Source);
            Assert.AreEqual("iPhone16,2", result.State.DeviceIdentifier);
            Assert.AreEqual(ResultMode.Links, result.State.ResultMode);
            Assert.AreEqual(0, result.Warnings.Count);
        }

        [TestMethod]
        public void ParseFromQuery_RejectsUnknownFieldsSafely()
        {
            var query = new NameValueCollection
            {
                { "source", "unknown" },
                { "device", "<script>" },
                { "result", "grid" },
                { "version", "999.0" }
            };

            var knownDevices = new HashSet<string>();
            var result = SelectionStateValidator.ParseFromQuery(query, knownDevices);

            Assert.IsFalse(result.State.Source.HasValue);
            Assert.AreEqual(string.Empty, result.State.DeviceIdentifier);
            Assert.AreEqual(ResultMode.Table, result.State.ResultMode);
            Assert.IsTrue(result.Warnings.Count >= 2);
        }

        [TestMethod]
        public void ValidateSelection_RequiresKnownDeviceForOfficial()
        {
            var state = new SelectionState
            {
                Source = FirmwareSource.Official,
                DeviceIdentifier = "iPhone16,2"
            };

            var knownDevices = new HashSet<string>();
            var ok = SelectionStateValidator.ValidateSelection(state, knownDevices, out var message);

            Assert.IsFalse(ok);
            Assert.AreEqual("The selected device is not recognized.", message);
        }

        [TestMethod]
        public void ValidateSelection_AcceptsKnownVersion()
        {
            var state = new SelectionState
            {
                Source = FirmwareSource.Version,
                Version = VersionData.iOSVersions[0]
            };

            var ok = SelectionStateValidator.ValidateSelection(state, null, out var message);

            Assert.IsTrue(ok);
            Assert.AreEqual(string.Empty, message);
        }

        [DataTestMethod]
        [DataRow("iPhone16,2", 64, true)]
        [DataRow("<script>", 64, false)]
        [DataRow("", 64, false)]
        [DataRow("valid-token", 5, false)]
        public void IsSafeToken_ValidatesFormatAndLength(string token, int maxLength, bool expected)
        {
            var actual = SelectionStateValidator.IsSafeToken(token, maxLength);

            Assert.AreEqual(expected, actual);
        }
    }
}
