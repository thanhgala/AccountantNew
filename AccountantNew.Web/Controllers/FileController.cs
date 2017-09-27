using AccountantNew.Model.Models;
using AccountantNew.Service;
using AccountantNew.Web.Infastructure.Core;
using AccountantNew.Web.Models;
using AutoMapper;
using CKFinder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AccountantNew.Web.Controllers
{
    public class FileController : BaseController
    {
        private INewCategoryService _newCategoryService;
        private INewService _newService;
        private IFileService _fileService;

        public FileController(INewCategoryService newCategoryService, INewService newService, IFileService fileService)
        {
            this._newCategoryService = newCategoryService;
            this._newService = newService;
            this._fileService = fileService;
        }

        [HandleError]
        [FilePermission(Type = "Category")]
        [HttpGet]
        public ActionResult FileCategory(string category, int id)
        {
            var categoryModel = _newCategoryService.GetByID(id);
            //var categoryModel = _newCategoryService.GetByAlias(category);
            ViewBag.PageTitle = categoryModel.Name;
            TempData["Category"] = category;

            //CKFinder.FileBrowser ckf = new FileBrowser();
            //ckf.BasePath = Server.MapPath("~/ckfinder");

            return View();
        }

        #region LoadChildCategory
        public void loadMenuChild(int parentID, TreeFileCategory<NewCategoryViewModel> res, List<TreeFileCategory<NewCategoryViewModel>> lstRes)
        {
            var modelChild = _newCategoryService.GetChildCategory(parentID);
            if (modelChild.Count() > 0)
            {
                res = lstRes.Find(x => x.text == res.text);
                var lstAdd = new List<TreeFileCategory<NewCategoryViewModel>>();
                var opened = new Dictionary<string, bool>
                {
                    { "opened" , true }
                };
                foreach (var item in modelChild)
                {
                    var itemViewModel = new TreeFileCategory<NewCategoryViewModel>();
                    itemViewModel.text = item.Name;
                    itemViewModel.alias = item.Alias;
                    itemViewModel.id = item.ID;
                    itemViewModel.parentid = item.ParentID;
                    itemViewModel.state = opened;
                    lstAdd.Add(itemViewModel);
                }
                res.children = lstAdd;
                //var viewModel = Mapper.Map<List<NewCategory>, List<TreeFileCategory<NewCategoryViewModel>>>(modelChild.ToList());
                foreach (var childItem in res.children)
                {
                    loadMenuChild(childItem.id, childItem, res.children);
                }
            }
        }
        #endregion

        public JsonResult GetJsonCategory()
        {
            string category = TempData["Category"].ToString();
            TempData["Category"] = null;

            var categoryModel = _newCategoryService.GetByAlias(category);

            var opened = new Dictionary<string, bool>
            {
                { "opened" , true }
            };

            var childModel = _newCategoryService.GetChildCategory(categoryModel.ID);
            var responseData = new TreeFileCategory<NewCategoryViewModel>();
            var lstModel = new List<TreeFileCategory<NewCategoryViewModel>>();
            foreach (var item in childModel)
            {
                var itemViewModel = Mapper.Map<NewCategory, NewCategoryViewModel>(item);
                responseData.text = itemViewModel.Name;
                responseData.alias = itemViewModel.Alias;
                responseData.id = itemViewModel.ID;
                responseData.parentid = itemViewModel.ParentID;
                responseData.state = opened;
                lstModel.Add(responseData);
                loadMenuChild(itemViewModel.ID, responseData, lstModel);
                responseData = new TreeFileCategory<NewCategoryViewModel>();
            }
            return Json(new
            {
                data = lstModel,
            }, JsonRequestBehavior.AllowGet);
        }

        [FilePermission(Type = "FileCategory")]
        [HttpPost]
        public ActionResult GetListFile(int idPermission)
        {
            var lstNewCategory = _newCategoryService.GetChildCategory(idPermission);
            var listFileModel = _fileService.GetListFileByCateID(idPermission);
            var listFileViewModel = Mapper.Map<IEnumerable<File>, IEnumerable<FileViewModel>>(listFileModel);
            return PartialView(listFileViewModel);
        }

        //public FileResult DisplayPDF()
        //{
        //    return File("/UploadedFiles/FilePdf/cong-ty-123/Unity AI Programming Essentials.pdf", "application/pdf");
        //}

        [HttpGet]
        public ActionResult DisplayPDF(string path)
        {
            return PartialView("DisplayPDF", path);
        }
    }
}