namespace BlackCatList.Web.Controllers
{
    using System.Data;
    using System.Data.Entity;
    using System.Linq;
    using System.Net;
    using System.Threading.Tasks;
    using System.Web;
    using System.Web.Mvc;
    using BlackCatList.Web.Models;
    using Microsoft.AspNet.Identity.Owin;

    [Authorize(Roles = "Administrator")]
    public class UsersController : Controller
    {
        private ApplicationUserManager userManager;

        public UsersController()
        {
        }

        public UsersController(ApplicationUserManager userManager)
        {
            this.UserManager = userManager;
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return this.userManager ?? this.HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }

            private set
            {
                this.userManager = value;
            }
        }

        // GET: Users
        public async Task<ActionResult> Index(UserListViewModel model)
        {
            var users = this.UserManager.Users.Select(x => new UserListViewModel
            {
                Id = x.Id,
                Email = x.Email
            });

            if (model.Email != null)
            {
                users = users.Where(x => x.Email.Contains(model.Email));
            }

            return this.View(await users.ToListAsync());
        }

        // GET: Users/ManageRoles/5
        public async Task<ActionResult> ManageRoles(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var user = await this.UserManager.FindByIdAsync(id);
            if (user == null)
            {
                return this.HttpNotFound();
            }

            return this.View(new ManageRolesViewModel
            {
                Id = user.Id,
                Email = user.Email,
                IsModerator = await this.UserManager.IsInRoleAsync(id, "Moderator")
            });
        }

        // POST: Users/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ManageRoles(ManageRolesViewModel model)
        {
            if (this.ModelState.IsValid)
            {
                if (model.IsModerator)
                {
                    await this.UserManager.AddToRoleAsync(model.Id, "Moderator");
                }
                else
                {
                    await this.UserManager.RemoveFromRoleAsync(model.Id, "Moderator");
                }

                return this.RedirectToAction(nameof(this.Index));
            }

            return this.View(model);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && this.userManager != null)
            {
                this.userManager.Dispose();
                this.userManager = null;
            }

            base.Dispose(disposing);
        }
    }
}