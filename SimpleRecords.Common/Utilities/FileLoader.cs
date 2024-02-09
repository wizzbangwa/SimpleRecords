using SimpleRecords.Common.Models;
using System;
using System.Collections.Generic;
using System.IO;

namespace SimpleRecords.Common.Utilities
{
    public class FileLoader
    {
        public string FileName { private get; set; }

        public List<PersonDetails> PersonDetailsList { get; private set; }

        public void ReadFile(string filename)
        {
            if (string.IsNullOrEmpty(filename))
                throw new ArgumentException("filename argument cannot be null or empty.");

            FileName = filename;
            ReadFile();
        }

        public void ReadFile()
        {
            if (string.IsNullOrEmpty(FileName))
                throw new Exception("Need a file name to load from a file.");

            if (!File.Exists(FileName))
                throw new Exception("File does not exist.");

            PersonDetailsList = new List<PersonDetails>();

            string delimiter = null;
            foreach (string line in File.ReadLines(FileName))
            {
                if (delimiter == null)
                {
                    delimiter = DetectDelimiter(line);
                }

                PersonDetailsList.Add(PersonDetails.FromDelimitedString(line, delimiter));
            }
        }

        private string DetectDelimiter(string line)
        {
            string delimiter;

            try
            {
                delimiter = DelimiterDetector.DetectSeparator(line);
            }
            catch (Exception ex)
            {
                throw new Exception("Delimiter cannot be detected", ex);
            }

            if (delimiter == null)
                throw new Exception("Unable to detect delimiter.");
                
            return delimiter;
        }
    }
}
