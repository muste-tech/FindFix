using FindFix.Data;
using FindFix.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FindFix.Controllers;

[Authorize]
public class NotificationController : Controller
{
    private readonly AppDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;
    public NotificationController(AppDbContext context, UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }
    [HttpGet]
    public async Task <IActionResult>JobStatuses()
    {  
        var userId = _userManager.GetUserId(User);
        List <object> jobs = new();
        if(User.IsInRole("Customer"))
        {
             var customer = await _context.Customers.FirstOrDefaultAsync(c => c.UserId == userId);
             if(customer != null)
            {
                jobs = await _context.JobRequests
                .Where(j => j.CustomerId==customer.Id)
                .Select(j => (object)new{id = j.Id, status = j.Status.ToString()})
                .ToListAsync();
            }
        }
        else if(User.IsInRole("Technician"))
        {
            var technician = await _context.Technicians.FirstOrDefaultAsync(t=>t.UserId == userId);
            if(technician!= null)
            {
                jobs = await _context.JobRequests
                .Where(j=>j.TechnicianId==technician.Id)
                .Select(j=> (object)new{id = j.Id, status = j.Status.ToString()})
                .ToListAsync();
            }
        }
        // Owner keeps each user's baseline seperate if two accounts use the same browser
        return Json (new{owner= User.Identity?.Name, jobs});

        
    }

}
