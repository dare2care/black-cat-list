namespace BlackCatList.Web.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class City : IIdentityEntity<int>, ICreatedEntity
    {
        [Key]
        public int Id { get; set; }

        [Index]
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [Index]
        [Required]
        [ForeignKey(nameof(Country))]
        public int CountryId { get; set; }

        public string CreatedById { get; set; }

        public DateTime CreatedOn { get; set; }

        public virtual Country Country { get; set; }

        public virtual ICollection<Person> People { get; set; }

        public virtual ICollection<Organization> Organizations { get; set; }

        public virtual User CreatedBy { get; set; }
    }
}