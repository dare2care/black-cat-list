namespace BlackCatList.Web.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public interface IAddressEntity
    {
        [Index]
        [Required]
        [ForeignKey(nameof(Country))]
        int CountryId { get; set; }

        [Index]
        [ForeignKey(nameof(City))]
        int? CityId { get; set; }

        [Index]
        [ForeignKey(nameof(Street))]
        int? StreetId { get; set; }

        Country Country { get; set; }

        City City { get; set; }

        Street Street { get; set; }
    }
}