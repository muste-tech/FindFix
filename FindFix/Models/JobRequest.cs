namespace FindFix.Models;
public class JobRequest
{
    public int Id {get; set;}
    
    public int CustomerId {get; set;}
    public Customer Customer {get; set;}= null!;
    public int TechnicianId{get; set;}
    public Technician Technician {get; set;}= null!;
    public int ServiceId {get; set;}
    public Service Service {get; set;}= null!;
     public Review? Review {get; set;}
    public string Description {get; set;} = string.Empty;
    public string Location {get; set;} = string.Empty;
    public JobStatus Status {get; set;} = JobStatus.pending;
       public DateTime DateCreated {get; set;}


}