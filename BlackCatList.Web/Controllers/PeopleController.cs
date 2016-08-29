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
        public PeopleController(ApplicationDbContext dbContext, AddressMapper addressMapper)
        {
            this.DbContext = dbContext;
            this.AddressMapper = addressMapper;
        }

        private ApplicationDbContext DbContext { get; }

        private AddressMapper AddressMapper { get; }

        // GET: People
        public async Task<ActionResult> Index()
        {
            var people = this.DbContext.People.Include(p => p.Image).Include(p => p.Organization);
            return this.View((await people.ToListAsync()).Select(PersonViewModel.Create));
        }

        // GET: People/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var entity = await this.DbContext.People.FindAsync(id);
            if (entity == null)
            {
                return this.HttpNotFound();
            }

            return this.View(PersonViewModel.Create(entity));
        }

        // GET: People/Create
        public ActionResult Create()
        {
            return this.View(new PersonViewModel
            {
                Organizations = new SelectList(this.DbContext.Organizations, "Id", "Name")
            });
        }

        // POST: People/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(PersonViewModel person)
        {
            if (this.ModelState.IsValid)
            {
                await this.AddressMapper.MapAsync(person);

                this.DbContext.People.Add(person.ToEntity());

                await this.DbContext.SaveChangesAsync();

                return this.RedirectToAction("Index");
            }

            person.Organizations = new SelectList(this.DbContext.Organizations, "Id", "Name", person.OrganizationId);

            return this.View(person);
        }

        // GET: People/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var entity = await this.DbContext.People.FindAsync(id);
            if (entity == null)
            {
                return this.HttpNotFound();
            }

            var person = PersonViewModel.Create(entity);

            person.Organizations = new SelectList(this.DbContext.Organizations, "Id", "Name", entity.OrganizationId);

            return this.View(person);
        }

        // POST: People/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(PersonViewModel person)
        {
            if (this.ModelState.IsValid)
            {
                var entity = await this.DbContext.People.FindAsync(person.Id);

                await this.AddressMapper.MapAsync(person);
                if (person.Photo != null && entity.Image != null)
                {
                    this.DbContext.Images.Remove(entity.Image);
                }

                this.DbContext.Entry(person.ToEntity(entity)).State = EntityState.Modified;

                await this.DbContext.SaveChangesAsync();

                return this.RedirectToAction("Index");
            }

            person.Organizations = new SelectList(this.DbContext.Organizations, "Id", "Name", person.OrganizationId);

            return this.View(person);
        }

        // GET: People/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var entity = await this.DbContext.People.FindAsync(id);
            if (entity == null)
            {
                return this.HttpNotFound();
            }

            var person = PersonViewModel.Create(entity);

            person.Organizations = new SelectList(this.DbContext.Organizations, "Id", "Name", person.OrganizationId);

            return this.View(person);
        }

        // POST: People/Delete/5
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            var entity = await this.DbContext.People.FindAsync(id);
            this.DbContext.People.Remove(entity);
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
