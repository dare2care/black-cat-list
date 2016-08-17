namespace BlackCatList.Web.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Town : IIdentityEntity<int>
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Index]
        [Required]
        [ForeignKey(nameof(Country))]
        public int CountryId { get; set; }

        public virtual Country Country { get; set; }
    }
}