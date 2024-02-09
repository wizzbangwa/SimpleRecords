using log4net.Config;
using System.Web.Http;

namespace SimpleRecords.API
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);

            XmlConfigurator.Configure();
        }
    }
}
