namespace BlackCatList.Web.Models.Home
{
    using System.Collections.Generic;

    public class HomeViewModel
    {
        public IEnumerable<HomeOrganizationViewModel> Organizations { get; set; }
    }
}