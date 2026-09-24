
using FindFix.Models;
namespace FindFix.Models.ViewModels;
public class TechnicianProfileViewModel
{
   public Technician Technician {get; set;} = new();
   public List<Review> Reviews {get; set;} =new();
   public double AverageRating {get; set;}
    
}