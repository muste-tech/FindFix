using FindFix.Data;
using FindFix.Models;
using FindFix.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace FindFix.Controllers;
 [Authorize (Roles = "Customer")]
 public class CustomerController : Controller
{
    private readonly AppDbContext _context;
    public CustomerController (AppDbContext context)
    {
        _context = context;
    }
    public IActionResult Index ()
    {
         return View();
    }
    public async Task<IActionResult> Search (string? serviceQuery, string? locationQuery)
    {
        var technicians = await _context.Technicians
        .Include(t =>t.User)
        .Include(t=> t.Services)
        .Where (t => t.IsAvailable && t.IsProfileComplete)
        .ToListAsync();
        if(!string.IsNullOrWhiteSpace(serviceQuery))
        {
            technicians = technicians
            .Where(t => t.Services.Any(s => s.Name.Contains(serviceQuery, StringComparison.OrdinalIgnoreCase)))
            .ToList();
        }
        if (!string.IsNullOrWhiteSpace(locationQuery))
        {
            technicians=technicians
            .Where(t=>t.Location.Contains(locationQuery, StringComparison.OrdinalIgnoreCase)).
            ToList();
        }
        double siteAverage = technicians.Any(t => t.ReviewCount > 0)
        ? technicians.Where(t => t.ReviewCount > 0).Average(t => t.AverageRating) : 4.0;
        const int minimumReviews = 5;
        var ranked = technicians
        .Select(t => new
        {
              Technician = t,
              Score =(t.ReviewCount/ (double)(t.ReviewCount + minimumReviews))*t.AverageRating
              +(minimumReviews/(double)(t.ReviewCount + minimumReviews)) * siteAverage
        } )
        .OrderByDescending (x => x.Score)
        .Select(x => x.Technician)
        .ToList();
        var model = new SearchViewModel
        {
            ServiceQuery = serviceQuery,
            LocationQuery = locationQuery,
            Results = ranked.Select(t => new TechnicianCardViewModel
            {
                Id = t.Id,
                FullName = t.User.FullName,
                Description = t.Description,
                Location = t.Location,
                YearsOfExperience = t.YearsOfExperience,
                AverageRating = t.AverageRating,
                ReviewCount = t.ReviewCount,
                ProfileImageUrl = t.ProfileImageUrl,
                ServiceNames = t.Services.Select(s => s.Name).ToList()

            }).ToList()
        };
        return View (model);

        }
      public async Task<IActionResult> Details (int Id)
    {
        var technician = await _context.Technicians
        .Include(t => t.User)
        .Include (t => t.Services)
        .Include(t=>t.Qualifications)
        .FirstOrDefaultAsync (t => t.Id == Id && t.IsProfileComplete);
        if (technician == null)
        {
            return NotFound();
        }
        var model = new TechnicianDetailsViewModel
        {
            Id = technician.Id,
            FullName = technician.User.FullName,
            Description = technician.Description,
            Location = technician.Location,
            YearsOfExperience = technician.YearsOfExperience,
            AverageRatng = technician.AverageRating,
            ReviewCount = technician.ReviewCount,
            ProfileImageUrl = technician.ProfileImageUrl,
            ServiceNames = technician.Services.Select(s=> s.Name).ToList(),
            Qualifications = technician.Qualifications.Select(q=> new QualificationDisplayViewModel
            {
                Title = q.Title,
                Institution = q.Institution,
                Year = q.Year
            }).ToList()
        };
        return View(model);
    }
   }

   
