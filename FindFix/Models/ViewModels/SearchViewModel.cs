using Microsoft.Win32.SafeHandles;

namespace FindFix.Models.ViewModels;
public class SearchViewModel
{
    public string? ServiceQuery {get; set;}
    public string? LocationQuery {get; set;}
    public List<TechnicianCardViewModel> Results {get; set;} = new();
}
public class TechnicianCardViewModel
{
    public int Id {get; set;}
    public string FullName {get; set;} = string.Empty;
    public string Description {get; set;}= string.Empty;
    public string Location {get; set;} = string.Empty;
    public int YearsOfExperience {get; set;}
    public double AverageRating {get; set;}
    public int ReviewCount {get; set;}
    public string ProfileImageUrl {get; set;} = string.Empty;
    public List<string> ServiceNames {get; set;} = new(); 
}