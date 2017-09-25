using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AccountantNew.Web.Controllers
{
    public class PagesController : Controller
    {
        // GET: Pages
        public ActionResult Index(string alias)
        {
            if (alias == "so-do-to-chuc")
            {
                return View("Organizational");
            }
            return View();
        }
    }
}