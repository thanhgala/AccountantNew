using AccountantNew.Model.Models;
using AccountantNew.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AccountantNew.Web.Infastructure.Extensions
{
    public static class EntityExtensions
    {
        public static void UpdateNewCategory(this NewCategory newCategory, NewCategoryViewModel newCategoryVM)
        {
            newCategory.ID = newCategoryVM.ID;
            newCategory.Name = newCategoryVM.Name;
            newCategory.Alias = newCategoryVM.Alias;
            newCategory.Description = newCategoryVM.Description;
            newCategory.ParentID = newCategoryVM.ParentID;
            newCategory.DisplayOrder = newCategoryVM.DisplayOrder;

            newCategory.CreatedDate = newCategoryVM.CreatedDate;
            newCategory.CreatedBy = newCategoryVM.CreatedBy;
            newCategory.UpdatedDate = newCategoryVM.UpdatedDate;
            newCategory.UpdateBy = newCategoryVM.UpdateBy;
            newCategory.Status = newCategoryVM.Status;
        }

        public static void UpdateNew(this New news, NewViewModel newVM)
        {
            news.ID = newVM.ID;
            news.Name = newVM.Name;
            news.Alias = newVM.Alias;
            news.NewCategoryID = newVM.NewCategoryID;
            news.ApplicationUserId = newVM.ApplicationUserId;
            news.Private = newVM.Private;
            news.Content = newVM.Content;
            news.Image = newVM.Image;
            news.HomeFlag = newVM.HomeFlag;
            news.HotFlag = newVM.HotFlag;
            news.ViewCount = newVM.ViewCount;
            news.Tags = newVM.Tags;

            news.CreatedDate = newVM.CreatedDate;
            news.CreatedBy = newVM.CreatedBy;
            news.UpdatedDate = newVM.UpdatedDate;
            news.UpdateBy = newVM.UpdateBy;
            news.Status = newVM.Status;
        }

        public static void UpdateFocus(this FocusNotification focus, FocusNotificationViewModel focusVM)
        {
            focus.ID = focusVM.ID;
            focus.Message = focusVM.Message;
            focus.Status = focusVM.Status;
            if (focusVM.Type == 3)
            {
                focus.EndDate = null;
            }
            else
            {
                focus.EndDate = focusVM.EndDate;
            }
            switch (focusVM.Type)
            {
                case 1:
                    focus.Image = "/UploadedFiles/iconHeader/wedding-couple.png";
                    break;
                case 2:
                    focus.Image = "/UploadedFiles/iconHeader/coffin.png";
                    break;
                case 3:
                    focus.Image = "/UploadedFiles/iconHeader/employee.png";
                    break;
            }
            focus.Type = focusVM.Type;
        }

        public static void UpdateApplicationGroup(this ApplicationGroup appGroup, ApplicationGroupViewModel appGroupViewModel)
        {
            appGroup.ID = appGroupViewModel.ID;
            appGroup.Name = appGroupViewModel.Name;
        }

        public static void UpdatePost(this Post post, PostViewModel postViewModel)
        {
            post.ID = postViewModel.ID;
            post.Name = postViewModel.Name;
            post.Alias = postViewModel.Alias;
            post.NewCategoryID = postViewModel.NewCategoryID;
            post.Content = postViewModel.Content;
            post.ViewCount = postViewModel.ViewCount;

            post.CreatedDate = postViewModel.CreatedDate;
            post.CreatedBy = postViewModel.CreatedBy;
            post.UpdatedDate = postViewModel.UpdatedDate;
            post.UpdateBy = postViewModel.UpdateBy;
            post.Status = postViewModel.Status;
        }

        public static void UpdateComment(this Comment comment, CommentViewModel commentViewModel)
        {
            comment.ID = commentViewModel.ID;
            comment.PostID = commentViewModel.PostID;
            comment.UserID = commentViewModel.UserID;
            comment.Content = commentViewModel.Content;
            comment.CreateDate = commentViewModel.CreateDate;
        }

        public static void UpdateFile(this File file, FileViewModel fileViewModel)
        {
            file.ID = fileViewModel.ID;

            file.Name = fileViewModel.Name;

            file.Alias = fileViewModel.Alias;

            file.NewCategoryID = fileViewModel.NewCategoryID;

            file.Path = fileViewModel.Path;

            file.Describtion = fileViewModel.Describtion;

            file.TimeStarted = fileViewModel.TimeStarted;

            file.CreatedDate = fileViewModel.CreatedDate; 

            file.CreatedBy = fileViewModel.CreatedBy; 

            file.UpdatedDate = fileViewModel.UpdatedDate;

            file.UpdateBy = fileViewModel.UpdateBy;

            file.Status = fileViewModel.Status;
        }

        public static void UpdateUser(this ApplicationUser appUser, ApplicationUserViewModel appUserViewModel)
        {
            appUser.Id = appUserViewModel.Id;
            appUser.FullName = appUserViewModel.FullName;
            appUser.BirthDay = appUserViewModel.BirthDay;
            appUser.Email = appUserViewModel.Email;
            appUser.UserName = appUserViewModel.UserName;
            appUser.PhoneNumber = appUserViewModel.PhoneNumber;
            appUser.Avartar = appUserViewModel.Avartar;
        }

        public static void UpdateUser2(this ApplicationUser appUser, ApplicationUserViewModel appUserViewModel)
        {
            appUser.FullName = appUserViewModel.FullName;
            appUser.BirthDay = appUserViewModel.BirthDay;
            appUser.Email = appUserViewModel.Email;
            appUser.Address = appUserViewModel.Address;
            appUser.PhoneNumber = appUserViewModel.PhoneNumber;
            if (!string.IsNullOrEmpty(appUserViewModel.Avartar))
            {
                appUser.Avartar = appUserViewModel.Avartar;
            }
        }
    }
}