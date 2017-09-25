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

        public ActionResult NewCategory(string category)
        {
            var categoryModel = _newCategoryService.GetByAlias(category);
            var lstAdd = new List<New>();

            loadChildCategory(categoryModel.ID, lstAdd);
            ViewBag.PageTitle = categoryModel.Name;
            var listAddViewModel = Mapper.Map<List<New>, List<NewViewModel>>(lstAdd);
            var model = new HomeViewModel();
            List<CurrencyModel> lstCurrent = new List<CurrencyModel>();
            model.CurrencyRate = XmlHelper.ParseXmlData(lstCurrent);
            ViewBag.ModelCurrencyRate = model;

            return View(listAddViewModel);
        }

        public ActionResult Detail(int id)
        {
            var newModel = _newService.GetByID(id);
            ViewBag.PageTitle = _newCategoryService.GetByID(newModel.NewCategoryID).Name;
            List<CurrencyModel> lstCurrent = new List<CurrencyModel>();
            HomeViewModel model = new HomeViewModel();
            model.CurrencyRate = XmlHelper.ParseXmlData(lstCurrent);
            ViewBag.Model = model;

            var newViewModel = Mapper.Map<New, NewViewModel>(newModel);
            return View(newViewModel);
        }
    }
}