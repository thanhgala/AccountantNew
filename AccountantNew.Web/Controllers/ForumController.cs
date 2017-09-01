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
        public ActionResult ForumCategory(int id)
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
                foreach (var item in listPostModel)
                {
                    item.Comments = _commentService.GetListCommentByPostID(item.ID);
                }
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

            ViewBag.ListComment = Mapper.Map<IEnumerable<Comment>, IEnumerable<CommentViewModel>>(_commentService.GetListCommentByPostID(postModel.ID));
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