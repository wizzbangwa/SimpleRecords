using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using SimpleRecords.API.Controllers;
using SimpleRecords.Common.Models;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web.Http;
using System.Web.Http.Results;

namespace SimpleRecords.API.Tests.ControllersTests
{
    [TestClass]
    public class RecordsControllerTests
    {
        private string _assemblyPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

        [TestMethod]
        public void GetAllRecords_SortByColor_ReturnsContent()
        {
            var testDetails = GetTestRecords(); 
            var sortedPersonDetails = testDetails.OrderBy(x => x.FavoriteColor);
            var controller = new RecordsController(testDetails);

            IHttpActionResult result = controller.Get("color");
            Assert.IsNotNull(result);
            var contentResult = result as OkNegotiatedContentResult<IOrderedEnumerable<PersonDetails>>;
            Assert.IsNotNull(contentResult);
            IOrderedEnumerable<PersonDetails> resultsList = contentResult.Content;
            Assert.IsNotNull(contentResult.Content);
            Assert.AreEqual(sortedPersonDetails.Count(), resultsList.Count());
            Assert.IsTrue(sortedPersonDetails.All(resultsList.Contains));
        }

        [TestMethod]
        public void GetAllRecords_SortByName_ReturnsContent()
        {
            var testDetails = GetTestRecords();
            var sortedPersonDetails = testDetails.OrderBy(x => x.LastName);
            var controller = new RecordsController(testDetails);

            IHttpActionResult result = controller.Get("name");
            Assert.IsNotNull(result);
            var contentResult = result as OkNegotiatedContentResult<IOrderedEnumerable<PersonDetails>>;
            Assert.IsNotNull(contentResult);
            IOrderedEnumerable<PersonDetails> resultsList = contentResult.Content;
            Assert.IsNotNull(contentResult.Content);
            Assert.AreEqual(sortedPersonDetails.Count(), resultsList.Count());
            Assert.IsTrue(sortedPersonDetails.All(resultsList.Contains));
        }

        [TestMethod]
        public void GetAllRecords_SortByBirthDate_ReturnsContent()
        {
            var testDetails = GetTestRecords();
            var sortedPersonDetails = testDetails.OrderBy(x => x.BirthDate);
            var controller = new RecordsController(testDetails);

            IHttpActionResult result = controller.Get("birthdate");
            Assert.IsNotNull(result);
            var contentResult = result as OkNegotiatedContentResult<IOrderedEnumerable<PersonDetails>>;
            Assert.IsNotNull(contentResult);
            IOrderedEnumerable<PersonDetails> resultsList = contentResult.Content;
            Assert.IsNotNull(contentResult.Content);
            Assert.AreEqual(sortedPersonDetails.Count(), resultsList.Count());
            Assert.IsTrue(sortedPersonDetails.All(resultsList.Contains));
        }

        [TestMethod]
        public void GetAllRecords_UnsupportedSort_ReturnsBadRequest()
        {
            var testDetails = GetTestRecords();
            var sortedPersonDetails = testDetails.OrderBy(x => x.BirthDate);
            var controller = new RecordsController(testDetails);

            IHttpActionResult result = controller.Get("firstname");
            var contentResult = result as BadRequestErrorMessageResult;

            Assert.IsNotNull(result);
            Assert.IsNotNull(contentResult.Message);
        }

        [TestMethod]
        public void GetAllRecords_Sort_EmptyList_ReturnsEmptyJSON()
        {
            var controller = new RecordsController(new List<PersonDetails>());

            IHttpActionResult result = controller.Get("firstname");
            var contentResult = result as OkNegotiatedContentResult<string>;

            Assert.IsNotNull(contentResult);
            Assert.AreEqual("{}", contentResult.Content);
        }

        [TestMethod]
        public void GetAllRecords_Unsorted_ReturnsContent()
        {
            var testDetails = GetTestRecords();
            var controller = new RecordsController(testDetails);

            IHttpActionResult result = controller.Get();
            var contentResult = result as OkNegotiatedContentResult<List<PersonDetails>>;
            List<PersonDetails> resultsList = contentResult.Content;

            Assert.IsNotNull(contentResult);
            Assert.IsNotNull(contentResult.Content);
            Assert.AreEqual(testDetails.Count, resultsList.Count);
            Assert.IsTrue(testDetails.All(resultsList.Contains));
        }

        [TestMethod]
        public void GetAllRecords_Unsorted_EmptyList_ReturnsEmptyJSON()
        {
            var controller = new RecordsController(new List<PersonDetails>());

            IHttpActionResult result = controller.Get();
            var contentResult = result as OkNegotiatedContentResult<string>;

            Assert.IsNotNull(contentResult);
            Assert.AreEqual("{}", contentResult.Content);
        }

        [DataTestMethod]
        [DataRow("Smith, John, john@yahoo.com, red, 5/2/1985", ", ")]
        [DataRow("Doe | Jane | jane@hotmail.com | blue | 8/10/1995", " | ")]
        [DataRow("Wrath Fuller wrathful@live.com yellow 7/15/1935", " ")]
        public void PostRecord_ReturnsContent(string newDetail, string delimiter)
        {
            var controller = new RecordsController(new List<PersonDetails>());

            IHttpActionResult result = controller.Post(newDetail);
            var contentResult = result as OkNegotiatedContentResult<PersonDetails>;

            Assert.IsNotNull(result);
            Assert.IsNotNull(contentResult.Content);
        }

        [DataTestMethod]
        [DataRow("Smith John, john@yahoo.com, red, 5/2/1985")]
        [DataRow("")]
        public void PostBadRecord_ReturnsBadRequest(string newDetail)
        {
            var controller = new RecordsController(new List<PersonDetails>());

            IHttpActionResult result = controller.Post(newDetail);
            var contentResult = result as BadRequestErrorMessageResult;

            Assert.IsNotNull(result);
            Assert.IsNotNull(contentResult.Message);
        }

        private List<PersonDetails> GetTestRecords()
        {
            string unsortedJson = File.ReadAllText(Path.Combine(_assemblyPath, "resources", "json-unsorted.json"));

            return JsonConvert.DeserializeObject<List<PersonDetails>>(unsortedJson);
        }
    }
}
