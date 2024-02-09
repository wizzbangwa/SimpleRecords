using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using SimpleRecords.Common.Utilities;

namespace SimpleRecords.Common.Tests.UtilitiesTests
{
    [TestClass]
    public class DelimiterDetectorTests
    {
        [DataTestMethod]
        [DataRow("\"firstname\", \"lastname\"", ", ")]
        [DataRow("\"firstname\" \"lastname\"", " ")]
        [DataRow("\"firstname\" | \"lastname\"", " | ")]
        [DataRow("firstname, lastname", ", ")]
        [DataRow("firstname | lastname", " | ")]
        [DataRow("firstname lastname", " ")]
        public void ProperDelimiter_GetDelimiter(string delimitedString, string delimiter)
        {
            string detectedDelimiter = DelimiterDetector.DetectSeparator(delimitedString);

            if (string.IsNullOrEmpty(detectedDelimiter))
                Assert.Fail("Delimiter not detected.");

            Assert.AreEqual(delimiter, detectedDelimiter, "Proper delimiter was not detected.");
        }

        [DataTestMethod]
        [DataRow("\"firstname\";\"lastname\"")]
        [DataRow("\"firstname\"$\"lastname\"")]
        public void UnsupportedDelimiter_GetNull(string delimitedString)
        {
            string detectedDelimiter = DelimiterDetector.DetectSeparator(delimitedString);

            Assert.IsNull(detectedDelimiter, "Unsupported delimiter should return null.");
        }
    }
}
