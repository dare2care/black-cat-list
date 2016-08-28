namespace BlackCatList.Web.Models
{
    using System.ComponentModel.DataAnnotations;

    public class Category : IIdentityEntity<int>
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
    }
}