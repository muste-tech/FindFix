namespace FindFix.Models
{
    public class Customer
    {
        public int Id {get; set;}
        public string FullName{get; set;}= string.Empty;
        public string PhoneNumber {get; set;} = string.Empty;
        public string Email {get; set;} = string.Empty;
        public string Location {get; set;} = string.Empty;
        public string ProfileImageUrl {get; set;} = string.Empty;
        public DateTime DateRegistered {get; set;}
        public List<JobRequest> JobRequests {get; set;} = new();
        public List <Review> Reviews {get; set;} = new();
        
    

    }
}