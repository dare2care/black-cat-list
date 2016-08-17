namespace BlackCatList.Web.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Street : IIdentityEntity<int>
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Index]
        [Required]
        [ForeignKey(nameof(District))]
        public int DistrictId { get; set; }

        public virtual District District { get; set; }
    }
}