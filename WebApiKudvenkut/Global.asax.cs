﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace WebApiKudvenkut
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        string con = ConfigurationManager.ConnectionStrings["sqlConString"].ConnectionString;
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            //SqlDependency.Start(con);
        }

        protected void Session_Start(object sender, EventArgs e)
        {
            //NotificationComponents NC = new NotificationComponents();
            //var currentTime = DateTime.Now;
            //HttpContext.Current.Session["LastUpdated"] = currentTime;
            //NC.RegisterNotification(currentTime);
        }


        protected void Application_End()
        {
            //here we will stop Sql Dependency
            //SqlDependency.Stop(con);
        }
    }
}
