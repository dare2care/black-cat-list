namespace BlackCatList.Web.Controllers
{
    using System;
    using System.Data.Entity;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Models;
    using Models.Home;

    [RequireHttps]
    public class HomeController : Controller
    {
        public HomeController(ApplicationDbContext dbContext)
        {
            this.DbContext = dbContext;
        }

        private ApplicationDbContext DbContext { get; }

        public async Task<ActionResult> Index()
        {
            var viewModel = new HomeViewModel();

            var utcNow = DateTime.UtcNow;

            viewModel.Organizations = await this.DbContext.Organizations
                .Where(x => DbFunctions.DiffMonths(x.CreatedOn, utcNow) <= 1)
                .Select(x => new HomeOrganizationViewModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    ImageId = x.ImageId,
                    Category = x.Category.Name,
                    Country = x.Country.Name,
                    City = x.City.Name,
                    Rating = x.Reviews.Select(y => (double)y.Rating).DefaultIfEmpty().Average()
                })
                .Where(x => x.Rating > 0)
                .OrderByDescending(x => x.Rating)
                .Take(12)
                .ToListAsync();

            return this.View(viewModel);
        }
    }
}