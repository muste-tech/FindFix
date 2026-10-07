
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
namespace FindFix.Models.ViewModels;
public class TechnicianProfileViewModel
{
   [Required]
   [StringLength(500)]
   public string Description {get; set;} = string.Empty;
   [Required]
   public string Location {get; set;} = string.Empty;
   
   [Range(0, 60)]
   [Display (Name = "Year Of Experience")]
   public int YearsOfExperience {get; set;}

   [Display (Name = "Services Offered")]
   public List<int> SelectedServiceIds {get; set;} = new();
   public List<ServiceOptionViewModel> AllServices {get; set;}= new();
   public List <QualificationInputViewModel> Qualifications {get; set;}= new();

    
}
public class ServiceOptionViewModel
{
   public int Id {get; set;}
   public string Name  {get; set;} = string.Empty;
}
 public class QualificationInputViewModel
{
   [Required]
   public string Title {get; set;} = string.Empty;
   [Required]
   public string Institution {get; set;} = string.Empty;
   [Range (1950, 2100)]
   public int Year {get; set;}
}