using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using XP_Scrum_Grupp2.Models;
using Quartz;

namespace XP_Scrum_Grupp2
{
    public class MvcApplication : System.Web.HttpApplication
    {

        protected void Application_Start()
        {
            ApplicationUser user = new ApplicationUser();

            if(user.NewFormalPostsNotification == true)
            {
                BlogScheduler.Start();
                Database.SetInitializer(new UserInitializer());
                AreaRegistration.RegisterAllAreas();
                FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
                RouteConfig.RegisterRoutes(RouteTable.Routes);
                BundleConfig.RegisterBundles(BundleTable.Bundles);
            }

            else
            {
                Database.SetInitializer(new UserInitializer());
                AreaRegistration.RegisterAllAreas();
                FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
                RouteConfig.RegisterRoutes(RouteTable.Routes);
                BundleConfig.RegisterBundles(BundleTable.Bundles);
            }
        }
    }
}
