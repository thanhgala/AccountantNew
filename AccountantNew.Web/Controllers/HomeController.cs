using AccountantNew.Common;
using AccountantNew.Model.Models;
using AccountantNew.Service;
using AccountantNew.Web.Infastructure.Core;
using AccountantNew.Web.Models;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using System.Xml.Linq;

namespace AccountantNew.Web.Controllers
{
    public class HomeController : BaseController
    {
        private INewCategoryService _newCategoryService;
        private INewService _newService;

        public HomeController(INewCategoryService newCategoryService, INewService newService)
        {
            this._newCategoryService = newCategoryService;
            this._newService = newService;
        }

        public ActionResult Index()
        {
            var hotNewModel = _newService.GetHotNew(4);
            var newModel = _newService.GetLastestNew(6, CommonConstants.NewID);
            var policyModel = _newService.GetListNewByParentID(CommonConstants.PolicyID).Take(4);

            var homeViewModel = new HomeViewModel();
            homeViewModel.HotNews = Mapper.Map<IEnumerable<New>, IEnumerable<NewViewModel>>(hotNewModel.Where(x=>x.NewCategoryID == CommonConstants.NewID || x.NewCategory.ParentID == CommonConstants.NewID));
            homeViewModel.LatestNews = Mapper.Map<IEnumerable<New>, IEnumerable<NewViewModel>>(newModel);
            homeViewModel.PolicyNews = Mapper.Map<IEnumerable<New>, IEnumerable<NewViewModel>>(policyModel);

            List<CurrencyModel> lstCurrency = new List<CurrencyModel>();
            homeViewModel.CurrencyRate = XmlHelper.ParseXmlData(lstCurrency);

            return View(homeViewModel);
        }

        [ChildActionOnly]
        public ActionResult Header()
        {
            var lstCategorymodel = _newCategoryService.GetAll();
            var lstCategoryViewModel = Mapper.Map<IEnumerable<NewCategory>, IEnumerable<NewCategoryViewModel>>(lstCategorymodel);

            var lstNewModel = _newService.GetListNewByParentID(CommonConstants.NewID).Take(3);
            var lstNewNotifiModel = _newService.GetListNewByParentID(119).Take(3);

            var lstNewViewModel = Mapper.Map<IEnumerable<New>, IEnumerable<NewViewModel>>(lstNewModel);
            var lstNewNotifiViewModel = Mapper.Map<IEnumerable<New>, IEnumerable<NewViewModel>>(lstNewNotifiModel);

            ViewBag.LstNew = lstNewViewModel;
            ViewBag.LstNotifi = lstNewNotifiViewModel;
            return PartialView(lstCategoryViewModel);
        }

        [ChildActionOnly]
        public ActionResult Footer()
        {
            return PartialView();
        }

    }
}