using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BlackCatList.Web.Models
{
    public class UserListViewModel
    {
        public string Id { get; set; }
        public string Email { get; set; }
    }

    public class ManageRolesViewModel
    {
        public string Id { get; set; }
        public string Email { get; set; }

        [Display(Name = "Is Moderator")]
        public bool IsModerator { get; set; }
    }
}