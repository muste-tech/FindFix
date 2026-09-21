namespace FindFix.Models;
public class JobRequest
{
    public int Id {get; set;}
    
    public int CustomerId {get; set;}
    public int TechnicianId{get; set;}
    public string Description {get; set;} = string.Empty;
    public string Location {get; set;} = string.Empty;
    public JobStatus Status {get; set;} = JobStatus.pending;
       public DateTime DateCreated {get; set;}


}