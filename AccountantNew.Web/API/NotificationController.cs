using AccountantNew.Web.Infastructure.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AccountantNew.Service;
using AutoMapper;
using AccountantNew.Web.Models;
using AccountantNew.Model.Models;
using AccountantNew.Web.Infastructure.Extensions;
using System.Data.Entity.Validation;
using System.Web.Script.Serialization;

namespace AccountantNew.Web.API
{
    [RoutePrefix("api/notification")]
    [Authorize]
    public class NotificationController : ApiControllerBase
    {
        private INewService _newService;

        public NotificationController(IErrorService errorService, INewService newService) : base(errorService)
        {
            this._newService = newService;
        }

        [HttpGet]
        [Route("getallnotification")]
        [AuthorizeApi(Role = "Notification", Action = "Read")]
        public HttpResponseMessage GetAllNotification(HttpRequestMessage request, string keyword, int page, int pageSize = 20)
        {
            int totalRow = 0;

            var model = _newService.GetAllNotifications(keyword);

            totalRow = model.Count();

            var query = model.OrderByDescending(n => n.CreatedDate).Skip(page * pageSize).Take(pageSize);

            var reponseData = Mapper.Map<IEnumerable<New>, IEnumerable<NewViewModel>>(query);

            var paginationSet = new PaginationSet<NewViewModel>()
            {
                Items = reponseData,
                Page = page,
                TotalCount = totalRow,
                TotalPages = (int)Math.Ceiling((decimal)totalRow / pageSize)
            };
            HttpResponseMessage response = request.CreateResponse(HttpStatusCode.OK, paginationSet);
            return response;
        }

        [Route("getid/{id:int}")]
        [AuthorizeApi(Role = "Notification", Action = "Read")]
        [HttpGet]
        public HttpResponseMessage GetID(HttpRequestMessage request, int id)
        {
            return CreateHttpRespond(request, () =>
            {
                var model = _newService.GetByID(id);

                var responseData = Mapper.Map<New, NewViewModel>(model);

                return request.CreateResponse(HttpStatusCode.OK, responseData);
            });
        }

        [Route("create")]
        [HttpPost]
        [AuthorizeApi(Role = "Notification", Action = "Create")]
        public HttpResponseMessage Create(HttpRequestMessage request, NewViewModel newVM)
        {
            return CreateHttpRespond(request, () =>
            {
                HttpResponseMessage response = null;
                if (!ModelState.IsValid)
                {
                    response = request.CreateResponse(HttpStatusCode.BadRequest, ModelState);
                }
                var news = new New();
                news.UpdateNew(newVM);
                news.CreatedDate = DateTime.Now;
                _newService.Add(news);
                _newService.Save();

                var responseData = Mapper.Map<New, NewViewModel>(news);
                response = request.CreateResponse(HttpStatusCode.Created, responseData);
                return response;
            });
        }

        [Route("update")]
        [HttpPut]
        [AllowAnonymous]
        [AuthorizeApi(Role = "Notification", Action = "Update")]
        public HttpResponseMessage Update(HttpRequestMessage request, NewViewModel newVm)
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
                    var news = _newService.GetByID(newVm.ID);

                    news.UpdateNew(newVm);
                    news.UpdatedDate = DateTime.Now;

                    _newService.Update(news);
                    try
                    {
                        _newService.Save();
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
                    var responseData = Mapper.Map<New, NewViewModel>(news);
                    response = request.CreateResponse(HttpStatusCode.Created, responseData);
                }

                return response;
            });
        }

        [Route("delete")]
        [HttpDelete]
        [AuthorizeApi(Role = "Notification", Action = "Delete")]
        public HttpResponseMessage Delete(HttpRequestMessage request, int id)
        {
            return CreateHttpRespond(request, () =>
            {
                HttpResponseMessage response = null;
                if (!ModelState.IsValid)
                {
                    response = request.CreateResponse(HttpStatusCode.BadGateway, ModelState);
                }

                var model = _newService.Delete(id);
                _newService.Save();

                var responseData = Mapper.Map<New, NewViewModel>(model);
                response = request.CreateResponse(HttpStatusCode.OK, responseData);
                return response;
            });
        }

        [Route("deletemulti")]
        [AuthorizeApi(Role = "Notification", Action = "Delete")]
        [HttpDelete]
        public HttpResponseMessage DeleteMulti(HttpRequestMessage request, string checkedProduct)
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
                    var listProduct = new JavaScriptSerializer().Deserialize<List<int>>(checkedProduct);

                    foreach (var item in listProduct)
                    {
                        _newService.Delete(item);
                    }
                    _newService.Save();
                    response = request.CreateResponse(HttpStatusCode.OK, listProduct.Count);
                }
                return response;
            });
        }
    }
}
