using AccountantNew.Common;
using AccountantNew.Model.Models;
using AccountantNew.Service;
using AccountantNew.Web.Models;
using AutoMapper;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static AccountantNew.Web.App_Start.ApplicationUserStore;

namespace AccountantNew.Web.Infastructure.Core
{
    public class ForumPermissionAttribute : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (!httpContext.Request.IsAuthenticated)
            {
                return false;
            }
            else
            {
                bool checkAccess;
                int id;
                if (string.IsNullOrEmpty(httpContext.Request.Form["NewCategoryID"]))
                {
                    id = int.Parse((httpContext.Request.RequestContext.RouteData.Values["id"] as string)
                        ??
                        (httpContext.Request["id"] as string));
                }
                else
                {
                    id = int.Parse(httpContext.Request.Form["NewCategoryID"] as string);
                }

                var iCategoryRespository = ServiceFactory.Get<INewCategoryService>();
                if (id == CommonConstants.ConsultantID || iCategoryRespository.GetByID(id).ParentID == CommonConstants.ConsultantID)
                {
                    return true;
                }
                var userManager = ServiceFactory.Get<ApplicationUserManager>();
                var user = userManager.FindByIdAsync(httpContext.User.Identity.GetUserId());

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
                            var category = iCategoryRespository.GetByID(idCategory);
                            if (category.ID == id)
                            {
                                return true;
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
                                if (loadChildCategory(categoryFile, item.NewCategory))
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
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            filterContext.Result = new RedirectResult("~/Admin/NotFound");
        }

        public bool loadChildCategory(NewCategory category, IEnumerable<int> listID)
        {
            var newCategoryService = ServiceFactory.Get<INewCategoryService>();
            var lstChildCategory = newCategoryService.GetChildCategory(category.ID);
            bool isOk = false;
            if (lstChildCategory.Count() > 0)
            {
                foreach (var item in lstChildCategory)
                {
                    if (loadChildCategory(item, listID))
                    {
                        return true;
                    }
                    else
                    {
                        
                    }
                }
                return false;
            }
            else
            {
                foreach (var itemID in listID)
                {
                    if (category.ID == itemID)
                    {
                        isOk = true;
                    }
                    if (isOk)
                    {
                        return true;
                    }
                }
                return false;
            }
        }
    } 
}