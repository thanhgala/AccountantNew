using AccountantNew.Common;
using AccountantNew.Model.Models;
using AccountantNew.Service;
using AccountantNew.Web.Models;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static AccountantNew.Web.App_Start.ApplicationUserStore;

namespace AccountantNew.Web.Infastructure.Core
{
    public class FilePermissionAttribute : AuthorizeAttribute
    {
        public string Type { set; get; }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (!httpContext.Request.IsAuthenticated)
            {
                return false;
            }
            else
            {
                int id;
                bool checkAccess;
                if (Type == "Category")
                {
                    id = int.Parse((httpContext.Request.RequestContext.RouteData.Values["id"] as string)
                    ??
                    (httpContext.Request["id"] as string));
                }
                else
                {
                    id = int.Parse((httpContext.Request.RequestContext.RouteData.Values["idPermission"] as string)
                    ??
                    (httpContext.Request["idPermission"] as string));
                }

                var userManager = ServiceFactory.Get<ApplicationUserManager>();
                var user = userManager.FindByNameAsync(httpContext.User.Identity.Name);

                var applicationGroupService = ServiceFactory.Get<IApplicationGroupService>();
                var listGroup = applicationGroupService.GetListGroupByUserId(user.Result.Id);
                var listGroupViewModel = Mapper.Map<List<ApplicationGroup>, List<ApplicationGroupViewModel>>(listGroup.ToList());

                foreach (var item in listGroupViewModel)
                {
                    var listIDCategory = ServiceFactory.Get<INewCategoryService>().GetListCategoryByGroupId(item.ID);
                    item.NewCategory = listIDCategory;
                }
                if (listGroupViewModel.Any(x => x.Name == CommonConstants.SupperAdmin))
                {
                    return true;
                }
                if (listGroupViewModel.Any(x => x.NewCategory.Count() > 0))
                {
                    foreach (var item in listGroupViewModel)
                    {
                        foreach (var idCategory in item.NewCategory)
                        {
                            var category = ServiceFactory.Get<INewCategoryService>().GetByID(idCategory);
                            if (Type == "Category")
                            {
                                if (category.ParentID == id)
                                {
                                    return true;
                                }
                            }
                            else
                            {
                                foreach (var itemID in item.NewCategory)
                                {
                                    if (itemID == id)
                                    {
                                        return true;
                                    }
                                }
                                var categoryFile = ServiceFactory.Get<INewCategoryService>().GetByID(id);
                                if (loadRootCategory(categoryFile, item.NewCategory))
                                {
                                    return true;
                                }
                                else
                                {
                                    checkAccess = false;
                                }
                            }
                        }
                    }
                }
                return false;
            }
        }

        public bool loadRootCategory(NewCategory category, IEnumerable<int> listID)
        {
            var newCategoryService = ServiceFactory.Get<INewCategoryService>();
            var parentCategory = newCategoryService.GetByID((int)category.ParentID);
            if (parentCategory.ParentID != null)
            {
                foreach (var itemID in listID)
                {
                    if (parentCategory.ID == itemID)
                    {
                        return true;
                    }
                }
                if (loadRootCategory(parentCategory, listID))
                    return true;
                else
                    return false;
            }
            else
            {
                return false;
            }
        }
    }
}