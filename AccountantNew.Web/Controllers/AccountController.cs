using AccountantNew.Common;
using AccountantNew.Model.Models;
using AccountantNew.Web.Infastructure.Core;
using AccountantNew.Web.Infastructure.Extensions;
using AccountantNew.Web.Models;
using AutoMapper;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using static AccountantNew.Web.App_Start.ApplicationUserStore;

namespace AccountantNew.Web.Controllers
{
    public class AccountController : BaseController
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        [HttpPost]
        public async Task<ActionResult> Login(LoginViewModel userModel)
        {
            if (!ModelState.IsValid)
            {
                return Content("Vui lòng nhập đầy đủ userdomain và mật khẩu.");
            }
            //LdapAuthentication LDap = new LdapAuthentication("cp.com.vn");
            //bool login = LDap.ValidateUser(userModel.UserName, userModel.Password);
            bool login = false;
            if(!login)
            {
                ApplicationUser user = await _userManager.FindByIdAsync(userModel.UserName);
                if (user != null)
                {
                    IAuthenticationManager authenticationManager = HttpContext.GetOwinContext().Authentication;
                    authenticationManager.SignOut(DefaultAuthenticationTypes.ExternalCookie);

                    ClaimsIdentity identity = _userManager.CreateIdentity(user, DefaultAuthenticationTypes.ApplicationCookie);
                    AuthenticationProperties props = new AuthenticationProperties();
                    props.IsPersistent = userModel.RememberMe;
                    authenticationManager.SignIn(props, identity);
                    //SignInManager.SignIn(user, props.IsPersistent, props.IsPersistent);

                    SetAlert("Đăng nhập thành công.", "success");
                    return Content("<script>window.location.reload()</script>");
                }
                else
                {
                    return Content("User domain này chưa xác nhận với hệ thống, vui lòng xác nhận.");
                }
            }
            else
            {
                return Content("User domain hoặc mật khẩu chưa đúng.");
            }
        }

        [HttpGet]
        [AuthenProfile]
        public ActionResult Detail(string id)
        {
            ApplicationUser user = _userManager.FindById(id);
            var userViewModel = Mapper.Map<ApplicationUser, ApplicationUserViewModel>(user);
            return View(userViewModel);
        }

        [HttpPost]
        public async Task<ActionResult> Update(ApplicationUserViewModel userViewmodel,HttpPostedFileBase Avarta)
        {
            if (ModelState.IsValid)
            {
                //var userByName = await _userManager.FindByNameAsync(userViewmodel.UserName);
                //if (userByName != null)
                //{
                //    ModelState.AddModelError("name", "Domain này đã đăng ký");
                //}
                var appUser = await _userManager.FindByIdAsync(userViewmodel.Id);
                if(Avarta !=null)
                {
                    if (Avarta.ContentLength > 0)
                    {
                        //Kiểm tra định dạng của hình ảnh
                        if (Avarta.ContentType != "image/jpeg" && Avarta.ContentType != "image/png" && Avarta.ContentType != "image/gif" && Avarta.ContentType != "image/jpg")
                        {
                            return null;
                        }
                        else
                        {
                            //Lấy tên hình ảnh
                            var fileName = Path.GetFileName(Avarta.FileName);
                            userViewmodel.Avartar = "/UploadedFiles/avartaUser/" + fileName;
                            //Lấy hình ảnh chuyển vào thư mục cần chứa
                            var localPath = Path.Combine(Server.MapPath("~/UploadedFiles/avartaUser"), fileName);
                            //Nếu thư mục hình ảnh đó đã có thì xuất thông báo
                            Avarta.SaveAs(localPath);
                        }
                    }
                }

                appUser.UpdateUser2(userViewmodel);
                var result = await _userManager.UpdateAsync(appUser);
                if (result.Succeeded)
                {
                    SetAlert("Cập nhật tài khoản thành công", "success");
                }
                else
                {
                    SetAlert("Cập nhật tài khoản không thành công", "error");
                }
                userViewmodel = Mapper.Map<ApplicationUser, ApplicationUserViewModel>(appUser);
                return View("Detail", userViewmodel);
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOut()
        {
            IAuthenticationManager authenticationManager = HttpContext.GetOwinContext().Authentication;
            authenticationManager.SignOut();
            return RedirectToAction("Index", "Home");
        }
    }
}