namespace BlackCatList.Web.Models
{
    using System.ComponentModel.DataAnnotations;

    public class Image : IIdentityEntity<int>
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public byte[] Content { get; set; }
    }
}