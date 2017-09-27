using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;
using AccountantNew.Data;
using static AccountantNew.Web.App_Start.ApplicationUserStore;
using Microsoft.AspNet.Identity;
using AccountantNew.Model.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System.Collections.Generic;
using Microsoft.Owin.Security.OAuth;
using Microsoft.Owin.Security.Cookies;
using AutoMapper;
using AccountantNew.Web.Infastructure.Core;
using AccountantNew.Service;
using System.Linq;
using AccountantNew.Common;
using System.Security.Claims;
using Newtonsoft.Json;
using AccountantNew.Web.Models;

[assembly: OwinStartup(typeof(AccountantNew.Web.App_Start.Startup))]

namespace AccountantNew.Web.App_Start
{
    public partial class Startup
    {
        public void ConfigureAuth(IAppBuilder app)
        {
            //UserManagerFactory = () => new UserManager<IdentityUser>(new UserStore<IdentityUser>(new BungBungShopDbContext()));
            // Configure the db context, user manager and signin manager to use a single instance per request
            app.CreatePerOwinContext(AccountantNewDbContext.Create);
            app.CreatePerOwinContext<ApplicationUserManager>(ApplicationUserManager.Create);
            app.CreatePerOwinContext<ApplicationSignInManager>(ApplicationSignInManager.Create);

            // Enable the application to use a cookie to store information for the signed in user
            // and to use a cookie to temporarily store information about a user logging in with a third party login provider

            //chúng ta tạo ra OwinContext(nó là 1 middleware quản lý usermanager, giúp ta giảm sự phụ thuộc giữa server và application, là 1 cơ chế riêng không phu thuoc vào server)
            app.CreatePerOwinContext<UserManager<ApplicationUser>>(CreateManager);

            app.UseOAuthAuthorizationServer(new OAuthAuthorizationServerOptions
            {
                TokenEndpointPath = new PathString("/oauth/token"),
                Provider = new AuthorizationServerProvider(),
                AccessTokenExpireTimeSpan = TimeSpan.FromMinutes(30),
                AllowInsecureHttp = true
            });

            //app.UseOAuthBearerTokens(new OAuthAuthorizationServerOptions
            //{
            //    TokenEndpointPath = new PathString("/oauth/token"),
            //    Provider = new AuthorizationServerProvider(),
            //    AccessTokenExpireTimeSpan = TimeSpan.FromMinutes(30),
            //    AllowInsecureHttp = true
            //});
            app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());

