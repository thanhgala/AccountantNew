﻿using System;
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
               name: "DetailPost",
               url: "{category}-{id}-forum/{alias}-{idpost}.html",
               defaults: new { controller = "Forum", action = "DetailPost", id = UrlParameter.Optional },
               namespaces: new string[] { "AccountantNew.Web.Controllers" }
               );

            routes.MapRoute(
                name: "FileCategory",
                url: "{category}-{id}-file.html",
                defaults: new { controller = "File", action = "FileCategory", id = UrlParameter.Optional },
                namespaces: new string[] { "AccountantNew.Web.Controllers" }
                );

            routes.MapRoute(
               name: "ForumCategory",
               url: "{category}-{id}-forum.html",
               defaults: new { controller = "Forum", action = "ForumCategory", id = UrlParameter.Optional },
               namespaces: new string[] { "AccountantNew.Web.Controllers" }
               );

            routes.MapRoute(
               name: "Page",
               url: "page-{alias}.html",
               defaults: new { controller = "Pages", action = "Index", alias = UrlParameter.Optional },
               namespaces: new string[] { "AccountantNew.Web.Controllers" }
               );

            routes.MapRoute(
               name: "UserDetail",
                 url: "chi-tiet-{id}.html",
                 defaults: new { controller = "Account", action = "Detail", id = UrlParameter.Optional },
                 namespaces: new string[] { "AccountantNew.Web.Controllers" }
             );

            routes.MapRoute(
                name: "Ask",
                url: "{category}/ask-{id}.html",
                defaults: new { controller = "Forum", action = "Ask", id = UrlParameter.Optional },
                namespaces: new string[] { "AccountantNew.Web.Controllers" }
            );

            routes.MapRoute(
              name: "New",
              url: "{category}-{new}-{id}.html",
              defaults: new { controller = "New", action = "Detail", id = UrlParameter.Optional },
              namespaces: new string[] { "AccountantNew.Web.Controllers" }
          );

            routes.MapRoute(
                name: "NewCategory",
                url: "{category}.html",
                defaults: new { controller = "New", action = "NewCategory", id = UrlParameter.Optional },
                namespaces: new string[] { "AccountantNew.Web.Controllers" }
            );

            //routes.MapRoute("NotFound", "{*url}", new { controller = "Home", action = "NotFound" });

            routes.MapRoute(
                "ErrorHandler",
                "Error/{action}/{errMsg}",
                new { controller = "Admin", action = "NotFound", errMsg = UrlParameter.Optional }
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
