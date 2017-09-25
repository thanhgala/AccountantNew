using AccountantNew.Web.Infastructure.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AccountantNew.Service;
using static AccountantNew.Web.App_Start.ApplicationUserStore;
using AccountantNew.Model.Models;
using AccountantNew.Web.Models;
using AutoMapper;
using System.Threading.Tasks;
using AccountantNew.Web.Infastructure.Extensions;
using AccountantNew.Common;

namespace AccountantNew.Web.API
{
    [RoutePrefix("api/applicationUser")]
    public class ApplicationUserController : ApiControllerBase
    {
        private ApplicationUserManager _userManager;
        private IApplicationGroupService _appGroupService;
        private IApplicationRoleService _appRoleService;

        public ApplicationUserController(IErrorService errorService,
            ApplicationUserManager userManager,
            IApplicationGroupService appGroupService,
            IApplicationRoleService appRoleService) : base(errorService)
        {
            this._userManager = userManager;
            this._appGroupService = appGroupService;
            this._appRoleService = appRoleService;
        }

        [HttpGet]
        [Route("getlistpaging")]
        public HttpResponseMessage GetListPaging(HttpRequestMessage request, string keyword, int page, int pageSize = 20)
        {
            int totalRow = 0;
            var model = _userManager.Users;
           
            List<ApplicationUser> users;
            if (!string.IsNullOrEmpty(keyword))
            {
                users = model.Where(x => x.UserName.Contains(keyword) || x.FullName.Contains(keyword)).ToList();
                totalRow = users.Count();
                users = users.OrderBy(x => x.UserName).Skip(page * pageSize).Take(pageSize).ToList();
            }
            else
            {
                users = _userManager.Users.OrderBy(x => x.UserName).Skip(page * pageSize).Take(pageSize).ToList();
                totalRow = model.Count();
            }

            IEnumerable<ApplicationUserViewModel> usersViewModel = Mapper.Map<List<ApplicationUser>, List<ApplicationUserViewModel>>(users);

            var pagedSet = new PaginationSet<ApplicationUserViewModel>()
            {
                Items = usersViewModel,
                Page = page,
                TotalCount = totalRow,
                TotalPages = (int)Math.Ceiling((decimal)totalRow / pageSize)
            };
            HttpResponseMessage response = request.CreateResponse(HttpStatusCode.OK, pagedSet);
            return response;
        }

        [Route("detail/{id}")]
        [HttpGet]
        public HttpResponseMessage Details(HttpRequestMessage request, string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return request.CreateErrorResponse(HttpStatusCode.BadRequest, nameof(id) + " is required.");
            }
            var user = _userManager.FindByIdAsync(id);

            if (user == null)
            {
                return request.CreateErrorResponse(HttpStatusCode.NoContent, "Không có dữ liệu");
            }
            else
            {
                var applicationuserViewModel = Mapper.Map<ApplicationUser, ApplicationUserViewModel>(user.Result);
                var listGroup = _appGroupService.GetListGroupByUserId(applicationuserViewModel.Id);
                applicationuserViewModel.Groups = Mapper.Map<IEnumerable<ApplicationGroup>, IEnumerable<ApplicationGroupViewModel>>(listGroup);
                return request.CreateResponse(HttpStatusCode.OK, applicationuserViewModel);
            }
        }

        [Route("update")]
        [HttpPut]
        public async Task<HttpResponseMessage> Update(HttpRequestMessage request, ApplicationUserViewModel appUserViewModel)
        {
            if (ModelState.IsValid)
            {
                var appUser = await _userManager.FindByIdAsync(appUserViewModel.Id);
                try
                {
                    appUser.UpdateUser(appUserViewModel);
                    var result = await _userManager.UpdateAsync(appUser);
                    if (result.Succeeded)
                    {
                        //delete all role into user
                        var listGroup = _appGroupService.GetListGroupByUserId(appUser.Id);
                        foreach (var group in listGroup)
                        {
                            var listOldRole = _appRoleService.GetListRoleByGroupId(group.ID);
                            foreach (var role in listOldRole)
                            {
                                await _userManager.RemoveFromRoleAsync(appUser.Id, role.Name);
                            }
                        }
                        //add new role into user
                        var listAppUserGroup = new List<ApplicationUserGroup>();
                        foreach (var newGroup in appUserViewModel.Groups)
                        {
                            listAppUserGroup.Add(new ApplicationUserGroup
                            {
                                GroupId = newGroup.ID,
                                UserId = appUser.Id
                            });
                            var listRole = _appRoleService.GetListRoleByGroupId(newGroup.ID);
                            foreach (var newRole in listRole)
                            {
                                //await _userManager.RemoveFromRoleAsync(appUser.Id, role.Name);
                                //await _userManager.RemoveFromRoleAsync(appUser.Id, newRole.Name);
                                await _userManager.AddToRoleAsync(appUser.Id, newRole.Name);
                            }
                        }
                        _appGroupService.AddUserToGroups(listAppUserGroup, appUser.Id);
                        _appGroupService.Save();
                        return request.CreateResponse(HttpStatusCode.OK, appUserViewModel);
                    }
                    else
                    {
                        return request.CreateErrorResponse(HttpStatusCode.OK, string.Join(",", result.Errors));
                    }

                }
                catch (NameDuplicatedException dx)
                {
                    return request.CreateErrorResponse(HttpStatusCode.BadRequest, dx.Message);
                }
            }
            else
            {
                return request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }
        }

        [Route("delete")]
        [HttpDelete]
        public async Task<HttpResponseMessage> Delete(HttpRequestMessage request, string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            var result = await _userManager.DeleteAsync(user);
            if (result.Succeeded)
            {
                return request.CreateResponse(HttpStatusCode.OK, id);
            }
            else
            {
                return request.CreateErrorResponse(HttpStatusCode.OK, string.Join(",", result.Errors));
            }
        }
    }
}
