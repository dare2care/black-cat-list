namespace BlackCatList.Web.Controllers
{
    using System.Web.Mvc;

    [RequireHttps]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return this.View();
        }
    }
}