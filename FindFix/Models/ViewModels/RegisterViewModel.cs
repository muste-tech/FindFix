using System.ComponentModel.DataAnnotations;
using FindFix.Models;
namespace FindFix.Models.ViewModels;
public class RegisterViewModel
{
    [Required]
    [Display(Name ="Full Name" )]
    public string FullName {get; set;} = string.Empty;
    [Required]
    [EmailAddress]
    public string Email {get; set;} = string.Empty;
    [Required]
    [DataType(DataType.Password)]
    public string Password {get;set;} = string.Empty;
    [Required]
    [DataType(DataType.Password)]
    [Compare("Password", ErrorMessage ="Passwords do not match")]
    [Display(Name = "Confirm Password")]
     public string ConifirmPassword {get;set;} = string.Empty;
     [Required]
     public string Role {get; set;} = "Customer";
}