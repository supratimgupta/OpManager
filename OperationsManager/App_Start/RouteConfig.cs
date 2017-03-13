using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace OperationsManager
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            var route1 = routes.MapRoute(
                name: "Default",
                url: "",
                defaults: new { controller = "Student", action = "Search", id = UrlParameter.Optional},
                namespaces: new [] { "OperationsManagers.Controllers" }
                
            );
            route1.DataTokens["area"] = "Student";
        }
    }
}
