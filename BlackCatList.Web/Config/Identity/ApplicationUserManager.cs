namespace BlackCatList.Web
{
    using System;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Microsoft.AspNet.Identity.Owin;
    using Microsoft.Owin;
    using Microsoft.Owin.Security.DataProtection;
    using Models;

    // Configure the application user manager used in this application.
    // UserManager is defined in ASP.NET Identity and is used by the application.
    public class ApplicationUserManager : UserManager<User>
    {
        public ApplicationUserManager(
            IOwinContext context,
            ApplicationDbContext dbContext,
            IDataProtectionProvider dataProtectionProvider)
            : base(new UserStore<User>(dbContext))
        {
            this.Context = context;
            this.DbContext = dbContext;
            this.DataProtectionProvider = dataProtectionProvider;

            this.Configure();
        }

        private IOwinContext Context { get; }

        private IDataProtectionProvider DataProtectionProvider { get; }

        private ApplicationDbContext DbContext { get; }

        private void Configure()
        {
            // Configure validation logic for usernames
            this.UserValidator = new UserValidator<User>(this)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = true
            };

            // Configure validation logic for passwords
            this.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 6,
                RequireNonLetterOrDigit = true,
                RequireDigit = true
            };

            // Configure user lockout defaults
            this.UserLockoutEnabledByDefault = true;
            this.DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(5);
            this.MaxFailedAccessAttemptsBeforeLockout = 5;

            // Configure email two-factor provider
            var emailTokenProvider = new EmailTokenProvider<User>
            {
                Subject = "Security Code",
                BodyFormat = "Your security code is {0}"
            };
            this.RegisterTwoFactorProvider("Email Code", emailTokenProvider);
            this.EmailService = new EmailService();

            // Configure data protection provider
            if (this.DataProtectionProvider != null)
            {
                this.UserTokenProvider = new DataProtectorTokenProvider<User>(
                    this.DataProtectionProvider.Create("ASP.NET Identity"));
            }
        }
    }
}