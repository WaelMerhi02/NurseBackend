namespace NurseApp.Models
{
    public class NurseBasicInfo
    {
        public int NurseId { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Expertise { get; set; }
        public string Location { get; set; }
        public bool IsBanned { get; set; }
        public string? Profile { get; set; } // Assuming profile is a string
        
    }
}
