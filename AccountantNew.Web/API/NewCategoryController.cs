﻿using AccountantNew.Web.Infastructure.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AccountantNew.Service;
using AutoMapper;
using AccountantNew.Model.Models;
using AccountantNew.Web.Models;
using AccountantNew.Web.Infastructure.Extensions;
using System.Data.Entity.Validation;

namespace AccountantNew.Web.API
{
    [RoutePrefix("api/newcategory")]
    public class NewCategoryController : ApiControllerBase
    {
        private INewCategoryService _newCategoryService;
        public NewCategoryController(IErrorService errorService,
            INewCategoryService newCategoryService) : base(errorService)
        {
            this._newCategoryService = newCategoryService;
        }

        [Route("getall")]
        [HttpGet]
        public HttpResponseMessage GetAll(HttpRequestMessage request)
        {
            var model = _newCategoryService.GetRootCategory();
            var responseData = new TreeViewCategory<NewCategoryViewModel>();
            var lstModel = new List<TreeViewCategory<NewCategoryViewModel>>();
            foreach (var item in model)
            {
                var itemViewModel = Mapper.Map<NewCategory, NewCategoryViewModel>(item);
                responseData.ID = itemViewModel.ID;
                responseData.Name = itemViewModel.Name;
                responseData.Alias = itemViewModel.Alias;
                responseData.DisplayOrder = itemViewModel.DisplayOrder;
                responseData.ParentID = itemViewModel.ParentID;
                lstModel.Add(responseData);
                loadMenuChild(itemViewModel.ID, responseData, lstModel);
                responseData = new TreeViewCategory<NewCategoryViewModel>();
            }

            //var responseData = Mapper.Map<IEnumerable<NewCategory>, IEnumerable<NewCategoryViewModel>>(model);

            return request.CreateResponse(HttpStatusCode.OK, lstModel);
        }

        public void loadMenuChild(int parentID, TreeViewCategory<NewCategoryViewModel> res, List<TreeViewCategory<NewCategoryViewModel>> lstRes)
        {
            var modelChild = _newCategoryService.GetChildCategory(parentID);
            if(modelChild.Count() > 0)
            {
                var viewModel = Mapper.Map<List<NewCategory>, List<TreeViewCategory<NewCategoryViewModel>>>(modelChild.ToList());
                res = lstRes.Find(x => x.Name == res.Name);
                res.Nodes = viewModel;
                foreach (var childItem in res.Nodes)
                {
                    loadMenuChild(childItem.ID, childItem, res.Nodes);                 
                }
            }
        }

        [Route("getallparent")]
        [HttpGet]
        public HttpResponseMessage GetParent(HttpRequestMessage request)
        {
            return CreateHttpRespond(request, () =>
            {
                var model = _newCategoryService.GetAll();
                var responseData = Mapper.Map<IEnumerable<NewCategory>, IEnumerable<NewCategoryViewModel>>(model);

                return request.CreateResponse(HttpStatusCode.OK, responseData);
            });
        }

        [Route("getrootparent")]
        [HttpGet]
        public HttpResponseMessage GetRootParent(HttpRequestMessage request)
        {
            return CreateHttpRespond(request, () =>
            {
                var model = _newCategoryService.GetRootCategory();
                var responseData = Mapper.Map<IEnumerable<NewCategory>, IEnumerable<NewCategoryViewModel>>(model);

                return request.CreateResponse(HttpStatusCode.OK, responseData);
            });
        }

        [Route("getchildrootparent/{id:int}")]
        [HttpGet]
        public HttpResponseMessage GetChildRootParent(HttpRequestMessage request, int id)
        {
            return CreateHttpRespond(request, () =>
            {
                var lstChild = _newCategoryService.GetChildCategory(id);
                if (lstChild.Count() > 0)
                {
                    var lstChildViewModel = Mapper.Map<IEnumerable<NewCategory>, IEnumerable<NewCategoryViewModel>>(lstChild);
                    return request.CreateResponse(HttpStatusCode.OK, lstChildViewModel);
                }
                return request.CreateResponse(HttpStatusCode.OK);
            });
        }

        [Route("getid/{id:int}")]
        [HttpGet]
        public HttpResponseMessage GetID(HttpRequestMessage request, int id)
        {
            return CreateHttpRespond(request, () =>
            {
                var model = _newCategoryService.GetByID(id);

                var responseData = Mapper.Map<NewCategory, NewCategoryViewModel>(model);

                return request.CreateResponse(HttpStatusCode.OK, responseData);
            });
        }

        [Route("create")]
        [HttpPost]
        [AllowAnonymous]
        public HttpResponseMessage Create(HttpRequestMessage request, NewCategoryViewModel newCategoryVM)
        {
            return CreateHttpRespond(request, () =>
            {
                HttpResponseMessage response = null;
                if (!ModelState.IsValid)
                {
                    response = request.CreateResponse(HttpStatusCode.BadRequest, ModelState);
                }
                var newCategory = new NewCategory();
                newCategory.UpdateNewCategory(newCategoryVM);
                newCategory.CreatedDate = DateTime.Now;
                _newCategoryService.Add(newCategory);
                _newCategoryService.Save();

                var responseData = Mapper.Map<NewCategory, NewCategoryViewModel>(newCategory);
                response = request.CreateResponse(HttpStatusCode.Created, responseData);
                return response;
            });
        }

        [Route("update")]
        [HttpPut]
        [AllowAnonymous]
        public HttpResponseMessage Update(HttpRequestMessage request, NewCategoryViewModel newCategoryVm)
        {
            return CreateHttpRespond(request, () =>
            {
                HttpResponseMessage response = null;
                if (!ModelState.IsValid)
                {
                    response = request.CreateResponse(HttpStatusCode.BadRequest, ModelState);
                }
                else
                {
                    var newCategory = _newCategoryService.GetByID(newCategoryVm.ID);

                    newCategory.UpdateNewCategory(newCategoryVm);
                    newCategory.UpdatedDate = DateTime.Now;

                    _newCategoryService.Update(newCategory);
                    try
                    {
                        _newCategoryService.Save();
                    }
                    catch (DbEntityValidationException e)
                    {
                        foreach (var eve in e.EntityValidationErrors)
                        {
                            Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                                eve.Entry.Entity.GetType().Name, eve.Entry.State);
                            foreach (var ve in eve.ValidationErrors)
                            {
                                Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                                    ve.PropertyName, ve.ErrorMessage);
                            }
                        }
                        throw;
                    }


                    var responseData = Mapper.Map<NewCategory, NewCategoryViewModel>(newCategory);
                    response = request.CreateResponse(HttpStatusCode.Created, responseData);
                }

                return response;
            });
        }

        [Route("delete")]
        [HttpDelete]
        public HttpResponseMessage Delete(HttpRequestMessage request, int id)
        {
            HttpResponseMessage response = null;
            if (!ModelState.IsValid)
            {
                response = request.CreateResponse(HttpStatusCode.BadRequest, ModelState);
            }
            else
            {
                var model = _newCategoryService.Delete(id);
                _newCategoryService.Save();

                var responseData = Mapper.Map<NewCategory, NewCategoryViewModel>(model);
                response = request.CreateResponse(HttpStatusCode.OK, responseData);
            }
            return response;
        }

    }
}
