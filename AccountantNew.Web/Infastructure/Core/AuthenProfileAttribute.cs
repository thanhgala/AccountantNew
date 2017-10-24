using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AccountantNew.Web.Infastructure.Core
{
    public class AuthenProfileAttribute : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (!httpContext.Request.IsAuthenticated)
            {
                return false;
            }
            else
            {
                string id;
                id = (httpContext.Request.RequestContext.RouteData.Values["id"] as string
                   ??
                   (httpContext.Request["id"] as string));

                if (id == httpContext.User.Identity.GetUserId())
                {
                    return true;
                }
                return false;
            }
        }
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            filterContext.Result = new RedirectResult("~/Admin/NotFound");
        }
    } 
}