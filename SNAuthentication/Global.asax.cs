using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace SNAuthentication
{
    ////Integration of MVC entity framework with MySql
    ////https://www.asp.net/identity/overview/getting-started/aspnet-identity-using-mysql-storage-with-an-entityframework-mysql-provider

    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            var log4NetPath = Server.MapPath("~/Log4Net.config");
            log4net.Config.XmlConfigurator.ConfigureAndWatch(new System.IO.FileInfo(log4NetPath));

            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}
