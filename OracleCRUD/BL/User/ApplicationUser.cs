namespace BL.User
{
    public class ApplicationUser
    {
        public double Id { get; set; }

        public string Username { get; set; }
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }
        public string PasswordHash { get; set; }

        public string Quota { get; set; }

        public int AccountStatus { get; set; }

        public string TableSpaceName { get; set; }

        public string ProfileName { get; set; }

    }
}
