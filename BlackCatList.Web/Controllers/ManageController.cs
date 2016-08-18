namespace BlackCatList.Web.Controllers
{
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using BlackCatList.Web.Models;
    using Microsoft.AspNet.Identity;
    using Microsoft.Owin.Security;

    [Authorize]
    public class ManageController : Controller
    {
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        public ManageController(
            ApplicationUserManager userManager,
            ApplicationSignInManager signInManager,
            IAuthenticationManager authenticationManager)
        {
            this.UserManager = userManager;
            this.SignInManager = signInManager;
            this.AuthenticationManager = authenticationManager;
        }

        private ApplicationUserManager UserManager { get; }

        private ApplicationSignInManager SignInManager { get; }

        private IAuthenticationManager AuthenticationManager { get; }

        // GET: /Manage/ChangePassword
        public ActionResult ChangePassword()
        {
            return this.View();
        }

        // POST: /Manage/ChangePassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(model);
            }

            var result = await this.UserManager.ChangePasswordAsync(this.User.Identity.GetUserId(), model.OldPassword, model.NewPassword);
            if (result.Succeeded)
            {
                var user = await this.UserManager.FindByIdAsync(this.User.Identity.GetUserId());
                if (user != null)
                {
                    await this.SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                }

                return this.RedirectToAction(nameof(this.Index), new { Message = ManageMessageId.ChangePasswordSuccess });
            }

            this.AddErrors(result);
            return this.View(model);
        }

        // POST: /Manage/DisableTwoFactorAuthentication
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DisableTwoFactorAuthentication()
        {
            await this.UserManager.SetTwoFactorEnabledAsync(this.User.Identity.GetUserId(), false);
            var user = await this.UserManager.FindByIdAsync(this.User.Identity.GetUserId());
            if (user != null)
            {
                await this.SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
            }

            return this.RedirectToAction(nameof(this.Index), "Manage");
        }

        // POST: /Manage/EnableTwoFactorAuthentication
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EnableTwoFactorAuthentication()
        {
            await this.UserManager.SetTwoFactorEnabledAsync(this.User.Identity.GetUserId(), true);
            var user = await this.UserManager.FindByIdAsync(this.User.Identity.GetUserId());
            if (user != null)
            {
                await this.SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
            }

            return this.RedirectToAction(nameof(this.Index), "Manage");
        }

        // GET: /Manage/Index
        public async Task<ActionResult> Index(ManageMessageId? message)
        {
            this.ViewBag.StatusMessage =
                message == ManageMessageId.ChangePasswordSuccess ? "Your password has been changed."
                : message == ManageMessageId.SetPasswordSuccess ? "Your password has been set."
                : message == ManageMessageId.SetTwoFactorSuccess ? "Your two-factor authentication provider has been set."
                : message == ManageMessageId.Error ? "An error has occurred."
                : string.Empty;

            var userId = this.User.Identity.GetUserId();
            var model = new IndexViewModel
            {
                HasPassword = this.HasPassword(),
                TwoFactor = await this.UserManager.GetTwoFactorEnabledAsync(userId),
                Logins = await this.UserManager.GetLoginsAsync(userId),
                BrowserRemembered = await this.AuthenticationManager.TwoFactorBrowserRememberedAsync(userId)
            };
            return this.View(model);
        }

        // POST: /Manage/LinkLogin
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LinkLogin(string provider)
        {
            // Request a redirect to the external login provider to link a login for the current user
            return new ChallengeResult(
                provider,
                this.Url.Action(nameof(this.LinkLoginCallback), "Manage"),
                this.User.Identity.GetUserId(),
                this.AuthenticationManager);
        }

        // GET: /Manage/LinkLoginCallback
        public async Task<ActionResult> LinkLoginCallback()
        {
            var loginInfo = await this.AuthenticationManager.GetExternalLoginInfoAsync(XsrfKey, this.User.Identity.GetUserId());
            if (loginInfo == null)
            {
                return this.RedirectToAction(nameof(this.ManageLogins), new { Message = ManageMessageId.Error });
            }

            var result = await this.UserManager.AddLoginAsync(this.User.Identity.GetUserId(), loginInfo.Login);
            return result.Succeeded ? this.RedirectToAction(nameof(this.ManageLogins)) : this.RedirectToAction(nameof(this.ManageLogins), new { Message = ManageMessageId.Error });
        }

        // GET: /Manage/ManageLogins
        public async Task<ActionResult> ManageLogins(ManageMessageId? message)
        {
            this.ViewBag.StatusMessage =
                message == ManageMessageId.RemoveLoginSuccess ? "The external login was removed."
                : message == ManageMessageId.Error ? "An error has occurred."
                : string.Empty;
            var user = await this.UserManager.FindByIdAsync(this.User.Identity.GetUserId());
            if (user == null)
            {
                return this.View("Error");
            }

            var userLogins = await this.UserManager.GetLoginsAsync(this.User.Identity.GetUserId());
            var otherLogins = this.AuthenticationManager.GetExternalAuthenticationTypes().Where(auth => userLogins.All(ul => auth.AuthenticationType != ul.LoginProvider)).ToList();
            this.ViewBag.ShowRemoveButton = user.PasswordHash != null || userLogins.Count > 1;
            return this.View(new ManageLoginsViewModel
            {
                CurrentLogins = userLogins,
                OtherLogins = otherLogins
            });
        }

        // POST: /Manage/RemoveLogin
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RemoveLogin(string loginProvider, string providerKey)
        {
            ManageMessageId? message;
            var result = await this.UserManager.RemoveLoginAsync(this.User.Identity.GetUserId(), new UserLoginInfo(loginProvider, providerKey));
            if (result.Succeeded)
            {
                var user = await this.UserManager.FindByIdAsync(this.User.Identity.GetUserId());
                if (user != null)
                {
                    await this.SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                }

                message = ManageMessageId.RemoveLoginSuccess;
            }
            else
            {
                message = ManageMessageId.Error;
            }

            return this.RedirectToAction(nameof(this.ManageLogins), new { Message = message });
        }

        // GET: /Manage/SetPassword
        public ActionResult SetPassword()
        {
            return this.View();
        }

        // POST: /Manage/SetPassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SetPassword(SetPasswordViewModel model)
        {
            if (this.ModelState.IsValid)
            {
                var result = await this.UserManager.AddPasswordAsync(this.User.Identity.GetUserId(), model.NewPassword);
                if (result.Succeeded)
                {
                    var user = await this.UserManager.FindByIdAsync(this.User.Identity.GetUserId());
                    if (user != null)
                    {
                        await this.SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                    }

                    return this.RedirectToAction(nameof(this.Index), new { Message = ManageMessageId.SetPasswordSuccess });
                }

                this.AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            return this.View(model);
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                this.ModelState.AddModelError(string.Empty, error);
            }
        }

        private bool HasPassword()
        {
            var user = this.UserManager.FindById(this.User.Identity.GetUserId());
            if (user != null)
            {
                return user.PasswordHash != null;
            }

            return false;
        }
    }
}