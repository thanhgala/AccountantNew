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

        public ActionResult Category(string category)
        {
            var categoryModel = _newCategoryService.GetByAlias(category);
            var lstAdd = new List<New>();

            loadChildCategory(categoryModel.ID, lstAdd);
            ViewBag.PageTitle = categoryModel.Name;
            var listAddViewModel = Mapper.Map<List<New>, List<NewViewModel>>(lstAdd);
            return View(listAddViewModel);
        }

        private void loadChildCategory(int id,List<New> lstAdd)
        {
            var categoryModel = _newCategoryService.GetChildCategory(id);
            if (categoryModel.Count() > 0)
            {
                foreach (var itemCategory in categoryModel)
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

        private void loadNew(List<New> lst,List<New> lstAdd)
        {
            foreach (var itemNew in lst)
            {
                lstAdd.Add(itemNew);
            }
        }

        public ActionResult Detail(int id)
        {
            return View();
        }
    }
}