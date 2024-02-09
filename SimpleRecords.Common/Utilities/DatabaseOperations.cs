using SimpleRecords.Common.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions;

namespace SimpleRecords.Common.Utilities
{
    public class DatabaseOperations
    {
        private readonly IFileSystem _fileSystem;  // allows unit testing file operations

        public DatabaseOperations() 
        {
            _fileSystem = new FileSystem();
        }

        public DatabaseOperations(IFileSystem fileSystem)  
        {  
            _fileSystem = fileSystem;  
        }

        public string DatabaseFile { private get; set; }
        
        public List<PersonDetails> PersonDetailsList { get; set; }

        public void SaveToDatabase(List<PersonDetails> details)
        {
            if (details == null || details.Count == 0)
                throw new Exception("Cannot save a null or empty person details list.");

            if (PersonDetailsList == null)
                PersonDetailsList = new List<PersonDetails>();

            PersonDetailsList.AddRange(details);

            SaveToDatabase();
        }

        public void SaveToDatabase()
        {
            if (string.IsNullOrEmpty(DatabaseFile))
                throw new Exception("Cannot save to database, database file string is empty");

            if (PersonDetailsList == null || PersonDetailsList.Count == 0)
                throw new Exception("Cannot save a null or empty person details list.");

            if (!_fileSystem.File.Exists(DatabaseFile))
            {
                CreateDatabaseFile();
            }
            else
            {
                ReadDatabaseContents();
            }

            try
            {
                var jsonString = JsonConvert.SerializeObject(PersonDetailsList);
                _fileSystem.File.WriteAllText(DatabaseFile, jsonString);
            }
            catch (Exception ex)
            {
                throw new Exception($"Cannot save to database file: {ex.Message}");
            }
        }

        private void CreateDatabaseFile()
        {
            try
            {
                FileInfo fileInfo = new FileInfo(DatabaseFile);
                _fileSystem.Directory.CreateDirectory(fileInfo.Directory.FullName);
                _fileSystem.File.Create(DatabaseFile).Close();
            }
            catch (Exception ex)
            {
                throw new Exception($"Unable to create database file: {ex.Message}");
            }
        }

        public void ReadDatabaseContents()
        {
            string jsonString;

            if (!_fileSystem.File.Exists(DatabaseFile))
            {
                CreateDatabaseFile();
            }

            try
            {
                jsonString = _fileSystem.File.ReadAllText(DatabaseFile);
            }
            catch (Exception ex)
            {
                throw new Exception($"Unable to read database file: {ex.Message}");
            }

            if (!string.IsNullOrEmpty(jsonString))
            {
                if (PersonDetailsList == null)
                    PersonDetailsList = new List<PersonDetails>();

                PersonDetailsList.AddRange(
                    JsonConvert.DeserializeObject<List<PersonDetails>>(jsonString));
            }
        }
    }
}
