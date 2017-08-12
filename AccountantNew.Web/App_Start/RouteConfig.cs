using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace AccountantNew.Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "FileCategory",
                url: "{category}-file.html",
                defaults: new { controller = "File", action = "FileCategory", id = UrlParameter.Optional },
                namespaces: new string[] { "AccountantNew.Web.Controllers" }
                );

            routes.MapRoute(
                name: "NewCategory",
                url: "{category}.html",
                defaults: new { controller = "New", action = "NewCategory", id = UrlParameter.Optional },
                namespaces: new string[] { "AccountantNew.Web.Controllers" }
            );

            routes.MapRoute(
               name: "New",
               url: "{category}/{new}-{id}.html",
               defaults: new { controller = "New", action = "Detail", id = UrlParameter.Optional },
               namespaces: new string[] { "AccountantNew.Web.Controllers" }
           );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                namespaces: new string[] { "AccountantNew.Web.Controllers" }
            );
        }
    }
}
