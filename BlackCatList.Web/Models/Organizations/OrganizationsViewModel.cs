namespace BlackCatList.Web.Models.Organizations
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;

    public class OrganizationsViewModel : IAddressViewModel, IImageViewModel
    {
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

        public HttpPostedFileBase Image { get; set; }

        [Required]
        public string Country { get; set; }

        public string City { get; set; }

        public string Street { get; set; }

        [Required]
        [MaxLength(10000)]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        public int CountryId { get; set; }

        public int? CityId { get; set; }

        public int? StreetId { get; set; }

        public double Rating { get; set; }

        public int ReviewsCount { get; set; }

        public ICollection<ReviewViewModel> Reviews { get; set; }

        [Display(Name = "Created By")]
        public string CreatedBy { get; set; }

        [Display(Name = "Created On")]
        public DateTime CreatedOn { get; set; }

        [Display(Name = "Modified By")]
        public string ModifiedBy { get; set; }

        [Display(Name = "Modified On")]
        public DateTime ModifiedOn { get; set; }

        public string Category { get; set; }

        public SelectList Categories { get; set; }

        [Required]
        [Display(Name = "Category")]
        public int CategoryId { get; set; }

        public static OrganizationsViewModel Create(Organization organization)
        {
            return new OrganizationsViewModel
            {
                Id = organization.Id,
                Name = organization.Name,
                Phone = organization.Phone,
                Email = organization.Email,
                ImageId = organization.ImageId,
                Description = organization.Description,
                Country = organization.Country?.Name,
                City = organization.City?.Name,
                Street = organization.Street?.Name,
                Category = organization.Category.Name,
                CategoryId = organization.CategoryId,
                CreatedBy = organization.CreatedBy.UserName,
                CreatedOn = organization.CreatedOn,
                ModifiedBy = organization.ModifiedBy.UserName,
                ModifiedOn = organization.ModifiedOn,
                ReviewsCount = organization.Reviews.Count,
                Rating = organization.Reviews.Select(x => (double)x.Rating).DefaultIfEmpty().Average(),
                Reviews = organization.Reviews.Select(ReviewViewModel.Create).ToList()
            };
        }

        public Organization ToEntity(Organization entity = null)
        {
            entity = entity ?? new Organization();

            entity.Id = this.Id;
            entity.Name = this.Name;
            entity.Phone = this.Phone;
            entity.Email = this.Email;
            entity.CountryId = this.CountryId;
            entity.CityId = this.CityId;
            entity.StreetId = this.StreetId;
            entity.Description = this.Description;
            entity.CategoryId = this.CategoryId;

            entity.SaveImage(this.Image);

            return entity;
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (!string.IsNullOrWhiteSpace(this.Street) && string.IsNullOrWhiteSpace(this.City))
            {
                yield return new ValidationResult("The City field is required.", new[] { nameof(this.City) });
            }
        }
    }
}