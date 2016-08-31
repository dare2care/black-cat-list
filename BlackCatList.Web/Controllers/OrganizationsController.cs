namespace BlackCatList.Web.Controllers
{
    using System.Data.Entity;
    using System.Linq;
    using System.Net;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Microsoft.AspNet.Identity;
    using Models;
    using Models.Organizations;

    [Authorize]
    public class OrganizationsController : SharedController
    {
        public OrganizationsController(ApplicationDbContext dbContext, AddressMapper addressMapper)
        {
            this.DbContext = dbContext;
            this.AddressMapper = addressMapper;
        }

        private ApplicationDbContext DbContext { get; }

        private AddressMapper AddressMapper { get; }

        // GET: Organizations
        [Authorize(Roles = "Administrator,Moderator")]
        public async Task<ActionResult> Index()
        {
            return this.View((await this.DbContext.Organizations.ToListAsync()).Select(OrganizationViewModel.Create));
        }

        // GET: Organizations/Details/5
        [AllowAnonymous]
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var entity = await this.DbContext.Organizations.FindAsync(id);
            if (entity == null)
            {
                return this.HttpNotFound();
            }

            return this.View(OrganizationViewModel.Create(entity));
        }

        // GET: Organizations/Details/5
        public async Task<ActionResult> Review(int? id)
        {
            var detailsResult = await this.Details(id);

            var detailsViewResult = detailsResult as ViewResult;
            if (detailsViewResult != null)
            {
                detailsViewResult.ViewName = nameof(this.Details);

                this.ViewBag.ExpandReview = true;
            }

            return detailsResult;
        }

        // GET: Organizations/Create
        public ActionResult Create()
        {
            return this.View(new OrganizationViewModel
            {
                Categories = new SelectList(this.DbContext.Categories, "Id", "Name")
            });
        }

        // POST: Organizations/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(OrganizationViewModel organization)
        {
            if (this.ModelState.IsValid)
            {
                await this.AddressMapper.MapAsync(organization);

                var entity = organization.ToEntity();
                this.DbContext.Organizations.Add(entity);

                await this.DbContext.SaveChangesAsync();

                return this.RedirectToAuthorizedAction(entity.Id);
            }

            organization.Categories = new SelectList(this.DbContext.Categories, "Id", "Name", organization.CategoryId);

            return this.View(organization);
        }

        // GET: Organizations/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var entity = await this.DbContext.Organizations.FindAsync(id);
            if (entity == null)
            {
                return this.HttpNotFound();
            }

            if (this.IsNotAuthorizedCreator(entity))
            {
                return new HttpUnauthorizedResult();
            }

            var organization = OrganizationViewModel.Create(entity);

            organization.Categories = new SelectList(this.DbContext.Categories, "Id", "Name", organization.CategoryId);

            return this.View(organization);
        }

        // POST: Organizations/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(OrganizationViewModel organization)
        {
            if (this.ModelState.IsValid)
            {
                var entity = await this.DbContext.Organizations.FindAsync(organization.Id);
                if (this.IsNotAuthorizedCreator(entity))
                {
                    return new HttpUnauthorizedResult();
                }

                await this.AddressMapper.MapAsync(organization);
                if (organization.Image != null && entity.Image != null)
                {
                    this.DbContext.Images.Remove(entity.Image);
                }

                this.DbContext.Entry(organization.ToEntity(entity)).State = EntityState.Modified;
                await this.DbContext.SaveChangesAsync();

                return this.RedirectToAuthorizedAction(organization.Id);
            }

            organization.Categories = new SelectList(this.DbContext.Categories, "Id", "Name", organization.CategoryId);

            return this.View(organization);
        }

        // GET: Organizations/Delete/5
        [Authorize(Roles = "Administrator,Moderator")]
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var entity = await this.DbContext.Organizations.FindAsync(id);
            if (entity == null)
            {
                return this.HttpNotFound();
            }

            return this.View(OrganizationViewModel.Create(entity));
        }

        // POST: Organizations/Delete/5
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator,Moderator")]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            var entity = await this.DbContext.Organizations.FindAsync(id);
            this.DbContext.Organizations.Remove(entity);
            await this.DbContext.SaveChangesAsync();
            return this.RedirectToAuthorizedAction(id);
        }

        // POST: Reviews/Create
        [HttpPost]
        public async Task<ActionResult> SaveReview(ReviewViewModel review)
        {
            if (this.ModelState.IsValid)
            {
                var userId = this.User.Identity.GetUserId();
                var entity = await this.DbContext.OrganizationReviews
                    .Where(x => x.OrganizationId == review.OrganizationId)
                    .FirstOrDefaultAsync(x => x.CreatedById == userId);

                if (entity != null)
                {
                    this.DbContext.OrganizationReviews.Remove(entity);
                }

                this.DbContext.OrganizationReviews.Add(review.ToEntity());

                await this.DbContext.SaveChangesAsync();

                return this.RedirectToActionPermanent(nameof(this.Details), new { id = review.OrganizationId });
            }

            return await this.Review(review.OrganizationId);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.DbContext.Dispose();
            }

            base.Dispose(disposing);
        }

        private ActionResult RedirectToAuthorizedAction(int id)
        {
            if (this.IsUser())
            {
                return this.RedirectToAction("Details", new { id });
            }

            return this.RedirectToAction("Index");
        }

        private bool IsNotAuthorizedCreator(Organization entity)
        {
            return this.IsUser() && this.IsNotCreator(entity);
        }

        private bool IsUser()
        {
            return !this.User.IsInRole("Administrator") && !this.User.IsInRole("Moderator");
        }
    }
}
