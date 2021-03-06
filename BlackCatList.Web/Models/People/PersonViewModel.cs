﻿namespace BlackCatList.Web.Models.People
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Web;

    public class PersonViewModel : IAddressViewModel, IImageViewModel
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

        public byte? Age { get; set; }

        public int? ImageId { get; set; }

        public HttpPostedFileBase Photo { get; set; }

        [Required]
        public string Country { get; set; }

        public string City { get; set; }

        public string Street { get; set; }

        [Required]
        [DataType(DataType.MultilineText)]
        public string Comment { get; set; }

        public int CountryId { get; set; }

        public int? CityId { get; set; }

        public int? StreetId { get; set; }

        [Display(Name = "Created By")]
        public string CreatedBy { get; set; }

        [Display(Name = "Modified By")]
        public string ModifiedBy { get; set; }

        [Display(Name = "Created On")]
        public DateTime CreatedOn { get; set; }

        [Display(Name = "Modified On")]
        public DateTime ModifiedOn { get; set; }

        public static PersonViewModel Create(Person person)
        {
            return new PersonViewModel
            {
                Id = person.Id,
                Name = person.Name,
                Phone = person.Phone,
                Email = person.Email,
                Age = person.Age,
                ImageId = person.ImageId,
                Comment = person.Comment,
                Country = person.Country?.Name,
                City = person.City?.Name,
                Street = person.Street?.Name,
                CreatedBy = person.CreatedBy.UserName,
                CreatedOn = person.CreatedOn,
                ModifiedBy = person.ModifiedBy.UserName,
                ModifiedOn = person.ModifiedOn
            };
        }

        public Person ToEntity(Person entity = null)
        {
            entity = entity ?? new Person();

            entity.Id = this.Id;
            entity.Name = this.Name;
            entity.Phone = this.Phone;
            entity.Email = this.Email;
            entity.Age = this.Age;
            entity.CountryId = this.CountryId;
            entity.CityId = this.CityId;
            entity.StreetId = this.StreetId;
            entity.Comment = this.Comment;

            entity.SaveImage(this.Photo);

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