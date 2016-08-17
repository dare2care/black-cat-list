namespace BlackCatList.Web.Models
{
    using System.ComponentModel.DataAnnotations;

    public interface IIdentityEntity<out TKey>
    {
        [Key]
        TKey Id { get; }
    }
}