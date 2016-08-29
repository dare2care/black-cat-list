namespace BlackCatList.Web.Controllers
{
    using System.Data;
    using System.Data.Entity;
    using System.Linq;
    using System.Net;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using BlackCatList.Web.Models;

    [Authorize(Roles = "Administrator")]
    public class UsersController : Controller
    {
        public UsersController(ApplicationUserManager userManager)
        {
            this.UserManager = userManager;
        }

        private ApplicationUserManager UserManager { get; }

        // GET: Users
        public async Task<ActionResult> Index()
        {
            var users = this.UserManager.Users
                .Where(x => x.EmailConfirmed)
                .Select(x => new UserListViewModel
                {
                    Id = x.Id,
                    Email = x.Email,
                    UserName = x.UserName
                });

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
    }
}