namespace FindFix.Models;
public class JobRequest
{
    public int id {get; set;}
    public int CustomerId {get; set;}
    public int TechnicianId{get; set;}
    public string Description {get; set;} = string.Empty;
    public string Location {get; set;} = string.Empty;
    public string Status {get; set;} = string.Empty;
    public DateTime DateCreated {get; set;}


}