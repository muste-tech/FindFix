using FindFix.Data;
using FindFix.Models;
using FindFix.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace FindFix.Controllers;
 [Authorize (Roles = "Customer")]
 public class CustomerController : Controller
{
    private readonly AppDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;
    public CustomerController (AppDbContext context,  UserManager<ApplicationUser> userManager )
    {
        _context = context;
        _userManager = userManager;
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
    [HttpGet]
     public async Task<IActionResult> RequestJob (int technicianId)
    {
        var technician = await _context.Technicians
        .Include(t=>t.User)
        .Include(t=>t.Services)
        .FirstOrDefaultAsync(t=> t.Id==technicianId && t.IsProfileComplete);
        if (technician == null)
        {
            return NotFound();
        }
        var model = new CreateJobRequestViewModel
        {
            TechnicianId = technician.Id,
            TechnicianName = technician.User.FullName,
            AvailableServices =technician.Services.Select(s => new ServiceOptionViewModel
            {
                Id = s.Id,
                Name = s.Name
            }).ToList()
        }; 
        return View (model);
   }
   [HttpPost]
   public async Task <IActionResult> RequestJob (CreateJobRequestViewModel model)
    {
        var technician = await _context.Technicians
        .Include(t=> t.Services)
        .FirstOrDefaultAsync(t=> t.Id == model.TechnicianId);
        if(technician == null)
        {
            return NotFound();
        }
         model.AvailableServices = technician.Services.Select(s => new ServiceOptionViewModel
         {
             Id = s.Id,
             Name = s.Name
         }).ToList();
         if(!ModelState.IsValid)
        {
            return View(model);
        }
        var userId = _userManager.GetUserId(User);
        var customer = await _context.Customers.FirstOrDefaultAsync(c => c.UserId == userId);
        if(customer== null)
        {
            return NotFound();
        }
        var JobRequest = new JobRequest
        {
            CustomerId = customer.Id,
            TechnicianId = technician.Id,
            ServiceId = model.ServiceId,
            Description = model.Description,
            Location = model.Location,
            Status = JobStatus.pending,
            DateCreated = DateTime.UtcNow
        };
        _context.JobRequests.Add(JobRequest);
        await _context.SaveChangesAsync();
        return RedirectToAction("MyRequests");
    }
    public async Task<IActionResult> MyRequests ()
    {
        var userId = _userManager.GetUserId(User);
        var customer = await _context.Customers.FirstOrDefaultAsync(c=> c.UserId == userId);
        if(customer == null)
        {
            return View (new List<JobRequest>());
        }
        var requests = await _context.JobRequests
        .Include(j => j.Technician).ThenInclude(t=> t.User)
        .Include(j => j.Service)
        .Where(j => j.CustomerId== customer.Id)
        .OrderByDescending (j=> j.DateCreated)
        .ToListAsync();
        return View(requests);
    }
   
}

   
