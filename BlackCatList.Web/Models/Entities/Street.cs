namespace BlackCatList.Web.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Street : IIdentityEntity<int>
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(200)]
        public string Name { get; set; }

        [Index]
        [Required]
        [ForeignKey(nameof(City))]
        public int CityId { get; set; }

        public virtual City City { get; set; }
    }
}