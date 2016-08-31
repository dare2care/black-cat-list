namespace BlackCatList.Web.Controllers
{
    using System.Data.Entity;
    using System.Net;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Microsoft.AspNet.Identity;
    using Models;
    using Models.Organizations;

    [Authorize]
    public class ReviewsController : SharedController
    {
        public ReviewsController(ApplicationDbContext dbContext)
        {
            this.DbContext = dbContext;
        }

        private ApplicationDbContext DbContext { get; }

        // POST: Reviews/Create
        [HttpPost]
        public async Task<ActionResult> Save(ReviewViewModel review)
        {
            if (this.ModelState.IsValid)
            {
                var userId = this.User.Identity.GetUserId();
                var entity = await this.DbContext.OrganizationReviews
                    .FirstOrDefaultAsync(x => x.CreatedById == userId);

                this.DbContext.Entry(review.ToEntity(entity)).State = EntityState.Modified;

                await this.DbContext.SaveChangesAsync();
            }

            return this.RedirectToActionPermanent("Details", "Organizations", new { id = review.OrganizationId });
        }

        // POST: Reviews/Delete/5
        [HttpPost]
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var entity = await this.DbContext.OrganizationReviews.FindAsync(id);
            if (entity == null)
            {
                return this.HttpNotFound();
            }

            if (this.IsNotCreator(entity))
            {
                return new HttpUnauthorizedResult();
            }

            this.DbContext.OrganizationReviews.Remove(entity);

            await this.DbContext.SaveChangesAsync();

            return this.RedirectToRefererPermanent();
        }
    }
}