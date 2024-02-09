using SimpleRecords.Common.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Reflection;

namespace SimpleRecords.Common.Tests.UtilitiesTests
{
    [TestClass]
    public class FileLoaderTests
    {
        [TestMethod]
        [ExpectedException(typeof(Exception), "Exception is expected when the file name is missing.")]
        public void ReadFile_NoFileName_GetException()
        {
            FileLoader loader = new FileLoader
            {
                FileName = null
            };
            loader.ReadFile();
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), "Exception is expected when the file doesn't exist.")]
        public void ReadFile_InvalidFileName_GetException()
        {
            FileLoader loader = new FileLoader
            {
                FileName = "C:\\thatfile.txt"
            };
            loader.ReadFile();
        }

        [DataTestMethod]
        [DataRow("nameslist.csv")]
        [DataRow("nameslist.txt")]
        [DataRow("nameslist.psv")]
        public void ReadFile_ExistingFile_ContentsAreLoaded(string filename)
        {
            string assemblyPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            FileLoader loader = new FileLoader()
            {
                FileName = Path.Combine(assemblyPath, "resources", filename)
            };

            loader.ReadFile();

            Assert.IsTrue(loader.PersonDetailsList.Count > 0, "Resulting list should contain all entries when read from the test file.");
        }

    }
}
