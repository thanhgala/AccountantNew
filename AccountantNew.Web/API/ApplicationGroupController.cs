using AccountantNew.Web.Infastructure.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AccountantNew.Service;
using static AccountantNew.Web.App_Start.ApplicationUserStore;
using AccountantNew.Web.Models;
using AutoMapper;
using AccountantNew.Model.Models;
using AccountantNew.Common;
using System.Web.Script.Serialization;
using AccountantNew.Web.Infastructure.Extensions;
using System.Threading.Tasks;

namespace AccountantNew.Web.API
{
    [RoutePrefix("api/applicationGroup")]
    public class ApplicationGroupController : ApiControllerBase
    {
        private IApplicationGroupService _appGroupService;
        private IApplicationRoleService _appRoleService;
        private ApplicationUserManager _userManager;
        public ApplicationGroupController(IErrorService errorService,
            IApplicationGroupService appGroupService,
            IApplicationRoleService appRoleService,
            ApplicationUserManager userManager) : base(errorService)
        {
            this._appGroupService = appGroupService;
            this._appRoleService = appRoleService;
            this._userManager = userManager;
        }

        [Route("getlistpaging")]
        [HttpGet]
        public HttpResponseMessage GetListPaging(HttpRequestMessage request, int page, int pageSize, string keyword = null)
        {
            return CreateHttpRespond(request, () =>
            {
                HttpResponseMessage response = null;
                int totalRow = 0;
                var model = _appGroupService.GetAll(page, pageSize, out totalRow, keyword);
                IEnumerable<ApplicationGroupViewModel> modelVM = Mapper.Map<IEnumerable<ApplicationGroup>, IEnumerable<ApplicationGroupViewModel>>(model);

                PaginationSet<ApplicationGroupViewModel> pagedSet = new PaginationSet<ApplicationGroupViewModel>()
                {
                    Page = page,
                    TotalCount = totalRow,
                    TotalPages = (int)Math.Ceiling((decimal)totalRow / pageSize),
                    Items = modelVM
                };
                response = request.CreateResponse(HttpStatusCode.OK, pagedSet);
                return response;
            });
        }

        [Route("detail/{id:int}")]
        [HttpGet]
        public HttpResponseMessage Details(HttpRequestMessage request, int id)
        {
            if (id == 0)
            {
                return request.CreateErrorResponse(HttpStatusCode.BadRequest, nameof(id) + " is required.");
            }
            ApplicationGroup appGroup = _appGroupService.GetDetail(id);
            var appGroupViewModel = Mapper.Map<ApplicationGroup, ApplicationGroupViewModel>(appGroup);
            if (appGroup == null)
            {
                return request.CreateErrorResponse(HttpStatusCode.NoContent, "No group");
            }

            var listRole = _appRoleService.GetListRoleByGroupId(appGroupViewModel.ID);

            appGroupViewModel.Roles = Mapper.Map<IEnumerable<ApplicationRole>, IEnumerable<ApplicationRoleViewModel>>(listRole);

            foreach (var roleGroup in appGroupViewModel.Roles)
            {
                var roleGroupModel = _appRoleService.GetRoleGroup(appGroupViewModel.ID, roleGroup.ID);
                roleGroup.ApplicationRoleGroup = Mapper.Map<ApplicationRoleGroup,ApplicationRoleGroupViewModel>(roleGroupModel);
            }

            return request.CreateResponse(HttpStatusCode.OK, appGroupViewModel);
        }