            // Configure the sign in cookie
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                //LoginPath = new PathString("/dang-nhap"),
                Provider = new CookieAuthenticationProvider
                {
                    // Enables the application to validate the security stamp when the user logs in.
                    // This is a security feature which is used when you change a password or add an external login to your account.
                    OnValidateIdentity = SecurityStampValidator.OnValidateIdentity<ApplicationUserManager, ApplicationUser>(
                        validateInterval: TimeSpan.FromMinutes(30),
                        regenerateIdentity: (manager, user) => user.GenerateUserIdentityAsync(manager))
                }
            });
            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

            //// Enables the application to temporarily store user information when they are verifying the second factor in the two-factor authentication process.
            //app.UseTwoFactorSignInCookie(DefaultAuthenticationTypes.TwoFactorCookie, TimeSpan.FromMinutes(5));

            //// Enables the application to remember the second login verification factor such as phone or email.
            //// Once you check this option, your second step of verification during the login process will be remembered on the device where you logged in from.
            //// This is similar to the RememberMe option when you log in.
            //app.UseTwoFactorRememberBrowserCookie(DefaultAuthenticationTypes.TwoFactorRememberBrowserCookie);

            // Uncomment the following lines to enable logging in with third party login providers
            //app.UseMicrosoftAccountAuthentication(
            //    clientId: "",
            //    clientSecret: "");

            //app.UseTwitterAuthentication(
            //   consumerKey: "",
            //   consumerSecret: "");

            //app.UseFacebookAuthentication(
            //   appId: "",
            //   appSecret: "");

            //app.UseGoogleAuthentication(new GoogleOAuth2AuthenticationOptions()
            //{
            //    ClientId = "",
            //    ClientSecret = ""
            //});
        }

        public class AuthorizationServerProvider : OAuthAuthorizationServerProvider
        {
            //khi mà đăng nhập thì sẽ gửi lên server 1 cái request và nó sẽ validate tất cả các request
            //validate token
            public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
            {
                if (context.ClientId == null)
                {
                    context.Validated();
                }
                return Task.FromResult<object>(null);
            }

            //khi mà đăng nhập xong sẽ gán nguồn ResourceOwnerCredentials
            //gán token

            public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
            {
                var allowedOrigin = context.OwinContext.Get<string>("as:clientAllowedOrigin");

                if (allowedOrigin == null) allowedOrigin = "*";

                context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { allowedOrigin });

                //tạo 1 lớp UserManager để tương tác với UserManager trong identity
                UserManager<ApplicationUser> userManager = context.OwinContext.GetUserManager<UserManager<ApplicationUser>>();
                ApplicationUser user;
                try
                {
                    user = await userManager.FindByNameAsync(context.UserName);
                }
                catch
                {
                    // Could not retrieve the user due to error.
                    context.SetError("server_error");
                    context.Rejected();
                    return;
                }
                //khi login thành công thì tạo ra ClaimsIdentity(cục thông tin user để gán vào session)
                if (user != null)
                {
                    var applicationGroupService = ServiceFactory.Get<IApplicationGroupService>();
                    var listGroup = applicationGroupService.GetListGroupByUserId(user.Id);

                    var listGroupViewModel = Mapper.Map<IEnumerable<ApplicationGroup>, IEnumerable<ApplicationGroupViewModel>>(listGroup);
                    foreach (var item in listGroupViewModel)
                    {
                        var listRole = ServiceFactory.Get<IApplicationRoleService>().GetListRoleByGroupId(item.ID);
                        item.Roles = Mapper.Map<IEnumerable<ApplicationRole>, IEnumerable<ApplicationRoleViewModel>>(listRole);
                    }

                    ////var roles = userManager.GetRoles(user.Id);
                    if (listGroupViewModel.Count(x => x.Name.Contains(CommonConstants.Admin)) > 0)
                    {
                        ClaimsIdentity identity = await userManager.CreateIdentityAsync(user,
                            context.Options.AuthenticationType);
                        identity.AddClaim(new Claim("groups", JsonConvert.SerializeObject(listGroupViewModel)));
                        //identity.AddClaim(new Claim("roles", JsonConvert.SerializeObject(roles)));
                        AuthenticationProperties properties = CreateProperties(user.Avartar, JsonConvert.SerializeObject(listGroupViewModel)/*, JsonConvert.SerializeObject(roles)*/);
                        AuthenticationTicket ticket = new AuthenticationTicket(identity, properties);
                        context.Validated(ticket);
                    }
                    else
                    {
                        context.Rejected();
                        context.SetError("invalid_group", "Bạn không phải là admin.");
                    }

                    //ClaimsIdentity identity = await userManager.CreateIdentityAsync(user,
                    //        context.Options.AuthenticationType);
                    //AuthenticationProperties properties = CreateProperties(user.Avartar);
                    //AuthenticationTicket ticket = new AuthenticationTicket(identity, properties);
                    //context.Validated(ticket);
                }
                else
                {
                    context.Rejected();
                    context.SetError("invalid_grant", "Tài khoản hoặc mật khẩu không đúng.");
                }
            }

            public override Task TokenEndpoint(OAuthTokenEndpointContext context)
            {
                foreach (KeyValuePair<string, string> property in context.Properties.Dictionary)
                {
                    context.AdditionalResponseParameters.Add(property.Key, property.Value);
                }

                return Task.FromResult<object>(null);
            }

            //public override Task ValidateClientRedirectUri(OAuthValidateClientRedirectUriContext context)
            //{

            //        Uri expectedRootUri = new Uri(context.Request.Uri, "/");

            //        if (expectedRootUri.AbsoluteUri == context.RedirectUri)
            //        {
            //            context.Validated();
            //        }
            //    return Task.FromResult<object>(null);
            //}

        }

        private static UserManager<ApplicationUser> CreateManager(IdentityFactoryOptions<UserManager<ApplicationUser>> options, IOwinContext context)
        {
            var userStore = new UserStore<ApplicationUser>(context.Get<AccountantNewDbContext>());
            var owinManager = new UserManager<ApplicationUser>(userStore);
            return owinManager;
        }

        public static AuthenticationProperties CreateProperties(string avarta, string listGroup)
        {
            IDictionary<string, string> data = new Dictionary<string, string>
            {
                { "avarta",avarta},
                { "groups",listGroup}
            };
            return new AuthenticationProperties(data);
        }
    }
}
