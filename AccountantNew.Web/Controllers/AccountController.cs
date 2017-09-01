using AccountantNew.Web.Infastructure.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AccountantNew.Web.Controllers
{
    public class AccountController : BaseController
    {

        [HttpPost]
        public ActionResult Login(FormCollection f)
        {
            LdapAuthentication LDap = new LdapAuthentication("cp.com.vn");
            string username = f["username"].ToString();
            string password = f["password"].ToString();
            bool test = LDap.ValidateUser(username, password);
            //Directory.CreateDirectory(Server.MapPath("dada"));
            return View();
        }
    }
}