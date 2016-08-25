namespace BlackCatList.Web.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public interface IMetadataEntity
    {
        [Index]
        [Required]
        [MaxLength(128)]
        [ForeignKey(nameof(CreatedBy))]
        string CreatedById { get; set; }

        [Index]
        [Required]
        [MaxLength(128)]
        [ForeignKey(nameof(ModifiedBy))]
        string ModifiedById { get; set; }

        [Required]
        DateTime CreatedOn { get; set; }

        [Required]
        DateTime ModifiedOn { get; set; }

        User CreatedBy { get; set; }

        User ModifiedBy { get; set; }
    }
}