namespace NurseApp.Models
{
    public class Users
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int RoleId { get; set; } //0 is Admin, 1 is User, 2 is Nurse,3 is for Patient
        public string Email { get; set; }
        public string Password { get; set; }
        //public byte[]? IdentityImage { get; set; }
        public string PhoneNumber { get; set; }
        public string Username { get; set; }
        //public bool? IsSubscribed { get; set; }
        //public int? YearsOfExperience { get; set; }
        //public string? Bio { get; set; }
        //public string? Expertise { get; set; }

    }
}

