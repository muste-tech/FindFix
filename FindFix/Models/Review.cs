namespace FindFix.Models;
public class Review
{
    public int Id {get; set;}
    public int CustomerId {get; set;}
    public int TechnicianId {get; set;}
    public int Rating {get; set;}
    public string Comment {get;set;} = string.Empty;
    public DateTime DateCreated {get; set;}

}