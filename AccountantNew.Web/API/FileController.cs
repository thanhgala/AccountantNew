using AccountantNew.Service;
using AccountantNew.Web.Infastructure.Core;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using AccountantNew.Model.Models;
using AccountantNew.Common;
using AutoMapper;
using AccountantNew.Web.Models;
using System.Collections.Generic;
using AccountantNew.Web.Infastructure.Extensions;
using System.Diagnostics;

namespace AccountantNew.Web.API
{
    [RoutePrefix("api/file")]
    public class FileController : ApiControllerBase
    {
        private IFileService _fileService;
        private INewCategoryService _newCategoryService;

        public FileController(IErrorService errorService, IFileService fileService, INewCategoryService newCategoryService) : base(errorService)
        {
            this._fileService = fileService;
            this._newCategoryService = newCategoryService;
        }

        [Route("getlistfile/{id:int}")]
        [HttpGet]
        public HttpResponseMessage GetListFile(HttpRequestMessage request,int id)
        {
            return CreateHttpRespond(request, () =>
            {
                var listFileModel = _fileService.GetListFileByCateID(id);
                var listViewModel = Mapper.Map<IEnumerable<Model.Models.File>, IEnumerable<FileViewModel>>(listFileModel);
                return request.CreateResponse(HttpStatusCode.OK, listViewModel);
            });
        }

        public void alo()
        {

        }

