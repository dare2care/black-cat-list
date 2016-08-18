namespace BlackCatList.Web.Models
{
    using System.Web.Mvc;
    using Microsoft.Owin.Security;

    public class ChallengeResult : HttpUnauthorizedResult
    {
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        public ChallengeResult(string provider, string redirectUri, IAuthenticationManager authenticationManager)
            : this(provider, redirectUri, null, authenticationManager)
        {
        }

        public ChallengeResult(string provider, string redirectUri, string userId, IAuthenticationManager authenticationManager)
        {
            this.LoginProvider = provider;
            this.RedirectUri = redirectUri;
            this.UserId = userId;
            this.AuthenticationManager = authenticationManager;
        }

        private string LoginProvider { get; }

        private string RedirectUri { get; }

        private string UserId { get; }

        private IAuthenticationManager AuthenticationManager { get; }

        public override void ExecuteResult(ControllerContext context)
        {
            var properties = new AuthenticationProperties { RedirectUri = this.RedirectUri };
            if (this.UserId != null)
            {
                properties.Dictionary[XsrfKey] = this.UserId;
            }

            this.AuthenticationManager.Challenge(properties, this.LoginProvider);
        }
    }
}