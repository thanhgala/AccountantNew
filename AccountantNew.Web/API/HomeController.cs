using AccountantNew.Web.Infastructure.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AccountantNew.Service;

namespace AccountantNew.Web.API
{
    [RoutePrefix("api/home")]
    [Authorize]
    public class HomeController : ApiControllerBase
    {
        IErrorService _errorService;
        public HomeController(IErrorService errorService) : base(errorService)
        {
            this._errorService = errorService;  
        }

        [HttpGet]
        [Route("testauthen")]
        [Authorize]
        public string TestAuthen()
        {
            return null;
        }
    }
}
