using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;

namespace ipsw.Tests
{
    [TestClass]
    public class UiTelemetryEventTests
    {
        [TestMethod]
        public void TryParse_AcceptsValidPayload()
        {
            var payload = JObject.Parse(@"{
                'eventName':'PageViewed',
                'sessionId':'sess-abc123',
                'journeyId':'journey-xyz789',
                'source':'official',
                'resultMode':'table',
                'step':'entry',
                'status':'loaded',
                'durationMs':123
            }");

            var ok = UiTelemetryEvent.TryParse(payload, out var parsed, out var error);

            Assert.IsTrue(ok);
            Assert.IsNotNull(parsed);
            Assert.AreEqual(UiTelemetryEventName.PageViewed, parsed.EventName);
            Assert.AreEqual(string.Empty, error);
        }

        [TestMethod]
        public void TryParse_RejectsUnknownField()
        {
            var payload = JObject.Parse(@"{
                'eventName':'PageViewed',
                'sessionId':'sess-abc123',
                'journeyId':'journey-xyz789',
                'extra':'nope'
            }");

            var ok = UiTelemetryEvent.TryParse(payload, out _, out var error);

            Assert.IsFalse(ok);
            Assert.AreEqual("Unknown field detected.", error);
        }

        [TestMethod]
        public void TryParse_RejectsInvalidSource()
        {
            var payload = JObject.Parse(@"{
                'eventName':'PageViewed',
                'sessionId':'sess-abc123',
                'journeyId':'journey-xyz789',
                'source':'unknown-source'
            }");

            var ok = UiTelemetryEvent.TryParse(payload, out _, out var error);

            Assert.IsFalse(ok);
            Assert.AreEqual("Invalid source.", error);
        }

        [TestMethod]
        public void TryParse_RejectsInvalidDuration()
        {
            var payload = JObject.Parse(@"{
                'eventName':'PageViewed',
                'sessionId':'sess-abc123',
                'journeyId':'journey-xyz789',
                'durationMs':700000
            }");

            var ok = UiTelemetryEvent.TryParse(payload, out _, out var error);

            Assert.IsFalse(ok);
            Assert.AreEqual("Invalid durationMs.", error);
        }

        [TestMethod]
        public void TryParse_RejectsUndefinedNumericEventName()
        {
            var payload = JObject.Parse(@"{
                'eventName':999,
                'sessionId':'sess-abc123',
                'journeyId':'journey-xyz789'
            }");

            var ok = UiTelemetryEvent.TryParse(payload, out _, out var error);

            Assert.IsFalse(ok);
            Assert.AreEqual("Unsupported eventName.", error);
        }
    }
}
