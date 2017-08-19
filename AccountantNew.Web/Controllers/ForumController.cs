using AccountantNew.Common;
using AccountantNew.Model.Models;
using AccountantNew.Service;
using AccountantNew.Web.Infastructure.Core;
using AccountantNew.Web.Infastructure.Extensions;
using AccountantNew.Web.Models;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AccountantNew.Web.Controllers
{
    public class ForumController : BaseController
    {
        private INewCategoryService _newCategoryService;
        private INewService _newService;
        private IPostService _postService;

        public ForumController(INewCategoryService newCategoryService, INewService newService,IPostService postService)
        {
            this._newCategoryService = newCategoryService;
            this._newService = newService;
            this._postService = postService;
        }
        public ActionResult ForumCategory(string category,int id)
        {
            var categoryModel = _newCategoryService.GetByID(id);
            var categoryViewModel = Mapper.Map<NewCategory, NewCategoryViewModel>(categoryModel);
            ViewBag.CategoryModel = categoryViewModel;

            List<CurrencyModel> lstCurrent = new List<CurrencyModel>();
            ViewBag.CurrentRate = XmlHelper.ParseXmlData(lstCurrent);

            var childCategory = _newCategoryService.GetChildCategory(categoryModel.ID);
            if (childCategory.Count() > 0)
            {
                var childCategoryViewModel = Mapper.Map<IEnumerable<NewCategory>, IEnumerable<NewCategoryViewModel>>(childCategory);
                return View("ForumCategory", childCategoryViewModel);
            }
            else
            {
                var listPostModel = _postService.GetListPost(id);
                var listPostViewModel = Mapper.Map<IEnumerable<Post>, IEnumerable<PostViewModel>>(listPostModel);
                return View("ForumPost", listPostViewModel);
            }
        }

        public ActionResult DetailPost(string alias)
        {
            var postModel = _postService.GetByAlias(alias);
            var cateModel = _newCategoryService.GetByID(postModel.NewCategoryID);

            var postViewModel = Mapper.Map<Post, PostViewModel>(postModel);
            ViewBag.CateName = cateModel.Name;
            return View(postViewModel);
        }

        [HttpGet]
        public ActionResult Ask(int id)
        {
            var cateModel = _newCategoryService.GetChildCategory(id);
            ViewBag.NewCategoryID = new SelectList(cateModel, "ID", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Ask(PostViewModel postViewModel,int id)
        {
            var cateModel = _newCategoryService.GetChildCategory(id);
            ViewBag.NewCategoryID = new SelectList(cateModel, "ID", "Name");

            if (ModelState.IsValid)
            {
                postViewModel.ID = 0;
                postViewModel.Alias = StringHelper.ToUnsignString(postViewModel.Name);
                postViewModel.CreatedDate = DateTime.Now;
                var postModel = new Post();
                postModel.UpdatePost(postViewModel);
                _postService.Add(postModel);
                _postService.Save();
                SetAlert("Bạn đã đăng bài viết thành công", "success");
                return View();
            }
            SetAlert("Bạn đăng bài viết không thành công, vui lòng điền đầy đủ thông tin", "error");
            return RedirectToAction("ForumCategory","Forum");
        }

        public ActionResult Answer()
        {
            return View();
        }
    }
}