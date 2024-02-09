using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimpleRecords.Common.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SimpleRecords.Common.Tests.ModelsTests
{
    [TestClass]
    public class PersonDetailsTests
    {
        [DataTestMethod]
        [DataRow(null, "lastname", "email@email.com", "red", "05/05/2000")]
        [DataRow("firstname", null, "email@email.com", "red", "05/05/2000")]
        [DataRow("firstname", "lastname", null, "red", "05/05/2000")]
        [DataRow("firstname", "lastname", "email@email.com", null, "05/05/2000")]
        [DataRow("firstname", "lastname", "email@email.com", "red", null)]
        public void NullProperties_FailsValidation(string firstName, string lastnName, 
            string email, string favoriteColor, string birthdate)
        {
            PersonDetails details = new PersonDetails()
            {
                FirstName = firstName,
                LastName = lastnName,
                Email = email,
                FavoriteColor = favoriteColor
            };

            if (!string.IsNullOrEmpty(birthdate))
                details.BirthDate = DateTime.Parse(birthdate);

            var validationResults = ValidateModel(details);

            Assert.IsTrue(validationResults.Count > 0, "A null attribute should cause a validation failure.");
        }

        [TestMethod]
        public void ValidProperties_PassValidation()
        {
            PersonDetails details = new PersonDetails()
            {
                FirstName = "firstname",
                LastName = "lastname",
                Email = "email@email.com",
                FavoriteColor = "red",
                BirthDate = new DateTime(2000, 5, 5)
            };

            var validationResults = ValidateModel(details);

            Assert.IsTrue(validationResults.Count == 0, "Valid attributes should pass validation.");
        }

        [DataTestMethod]
        [DataRow("email@email.com", true)]
        [DataRow("emailemail.com", false)]
        [DataRow("root@localhost", true)]
        public void EmailAddressChecks(string email, bool isvalid)
        {
            PersonDetails details = new PersonDetails()
            {
                FirstName = "firstname",
                LastName = "lastname",
                Email = email,
                FavoriteColor = "red",
                BirthDate = new DateTime(2000, 5, 5)
            };

            var validationResults = ValidateModel(details);

            bool valid = validationResults.Count == 0;

            Assert.AreEqual(isvalid, valid);
        }

        [DataTestMethod]
        [DataRow("\"firstname\", \"lastname\", \"email@email.com\", \"red\", \"05/05/2000\"", ", ")]
        [DataRow("\"firstname\" \"lastname\" \"email@email.com\" \"red\" \"05/05/2000\"", " ")]
        [DataRow("\"firstname\" | \"lastname\" | \"email@email.com\" | \"red\" | \"05/05/2000\"", " | ")]
        [DataRow("firstname, lastname, email@email.com, red, 05/05/2000", ", ")]
        [DataRow("firstname lastname email@email.com red 05/05/2000", " ")]
        [DataRow("firstname | lastname | email@email.com | red | 05/05/2000", " | ")]
        [DataRow("'firstname', 'lastname', 'email@email.com', 'red', '05/05/2000'", ", ")]
        [DataRow("'firstname' 'lastname' 'email@email.com' 'red' '05/05/2000'", " ")]
        [DataRow("'firstname' | 'lastname' | 'email@email.com' | 'red' | '05/05/2000'", " | ")]
        public void CreateNewInstanceFromDelimitedString(string text, string delimiter)
        {
            PersonDetails details = PersonDetails.FromDelimitedString(text, delimiter);

            Assert.IsNotNull(details);
        }

        [DataTestMethod]
        [DataRow("firstname,lastname,email@email.com,red,05/05/2000", ", ")]
        // The space in "last name" throws error, last name should not have a space when
        // using a space delimiter without quotes.
        [DataRow("firstname last name email@email.com red 05/05/2000", " ")]  
        [DataRow("firstname|lastname|email@email.com|red|05/05/2000", " | ")]
        [ExpectedException(typeof(Exception), "Exception is expected when delimited string is not supported.")]
        public void CreateNewInstanceFromUnsupportedDelimitedString_ThrowsError(string text, string delimiter)
        {
            PersonDetails.FromDelimitedString(text, delimiter);
        }

        private IList<ValidationResult> ValidateModel(object model)
        {
            var validationResults = new List<ValidationResult>();
            var context = new ValidationContext(model, null, null);
            Validator.TryValidateObject(model, context, validationResults, true);
            return validationResults;
        }
    }
}
