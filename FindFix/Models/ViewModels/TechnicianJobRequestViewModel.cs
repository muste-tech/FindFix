namespace FindFix.Models.ViewModels;
public class TechnicianJobRequestViewModel
{
    public int Id {get; set;}
    public string CustomerName {get; set;}= string.Empty;
    public string ServiceName {get; set;}= string.Empty;
    public string Description {get; set;}= string.Empty;
    public string Location {get; set;}= string.Empty;
    public string Status {get; set;}= string.Empty;
    public DateTime DateCreated {get; set;}
}