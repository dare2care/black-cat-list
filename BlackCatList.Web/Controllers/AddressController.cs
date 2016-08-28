namespace BlackCatList.Web.Controllers
{
    using System.Data.Entity;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Models;

    [Authorize]
    public class AddressController : Controller
    {
        public AddressController(ApplicationDbContext dbContext)
        {
            this.DbContext = dbContext;
        }

        public ApplicationDbContext DbContext { get; }

        [HttpPost]
        public async Task<JsonResult> Countries(string term)
        {
            var countries = this.DbContext.Countries
                .Where(x => x.Name.StartsWith(term))
                .OrderByDescending(x => x.Organizations.Count + x.People.Count)
                .ThenBy(x => x.Name)
                .Select(x => x.Name)
                .Take(10);

            return this.Json(await countries.ToListAsync());
        }

        [HttpPost]
        public async Task<JsonResult> Cities(string term, string country)
        {
            var cities = this.DbContext.Cities
                .Where(x => x.Name.StartsWith(term))
                .Where(x => x.Country.Name == country)
                .OrderByDescending(x => x.Organizations.Count + x.People.Count)
                .OrderBy(x => x.Name)
                .Select(x => x.Name)
                .Take(10);

            return this.Json(await cities.ToListAsync());
        }

        [HttpPost]
        public async Task<JsonResult> Streets(string term, string city)
        {
            var streets = this.DbContext.Streets
                .Where(x => x.Name.StartsWith(term))
                .Where(x => x.City.Name == city)
                .OrderBy(x => x.Name)
                .Select(x => x.Name)
                .Take(20);

            return this.Json(await streets.ToListAsync());
        }
    }
}
