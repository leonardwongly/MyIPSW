using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ipsw.Tests
{
    [TestClass]
    public class IpswUtilitiesTests
    {
        [DataTestMethod]
        [DataRow("https://example.com/path/file.ipsw", "file.ipsw")]
        [DataRow("file.ipsw", "file.ipsw")]
        [DataRow("/file.ipsw", "file.ipsw")]
        [DataRow("", "")]
        [DataRow(null, "")]
        public void GetFileNameFromUrl_ReturnsLastSegment(string input, string expected)
        {
            var result = IpswUtilities.GetFileNameFromUrl(input);

            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void GetFileNameFromUrl_ReturnsInputWhenTrailingSlash()
        {
            var input = "https://example.com/path/";

            var result = IpswUtilities.GetFileNameFromUrl(input);

            Assert.AreEqual(input, result);
        }

        [DataTestMethod]
        [DataRow("1024", 1024d)]
        [DataRow("1024.5", 1024.5d)]
        public void ParseFileSizeBytes_ParsesInvariantNumbers(string input, double expected)
        {
            var result = IpswUtilities.ParseFileSizeBytes(input);

            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void ParseFileSizeBytes_RejectsInvalidNumbers()
        {
            Assert.ThrowsException<FormatException>(() => IpswUtilities.ParseFileSizeBytes("1,024"));
            Assert.ThrowsException<FormatException>(() => IpswUtilities.ParseFileSizeBytes(string.Empty));
            Assert.ThrowsException<FormatException>(() => IpswUtilities.ParseFileSizeBytes(null));
        }

        [DataTestMethod]
        [DataRow(1073741824d, "1 GB")]
        [DataRow(1610612736d, "1.5 GB")]
        [DataRow(0d, "0 GB")]
        public void FormatGigabytes_FormatsInvariant(double bytes, string expected)
        {
            var result = IpswUtilities.FormatGigabytes(bytes);

            Assert.AreEqual(expected, result);
        }
    }
}
