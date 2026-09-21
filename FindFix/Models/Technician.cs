namespace FindFix.Models
{
    public class Technician
    {
        public int Id {get; set;}
        public string FullName {get; set;} = string.Empty;
        public string PhoneNumber {get; set;} = string.Empty;
        public string Email {get; set;} = string.Empty;
        public string Service {get; set;} = string.Empty;
        public string Location {get; set;} = string.Empty;
        public string Description {get; set;} = string.Empty;
        public int YearOfExperience {get; set;} 
        public bool IsVerified  {get; set;}
        public string ProfileImageUrl {get; set;} = string.Empty;
        public bool IsAvailable {get; set;}
        public DateTime DateRegistered {get; set; }
        
    }
}