        [Route("update")]
        [HttpPut]
        public HttpResponseMessage Update(HttpRequestMessage request, ApplicationGroupViewModel appGroupViewModel)
        {
            if (ModelState.IsValid)
            {
                var appGroup = _appGroupService.GetDetail(appGroupViewModel.ID);
                try
                {
                    appGroup.UpdateApplicationGroup(appGroupViewModel);
                    _appGroupService.Update(appGroup);
                    var listRoleGroup = new List<ApplicationRoleGroup>();
                    foreach (var role in appGroupViewModel.Roles)
                    {
                        listRoleGroup.Add(new ApplicationRoleGroup
                        {
                            GroupId = appGroup.ID,
                            RoleId = role.ID,
                            CanCreate = role.ApplicationRoleGroup.CanCreate,
                            CanRead = role.ApplicationRoleGroup.CanRead,
                            CanUpdate = role.ApplicationRoleGroup.CanUpdate,
                            CanDelete = role.ApplicationRoleGroup.CanDelete
                        });
                    }
                    //var listOldRole = _appRoleService.GetListRoleByGroupId(appGroup.ID);
                    //var listUserInGroup = _appGroupService.GetListUserByGroupId(appGroup.ID);
                    //foreach (var user in listUserInGroup)
                    //{
                    //    foreach (var roleOld in listOldRole)
                    //    {
                    //        await _userManager.RemoveFromRoleAsync(user.Id, roleOld.Name);
                    //    }
                    //}
                    _appRoleService.AddRolesToGroup(listRoleGroup, appGroup.ID);
                    _appRoleService.Save();

                    //var listRole = _appRoleService.GetListRoleByGroupId(appGroup.ID);
                    //foreach (var user in listUserInGroup)
                    //{
                    //    var listRoleName = listRole.Select(x => x.Name).ToArray();
                    //    foreach (var roleName in listRoleName)
                    //    {
                    //        //await _userManager.RemoveFromRoleAsync(user.Id, roleName);
                    //        await _userManager.AddToRoleAsync(user.Id, roleName);
                    //    }
                    //}
                    //_appGroupService.Save();
                    return request.CreateResponse(HttpStatusCode.OK, appGroupViewModel);
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

        [Route("add")]
        [HttpPost]
        public HttpResponseMessage Create(HttpRequestMessage request,ApplicationGroupViewModel appGroupViewModel)
        {
            if (ModelState.IsValid)
            {
                var newAppGroup = new ApplicationGroup();
                newAppGroup.Name = appGroupViewModel.Name;
                try
                {
                    var appGroup = _appGroupService.Add(newAppGroup);
                    _appGroupService.Save();

                    var listRoleGroup = new List<ApplicationRoleGroup>();
                    foreach (var role in appGroupViewModel.Roles)
                    {
                        listRoleGroup.Add(new ApplicationRoleGroup
                        {
                            GroupId = appGroup.ID,
                            RoleId = role.ID,
                            CanCreate = false,
                            CanRead = false,
                            CanUpdate = false,
                            CanDelete = false
                        });
                    }
                    _appRoleService.AddRolesToGroup(listRoleGroup, appGroup.ID);
                    _appRoleService.Save();
                    return request.CreateResponse(HttpStatusCode.OK, appGroupViewModel);

                }
                catch (NameDuplicatedException dex)
                {
                    return request.CreateErrorResponse(HttpStatusCode.BadRequest, dex.Message);
                }
            }
            else
            {
                return request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }
        }

        [Route("delete")]
        [HttpDelete]
        public HttpResponseMessage Delete(HttpRequestMessage request, int id)
        {
            var appGroup = _appGroupService.Delete(id);
            _appGroupService.Save();
            return request.CreateResponse(HttpStatusCode.OK, appGroup);
        }

        [Route("deletemulti")]
        [HttpDelete]
        public HttpResponseMessage DeleteMulti(HttpRequestMessage request, string checkedList)
        {
            return CreateHttpRespond(request, () =>
            {
                HttpResponseMessage response = null;
                if (!ModelState.IsValid)
                {
                    response = request.CreateResponse(HttpStatusCode.BadRequest, ModelState);
                }
                else
                {
                    var listItem = new JavaScriptSerializer().Deserialize<List<int>>(checkedList);
                    foreach (var item in listItem)
                    {
                        _appGroupService.Delete(item);
                    }

                    _appGroupService.Save();

                    response = request.CreateResponse(HttpStatusCode.OK, listItem.Count);
                }

                return response;
            });
        }
    }
}
