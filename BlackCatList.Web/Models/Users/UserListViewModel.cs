namespace BlackCatList.Web.Models
{
    using System.ComponentModel.DataAnnotations;

    public class UserListViewModel
    {
        public string Id { get; set; }

        public string Email { get; set; }

        [Display(Name = "Username")]
        public string UserName { get; set; }
    }
}