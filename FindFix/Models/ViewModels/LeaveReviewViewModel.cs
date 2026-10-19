using System.ComponentModel.DataAnnotations;
namespace FindFix.Models.ViewModels;
public class LeaveReviewViewModel
{
    public int JobRequestId {get; set;}
    public string TechnicianName{get; set;}= string.Empty;
    public string ServiceName {get; set;}= string.Empty;
    [Required]
    [Range(1,5)]
    public int Rating{get; set;}
    [StringLength(500)]
    public string Comment {get; set;}=string.Empty;

}