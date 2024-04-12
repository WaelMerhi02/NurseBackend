namespace NurseApp.Models
{
    public class AuthenticationResult
    {
        public bool IsLoggedIn { get; set; } 
        public int UserId { get; set; }
        public int RoleId { get; set; }
        public bool IsBanned { get; set; } 
    }
}
