namespace BlackCatList.Web.Models
{
    using System.ComponentModel.DataAnnotations.Schema;

    public interface IAddressEntity
    {
        Address Address { get; set; }

        [ForeignKey(nameof(Models.Address.CountryId))]
        Country Country { get; set; }

        [ForeignKey(nameof(Models.Address.CityId))]
        City City { get; set; }

        [ForeignKey(nameof(Models.Address.DistrictId))]
        District District { get; set; }

        [ForeignKey(nameof(Models.Address.StreetId))]
        Street Street { get; set; }
    }
}