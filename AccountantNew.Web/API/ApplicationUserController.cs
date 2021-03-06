﻿using AccountantNew.Common;
using AccountantNew.Model.Models;
using AccountantNew.Service;
using AccountantNew.Web.Infastructure.Core;
using AccountantNew.Web.Infastructure.Extensions;
using AccountantNew.Web.Models;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using static AccountantNew.Web.App_Start.ApplicationUserStore;

namespace AccountantNew.Web.API
{
    [RoutePrefix("api/applicationUser")]
    public class ApplicationUserController : ApiControllerBase
    {
        private ApplicationUserManager _userManager;
        private IApplicationGroupService _appGroupService;
        private IApplicationRoleService _appRoleService;
        private IApplicationUserService _appUserService;

        public ApplicationUserController(IErrorService errorService,
            ApplicationUserManager userManager,
            IApplicationGroupService appGroupService,
            IApplicationRoleService appRoleService,
            IApplicationUserService appUserService) : base(errorService)
        {
            this._userManager = userManager;
            this._appGroupService = appGroupService;
            this._appRoleService = appRoleService;
            this._appUserService = appUserService;
        }

        [HttpGet]
        [AuthorizeApi(Role = "Account", Action = "Read")]
        [Route("getlistpaging")]
        public HttpResponseMessage GetListPaging(HttpRequestMessage request, string keyword, int page, int pageSize = 20)
        {
            int totalRow = 0;
            IEnumerable<ApplicationUser> model = _appUserService.GetUser(keyword,page,pageSize,out totalRow);

            IEnumerable<ApplicationUserViewModel> usersViewModel = Mapper.Map<IEnumerable<ApplicationUser>, IEnumerable<ApplicationUserViewModel>>(model);

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
        [AuthorizeApi(Role = "Account", Action = "Read")]
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
        [AuthorizeApi(Role = "Account", Action = "Update")]
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
        [AuthorizeApi(Role = "Account", Action = "Delete")]
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