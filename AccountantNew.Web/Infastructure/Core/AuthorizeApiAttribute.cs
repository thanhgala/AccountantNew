using AccountantNew.Common;
using AccountantNew.Model.Models;
using AccountantNew.Service;
using AccountantNew.Web.Models;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using static AccountantNew.Web.App_Start.ApplicationUserStore;

namespace AccountantNew.Web.Infastructure.Core
{
    public class AuthorizeApiAttribute : AuthorizeAttribute
    {
        public string Role { set; get; }
        public string Action { set; get; }

        public override void OnAuthorization(HttpActionContext actionContext)
        {

            base.OnAuthorization(actionContext);
            var principal = actionContext.RequestContext.Principal as ClaimsPrincipal;

            if (!principal.Identity.IsAuthenticated)
            {
                actionContext.Response = new HttpResponseMessage(HttpStatusCode.Unauthorized);
            }
            else
            {
                //var roles = JsonConvert.DeserializeObject<List<string>>(principal.FindFirst("roles").Value);
                var userManager = ServiceFactory.Get<ApplicationUserManager>();
                var user = userManager.FindByNameAsync(principal.Identity.Name);

                //var groups = JsonConvert.DeserializeObject<List<ApplicationGroupViewModel>>(principal.FindFirst("groups").Value);

                var applicationGroupService = ServiceFactory.Get<IApplicationGroupService>();
                var listGroup = applicationGroupService.GetListGroupByUserId(user.Result.Id);

                var listGroupViewModel = Mapper.Map<List<ApplicationGroup>, List<ApplicationGroupViewModel>>(listGroup.ToList());
                foreach (var item in listGroupViewModel)
                {
                    var listRole = ServiceFactory.Get<IApplicationRoleService>().GetListRoleByGroupId(item.ID);
                    item.Roles = Mapper.Map<IEnumerable<ApplicationRole>, IEnumerable<ApplicationRoleViewModel>>(listRole);
                    foreach (var items in item.Roles)
                    {
                        items.ApplicationRoleGroup = Mapper.Map<ApplicationRoleGroup, ApplicationRoleGroupViewModel>(ServiceFactory.Get<IApplicationRoleService>().GetRoleGroup(item.ID, items.ID));
                    }
                }
                if (!listGroupViewModel.Any(x => x.Name == CommonConstants.SupperAdmin))
                {
                    if (listGroupViewModel.Any(x => x.Roles.Count() > 0))
                    {
                        string[] roles = Enum.GetNames(typeof(RoleEnum)).ToArray<string>();
                        for (int i = 0; i < roles.Length; i++)
                        {
                            if (Role == roles[i])
                            {
                                if (Action == "Read")
                                {
                                    if (!listGroupViewModel.Exists(x => x.Roles.Any(y => y.Name == Role && y.ApplicationRoleGroup.CanRead)))
                                    {
                                        actionContext.Response = new HttpResponseMessage(HttpStatusCode.Forbidden);
                                    }
                                    break;
                                }
                                else if (Action == "Create")
                                {
                                    if (!listGroupViewModel.Exists(x => x.Roles.Any(y => y.Name == Role && y.ApplicationRoleGroup.CanCreate)))
                                    {
                                        actionContext.Response = new HttpResponseMessage(HttpStatusCode.Forbidden);
                                    }
                                    break;
                                }
                                else if (Action == "Update")
                                {
                                    if (!listGroupViewModel.Exists(x => x.Roles.Any(y => y.Name == Role && y.ApplicationRoleGroup.CanUpdate)))
                                    {
                                        actionContext.Response = new HttpResponseMessage(HttpStatusCode.Forbidden);
                                    }
                                    break;
                                }
                                else if (Action == "Delete")
                                {
                                    if (!listGroupViewModel.Exists(x => x.Roles.Any(y => y.Name == Role && y.ApplicationRoleGroup.CanDelete)))
                                    {
                                        actionContext.Response = new HttpResponseMessage(HttpStatusCode.Forbidden);
                                    }
                                    break;
                                }
                            }
                            //actionContext.Response = new HttpResponseMessage(HttpStatusCode.OK);
                        }
                    }
                    else
                    {
                        actionContext.Response = new HttpResponseMessage(HttpStatusCode.Unauthorized);
                    }
                }
            }
        }
    }
}