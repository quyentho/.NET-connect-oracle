using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace OracleCRUD.Models.Admin
{
    public class UserEditViewModel
    {

        [Required]
        public string Username { get; set; }
        public string FirstName { get; set; }

        [DisplayName("Password")]
        public string PasswordHash { get; set; }

        public string LastName { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        public string Phone { get; set; }

        public string Quota { get; set; }

        public List<SelectListItem> TableSpaceList { get; set; }

        [Required]
        public string TableSpaceSelected { get; set; }

        [Required]
        public string ProfileSelected { get; set; }

        public List<SelectListItem> ProfileList { get; set; }
    }
}
