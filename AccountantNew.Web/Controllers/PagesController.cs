using AccountantNew.Data.Infrastructure;
using AccountantNew.Data.Repositories;
using AccountantNew.Model.Models;
using AccountantNew.Web.Infastructure.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AccountantNew.Web.Controllers
{
    public class PagesController : BaseController
    {
        private IOrganizationalRepository _organizationalRepository;
        private IUnitOfWork _unitOfWork;

        public PagesController(IOrganizationalRepository organizationalRepository, IUnitOfWork unitOfWork)
        {
            this._organizationalRepository = organizationalRepository;
            this._unitOfWork = unitOfWork;
        }

        public ActionResult Index(string alias)
        {
            if (alias == "so-do-to-chuc")
            {
                return View("Organizational");
            }
            return View();
        }

        public JsonResult Read()
        {
            var data = _organizationalRepository.GetAll();
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [OrganizationAdmin]
        public EmptyResult Update(List<Organizational> model)
        {
            foreach (var organizationalViewModel in model)
            {
                var organizationalModel = _organizationalRepository.GetSingleById(organizationalViewModel.ID);

                if (organizationalModel == null)
                {
                    organizationalModel = new Organizational();
                    _organizationalRepository.Add(organizationalModel);
                }

                organizationalModel.ID = organizationalViewModel.ID;
                organizationalModel.ParentID = organizationalViewModel.ParentID;
                organizationalModel.Name = organizationalViewModel.Name;
                organizationalModel.Position = organizationalViewModel.Position;
                if(organizationalViewModel.Image != "null")
                {
                    organizationalModel.Image = organizationalViewModel.Image;
                }
            }

            var modelIds = model.Select(p => p.ID);
            var removeOrgan = _organizationalRepository.GetMulti(p => !modelIds.Contains(p.ID));

            foreach (var organ in removeOrgan)
            {
                _organizationalRepository.Delete(organ);
            }

            _unitOfWork.Commit();

            return new EmptyResult();
        }

        [OrganizationAdmin]
        [HttpPost]
        public ActionResult UpdateImage(HttpPostedFileBase imagefile, int? id)
        {
            Organizational model = _organizationalRepository.GetSingleById((int)id);
            if (imagefile != null)
            {
                if (imagefile.ContentLength > 0)
                {
                    if (imagefile.ContentType != "image/jpeg" && imagefile.ContentType != "image/png" && imagefile.ContentType != "image/gif" && imagefile.ContentType != "image/jpg")
                    {
                        return Json("Upload không thành công");
                    }
                    else
                    {
                        var fileName = Path.GetFileName(imagefile.FileName);
                        model.Image = "/UploadedFiles/avartaOrganization/" + fileName;
                        var localPath = Path.Combine(Server.MapPath("~/UploadedFiles/avartaOrganization"), fileName);
                        imagefile.SaveAs(localPath);
                        _unitOfWork.Commit();
                    }
                }
            }
            imagefile = null;
            id = null;
            return Json("Upload thành công");
        }
    }
}