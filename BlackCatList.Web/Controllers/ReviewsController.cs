namespace BlackCatList.Web.Controllers
{
    using System.Net;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Models;

    [Authorize]
    public class ReviewsController : SharedController
    {
        public ReviewsController(ApplicationDbContext dbContext)
        {
            this.DbContext = dbContext;
        }

        private ApplicationDbContext DbContext { get; }

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