namespace BlackCatList.Web.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Country : IIdentityEntity<int>
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        public virtual ICollection<Person> People { get; set; }

        public virtual ICollection<Organization> Organizations { get; set; }
    }
}