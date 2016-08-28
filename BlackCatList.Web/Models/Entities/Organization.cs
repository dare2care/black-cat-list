namespace BlackCatList.Web.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Organization : IIdentityEntity<int>, IAddressEntity, IMetadataEntity, IImageEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [Phone]
        [MaxLength(50)]
        public string Phone { get; set; }

        [EmailAddress]
        [MaxLength(100)]
        public string Email { get; set; }

        public int? ImageId { get; set; }

        public int CountryId { get; set; }

        public int? CityId { get; set; }

        public int? StreetId { get; set; }

        public string CreatedById { get; set; }

        public string ModifiedById { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime ModifiedOn { get; set; }

        [Index]
        [Required]
        [ForeignKey(nameof(Category))]
        public int CategoryId { get; set; }

        [Required]
        public string Description { get; set; }

        public virtual Country Country { get; set; }

        public virtual City City { get; set; }

        public virtual Street Street { get; set; }

        public virtual User CreatedBy { get; set; }

        public virtual User ModifiedBy { get; set; }

        public virtual Image Image { get; set; }

        public virtual Category Category { get; set; }
    }
}