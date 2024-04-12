namespace NurseApp.Models
{
    public class NurseDetails
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string? ProfilePicture { get; set; }
        public string DateOfBirth { get; set; }
        public string Nationality { get; set; }
        public int YearsOfExperience { get; set; }
        public string Bio { get; set; }
        public string Expertise { get; set; }
        public decimal HourlyPrice { get; set; }
        public decimal DailyPrice { get; set; }
        public string? AvailableDates { get; set; }
        public decimal WeeklyPrice { get; set; }
        public string Location { get; set; }
    }

}
