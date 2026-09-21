using FindFix.Models;
namespace FindFix.Models.ViewModels;
public class TechnicianRegistrationViewModel
{
    public string FullName {get; set;} = string.Empty;
    public string PhoneNumber {get; set;} = string.Empty;
    public string Email {get; set;} = string.Empty;
    public string Service {get; set;} = string.Empty;
    public string Location {get; set;} = string.Empty;
    public string Description {get; set;} = string.Empty;
    public int YearOfExperience {get; set;}
}