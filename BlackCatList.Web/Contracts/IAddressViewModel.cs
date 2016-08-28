namespace BlackCatList.Web.Models
{
    using System.ComponentModel.DataAnnotations;

    public interface IAddressViewModel : IValidatableObject
    {
        int CountryId { get; set; }

        string Country { get; set; }

        int? CityId { get; set; }

        string City { get; set; }

        int? StreetId { get; set; }

        string Street { get; set; }
    }
}