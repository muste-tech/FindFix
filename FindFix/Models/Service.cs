namespace FindFix.Models;
public class Service
{
    public int Id {get; set;}
    public string Name {get; set;}= string.Empty;
    public string Description {get; set;} = string.Empty;
    public string ImageUrl {get; set;} = string.Empty;
    
    public List<Technician>  Technicians{get; set;} = new();

}