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

namespace AccountantNew.Web.API
{
    [RoutePrefix("api/applicationRole")]
    public class ApplicationRoleController : ApiControllerBase
    {
        IApplicationRoleService _appRoleService;
        public ApplicationRoleController(IErrorService errorService,
            IApplicationRoleService appRoleService) : base(errorService)
        {
            this._appRoleService = appRoleService;
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
    }
}
