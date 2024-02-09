using System.Configuration;
using System.IO;
using System.Reflection;

public class Class1
{
    internal static class Configuration
    {
        public static string DBFileLocation
        {
            get
            {
                string location = ConfigurationManager.AppSettings["DBFileLocation"].ToString();

                if (string.IsNullOrEmpty(location))
                    location = Path.Combine(
                        Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
                            "Data", "database.json");

                return location;
            }
        }
    }
}
