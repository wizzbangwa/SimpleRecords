using Alba.CsConsoleFormat;
using log4net;
using SimpleRecords.Common.Models;
using SimpleRecords.Common.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SimpleRecords.ConsoleApp
{
    internal class Program
    {
        internal static readonly ILog log = LogManager.GetLogger(typeof(Program));

        private static string _fileName =
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "nameslist.csv");

        private static readonly string _databaseFile = Configuration.DBFileLocation;
        static void Main(string[] args)
        {
            log.Info("Started delimited file import.");

            if (args != null && args.Length > 0)
                _fileName = args[0];
            else
                log.Debug("No file passed in, using default file.");

            log.Debug($"File to be imported: {_fileName}");
            log.Debug($"Database file being used: {_databaseFile}");

            FileLoader loader = new FileLoader()
            {
                FileName = _fileName
            };

            log.Info("Getting contents from file.");
            try
            {
                loader.ReadFile();
            }
            catch (Exception ex)
            {
                log.Error($"Cannot read file: {ex.Message}");
            }

            if (loader.PersonDetailsList != null &&
                loader.PersonDetailsList.Count > 0)
            {
                DatabaseOperations dbOps = new DatabaseOperations()
                {
                    DatabaseFile = _databaseFile
                };
                try
                {
                    log.Info("Saving imported list to the database.");
                    dbOps.SaveToDatabase(loader.PersonDetailsList);
                }
                catch (Exception ex)
                {
                    log.Error(ex.Message);
                }

                DisplayContents(dbOps.PersonDetailsList);
            }
            else
            {
                log.Warn("Nothing was loaded from the file. Check for errors and the file contents for proper formatting.");
            }

            log.Info("Completed. Press any key to exit.");
            Console.ReadKey();
        }

        private static void DisplayContents(List<PersonDetails> details)
        {
            Console.WriteLine("List sorted by favorite color then by last name ascending.");
            var sortedlist = details.OrderBy(x => x.FavoriteColor).ThenBy(x => x.LastName);
            DisplayPersonDetails(sortedlist);
            Console.WriteLine();

            Console.WriteLine("List sorted by birth date, ascending.");
            sortedlist = details.OrderBy(x => x.BirthDate);
            DisplayPersonDetails(sortedlist);
            Console.WriteLine();

            Console.WriteLine("List sorted by last name, descending.");
            sortedlist = details.OrderByDescending(x => x.LastName);
            DisplayPersonDetails(sortedlist);
            Console.WriteLine();
        }

        private static void DisplayPersonDetails(IOrderedEnumerable<PersonDetails> details)
        {
            if (details == null)
            {
                log.Warn("Cannot display the database contents, list is null.");
                return;
            }

            var headerThickness = new LineThickness(LineWidth.Double, LineWidth.Single);
            var doc = new Document(
                new Grid
                {
                    Color = ConsoleColor.Gray,
                    Columns = { GridLength.Auto, GridLength.Auto, GridLength.Auto, GridLength.Auto, GridLength.Auto },
                    Children = {
                        new Cell("First Name") { Stroke = headerThickness },
                        new Cell("Last Name") { Stroke = headerThickness },
                        new Cell("Email") { Stroke = headerThickness },
                        new Cell("Favorite Color") { Stroke = headerThickness },
                        new Cell("Birth Date") { Stroke = headerThickness },
                        details.Select(item => new[] {
                            new Cell(item.FirstName),
                            new Cell(item.LastName),
                            new Cell(item.Email),
                            new Cell(item.FavoriteColor),
                            new Cell(item.BirthDate.ToString("d"))
                        })
                    }
                }
            );
            ConsoleRenderer.RenderDocument(doc);
        }
    }
}
