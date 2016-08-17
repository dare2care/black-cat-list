﻿namespace BlackCatList.Web.Models
{
    using System.ComponentModel.DataAnnotations;

    public class Country : IIdentityEntity<int>
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
    }
}