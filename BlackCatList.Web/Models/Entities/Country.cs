namespace BlackCatList.Web.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Country : IIdentityEntity<int>
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        [Index(IsUnique = true)]
        public string Name { get; set; }

        public virtual ICollection<Person> People { get; set; }

        public virtual ICollection<Organization> Organizations { get; set; }
    }
}