namespace BlackCatList.Web.Models
{
    using System.ComponentModel.DataAnnotations.Schema;

    public interface IAddressEntity
    {
        Address Address { get; set; }

        [ForeignKey(nameof(Models.Address.StreetId))]
        Street Street { get; set; }
    }
}