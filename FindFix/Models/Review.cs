namespace FindFix.Models;
public class Review
{
    public int Id {get; set;}
    public int Rating {get; set;}
    public string  Comment {get; set;} = string.Empty;
    public DateTime DateCreated {get; set;}
    public int JobRequestId {get; set;}
    public JobRequest JobRequest {get; set;} = null!;
    public int CustomerId {get; set;}
    public Customer Customer {get; set;} = null!;
    public int TechnicianId {get; set;}
    public Technician Technician {get; set;} = null!;


}