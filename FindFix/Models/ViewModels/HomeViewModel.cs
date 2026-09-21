using FindFix.Models;
namespace FindFix.Models.ViewModels;
public class HomeViewModel
{
    public string SearchTerm {get; set;} = "";
    public string Location {get; set;} = string.Empty;
    public List<Service> PopularService {get; set;}= new();
}
