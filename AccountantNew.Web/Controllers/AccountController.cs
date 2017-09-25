using AccountantNew.Model.Models;
using AccountantNew.Web.Infastructure.Core;
using AccountantNew.Web.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
                ApplicationUser user = await _userManager.FindByNameAsync(userModel.UserName);
                if (user != null)
                {
                    IAuthenticationManager authenticationManager = HttpContext.GetOwinContext().Authentication;
                    authenticationManager.SignOut(DefaultAuthenticationTypes.ExternalCookie);

                    ClaimsIdentity identity = _userManager.CreateIdentity(user, DefaultAuthenticationTypes.ApplicationCookie);
                    AuthenticationProperties props = new AuthenticationProperties();
                    props.IsPersistent = userModel.RememberMe;
                    authenticationManager.SignIn(props, identity);

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

        [HttpPost]
        public async Task<ActionResult> Register(RegisterViewModel model,HttpPostedFileBase Image)
        {
            if (ModelState.IsValid)
            {
                var userByEmail = await _userManager.FindByEmailAsync(model.Email);
                if (userByEmail != null)
                {
                    return Content("Email này đã được đăng ký");
                }
                var userByName = await _userManager.FindByNameAsync(model.UserName);
                if (userByName != null)
                {
                    return Content("User domain này đã được đăng ký");
                }
                var user = new ApplicationUser()
                {
                    UserName = model.UserName,
                    Email = model.Email,
                    BirthDay = DateTime.Now,
                    EmailConfirmed = true,
                    FullName = model.FullName,
                    PhoneNumber = model.PhoneNumber,
                    Avartar = model.Image

                };

                if (Image.ContentLength > 0)
                {
                    //Kiểm tra định dạng của hình ảnh
                    if (Image.ContentType != "image/jpeg" && Image.ContentType != "image/png" && Image.ContentType != "image/gif" && Image.ContentType != "image/jpg")
                    {
                        return null;
                    }
                    else
                    {
                        //Lấy tên hình ảnh
                        var fileName = Path.GetFileName(Image.FileName);
                        user.Avartar = "/UploadedFiles/avartaUser/" + fileName;
                        //Lấy hình ảnh chuyển vào thư mục cần chứa
                        var localPath = Path.Combine(Server.MapPath("~/UploadedFiles/avartaUser"), fileName);
                        //Nếu thư mục hình ảnh đó đã có thì xuất thông báo
                        Image.SaveAs(localPath);
                    }
                }

                var result = await _userManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    return Content("Xác nhận tài khoản thành công.");
                }
            }
            else
            {
                return Content("Vui lòng cung cấp đủ thông tin");
            }
            return null;
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