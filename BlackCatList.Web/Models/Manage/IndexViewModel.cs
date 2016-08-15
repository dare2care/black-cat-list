namespace BlackCatList.Web.Models
{
    using System.Collections.Generic;
    using Microsoft.AspNet.Identity;

    public class IndexViewModel
    {
        public bool HasPassword { get; set; }

        public IList<UserLoginInfo> Logins { get; set; }

        public bool TwoFactor { get; set; }

        public bool BrowserRemembered { get; set; }
    }
}