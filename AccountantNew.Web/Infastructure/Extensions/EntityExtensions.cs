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
            news.AuthorID = newVM.AuthorID;
            news.Private = newVM.Private;
            news.Content = newVM.Content;
            news.Image = newVM.Image;
            news.HomeFlag = newVM.HomeFlag;
            news.HotFlag = newVM.HotFlag;
            news.ViewCount = newVM.ViewCount;
            news.CommentCount = newVM.CommentCount;
            news.Tags = newVM.Tags;

            news.CreatedDate = newVM.CreatedDate;
            news.CreatedBy = newVM.CreatedBy;
            news.UpdatedDate = newVM.UpdatedDate;
            news.UpdateBy = newVM.UpdateBy;
            news.Status = newVM.Status;
        }
    }
}