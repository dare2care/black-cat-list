﻿namespace BlackCatList.Web.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [ComplexType]
    public class Address
    {
        [Index]
        [ForeignKey(nameof(Street))]
        public int? StreetId { get; set; }

        [Range(0, int.MaxValue)]
        public int? Number { get; set; }
    }
}