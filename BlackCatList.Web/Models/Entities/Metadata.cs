namespace BlackCatList.Web.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [ComplexType]
    public class Metadata
    {
        [Index]
        [Required]
        [MaxLength(128)]
        public string CreatedById { get; set; }

        [Index]
        [Required]
        [MaxLength(128)]
        public string ModifiedById { get; set; }

        [Required]
        public DateTime CreatedOn { get; set; }

        [Required]
        public DateTime ModifiedOn { get; set; }
    }
}