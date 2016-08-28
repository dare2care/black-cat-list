namespace BlackCatList.Web.Models.Organizations
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class OrganizationsViewModel
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

        public byte[] Photo { get; set; }

        public string Country { get; set; }

        public string City { get; set; }

        public string Street { get; set; }

        [Required]
        public string Comment { get; set; }

        public int? CountryId { get; set; }

        public int? CityId { get; set; }

        public int? StreetId { get; set; }

        public string CreatedBy { get; set; }

        public string ModifiedBy { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime ModifiedOn { get; set; }

        public string Category { get; set; }

        [Required]
        public int CategoryId { get; set; }

        public static OrganizationsViewModel Create(Organization organization)
        {
            return new OrganizationsViewModel
            {
                Id = organization.Id,
                Name = organization.Name,
                Phone = organization.Phone,
                Email = organization.Email,
                Photo = organization.Image?.Content,
                Comment = organization.Comment,
                CountryId = organization.CountryId,
                Country = organization.Country?.Name,
                CityId = organization.CityId,
                City = organization.City?.Name,
                Street = organization.Street?.Name,
                StreetId = organization.StreetId,
                Category = organization.Category.Name,
                CreatedBy = organization.CreatedBy.UserName,
                CreatedOn = organization.CreatedOn,
                ModifiedBy = organization.ModifiedBy.UserName,
                ModifiedOn = organization.ModifiedOn
            };
        }

        public Organization ToEntity()
        {
            return new Organization
            {
                Id = this.Id,
                Name = this.Name,
                Phone = this.Phone,
                Email = this.Email,
                // Image = new Image { Content = this.Photo },
                Comment = this.Comment,
                CategoryId = this.CategoryId
            };
        }
    }
}