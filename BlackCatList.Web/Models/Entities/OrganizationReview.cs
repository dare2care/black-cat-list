namespace BlackCatList.Web.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class OrganizationReview : IIdentityEntity<int>, ICreatedEntity
    {
        [Key]
        public int Id { get; set; }

        [Index]
        [ForeignKey(nameof(Organization))]
        public int OrganizationId { get; set; }

        [Range(1, 5)]
        public byte Rating { get; set; }

        public string Comment { get; set; }

        public string CreatedById { get; set; }

        public DateTime CreatedOn { get; set; }

        public virtual Organization Organization { get; set; }

        public virtual User CreatedBy { get; set; }
    }
}