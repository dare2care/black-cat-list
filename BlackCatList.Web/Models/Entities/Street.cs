﻿namespace BlackCatList.Web.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Street : IIdentityEntity<int>, ICreatedEntity
    {
        [Key]
        public int Id { get; set; }

        [Index]
        [Required]
        [MaxLength(200)]
        public string Name { get; set; }

        [Index]
        [Required]
        [ForeignKey(nameof(City))]
        public int CityId { get; set; }

        public string CreatedById { get; set; }

        public DateTime CreatedOn { get; set; }

        public virtual City City { get; set; }

        public virtual User CreatedBy { get; set; }
    }
}