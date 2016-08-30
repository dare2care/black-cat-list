namespace BlackCatList.Web.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public interface IModifiedEntity : ICreatedEntity
    {
        [Index]
        [Required]
        [MaxLength(128)]
        [ForeignKey(nameof(ModifiedBy))]
        string ModifiedById { get; set; }

        [Required]
        DateTime ModifiedOn { get; set; }

        User ModifiedBy { get; set; }
    }
}