        [Route("import")]
        [HttpPost]
        public async Task<HttpResponseMessage> Import()
        {
            Stopwatch sw = Stopwatch.StartNew();

            if (!Request.Content.IsMimeMultipartContent())
            {
                return Request.CreateErrorResponse(HttpStatusCode.UnsupportedMediaType, "Định dạng không được server hỗ trợ");
            }

            var root = HttpContext.Current.Server.MapPath("~/UploadedFiles/FilePdf");
            if (!Directory.Exists(root))
            {
                Directory.CreateDirectory(root);
            }

            var provider = new MultipartFormDataStreamProvider(root);
            var result = await Request.Content.ReadAsMultipartAsync(provider);
            //Byte[] test = File.ReadAllBytes(provider.FileData[0].LocalFileName);

            bool status = Boolean.Parse(result.FormData["status"].ToString().Trim('"'));

            var dt = result.FormData["timeStarted"].ToString().Trim('"');
            DateTime timeStarted = Convert.ToDateTime(dt);

            string folder = result.FormData["folders"].ToString().Trim('"');

            string cateId = result.FormData["categoryId"].ToString().Trim('"');

            var folderRoot = HttpContext.Current.Server.MapPath("~/UploadedFiles/FilePdf/" + folder + "-" + cateId);
            if (!Directory.Exists(folderRoot))
            {
                Directory.CreateDirectory(folderRoot);
            }

            foreach (MultipartFileData fileData in result.FileData)
            {
                if (fileData.Headers.ContentType.MediaType != "application/pdf" || string.IsNullOrEmpty(fileData.Headers.ContentDisposition.FileName))
                {
                    System.IO.File.Delete(fileData.LocalFileName);
                    return Request.CreateResponse(HttpStatusCode.NotAcceptable, "Yêu cầu không đúng định dạng");
                }
                string fileName = fileData.Headers.ContentDisposition.FileName;
                if (fileName.StartsWith("\"") && fileName.EndsWith("\""))
                {
                    fileName = fileName.Trim('"');
                }
                if (fileName.Contains(@"/") || fileName.Contains(@"\"))
                {
                    fileName = Path.GetFileName(fileName);
                }
                var fullPath = Path.Combine(folderRoot, fileName);

                if (System.IO.File.Exists(fullPath))
                {
                    System.IO.File.Delete(fileData.LocalFileName);
                    return Request.CreateResponse(HttpStatusCode.NotAcceptable, "Tên file đã có trong hệ thống.");
                }
                else
                {
                    await Task.Run(() =>
                        _fileService.Add(new Model.Models.File
                        {
                            Name = fileName,
                            Alias = StringHelper.ToUnsignString(fileName),
                            NewCategoryID = int.Parse(cateId),
                            CreatedDate = DateTime.Now,
                            Status = status,
                            TimeStarted = timeStarted,
                            Path = CommonConstants.FileUpload + folder + "-" + cateId + "/" + fileName
                        })
                    ).ContinueWith((finishedTask) =>
                    {
                        _fileService.Save();
                    });

                    await Task.Run(() =>
                        System.IO.File.Copy(fileData.LocalFileName, fullPath, false)
                    ).ContinueWith((finishedTask) =>
                    {
                        System.IO.File.Delete(fileData.LocalFileName);
                    });

                    //System.IO.File.Copy(fileData.LocalFileName, fullPath, false);
                    //System.IO.File.Delete(fileData.LocalFileName);

                    //_fileService.Add(new Model.Models.File
                    //{
                    //    Name = fileName,
                    //    Alias = StringHelper.ToUnsignString(fileName),
                    //    NewCategoryID = int.Parse(cateId),
                    //    CreatedDate = DateTime.Now,
                    //    Status = status,
                    //    TimeStarted = timeStarted,
                    //    Path = CommonConstants.FileUpload + folder + "-" + cateId + "/" + fileName
                    //});
                    //_fileService.Save();

                    //MimeMapping.GetMimeMapping(filename)
                }
            }
            sw.Stop();
            var d = sw.Elapsed.TotalMilliseconds;
            return Request.CreateResponse(HttpStatusCode.OK, "Upload thành công.");
        }

        [Route("update")]
        [HttpPut]
        public HttpResponseMessage UpdateFile(HttpRequestMessage request,FileViewModel fileViewModel)
        {
            return CreateHttpRespond(request, () =>
            {
                var fullPath = HttpContext.Current.Server.MapPath("~/" + fileViewModel.Path);

                if (System.IO.File.Exists(fullPath))
                {
                    var folder = _newCategoryService.GetByID(fileViewModel.NewCategoryID);
                    string pathDest = HttpContext.Current.Server.MapPath("~/UploadedFiles/FilePdf/" + folder.Alias + "-" + folder.ID);
                    //if (System.IO.File.Exists(pathDest + "/" + fileViewModel.Name))
                    //{
                    //    return Request.CreateResponse(HttpStatusCode.NotAcceptable, "File này đã tồn tại trong danh mục " + folder.Name );
                    //}
                    if (!Directory.Exists(pathDest))
                    {
                        Directory.CreateDirectory(pathDest);
                    }
                    System.IO.File.Move(fullPath, pathDest + "/" + fileViewModel.Name);

                    var file = new Model.Models.File();
                    file.UpdateFile(fileViewModel);

                    file.TimeStarted = file.TimeStarted.AddDays(1);
                    file.Path = CommonConstants.FileUpload + folder.Alias + "-" + folder.ID + "/" + file.Name;
                    file.UpdatedDate = DateTime.Now;

                    _fileService.Update(file);
                    _fileService.Save();
                }
                return Request.CreateResponse(HttpStatusCode.OK, "Update thành công.");
            });
        }

        [Route("delete")]
        [HttpDelete]
        public HttpResponseMessage DeleteFile(HttpRequestMessage request, int id)
        {
            //var fileDetele = await Task.Run(() => _fileService.Delete(id));
            //var fullPath = await Task.Run(() => HttpContext.Current.Server.MapPath("~/" + fileDetele.Path)); 
            var fileDelete = _fileService.Delete(id);
            _fileService.Save();
            var fullPath = HttpContext.Current.Server.MapPath("~/" + fileDelete.Path);
            System.IO.File.Delete(fullPath);
            return Request.CreateResponse(HttpStatusCode.OK, "Delete thành công.");
        }
    }
}