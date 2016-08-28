namespace BlackCatList.Web.Controllers
{
    using System.Net;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using BlackCatList.Web.Models;

    public class ImagesController : Controller
    {
        public ImagesController(ApplicationDbContext dbContext)
        {
            this.DbContext = dbContext;
        }

        public ApplicationDbContext DbContext { get; }

        // GET: Image/5
        public async Task<ActionResult> Index(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var entity = await this.DbContext.Images.FindAsync(id);
            if (entity == null)
            {
                return this.HttpNotFound();
            }

            return this.File(entity.Content, entity.ContentType, entity.Name);
        }

        // GET: Image/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var entity = await this.DbContext.Images.FindAsync(id);
            if (entity == null)
            {
                return this.HttpNotFound();
            }

            this.DbContext.Images.Remove(entity);
            await this.DbContext.SaveChangesAsync();

            if (this.Request.UrlReferrer != null)
            {
                return this.RedirectPermanent(this.Request.UrlReferrer.ToString());
            }

            return this.RedirectToActionPermanent("Index", "Home");
        }
    }
}
