namespace BlackCatList.Web.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class City : IIdentityEntity<int>
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [Index]
        [Required]
        [ForeignKey(nameof(Country))]
        public int CountryId { get; set; }

        public virtual Country Country { get; set; }

        public virtual ICollection<Person> People { get; set; }

        public virtual ICollection<Organization> Organizations { get; set; }
    }
}