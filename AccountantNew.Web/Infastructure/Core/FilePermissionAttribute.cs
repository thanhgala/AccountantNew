using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AccountantNew.Web.Infastructure.Core
{
    public class FilePermissionAttribute : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            //var id = (httpContext.Request.RequestContext.RouteData.Values["id"] as string)
            //??
            //(httpContext.Request["id"] as string);

            int id = int.Parse(httpContext.Request["id"] as string);
            if (id == 124)
            {
                return false;
            }
            return true;
        }
    }
}