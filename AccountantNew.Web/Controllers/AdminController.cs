﻿using AccountantNew.Web.Infastructure.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AccountantNew.Web.Controllers
{
    public class AdminController : BaseController
    {
        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult NotFound()
        {
            SetAlert("Bạn không có quyền truy cập vào đây.", "error");
            return View("Error404");
        }
    }
}