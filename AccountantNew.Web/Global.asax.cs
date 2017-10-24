using AccountantNew.Web.App_Start;
using AccountantNew.Web.Mappings;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using System;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using AccountantNew.Data;
using AccountantNew.Model.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using static AccountantNew.Web.App_Start.ApplicationUserStore;

namespace AccountantNew.Web
{
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            GlobalConfiguration.Configure(WebApiConfig.Register);

            AutoMapperConfigruation.Configure();

            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);

            RouteConfig.RegisterRoutes(RouteTable.Routes);

            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        //void Application_EndRequest(object sender, System.EventArgs e)
        //{
        //    // If the user is not authorised to see this page or access this function, send them to the error page.
        //    if (Response.StatusCode == 401)
        //    {
        //        Response.ClearContent();
        //        Response.RedirectToRoute("ErrorHandler", (RouteTable.Routes["ErrorHandler"] as Route).Defaults);
        //    }
        //}

        private void Application_Error(object sender, EventArgs e)
        {
            Exception ex = Server.GetLastError();
            if (ex is HttpException && ((HttpException)ex).GetHttpCode() == 404)
            {
                Response.Redirect("~/Admin/NotFound");
            }
            else
            {
                // your global error handling here!
            }
        }
    }
}