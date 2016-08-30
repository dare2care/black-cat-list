namespace BlackCatList.Web.Models.Organizations
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class ReviewViewModel
    {
        public int Id { get; set; }

        public int? AvatarId { get; set; }

        [Range(1, 5)]
        public byte Rating { get; set; }

        [MaxLength(1000)]
        public string Comment { get; set; }

        [Display(Name = "Created By")]
        public string CreatedBy { get; set; }

        [Display(Name = "Created On")]
        public DateTime CreatedOn { get; set; }

        public static ReviewViewModel Create(OrganizationReview review)
        {
            return new ReviewViewModel
            {
                Id = review.Id,
                Rating = review.Rating,
                Comment = review.Comment,
                AvatarId = review.CreatedBy.ImageId,
                CreatedBy = review.CreatedBy.UserName,
                CreatedOn = review.CreatedOn
            };
        }

        public OrganizationReview ToEntity(OrganizationReview entity = null)
        {
            entity = entity ?? new OrganizationReview();

            entity.Id = this.Id;
            entity.Rating = this.Rating;
            entity.Comment = this.Comment;

            return entity;
        }
    }
}