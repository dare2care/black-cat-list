namespace BlackCatList.Web.Models
{
    using System.ComponentModel.DataAnnotations;

    public class Street : IIdentityEntity<int>
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(200)]
        public string Name { get; set; }
    }
}