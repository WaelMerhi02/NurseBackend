namespace NurseApp.Models
{
    public class Users
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int RoleId { get; set; } 
        public string Email { get; set; }
        public string Password { get; set; }
        public bool IsBanned { get; set; }
        public string PhoneNumber { get; set; }
        public string Username { get; set; }
        public string? ProfilePicture { get; set; }

      

    }
}

