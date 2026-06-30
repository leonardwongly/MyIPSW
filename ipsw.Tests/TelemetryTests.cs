using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Text;

namespace ipsw.Tests
{
    [TestClass]
    public class TelemetryTests
    {
        [TestMethod]
        public void TryReadRequestBodyWithinLimit_AllowsPayloadAtOrBelowLimit()
        {
            var payload = "{\"eventName\":\"PageViewed\"}";
            var bytes = Encoding.UTF8.GetBytes(payload);

            using (var stream = new MemoryStream(bytes))
            {
                var ok = Telemetry.TryReadRequestBodyWithinLimit(stream, bytes.Length, out var body);

                Assert.IsTrue(ok);
                Assert.AreEqual(payload, body);
            }
        }

        [TestMethod]
        public void TryReadRequestBodyWithinLimit_RejectsOversizedPayload()
        {
            var payload = new string('a', 3000);
            var bytes = Encoding.UTF8.GetBytes(payload);

            using (var stream = new MemoryStream(bytes))
            {
                var ok = Telemetry.TryReadRequestBodyWithinLimit(stream, 2048, out var body);

                Assert.IsFalse(ok);
                Assert.AreEqual(string.Empty, body);
            }
        }
    }
}
