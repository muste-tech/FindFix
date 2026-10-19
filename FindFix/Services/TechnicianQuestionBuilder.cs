using FindFix.Data;
using FindFix.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
namespace FindFix.Services;
public class TechnicianQuestionBuilder
{
     private readonly AppDbContext _context;
    private static readonly string[] AllExperienceRanges =
    {
        "0 to 3 years",
        "4 to 6 years",
        "7 or more years"
    };
    public TechnicianQuestionBuilder( AppDbContext context)
    {
        _context = context;
    }
    public async Task<List<SecurityQuestion>> BuildAsync (Technician technician)
    {
        var questions = new List<SecurityQuestion>();
        var rng = Random.Shared;
        // Question 1: a service the technician actually offers 
        var  technician_services =  await _context.Technicians
        .Where(t=>t.Id== technician.Id)
        .SelectMany(t => t.Services)
        .Select(s => s.Name)
        .ToListAsync();
        if(technician_services.Count > 0)
        {
            var correctService = technician_services[0];
            var otherServices = await _context.Services
            .Where(s => !technician_services.Contains(s.Name))
            .Select(s => s.Name)
            .ToListAsync();
            var wrong = otherServices.OrderBy(_ => rng.Next()).Take(2).ToList();
            questions.Add(MakeQuestion(
                "Which service do you offer on FindFix?",
                 correctService, wrong, rng));
        }
        // Question 2: the location on the profile
        if (!string.IsNullOrWhiteSpace(technician.Location))
        {
            var places = new[] {"Akure", "Ondo","Ado-Ekiti", "Ibadan", "Lagos"}
            .Where(p => !p.Equals(technician.Location, StringComparison.OrdinalIgnoreCase))
            .OrderBy(_ => rng.Next())
            .Take(2)
            .ToList();
            questions.Add(MakeQuestion(
                "Which location did you enter on your profile?",
               technician.Location, places, rng ));
        }
        // Question 3: years of experience, as a range
        var correctRange = ExperienceRange(technician.YearsOfExperience);
        var wrongRanges = AllExperienceRanges.Where(r=> r != correctRange).ToList();
        questions.Add(MakeQuestion(
            "How many years of experience do you have",
            correctRange, wrongRanges, rng ));
            return questions;
        
    }
    private static string ExperienceRange(int years) => years switch
    {
        <= 3 => "0 to 3 years",
        <= 6 => "4 to 6 years",
        _ => "7 or more year"
    };
    private static SecurityQuestion MakeQuestion(string text, string correct, List<string> wrong, Random rng)
    {
        var options = new List<string>(wrong) { correct };
        return new SecurityQuestion
        {
            Text = text,
            Options = options.OrderBy(_ => rng.Next()).ToList(),
            CorrectAnswer = correct
        };
    }
}













