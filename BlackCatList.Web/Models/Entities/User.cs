namespace BlackCatList.Web.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;

    public class User : IdentityUser, IIdentityEntity<string>
    {
        [Required]
        public string DisplayName { get; set; }

        [Index]
        [ForeignKey(nameof(Organization))]
        public int? OrganizationId { get; set; }

        public virtual Organization Organization { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<User> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);

            userIdentity.AddClaim(new Claim(nameof(this.DisplayName), this.DisplayName));

            // Add custom user claims here
            return userIdentity;
        }
    }
}