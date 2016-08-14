using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BlackCatList.Web.Models;
using Microsoft.AspNet.Identity.Owin;

namespace BlackCatList.Web.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class UsersController : Controller
    {
        private ApplicationUserManager _userManager;

        public UsersController()
        {
        }

        public UsersController(ApplicationUserManager userManager)
        {
            UserManager = userManager;
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        // GET: Users
        public async Task<ActionResult> Index()
        {
            return View(await UserManager.Users.Select(x => new UserListViewModel
            {
                Id = x.Id,
                Email = x.Email
            }).ToListAsync());
        }

        // GET: Users/ManageRoles/5
        public async Task<ActionResult> ManageRoles(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var user = await UserManager.FindByIdAsync(id);
            if (user == null)
            {
                return HttpNotFound();
            }

            return View(new ManageRolesViewModel
            {
                Id = user.Id,
                Email = user.Email,
                IsModerator = await UserManager.IsInRoleAsync(id, "Moderator")
            });
        }

        // POST: Users/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ManageRoles(ManageRolesViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.IsModerator)
                {
                    await UserManager.AddToRoleAsync(model.Id, "Moderator");
                }
                else
                {
                    await UserManager.RemoveFromRoleAsync(model.Id, "Moderator");
                }

                return RedirectToAction("Index");
            }
            return View(model);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && _userManager != null)
            {
                _userManager.Dispose();
                _userManager = null;
            }

            base.Dispose(disposing);
        }
    }
}