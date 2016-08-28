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
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public virtual DbSet<Person> People { get; set; }

        public virtual DbSet<Country> Countries { get; set; }

        public virtual DbSet<City> Cities { get; set; }

        public virtual DbSet<Street> Streets { get; set; }

        public virtual DbSet<Image> Images { get; set; }

        public virtual DbSet<Organization> Organizations { get; set; }

        public virtual DbSet<Category> Categories { get; set; }

        private IEnumerable<IMetadataEntity> AddedMetadataEntities =>
            this.ChangeTracker.Entries<IMetadataEntity>()
                .Where(p => p.State == EntityState.Added)
                .Select(p => p.Entity);

        private IEnumerable<IMetadataEntity> ModifiedMetadataEntities =>
            this.ChangeTracker.Entries<IMetadataEntity>()
                .Where(p => p.State == EntityState.Modified)
                .Select(p => p.Entity);

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

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
            var userIdentity = HttpContext.Current?.User?.Identity;
            var utcNow = DateTime.UtcNow;

            foreach (var added in this.AddedMetadataEntities)
            {
                added.CreatedById = userIdentity.GetUserId();
                added.ModifiedById = userIdentity.GetUserId();
                added.CreatedOn = utcNow;
                added.ModifiedOn = utcNow;
            }

            foreach (var updated in this.ModifiedMetadataEntities)
            {
                this.Entry(updated).Property(x => x.CreatedOn).IsModified = false;
                this.Entry(updated).Property(x => x.CreatedById).IsModified = false;

                updated.ModifiedById = userIdentity.GetUserId();
                updated.ModifiedOn = utcNow;
            }
        }
    }
}