namespace BlackCatList.Web.Controllers
{
    using System.Web.Mvc;
    using Microsoft.AspNet.Identity;
    using Models;

    public abstract class SharedController : Controller
    {
        protected ActionResult RedirectToRefererPermanent()
        {
            if (this.Request.UrlReferrer != null)
            {
                return this.RedirectPermanent(this.Request.UrlReferrer.ToString());
            }

            return this.RedirectToActionPermanent("Index", "Home");
        }

        protected virtual bool IsNotCreator(ICreatedEntity entity)
        {
            return entity.CreatedById != this.User.Identity.GetUserId();
        }
    }
}