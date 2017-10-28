using AccountantNew.Common;
using AccountantNew.Service;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static AccountantNew.Web.App_Start.ApplicationUserStore;

namespace AccountantNew.Web.Infastructure.Core
{
    public class OrganizationAdmin : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            var applicationGroupService = ServiceFactory.Get<IApplicationGroupService>();
            var listGroup = applicationGroupService.GetListGroupByUserId(httpContext.User.Identity.GetUserId());

            if (listGroup.Any(x => x.Name == CommonConstants.SupperAdmin))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}