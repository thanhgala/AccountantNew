using AccountantNew.Common;
using AccountantNew.Data.Repositories;
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
using static AccountantNew.Web.App_Start.ApplicationUserStore;
using Microsoft.AspNet.Identity;

namespace AccountantNew.Web.Controllers
{
    public class HomeController : BaseController
    {
        private INewCategoryService _newCategoryService;
        private INewService _newService;
        private IFocusRepository _focusRepository;
        private ApplicationUserManager _appUser;

        public HomeController(INewCategoryService newCategoryService, INewService newService, IFocusRepository focusRepository,ApplicationUserManager appUser)
        {
            this._newCategoryService = newCategoryService;
            this._newService = newService;
            this._focusRepository = focusRepository;
            this._appUser = appUser;
        }

        public ActionResult Index()
        {
            var hotNewModel = _newService.GetHotNew(4);
            var newModel = _newService.GetLastestNew(6, CommonConstants.NewID);

            var homeViewModel = new HomeViewModel();
            homeViewModel.HotNews = Mapper.Map<IEnumerable<New>, IEnumerable<NewViewModel>>(hotNewModel.Where(x=>x.NewCategoryID == CommonConstants.NewID || x.NewCategory.ParentID == CommonConstants.NewID));
            homeViewModel.LatestNews = Mapper.Map<IEnumerable<New>, IEnumerable<NewViewModel>>(newModel);
            if (User.Identity.IsAuthenticated)
            {
                homeViewModel.Notifications = Mapper.Map<IEnumerable<New>, IEnumerable<NewViewModel>>(_newService.GetListNotification(CommonConstants.NotificationID).Take(5).OrderByDescending(x => x.CreatedDate));
            }
            else
            {
                homeViewModel.Notifications = Mapper.Map<IEnumerable<New>, IEnumerable<NewViewModel>>(_newService.GetListNewByID(CommonConstants.AnotherDepart).Take(5).OrderByDescending(x => x.CreatedDate));
            }
            List<CurrencyModel> lstCurrency = new List<CurrencyModel>();
            homeViewModel.CurrencyRate = XmlHelper.ParseXmlData(lstCurrency);

            return View(homeViewModel);
        }

        [ChildActionOnly]
        public ActionResult Header()
        {
            var headerViewModel = new HeaderViewModel();

            var lstCategorymodel = _newCategoryService.GetAll();

            var lstNewModel = _newService.GetListNewByParentID(CommonConstants.NewID).Take(3);
            IEnumerable<New> lstNewNotifiModel;
            if (User.Identity.IsAuthenticated)
            {
                lstNewNotifiModel = _newService.GetListNotification(CommonConstants.NotificationID).Take(3).OrderByDescending(x => x.CreatedDate);
                ViewBag.FullName = _appUser.FindByIdAsync(User.Identity.GetUserId()).Result.FullName;
            }
            else
            {
                lstNewNotifiModel = _newService.GetListNewByID(CommonConstants.AnotherDepart).Take(3).OrderByDescending(x => x.CreatedDate);
            }
            var userManager = ServiceFactory.Get<ApplicationUserManager>();
            IEnumerable<ApplicationUser> lstUserFocus = userManager.Users.Where(x => x.BirthDay.Month == DateTime.Now.Month && x.BirthDay.Day == DateTime.Now.Day);

            var lstFocus= _focusRepository.GetMulti(x => x.Status == true).OrderBy(x => x.Type);

            headerViewModel.ListCategory = Mapper.Map<IEnumerable<NewCategory>, IEnumerable<NewCategoryViewModel>>(lstCategorymodel);
            headerViewModel.ListNew = Mapper.Map<IEnumerable<New>, IEnumerable<NewViewModel>>(lstNewModel);
            headerViewModel.ListNotification = Mapper.Map<IEnumerable<New>, IEnumerable<NewViewModel>>(lstNewNotifiModel);
            headerViewModel.ListUserFocus = Mapper.Map<IEnumerable<ApplicationUser>, IEnumerable<ApplicationUserViewModel>>(lstUserFocus);
            headerViewModel.ListFocus = Mapper.Map<IEnumerable<FocusNotification>,IEnumerable<FocusNotificationViewModel>>(lstFocus);

            return PartialView(headerViewModel);
        }

        [ChildActionOnly]
        public ActionResult Footer()
        {
            return PartialView();
        }

    }
}