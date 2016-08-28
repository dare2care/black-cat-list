namespace BlackCatList.Web.Controllers
{
    using System.Data.Entity;
    using System.Linq;
    using System.Net;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Models;
    using Models.Organizations;

    [Authorize(Roles = "Administrator,Moderator")]
    public class OrganizationsController : Controller
    {
        public OrganizationsController(ApplicationDbContext dbContext)
        {
            this.DbContext = dbContext;
        }

        private ApplicationDbContext DbContext { get; }

        // GET: Organizations
        public async Task<ActionResult> Index()
        {
            var organizations = this.DbContext.Organizations.Include(o => o.City).Include(o => o.Country).Include(o => o.CreatedBy).Include(o => o.Image).Include(o => o.ModifiedBy).Include(o => o.Street);
            return this.View((await organizations.ToListAsync()).Select(OrganizationsViewModel.Create));
        }

        // GET: Organizations/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Organization organization = await this.DbContext.Organizations.FindAsync(id);
            if (organization == null)
            {
                return this.HttpNotFound();
            }

            return this.View(OrganizationsViewModel.Create(organization));
        }

        // GET: Organizations/Create
        public ActionResult Create()
        {
            this.ViewBag.CategoryId = new SelectList(this.DbContext.Categories, "Id", "Name");

            return this.View();
        }

        // POST: Organizations/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(OrganizationsViewModel organization)
        {
            if (this.ModelState.IsValid)
            {
                this.DbContext.Organizations.Add(organization.ToEntity());
                await this.DbContext.SaveChangesAsync();
                return this.RedirectToAction("Index");
            }

            this.ViewBag.CategoryId = new SelectList(this.DbContext.Categories, "Id", "Name", organization.CategoryId);

            return this.View(organization);
        }

        // GET: Organizations/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Organization organization = await this.DbContext.Organizations.FindAsync(id);
            if (organization == null)
            {
                return this.HttpNotFound();
            }

            this.ViewBag.CategoryId = new SelectList(this.DbContext.Categories, "Id", "Name", organization.CategoryId);

            return this.View(OrganizationsViewModel.Create(organization));
        }

        // POST: Organizations/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(OrganizationsViewModel organization)
        {
            if (this.ModelState.IsValid)
            {
                this.DbContext.Entry(organization.ToEntity()).State = EntityState.Modified;
                await this.DbContext.SaveChangesAsync();
                return this.RedirectToAction("Index");
            }

            this.ViewBag.CategoryId = new SelectList(this.DbContext.Categories, "Id", "Name", organization.CategoryId);

            return this.View(organization);
        }

        // GET: Organizations/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Organization organization = await this.DbContext.Organizations.FindAsync(id);
            if (organization == null)
            {
                return this.HttpNotFound();
            }

            return this.View(OrganizationsViewModel.Create(organization));
        }

        // POST: Organizations/Delete/5
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Organization organization = await this.DbContext.Organizations.FindAsync(id);
            this.DbContext.Organizations.Remove(organization);
            await this.DbContext.SaveChangesAsync();
            return this.RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.DbContext.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}
