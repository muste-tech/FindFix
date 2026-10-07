using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FindFix.Data;
using FindFix.Models;
using FindFix.Models.ViewModels;
using Microsoft.EntityFrameworkCore.Metadata.Internal;


namespace FindFix.Controllers;
 [Authorize (Roles = "Technician")]
 public class TechnicianController : Controller
{
    private readonly AppDbContext _context ;
    private readonly UserManager<ApplicationUser> _userManager;
    public TechnicianController (AppDbContext context, UserManager<ApplicationUser> userManager )
    {
        _context = context;
        _userManager = userManager;
    }
    public IActionResult Index ()
    {
         return View();
    }
    [HttpGet]
    public async Task<IActionResult> CompleteProfile ()
    {
        var userId = _userManager.GetUserId(User);
        var technician = await _context.Technicians.FirstOrDefaultAsync(t=> t.UserId == userId);
        if (technician == null)
        {
            return NotFound();
        }
        var allServices = await _context.Services.ToListAsync();
        var model = new TechnicianProfileViewModel
        {
            Description = technician.Description,
            Location = technician.Location,
            YearsOfExperience = technician.YearsOfExperience,
            AllServices = allServices.Select(s => new ServiceOptionViewModel
            {
                Id =s.Id,
                Name = s.Name
            }).ToList()

        };
        model.Qualifications.Add(new QualificationInputViewModel());
         return View (model);
    }
   
   [HttpPost]
   public async Task<IActionResult> CompleteProfile(TechnicianProfileViewModel model)
    {
        var allServices = await _context.Services.ToListAsync();
        model.AllServices =allServices.Select(s=> new ServiceOptionViewModel
        {
            Id = s.Id,
            Name = s.Name
        }).ToList();
        if (!model.SelectedServiceIds.Any())
        {
            ModelState.AddModelError(nameof(model.SelectedServiceIds), "Please select at least one service");
        }
        if (!ModelState.IsValid)
        {
            return View(model);
        }
         var UserId = _userManager.GetUserId(User);
         var technician = await _context.Technicians
         .Include (t => t.Services)
         .Include (t => t.Qualifications)
         .FirstOrDefaultAsync(t => t.UserId == UserId);

         if (technician == null)
         {
            return NotFound();
         }
         technician.Description = model.Description;
         technician.Location = model.Location;
         technician.YearsOfExperience = model.YearsOfExperience;
         technician.IsProfileComplete = true;

         technician.Services.Clear();
         var selectedServices = allServices.Where(s => model.SelectedServiceIds.Contains(s.Id));
         foreach (var service in selectedServices)
         {
            technician.Services.Add(service);
         }
         _context.Qualifications.RemoveRange(technician.Qualifications);
            foreach (var q in model.Qualifications)
           {
             if (!string.IsNullOrWhiteSpace(q.Title))
            {
                technician.Qualifications.Add(new Qualification
                {
                    Title = q.Title,
                    Institution = q.Institution,
                    Year =q.Year
                 });  
            }
           }
           await _context.SaveChangesAsync();
        return RedirectToAction ("Index");
    }
}