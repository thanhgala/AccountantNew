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
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;


namespace AccountantNew.Web.Controllers
{
    public class ForumController : BaseController
    {
        private INewCategoryService _newCategoryService;
        private INewService _newService;
        private IPostService _postService;
        private ICommentService _commentService;

        public ForumController(INewCategoryService newCategoryService, 
            INewService newService,
            IPostService postService,
            ICommentService commentService)
        {
            this._newCategoryService = newCategoryService;
            this._newService = newService;
            this._postService = postService;
            this._commentService = commentService;
        }

        [ForumPermission]
        public ActionResult ForumCategory(int id,int page = 1)
        {
            var categoryModel = _newCategoryService.GetByID(id);
            var categoryViewModel = Mapper.Map<NewCategory, NewCategoryViewModel>(categoryModel);
            ViewBag.CategoryModel = categoryViewModel;

            var childCategory = _newCategoryService.GetChildCategory(categoryModel.ID);
            var childCategoryViewModel = Mapper.Map<IEnumerable<NewCategory>, IEnumerable<NewCategoryViewModel>>(childCategory);
            if (childCategory.Count() > 0)
            {
                List<CurrencyModel> lstCurrent = new List<CurrencyModel>();
                var model = new HomeViewModel();
                model.CurrencyRate = XmlHelper.ParseXmlData(lstCurrent);
                ViewBag.ModelCurrentRate = model;
                return View("ForumCategory", childCategoryViewModel);
            }
            else
            {
                int postSize = int.Parse(ConfigHelper.GetByKey("PostSize"));
                int totalRow = 0;

                var listPostModel = _postService.GetListPostPaging(id,page,postSize,out totalRow);
                int totalPage = (int)Math.Ceiling((double)totalRow / postSize);
                foreach (var item in listPostModel)
                {
                    item.Comments = _commentService.GetListCommentByPostID(item.ID);
                }

                var listPostViewModel = Mapper.Map<IEnumerable<Post>, IEnumerable<PostViewModel>>(listPostModel);

                var paginationSet = new PaginationSet<PostViewModel>()
                {
                    Items = listPostViewModel,
                    MaxPage = int.Parse(ConfigHelper.GetByKey("MaxPage")),
                    Page = page,
                    TotalCount = totalRow,
                    TotalPages = totalPage
                };

                if (paginationSet.Items.Count() == 0)
                {
                    SetAlert("Không có bài viết", "warning");
                }
                int idParent = _newCategoryService.GetByID(categoryViewModel.ParentID.Value).ID;
                var childCategoryPost = _newCategoryService.GetChildCategory(idParent);
                var childCategoryPostViewModel = Mapper.Map<IEnumerable<NewCategory>, IEnumerable<NewCategoryViewModel>>(childCategoryPost);
                ViewBag.ChildCategoryPost = childCategoryPostViewModel;

                return View("ForumPost", paginationSet);
            }
        }

        [ForumPermission]
        public ActionResult DetailPost(int id, string alias,int idpost)
        {
            var postModel = _postService.GetPostWithAppUser(idpost);
            var cateModel = _newCategoryService.GetByID(postModel.NewCategoryID);
            postModel.ViewCount++;
            _postService.Save();
            var postViewModel = Mapper.Map<Post, PostViewModel>(postModel);
            ViewBag.CateName = cateModel.Name;

            ViewBag.ListComment = Mapper.Map<IEnumerable<Comment>, IEnumerable<CommentViewModel>>(_commentService.GetListCommentByPostID(postModel.ID));
            return View(postViewModel);
        }

        [HttpGet]
        public ActionResult Ask(int id)
        {
            var cateModel = _newCategoryService.GetChildCategory(id);
            ViewBag.NewCategoryID = new SelectList(cateModel, "ID", "Name");
            ViewBag.ForumID = id;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        [ForumPermission]
        public ActionResult Ask(PostViewModel postViewModel,int id, FormCollection f)
        {
            var cateModel = _newCategoryService.GetChildCategory(id);
            ViewBag.NewCategoryID = new SelectList(cateModel, "ID", "Name");
            ViewBag.ForumID = id;

            //if (id == CommonConstants.CommentID)
            //{

            //}

            if (ModelState.IsValid)
            {
                postViewModel.ID = 0;
                postViewModel.Alias = StringHelper.ToUnsignString(postViewModel.Name);
                postViewModel.CreatedDate = DateTime.Now;
                var postModel = new Post();
                postModel.UpdatePost(postViewModel);
                postModel.ApplicationUserId = User.Identity.GetUserId();
                _postService.Add(postModel);
                _postService.Save();
                SetAlert("Bạn đã đăng bài viết thành công", "success");

                return RedirectToAction("ForumCategory", "Forum", new { id = postModel.NewCategoryID });
            }
            SetAlert("Bạn đăng bài viết không thành công, vui lòng điền đầy đủ thông tin", "error");
            return View(postViewModel);
        }

        [ValidateInput(false)]
        [HttpGet]
        public ActionResult Answer(CommentViewModel commentViewModel)
        {
            IEnumerable<CommentViewModel> lstCommentViewModel;
            if (!ModelState.IsValid)
            {
                var lstComment = _commentService.GetListCommentByPostID(commentViewModel.PostID);
                lstCommentViewModel = Mapper.Map<IEnumerable<Comment>, IEnumerable<CommentViewModel>>(lstComment);
                SetAlert("Bạn phải nhập vào câu trả lời", "warning");
            }
            else
            {
                commentViewModel.CreateDate = DateTime.Now;
                Comment commentModel = new Comment();
                commentModel.UpdateComment(commentViewModel);

                _commentService.Add(commentModel);
                _commentService.Save();

                var lstComment = _commentService.GetListCommentByPostID(commentModel.PostID);
                lstCommentViewModel = Mapper.Map<IEnumerable<Comment>, IEnumerable<CommentViewModel>>(lstComment);

                SetAlert("Đăng bình luận thành công", "success");
            }
            return PartialView("AnswerPartial", lstCommentViewModel);
        }
    }
}