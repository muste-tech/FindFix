using System.ComponentModel.DataAnnotations;
namespace FindFix.Models.ViewModels; 

         public class CreateJobRequestViewModel
    {
       public int TechnicianId {get; set;}
       public string TechnicianName {get; set;} = string.Empty;
       [Required]
       public int ServiceId {get; set;}
       public List<ServiceOptionViewModel>AvailableServices {get; set;} = new();
        
        [Required]
        [StringLength (1000)]
        public string Description{get; set;} = string.Empty;
        [Required]
        public string Location {get; set;} = string.Empty;

    }
