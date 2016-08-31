namespace BlackCatList.Web.Models
{
    using System.Data.Entity;
    using Microsoft.AspNet.Identity.EntityFramework;

    public class ApplicationDbContextInitializer : DropCreateDatabaseIfModelChanges<ApplicationDbContext>
    {
        protected override void Seed(ApplicationDbContext context)
        {
            context.Roles.Add(new IdentityRole("Administrator"));
            context.Roles.Add(new IdentityRole("Moderator"));

            context.Categories.Add(new Category { Name = "Vet. Clinic" });
            context.Categories.Add(new Category { Name = "Breeder" });
            context.Categories.Add(new Category { Name = "Shop" });

            context.Countries.Add(new Country { Name = "Bulgaria" });
        }
    }
}