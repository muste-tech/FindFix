using FindFix.Models;
namespace FindFix.Models.ViewModels;
public class SearchResultViewModel
{
    public string SearchTerm {get; set;} = string.Empty;
    public string Location {get; set;} = string.Empty;
    public List<Technician> Technicians {get; set;}= new(); 
}