namespace FindFix.Models
{
    public class Customer
    {
        public int Id {get; set;}
        public string Location {get; set;} = string.Empty;
        public string ProfileImageUrl {get; set;} = string.Empty;
        public DateTime DateRegistered {get; set;}

        public string UserId {get; set;} =string.Empty;
        public ApplicationUser User {get; set;} = null!;
        
        public List<JobRequest> JobRequests {get; set;} = new();
        public List <Review> Reviews {get; set;} = new();
        
    

    }
}