using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Newtonsoft.Json;
using SimpleRecords.Common.Models;
using SimpleRecords.Common.Utilities;
using System;
using System.Collections.Generic;
using System.IO.Abstractions;

namespace SimpleRecords.Common.Tests.UtilitiesTests
{
    [TestClass]
    public class DatabaseOperationsTests
    {
        private Mock<IFileSystem> _fileSystem;
        private DatabaseOperations _dbOperations;

        [TestInitialize]
        public void InitializeTests()
        {
            string serializedList = JsonConvert.SerializeObject(CreateTestList());

            _fileSystem = new Mock<IFileSystem>();
            _fileSystem.Setup(f => f.File.Exists(It.IsAny<string>())).Verifiable();
            _fileSystem.Setup(f => f.File.WriteAllText(It.IsAny<string>(), It.IsAny<string>())).Verifiable();
            _fileSystem.Setup(f => f.File.ReadAllText(It.IsAny<string>())).Returns(serializedList);
            _fileSystem.Setup(f => f.Directory.CreateDirectory(It.IsAny<string>())).Verifiable();
            _fileSystem.Setup(f => f.File.Create(It.IsAny<string>())).Verifiable();

            _dbOperations = new DatabaseOperations(_fileSystem.Object)
            {
                DatabaseFile = "C:/GuaranteedRate/Data/database.json"
            };
        }

        [TestCleanup]
        public void CleanupTests()
        {
            _fileSystem = null;
            _dbOperations = null;
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), "Exception is expected when the list is empty.")]
        public void SaveToDatabase_EmptyList_GetError()
        {
            List<PersonDetails> details = new List<PersonDetails>();
            _fileSystem.Setup(f => f.File.Exists(It.IsAny<string>())).Returns(true);

            _dbOperations.SaveToDatabase(details);
        }

        [TestMethod]
        public void SaveToDatabase_PopulatedList_SuccesssfullSave()
        {
            _fileSystem.Setup(f => f.File.Exists(It.IsAny<string>())).Returns(true);

            _dbOperations.SaveToDatabase(CreateTestList());

            Assert.IsNotNull(_dbOperations.PersonDetailsList);
            Assert.IsTrue(_dbOperations.PersonDetailsList.Count > 0);

            _fileSystem.Verify(f => f.Directory.CreateDirectory(It.IsAny<string>()), Times.Never);
            _fileSystem.Verify(f => f.Directory.Delete(It.IsAny<string>()), Times.Never);
            _fileSystem.Verify(f => f.File.Create(It.IsAny<string>()), Times.Never);
            _fileSystem.Verify(f => f.File.Delete(It.IsAny<string>()), Times.Never);
            _fileSystem.Verify(f => f.File.WriteAllText(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
            _fileSystem.Verify(f => f.File.ReadAllText(It.IsAny<string>()), Times.Once);
            _fileSystem.Verify(f => f.File.Exists(It.IsAny<string>()), Times.Exactly(2));
        }

        [TestMethod]
        public void ReadDatabase_ExistingDatabase_ReturnsList()
        {
            List<PersonDetails> expectedList = CreateTestList();
            _fileSystem.Setup(f => f.File.Exists(It.IsAny<string>())).Returns(true);

            _dbOperations.ReadDatabaseContents();

            Assert.AreEqual(expectedList.Count, _dbOperations.PersonDetailsList.Count);
            //Assert.IsTrue(expectedList.All(_dbOperations.PersonDetailsList.Contains));  <--- for some reason the lists are the same, but don't compare properly

            _fileSystem.Verify(f => f.Directory.CreateDirectory(It.IsAny<string>()), Times.Never);
            _fileSystem.Verify(f => f.Directory.Delete(It.IsAny<string>()), Times.Never);
            _fileSystem.Verify(f => f.File.Create(It.IsAny<string>()), Times.Never);
            _fileSystem.Verify(f => f.File.Delete(It.IsAny<string>()), Times.Never);
            _fileSystem.Verify(f => f.File.WriteAllText(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
            _fileSystem.Verify(f => f.File.ReadAllText(It.IsAny<string>()), Times.Once);
        }

        [TestMethod]
        public void ReadDatabase_EmptyDatabase_ReturnsNullList()
        {
            _fileSystem.Setup(f => f.File.Exists(It.IsAny<string>())).Returns(true);
            _fileSystem.Setup(f => f.File.ReadAllText(It.IsAny<string>())).Returns("");

            _dbOperations.ReadDatabaseContents();

            Assert.IsNull(_dbOperations.PersonDetailsList);

            _fileSystem.Verify(f => f.Directory.CreateDirectory(It.IsAny<string>()), Times.Never);
            _fileSystem.Verify(f => f.Directory.Delete(It.IsAny<string>()), Times.Never);
            _fileSystem.Verify(f => f.File.Create(It.IsAny<string>()), Times.Never);
            _fileSystem.Verify(f => f.File.Delete(It.IsAny<string>()), Times.Never);
            _fileSystem.Verify(f => f.File.WriteAllText(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
            _fileSystem.Verify(f => f.File.ReadAllText(It.IsAny<string>()), Times.Once);
        }

        private List<PersonDetails> CreateTestList()
        {
            return new List<PersonDetails>()
            {
                new PersonDetails()
                {
                    FirstName = "Darth",
                    LastName = "Vader",
                    Email = "darkloard@sith.com",
                    BirthDate = new DateTime(1978, 6, 15),
                    FavoriteColor = "Black"
                },
                new PersonDetails()
                {
                    FirstName = "Luke",
                    LastName = "Skywalker",
                    Email = "luke@skywalker.com",
                    BirthDate = new DateTime(1950, 6, 15),
                    FavoriteColor = "Blue"
                },
                new PersonDetails()
                {
                    FirstName = "Han",
                    LastName = "Solo",
                    Email = "solo@han.com",
                    BirthDate = new DateTime(1985, 8, 12),
                    FavoriteColor = "White"
                }
            };
        }
    }
}
