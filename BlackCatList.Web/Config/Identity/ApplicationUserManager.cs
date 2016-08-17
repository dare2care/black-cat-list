namespace BlackCatList.Web
{
    using System;
    using BlackCatList.Web.Models;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Microsoft.AspNet.Identity.Owin;
    using Microsoft.Owin;

    // Configure the application user manager used in this application.
    // UserManager is defined in ASP.NET Identity and is used by the application.
    public class ApplicationUserManager : UserManager<User>
    {
        public ApplicationUserManager(IUserStore<User> store)
            : base(store)
        {
        }

        public static ApplicationUserManager Create(IdentityFactoryOptions<ApplicationUserManager> options, IOwinContext context)
        {
            var dbContext = context.Get<ApplicationDbContext>();
            var manager = new ApplicationUserManager(new UserStore<User>(dbContext));

            // Configure validation logic for usernames
            manager.UserValidator = new UserValidator<User>(manager)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = true
            };

            // Configure validation logic for passwords
            manager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 6,
                RequireNonLetterOrDigit = true,
                RequireDigit = true
            };

            // Configure user lockout defaults
            manager.UserLockoutEnabledByDefault = true;
            manager.DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(5);
            manager.MaxFailedAccessAttemptsBeforeLockout = 5;

            // Configure email two-factor provider
            var emailTokenProvider = new EmailTokenProvider<User>
            {
                Subject = "Security Code",
                BodyFormat = "Your security code is {0}"
            };
            manager.RegisterTwoFactorProvider("Email Code", emailTokenProvider);
            manager.EmailService = new EmailService();

            // Configure data protection provider
            var dataProtectionProvider = options.DataProtectionProvider;
            if (dataProtectionProvider != null)
            {
                manager.UserTokenProvider =
                    new DataProtectorTokenProvider<User>(dataProtectionProvider.Create("ASP.NET Identity"));
            }

            // Add default application roles
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(dbContext));
            AddDefaultRole(roleManager, "Administrator");
            AddDefaultRole(roleManager, "Moderator");

            return manager;
        }

        private static void AddDefaultRole(RoleManager<IdentityRole> roleManager, string roleName)
        {
            if (!roleManager.RoleExists(roleName))
            {
                roleManager.Create(new IdentityRole(roleName));
            }
        }
    }
}