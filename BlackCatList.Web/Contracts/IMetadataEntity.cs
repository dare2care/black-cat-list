namespace BlackCatList.Web.Models
{
    using System.ComponentModel.DataAnnotations.Schema;

    public interface IMetadataEntity
    {
        Metadata Metadata { get; set; }

        [ForeignKey(nameof(Models.Metadata.CreatedById))]
        User CreatedBy { get; set; }

        [ForeignKey(nameof(Models.Metadata.ModifiedById))]
        User ModifiedBy { get; set; }
    }
}