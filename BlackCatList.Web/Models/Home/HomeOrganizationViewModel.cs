namespace BlackCatList.Web.Models.Home
{
    public class HomeOrganizationViewModel : IImageViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int? ImageId { get; set; }

        public double Rating { get; set; }

        public string Category { get; set; }

        public string Country { get; set; }

        public string City { get; set; }
    }
}