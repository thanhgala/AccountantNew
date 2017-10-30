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
    [RoutePrefix("api/post")]
    [Authorize]
    public class PostController : ApiControllerBase
    {
        private IPostService _postService;

        public PostController(IErrorService errorService, IPostService postService) : base(errorService)
        {
            this._postService = postService;
        }

        [HttpGet]
        [Route("getall")]
        [AuthorizeApi(Role = "Post", Action = "Read")]
        public HttpResponseMessage GetAll(HttpRequestMessage request, string keyword, int page, int pageSize = 20)
        {
            int totalRow = 0;
            IEnumerable<Post> model;
            if (string.IsNullOrEmpty(keyword))
            {
                model = _postService.GetAll();
            }
            else
            {
                model = _postService.GetListPostApiWithSearch(keyword);
            }

            totalRow = model.Count();

            var query = model.OrderByDescending(n => n.CreatedDate).Skip(page * pageSize).Take(pageSize);

            var reponseData = Mapper.Map<IEnumerable<Post>, IEnumerable<PostViewModel>>(query);

            var paginationSet = new PaginationSet<PostViewModel>()
            {
                Items = reponseData,
                Page = page,
                TotalCount = totalRow,
                TotalPages = (int)Math.Ceiling((decimal)totalRow / pageSize)
            };
            HttpResponseMessage response = request.CreateResponse(HttpStatusCode.OK, paginationSet);
            return response;
        }

        [Route("delete")]
        [HttpDelete]
        [AuthorizeApi(Role = "Post", Action = "Delete")]
        public HttpResponseMessage Delete(HttpRequestMessage request, int id)
        {
            return CreateHttpRespond(request, () =>
            {
                HttpResponseMessage response = null;
                if (!ModelState.IsValid)
                {
                    response = request.CreateResponse(HttpStatusCode.BadGateway, ModelState);
                }

                var model = _postService.Delete(id);
                _postService.Save();

                response = request.CreateResponse(HttpStatusCode.OK);
                return response;
            });
        }

        [Route("deletemulti")]
        [AuthorizeApi(Role = "Post", Action = "Delete")]
        [HttpDelete]
        public HttpResponseMessage DeleteMulti(HttpRequestMessage request, string checkedPosts)
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
                    var listFocus = new JavaScriptSerializer().Deserialize<List<int>>(checkedPosts);

                    foreach (var item in listFocus)
                    {
                        _postService.Delete(item);
                    }
                    _postService.Save();
                    response = request.CreateResponse(HttpStatusCode.OK, listFocus.Count);
                }
                return response;
            });
        }
    }
}
