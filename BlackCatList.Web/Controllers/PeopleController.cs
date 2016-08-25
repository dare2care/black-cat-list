namespace BlackCatList.Web.Controllers
{
    using System.Data.Entity;
    using System.Linq;
    using System.Net;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Models;
    using Models.People;

    [Authorize(Roles = "Administrator,Moderator")]
    public class PeopleController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: People
        public async Task<ActionResult> Index()
        {
            var people = this.db.People.Include(p => p.Image).Include(p => p.Organization);
            return this.View((await people.ToListAsync()).Select(PersonViewModel.Create));
        }

        // GET: People/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Person person = await this.db.People.FindAsync(id);
            if (person == null)
            {
                return this.HttpNotFound();
            }

            return this.View(PersonViewModel.Create(person));
        }

        // GET: People/Create
        public ActionResult Create()
        {
            this.ViewBag.OrganizationId = new SelectList(this.db.Organizations, "Id", "Name");
            return this.View();
        }

        // POST: People/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(PersonViewModel person)
        {
            if (this.ModelState.IsValid)
            {
                this.db.People.Add(person.ToEntity());
                await this.db.SaveChangesAsync();
                return this.RedirectToAction("Index");
            }

            this.ViewBag.OrganizationId = new SelectList(this.db.Organizations, "Id", "Name", person.OrganizationId);
            return this.View(person);
        }

        // GET: People/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Person person = await this.db.People.FindAsync(id);
            if (person == null)
            {
                return this.HttpNotFound();
            }

            this.ViewBag.ImageId = new SelectList(this.db.Images, "Id", "Id", person.ImageId);
            this.ViewBag.OrganizationId = new SelectList(this.db.Organizations, "Id", "Name", person.OrganizationId);
            return this.View(PersonViewModel.Create(person));
        }

        // POST: People/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(PersonViewModel person)
        {
            if (this.ModelState.IsValid)
            {
                this.db.Entry(person.ToEntity()).State = EntityState.Modified;
                await this.db.SaveChangesAsync();
                return this.RedirectToAction("Index");
            }

            this.ViewBag.OrganizationId = new SelectList(this.db.Organizations, "Id", "Name", person.OrganizationId);
            return this.View(person);
        }

        // GET: People/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Person person = await this.db.People.FindAsync(id);
            if (person == null)
            {
                return this.HttpNotFound();
            }

            return this.View(PersonViewModel.Create(person));
        }

        // POST: People/Delete/5
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Person person = await this.db.People.FindAsync(id);
            this.db.People.Remove(person);
            await this.db.SaveChangesAsync();
            return this.RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.db.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}
