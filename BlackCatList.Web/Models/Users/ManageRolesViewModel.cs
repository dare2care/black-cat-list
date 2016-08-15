namespace BlackCatList.Web.Models
{
    using System.ComponentModel.DataAnnotations;

    public class ManageRolesViewModel
    {
        public string Id { get; set; }

        public string Email { get; set; }

        [Display(Name = "Is Moderator")]
        public bool IsModerator { get; set; }
    }
}