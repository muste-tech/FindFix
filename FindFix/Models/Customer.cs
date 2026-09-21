namespace FindFix.Models
{
    public class Customer
    {
        public int Id {get; set;}
        public string FullName{get; set;}= string.Empty;
        public string PhoneNumber {get; set;} = string.Empty;
        public string Email {get; set;} = string.Empty;
        public string Location {get; set;} = string.Empty;
        public DateTime DateRegistered {get; set;}
        public IFormFile? ProfilePicture{get; set;} 

    }
}