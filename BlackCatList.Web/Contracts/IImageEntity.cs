namespace BlackCatList.Web.Models
{
    using System.ComponentModel.DataAnnotations.Schema;

    public interface IImageEntity
    {
        [Index]
        [ForeignKey(nameof(Image))]
        int? ImageId { get; set; }

        Image Image { get; set; }
    }
}
