namespace FindFix.Models
{
    public class Technician
    {
        public int Id {get; set;}
        public string Location {get; set;} = string.Empty;
       // Technician short bio e. 2 years phone fixing experience 
        public string Description {get; set;} = string.Empty;
        public int YearsOfExperience {get; set;} 
        public bool IsVerified  {get; set;}
        public bool IsAvailable {get; set;}
         public string ProfileImageUrl {get; set;} = string.Empty;
         public double AverageRating {get; set;}
         public int ReviewCount {get; set;}
         public DateTime DateRegistered {get; set; }

        public string UserId {get; set;} =string.Empty;
        public ApplicationUser User {get; set;} = null!;

      
        public List<Service> Services {get; set;} = new();
        public List<Qualification> Qualifications {get; set;} = new();
        public List<JobRequest> JobRequests {get; set;} = new();
        public List<Review> Reviews {get; set;} = new();

        
    }
}