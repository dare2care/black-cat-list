namespace BlackCatList.Web.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [ComplexType]
    public class Address
    {
        [Index]
        public int? CountryId { get; set; }

        [Index]
        public int? DistrictId { get; set; }

        [Index]
        public int? CityId { get; set; }

        [Index]
        public int? StreetId { get; set; }

        [Range(0, int.MaxValue)]
        public int? Number { get; set; }
    }
}