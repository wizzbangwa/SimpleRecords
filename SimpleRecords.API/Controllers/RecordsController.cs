using log4net;
using SimpleRecords.Common.Models;
using SimpleRecords.Common.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SimpleRecords.API.Controllers
{
    public class RecordsController : ApiController
    {
        private readonly ILog _log = LogManager.GetLogger("API Logger");
        List<PersonDetails> _details;

        public RecordsController() { }

        public RecordsController(List<PersonDetails> details)
        {
            _log.Debug("In unit test mode");
            _details = details;
        }

        public IHttpActionResult Post([FromBody] string value)
        {
            _log.Info("Posting new person detail");

            string delimiter = DelimiterDetector.DetectSeparator(value);
            _log.Debug("Delimiter found: " + delimiter);

            PersonDetails details;

            _log.Debug("Converting string to person details object.");
            try
            {
                details = PersonDetails.FromDelimitedString(value, delimiter);
            }
            catch (Exception ex)
            {
                _log.Error(ex.Message);
                return BadRequest("Unrecognized record format.");
            }

            if (_details == null)
            {
                DatabaseOperations dbOps = new DatabaseOperations()
                {
                    DatabaseFile = API.Configuration.DBFileLocation
                };

                try
                {
                    _log.Debug("Read database contents");
                    dbOps.ReadDatabaseContents();
                    _log.Debug("Add new person details object");
                    dbOps.PersonDetailsList.Add(details);
                    _log.Debug("Save changes to database.");
                    dbOps.SaveToDatabase();
                }
                catch (Exception ex)
                {
                    _log.Error(ex.Message);
                    return InternalServerError(new Exception("Cannot save submission to the database."));
                }
            }

            _log.Debug("Return ok status.");
            return Ok(details);
        }

        public IHttpActionResult Get()
        {
            _log.Info("Get all records from database (unfiltered)");

            if (_details == null)
            {
                try
                {
                    _log.Debug("Get records from database");
                    _details = GetPersonDetailsList();
                }
                catch (Exception ex)
                {
                    _log.Error(ex.Message);
                    return InternalServerError(new Exception("Cannot get data from database."));
                }
            }

            if (_details == null ||
                _details.Count == 0)
            {
                _log.Warn("No person details in the database.");
                return Ok("{}");
            }

            _log.Debug("Return ok status and unfiltered list.");
            return Ok(_details);
        }

        public IHttpActionResult Get(string sortBy)
        {
            _log.Info("Getting sorted records from database.");
            _log.Debug("Sort term from URL: " + sortBy);

            if (_details == null)
            {
                try
                {
                    _log.Debug("Get records from database");
                    _details = GetPersonDetailsList();
                }
                catch (Exception ex)
                {
                    _log.Error(ex.Message);
                    return InternalServerError(new Exception("Cannot get data from database."));
                }
            }

            if (_details == null ||
                _details.Count == 0)
            {
                _log.Warn("No person details in the database.");
                return Ok("{}");
            }

            switch (sortBy)
            {
                case "color":
                    _log.Info("Return ok status and list sorted by color.");
                    return Ok(_details.OrderBy(x => x.FavoriteColor));
                case "birthdate":
                    _log.Info("Return ok status and list sorted by birth date.");
                    return Ok(_details.OrderBy(x => x.BirthDate));
                case "name":
                    _log.Info("Return ok status and list sorted by name.");
                    return Ok(_details.OrderBy(x => x.LastName));
                default:
                    _log.Warn("Return bad request, sort method not recognized.");
                    return BadRequest("Supplied sort type is not supported.");
            }

        }

        private List<PersonDetails> GetPersonDetailsList()
        {
            DatabaseOperations dbOps = new DatabaseOperations()
            {
                DatabaseFile = API.Configuration.DBFileLocation
            };

            dbOps.ReadDatabaseContents();

            if (dbOps.PersonDetailsList == null)
                return new List<PersonDetails>();

            return dbOps.PersonDetailsList;

        }
    }
}
