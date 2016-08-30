namespace BlackCatList.Web.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public interface ICreatedEntity
    {
        [Index]
        [Required]
        [MaxLength(128)]
        [ForeignKey(nameof(CreatedBy))]
        string CreatedById { get; set; }

        [Required]
        DateTime CreatedOn { get; set; }

        User CreatedBy { get; set; }
    }
}