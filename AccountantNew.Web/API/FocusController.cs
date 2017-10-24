using AccountantNew.Web.Infastructure.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AccountantNew.Service;
using AccountantNew.Data.Repositories;
using AutoMapper;
using AccountantNew.Model.Models;
using AccountantNew.Web.Models;
using AccountantNew.Web.Infastructure.Extensions;
using AccountantNew.Data.Infrastructure;
using System.Web.Script.Serialization;
using System.Data.Entity.Validation;

namespace AccountantNew.Web.API
{
    [RoutePrefix("api/focus")]
    [Authorize]
    public class FocusController : ApiControllerBase
    {
        private IFocusRepository _focusRepository;
        private IUnitOfWork _unitOfWork;

        public FocusController(IErrorService errorService, IFocusRepository focusRepository, IUnitOfWork unitOfWork) : base(errorService)
        {
            this._focusRepository = focusRepository;
            this._unitOfWork = unitOfWork;
        }

        [HttpGet]
        [Route("getall")]
        [AuthorizeApi(Role = "Focus", Action = "Read")]
        public HttpResponseMessage GetAll(HttpRequestMessage request, string keyword, int page, int pageSize = 20)
        {
            int totalRow = 0;
            IEnumerable<FocusNotification> model;
            if (string.IsNullOrEmpty(keyword))
            {
                model = _focusRepository.GetAll();
            }
            else
            {
                model = _focusRepository.GetMulti(x => x.Message.Contains(keyword));
            }

            totalRow = model.Count();

            var query = model.OrderByDescending(n => n.CreatedDate).Skip(page * pageSize).Take(pageSize);

            var reponseData = Mapper.Map<IEnumerable<FocusNotification>, IEnumerable<FocusNotificationViewModel>>(query);

            var paginationSet = new PaginationSet<FocusNotificationViewModel>()
            {
                Items = reponseData,
                Page = page,
                TotalCount = totalRow,
                TotalPages = (int)Math.Ceiling((decimal)totalRow / pageSize)
            };
            HttpResponseMessage response = request.CreateResponse(HttpStatusCode.OK, paginationSet);
            return response;
        }

        [Route("create")]
        [HttpPost]
        [AuthorizeApi(Role = "Focus", Action = "Create")]
        public HttpResponseMessage Create(HttpRequestMessage request, FocusNotificationViewModel vm)
        {
            return CreateHttpRespond(request, () =>
            {
                HttpResponseMessage response = null;
                if (!ModelState.IsValid)
                {
                    response = request.CreateResponse(HttpStatusCode.BadRequest, ModelState);
                }
                var focus = new FocusNotification();
                focus.UpdateFocus(vm);
                focus.CreatedDate = DateTime.Now;
                _focusRepository.Add(focus);
                _unitOfWork.Commit();

                var responseData = Mapper.Map<FocusNotification, FocusNotificationViewModel>(focus);
                response = request.CreateResponse(HttpStatusCode.Created, responseData);
                return response;
            });
        }

        [Route("getfocus/{id:int}")]
        [AuthorizeApi(Role = "Focus", Action = "Read")]
        [HttpGet]
        public HttpResponseMessage GetID(HttpRequestMessage request, int id)
        {
            return CreateHttpRespond(request, () =>
            {
                var model = _focusRepository.GetSingleById(id);

                var responseData = Mapper.Map<FocusNotification, FocusNotificationViewModel>(model);

                return request.CreateResponse(HttpStatusCode.OK, responseData);
            });
        }

        [Route("update")]
        [HttpPut]
        [AuthorizeApi(Role = "Focus", Action = "Update")]
        public HttpResponseMessage Update(HttpRequestMessage request, FocusNotificationViewModel focusVM)
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
                    var focus = _focusRepository.GetSingleById(focusVM.ID);

                    focus.UpdateFocus(focusVM);

                    _focusRepository.Update(focus);
                    try
                    {
                        _unitOfWork.Commit();
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
                    var responseData = Mapper.Map<FocusNotification, FocusNotificationViewModel>(focus);
                    response = request.CreateResponse(HttpStatusCode.Created, responseData);
                }

                return response;
            });
        }

        [Route("delete")]
        [HttpDelete]
        [AuthorizeApi(Role = "Focus", Action = "Delete")]
        public HttpResponseMessage Delete(HttpRequestMessage request, int id)
        {
            return CreateHttpRespond(request, () =>
            {
                HttpResponseMessage response = null;
                if (!ModelState.IsValid)
                {
                    response = request.CreateResponse(HttpStatusCode.BadGateway, ModelState);
                }

                var model = _focusRepository.Delete(id);
                _unitOfWork.Commit();

                response = request.CreateResponse(HttpStatusCode.OK);
                return response;
            });
        }

        [Route("deletemulti")]
        [AuthorizeApi(Role = "Focus", Action = "Delete")]
        [HttpDelete]
        public HttpResponseMessage DeleteMulti(HttpRequestMessage request, string checkedFocus)
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
                    var listFocus= new JavaScriptSerializer().Deserialize<List<int>>(checkedFocus);

                    foreach (var item in listFocus)
                    {
                        _focusRepository.Delete(item);
                    }
                    _unitOfWork.Commit();
                    response = request.CreateResponse(HttpStatusCode.OK, listFocus.Count);
                }
                return response;
            });
        }
    }
}
