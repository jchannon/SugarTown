using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using SugarTownDemoMVC.Model;

namespace SugarTownMVC
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.IgnoreRoute("SugarTown/{*pathInfo}");

            routes.MapRoute("blog", "blog/page/{pagenumber}", new { controller = "Blog", action = "BlogPaging", pagenumber = 1 });
            routes.MapRoute("blogtitle", "blog/{postTitle}", new { controller = "Blog", action = "BlogTitle", postTitle = "" });
            routes.MapRoute("singletag", "blog/tag/{tagName}", new { controller = "Blog", action = "Tag", tagName = "" });

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Home", action = "Index", id = UrlParameter.Optional } // Parameter defaults
            );

            routes.MapRoute("tag", "blog/tag/{tagName}/page/{pagenumber}", new { controller = "Blog", action = "TagPaging", tagName = "", pagenumber = 1 });

        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);

        }
    }
}