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
using static AccountantNew.Web.App_Start.ApplicationUserStore;

namespace AccountantNew.Web.Controllers
{
    public class NewController : BaseController
    {
        private INewCategoryService _newCategoryService;
        private INewService _newService;

        public NewController(INewCategoryService newCategoryService, INewService newService)
        {
            this._newCategoryService = newCategoryService;
            this._newService = newService;
        }

        #region loadChildCategory
        private void loadChildCategory(int id, List<New> lstAdd)
        {
            var childCategoryModel = _newCategoryService.GetChildCategory(id);
            if (childCategoryModel.Count() > 0)
            {
                foreach (var itemCategory in childCategoryModel)
                {
                    loadChildCategory(itemCategory.ID, lstAdd);
                }
            }
            else
            {
                var newModel = _newService.GetListNewByID(id).ToList();
                loadNew(newModel, lstAdd);
            }
        }

        private void loadNew(List<New> lst, List<New> lstAdd)
        {
            foreach (var itemNew in lst)
            {
                lstAdd.Add(itemNew);
            }
        }
        #endregion

        public ActionResult NewCategory(string category,int page = 1)
        {
            int pageSize = int.Parse(ConfigHelper.GetByKey("PageSize"));
            int totalRow = 0;

            var categoryModel = _newCategoryService.GetByAlias(category);

            IEnumerable<New> lstNewModel;

            if(categoryModel.ID != CommonConstants.NotificationID)
            {
                lstNewModel = _newService.GetListNewByCategoryIdPaging(categoryModel.ID, page, pageSize, out totalRow);
            }
            else
            {
                if (User.Identity.IsAuthenticated)
                {
                    lstNewModel = _newService.GetListNotificationByCategoryIdPaging(categoryModel.ID, page, pageSize, out totalRow);
                }
                else
                {
                    lstNewModel = _newService.GetListNewByCategoryIdPaging(CommonConstants.AnotherDepart, page, pageSize, out totalRow);
                }

            }
            var lstNewViewModel = Mapper.Map<IEnumerable<New>, IEnumerable<NewViewModel>>(lstNewModel);
            int totalPage = (int)Math.Ceiling((double)totalRow / pageSize);

            var paginationSet = new PaginationSet<NewViewModel>()
            {
                Items = lstNewViewModel,
                MaxPage = int.Parse(ConfigHelper.GetByKey("MaxPage")),
                Page = page,
                TotalCount = totalRow,
                TotalPages = totalPage
            };

            var model = new HomeViewModel();
            List<CurrencyModel> lstCurrent = new List<CurrencyModel>();
            model.CurrencyRate = XmlHelper.ParseXmlData(lstCurrent);
            ViewBag.ModelCurrencyRate = model;

            ViewBag.PageTitle = categoryModel.Name;

            return View(paginationSet);
        }

        public ActionResult Detail(int id)
        {
            var newModel = _newService.GetNewWithAppUser(id);
            ViewBag.PageTitle = _newCategoryService.GetByID(newModel.NewCategoryID).Name;
            List<CurrencyModel> lstCurrent = new List<CurrencyModel>();
            HomeViewModel model = new HomeViewModel();
            model.CurrencyRate = XmlHelper.ParseXmlData(lstCurrent);
            ViewBag.Model = model;
            newModel.ViewCount++;
            _newService.Save();
            var newViewModel = Mapper.Map<New, NewViewModel>(newModel);
            return View(newViewModel);
        }

        public JsonResult GetNewByName(string keyword)
        {
            IEnumerable<New> model;
            if (!User.Identity.IsAuthenticated)
            {
                model = _newService.GetListNewByName(keyword,false).OrderBy(x=>x.NewCategoryID);
            }
            else
            {
                model = _newService.GetListNewByName(keyword, true).OrderBy(x => x.NewCategoryID);
            }
            List<Dictionary<string, string>> listJson = new List<Dictionary<string, string>>();
            foreach (var item in model)
            {
                Dictionary<string, string> dataJson = new Dictionary<string, string>();
                dataJson["Name"] = item.Name;
                dataJson["Alias"] = item.Alias;
                dataJson["Img"] = item.Image;
                dataJson["ID"] = item.ID.ToString();
                dataJson["NewCategory"] = item.NewCategory.Alias;
                listJson.Add(dataJson);
            }
            return Json(new
            {
                data = listJson
            }, JsonRequestBehavior.AllowGet);
        }
    }
}