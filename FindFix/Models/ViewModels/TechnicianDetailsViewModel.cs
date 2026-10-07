namespace FindFix.Models.ViewModels;
public class TechnicianDetailsViewModel
{
    public int Id {get; set;}
    public string FullName{get; set;} = string.Empty;
    public string Description {get; set;} = string.Empty;
    public string Location {get; set;} = string.Empty;
    public int YearsOfExperience {get; set;} 
    public double AverageRatng {get; set;}
    public int ReviewCount {get; set;}
    public string ProfileImageUrl {get; set;} = string.Empty;
    public List<string> ServiceNames {get; set;}= new();
    public List<QualificationDisplayViewModel> Qualifications {get; set;} = new();



}

    public class QualificationDisplayViewModel
    {
        public string Title {get; set;} = string.Empty;
        public string Institution {get; set;} = string.Empty;
        public int Year {get; set;}
    }