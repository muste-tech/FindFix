using System.Security.Cryptography.X509Certificates;

namespace FindFix.Models;
public class Qualification
{
    public int Id {get; set;}
    // The Certicificate name e.g B/tech
    public string Title {get; set;} = string.Empty;
    public string Institution {get; set;} = string.Empty;
    // Year it was earned e.g 2026
    public int Year {get; set;}
    public string DocumentUrl {get; set;} = string.Empty;

    public int TechnicianId {get; set;}
    public Technician Technician {get; set;} = null! ;

}