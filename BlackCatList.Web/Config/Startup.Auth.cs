namespace BlackCatList.Web
{
    using System;
    using System.Collections.Generic;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using BlackCatList.Web.Models;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.Owin;
    using Microsoft.Owin;
    using Microsoft.Owin.Security.Cookies;
    using Microsoft.Owin.Security.Facebook;
    using Microsoft.Owin.Security.Google;
    using Owin;
    using SimpleFacebookClient;

    public partial class Startup
    {
        // For more information on configuring authentication, please visit http://go.microsoft.com/fwlink/?LinkId=301864
        public void ConfigureAuth(IAppBuilder app)
        {
            // Configure the db context, user manager and signin manager to use a single instance per request
            app.CreatePerOwinContext(ApplicationDbContext.Create);
            app.CreatePerOwinContext<ApplicationUserManager>(ApplicationUserManager.Create);
            app.CreatePerOwinContext<ApplicationSignInManager>(ApplicationSignInManager.Create);

            // Configure the sign in cookie
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Account/Login"),
                Provider = new CookieAuthenticationProvider
                {
                    // Enables the application to validate the security stamp when the user logs in.
                    // This is a security feature which is used when you change a password or add an external login to your account.
                    OnValidateIdentity = SecurityStampValidator.OnValidateIdentity<ApplicationUserManager, User>(
                        validateInterval: TimeSpan.FromMinutes(30),
                        regenerateIdentity: (manager, user) => user.GenerateUserIdentityAsync(manager))
                }
            });
            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);
            app.UseTwoFactorSignInCookie(DefaultAuthenticationTypes.TwoFactorCookie, TimeSpan.FromMinutes(5));
            app.UseTwoFactorRememberBrowserCookie(DefaultAuthenticationTypes.TwoFactorRememberBrowserCookie);

            var facebookOptions = new FacebookAuthenticationOptions
            {
                AppId = Environment.GetEnvironmentVariable("BlackCatList.Auth.Facebook.AppID"),
                AppSecret = Environment.GetEnvironmentVariable("BlackCatList.Auth.Facebook.AppSecret"),
                Scope = { "email" },
                Provider = new FacebookAuthenticationProvider
                {
                    OnAuthenticated = context =>
                    {
                        var client = new FacebookClient(context.AccessToken);
                        var info = client.Get<FacebookLoginData>(
                            "me",
                            new Dictionary<string, string> { ["fields"] = "name,id,email" });

                        context.Identity.AddClaim(new Claim(ClaimTypes.Email, info.Email));

                        return Task.FromResult(0);
                    }
                }
            };
            app.UseFacebookAuthentication(facebookOptions);

            app.UseGoogleAuthentication(new GoogleOAuth2AuthenticationOptions
            {
                ClientId = Environment.GetEnvironmentVariable("BlackCatList.Auth.Google.ClientID"),
                ClientSecret = Environment.GetEnvironmentVariable("BlackCatList.Auth.Google.ClientSecret")
            });
        }
    }
}