using AccountantNew.Web.Infastructure.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AccountantNew.Service;
using AccountantNew.Web.Models;
using AutoMapper;
using AccountantNew.Model.Models;
using AccountantNew.Web.Infastructure.Extensions;

namespace AccountantNew.Web.API
{
    [RoutePrefix("api/applicationRole")]
    public class ApplicationRoleController : ApiControllerBase
    {
        IApplicationRoleService _appRoleService;
        IApplicationGroupService _appGroupService;
        public ApplicationRoleController(IErrorService errorService,
            IApplicationRoleService appRoleService, IApplicationGroupService appGroupService) : base(errorService)
        {
            this._appRoleService = appRoleService;
            this._appGroupService = appGroupService;
        }

        [Route("getlistall")]
        [HttpGet]
        public HttpResponseMessage Getall(HttpRequestMessage request)
        {
            return CreateHttpRespond(request, () =>
            {
                HttpResponseMessage response = null;
                var model = _appRoleService.GetAll();
                IEnumerable<ApplicationRoleViewModel> modelVm = Mapper.Map<IEnumerable<ApplicationRole>, IEnumerable<ApplicationRoleViewModel>>(model);

                response = request.CreateResponse(HttpStatusCode.OK, model);

                return response;
            });
        }

        [Route("getlistrolebycondition/{id:int}")]
        [HttpGet]
        public HttpResponseMessage GetListRoleByCondition(HttpRequestMessage request,int id)
        {
            return CreateHttpRespond(request, () =>
            {
                HttpResponseMessage response = null;
                var fullList = _appRoleService.GetAll();
                var listReject = _appRoleService.GetListRoleByGroupId(id);
                var filteredList = fullList.Except(listReject);
                IEnumerable<ApplicationRoleViewModel> modelVm = Mapper.Map<IEnumerable<ApplicationRole>, IEnumerable<ApplicationRoleViewModel>>(filteredList);

                response = request.CreateResponse(HttpStatusCode.OK, modelVm);

                return response;
            });
        }
    }
}
