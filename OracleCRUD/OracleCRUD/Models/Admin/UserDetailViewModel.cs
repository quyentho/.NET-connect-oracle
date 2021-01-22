using System.ComponentModel.DataAnnotations;

namespace OracleCRUD.Models.Admin
{
    public class UserDetailViewModel
    {
        public double Id { get; set; }
        public string Username { get; set; }
        public string FirstName { get; set; }

        public string LastName { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        public string Phone { get; set; }

        public string Quota { get; set; }

        public string AccountStatus { get; set; }

        public string TableSpaceName { get; set; }


        public string ProfileName { get; set; }
    }
}
