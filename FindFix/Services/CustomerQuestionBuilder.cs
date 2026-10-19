using FindFix.Models;
using FindFix.Data;
using Microsoft.EntityFrameworkCore;

namespace FindFix.Services
{
    public class CustomerQuestionBuilder
    {
        private readonly AppDbContext _context;
        public CustomerQuestionBuilder(AppDbContext context)
        {
            _context = context;
        }
        public async Task<List<SecurityQuestion>> BuildAsync (Customer customer)
        {
            var questions = new List <SecurityQuestion>();
            var rng = Random.Shared;
            // Question 1: The service on the most recent request (skipped if no requests yet)
            var lastJob = await _context.JobRequests
            .Include(j=> j.Service)
            .Where(j=>j.CustomerId == customer.Id)
            .OrderByDescending(j => j.DateCreated)
            .FirstOrDefaultAsync();
            if (lastJob?.Service != null)
            {
                var otherServices = await _context.Services
                .Where(s=>s.Name != lastJob.Service.Name)
                .Select(s=> s.Name)
                .ToListAsync();
                 var wrong = otherServices.OrderBy(_ => rng.Next()).Take(2).ToList();
                 questions.Add(MakeQuestion(
                    "Which service did you request most recently?",
                    lastJob.Service.Name, wrong, rng ));
            }
        //   Question 2: the location on the profile
         if (!string.IsNullOrWhiteSpace(customer.Location))
         {
            var places =new[] {"Akure", "Ondo","Ado-Ekiti", "Ibadan", "Lagos"}
            .Where(p=>!p.Equals(customer.Location, StringComparison.OrdinalIgnoreCase))
            .OrderBy(_ => rng.Next())
            .Take(2)
            .ToList();
             questions.Add(MakeQuestion(
            "Which location did you enter on your profile?",
            customer.Location, places, rng));
         }
        // Question 3: the month the account was created
        var joined = customer.DateRegistered;
        var correctMonth= joined.ToString("MMM yyyy");
        var wrongMonths = new List<string>
        {
            joined.AddMonths(-2).ToString("MMM yyyy"),
            joined.AddMonths(3).ToString("MMM yyyy")
        };
        questions.Add(MakeQuestion(
            "When did you join FindFix?",
            correctMonth, wrongMonths, rng ));
            return questions;
        }
        private static SecurityQuestion MakeQuestion(string text, string correct, List <string> wrong, Random rng )
        {
            var options = new List<string> (wrong) {correct};
            return new SecurityQuestion
            {
                Text = text,
                Options = options.OrderBy(_ => rng.Next()).ToList(),
                CorrectAnswer = correct
            };
        }
    }
}