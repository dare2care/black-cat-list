namespace BlackCatList.Web.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class District : IIdentityEntity<int>
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Index]
        [Required]
        [ForeignKey(nameof(Town))]
        public int TownId { get; set; }

        public virtual City Town { get; set; }
    }
}