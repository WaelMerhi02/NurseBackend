using System.ComponentModel.DataAnnotations;

namespace NurseApp.Models
{
    public class NurseAdvancedInfo
    {
        public int Id { get; set; } 
        public int NurseId { get; set; } //got from shared prference
        public string DateofBirth { get; set; } // dd-mm-yyyy
        public string Nationality { get; set; } //dropdownlist
        public string Location { get; set; } //dropdownlist multiselection
        public bool? IsSubscribed { get; set; } 
        public string? SubscriptionDate { get; set; }
        public int YearsOfExperience { get; set; }
        public string Bio { get; set; }
        public string Expertise { get; set; } //multiple selection
        public string? passportimage { get; set; } //send it to me as base64 string
        public string? IdFrontImage { get; set; } //send it to me as base64 string
        public string? IdBackImage { get; set; } //send it to me as base64 string
        public decimal HourlyPrice { get; set; }
        public decimal DailyPrice { get; set; }
        public decimal WeeklyPrice { get; set; }
        
    }
}
