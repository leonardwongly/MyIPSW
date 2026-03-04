using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Reflection;
using System.Web.UI.WebControls;

namespace ipsw.Tests
{
    [TestClass]
    public class DefaultPageTests
    {
        [TestMethod]
        public void ClearResults_ResetsTelemetryStatus()
        {
            var page = new TestableDefaultPage();
            page.TelemetryStatus = "success";

            page.InvokeClearResults();

            Assert.AreEqual("none", page.TelemetryStatus);
        }

        private sealed class TestableDefaultPage : Default
        {
            public TestableDefaultPage()
            {
                tblData = new Table();
                lblLinkResults = new Label();
                btnDownloadAll = new Button();
                hfDownloadLinks = new HiddenField();
                hfTelemetryStatus = new HiddenField();
                hfResultCount = new HiddenField();
                lblResultSummary = new Label();
            }

            public string TelemetryStatus
            {
                get { return hfTelemetryStatus.Value; }
                set { hfTelemetryStatus.Value = value; }
            }

            public void InvokeClearResults()
            {
                var method = typeof(Default).GetMethod("ClearResults", BindingFlags.Instance | BindingFlags.NonPublic);
                if (method == null)
                {
                    throw new InvalidOperationException("ClearResults method was not found.");
                }

                method.Invoke(this, null);
            }
        }
    }
}
