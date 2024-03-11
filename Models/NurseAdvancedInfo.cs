namespace NurseApp.Models
{
    public class NurseAdvancedInfo
    {
        public int Id { get; set; }
        public int NurseId { get; set; }
        public DateTime DateofBirth { get; set; }
        public string Nationality { get; set; }
        public bool IsSubscribed { get; set; }
        public int YearsOfExperience { get; set; }
        public string Bio { get; set; }
        public List<string> Expertise { get; set; }
        // we need identity image as well (either passport or ID)
        public decimal HourlyPrice { get; set; }
        public decimal DailyPrice { get; set; }
        public decimal WeeklyPrice { get; set; }
        
    }
}
