namespace BlackCatList.Web.Models
{
    using System.ComponentModel.DataAnnotations;

    public class ExternalLoginConfirmationViewModel
    {
        [Required]
        [Display(Name = "Username")]
        public string UserName { get; set; }

        [Display(Name = "Email")]
        public string Email { get; set; }
    }
}