using SimpleRecords.Common.Validators;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace SimpleRecords.Common.Tests.ValidatorTests
{
    [TestClass]
    public class ValidAgeAttributeTests
    {
        [TestMethod]
        public void CurrentDateTime_IsValidAge()
        {
            var value = DateTime.Now;
            var attrib = new ValidAgeAttribute();

            var result = attrib.IsValid(value);

            Assert.IsTrue(result, "Current date should be a valid age.");
        }

        [TestMethod]
        public void DateTimeMInValue_IsNotValidAge()
        {
            var value = DateTime.MinValue;
            var attrib = new ValidAgeAttribute();

            var result = attrib.IsValid(value);

            Assert.IsFalse(result, "DateTime.MinValue should not be a valid age.");
        }

        [DataTestMethod]
        [DataRow(1900, 2, 15, false)]
        [DataRow(2000, 2, 15, true)]
        [DataRow(1970, 2, 15, true)]
        [DataRow(1920, 1, 1, true)]
        [DataRow(1919, 12, 31, false)]
        public void IsAValidAge(int year, int month, int day, bool isvalid)
        {
            DateTime value = new DateTime(year, month, day);
            var attrib = new ValidAgeAttribute();

            var result = attrib.IsValid(value);

            Assert.AreEqual(isvalid, result);
        }
    }
}
