namespace BlackCatList.Web.Models
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Web;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;

    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public ApplicationDbContext(HttpContextBase httpContext)
            : base("DefaultConnection", throwIfV1Schema: false)
        {
            this.UserId = httpContext.User?.Identity.GetUserId();
        }

        public virtual DbSet<Person> People { get; set; }

        public virtual DbSet<Country> Countries { get; set; }

        public virtual DbSet<City> Cities { get; set; }

        public virtual DbSet<Street> Streets { get; set; }

        public virtual DbSet<Image> Images { get; set; }

        public virtual DbSet<Organization> Organizations { get; set; }

        public virtual DbSet<OrganizationReview> OrganizationReviews { get; set; }

        public virtual DbSet<Category> Categories { get; set; }

        private IEnumerable<ICreatedEntity> AddedMetadataEntities =>
            this.ChangeTracker.Entries<ICreatedEntity>()
                .Where(p => p.State == EntityState.Added)
                .Select(p => p.Entity);

        private IEnumerable<IModifiedEntity> ModifiedMetadataEntities =>
            this.ChangeTracker.Entries<IModifiedEntity>()
                .Where(p => p.State == EntityState.Modified)
                .Select(p => p.Entity);

        private string UserId { get; }

        public override int SaveChanges()
        {
            this.UpdateMetadata();

            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            this.UpdateMetadata();

            return base.SaveChangesAsync(cancellationToken);
        }

        private void UpdateMetadata()
        {
            var utcNow = DateTime.UtcNow;

            foreach (var added in this.AddedMetadataEntities)
            {
                added.CreatedById = this.UserId;
                added.CreatedOn = utcNow;

                var modified = added as IModifiedEntity;
                if (modified != null)
                {
                    modified.ModifiedById = this.UserId;
                    modified.ModifiedOn = utcNow;
                }
            }

            foreach (var modified in this.ModifiedMetadataEntities)
            {
                this.Entry(modified).Property(x => x.CreatedOn).IsModified = false;
                this.Entry(modified).Property(x => x.CreatedById).IsModified = false;

                modified.ModifiedById = this.UserId;
                modified.ModifiedOn = utcNow;
            }
        }
    }
}