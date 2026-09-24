using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using FindFix.Data;
using FindFix.Models;
using FindFix.Models.ViewModels;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace FindFix.Controllers;
public class AccountController : Controller
{
   private readonly UserManager <ApplicationUser> _userManager ;
   private readonly SignInManager <ApplicationUser> _signInManager;
   public readonly AppDbCOntext _context;
   public AccountController (UserManager <ApplicationUser> userManager, SignInManager <ApplicationUser> signInManager, AppDbCOntext context )
   {
        _userManager = userManager;
        _signInManager = signInManager;
        _context = context;
    }
    [HttpGet]
    public IActionResult Register ()
    {
        return View();
    }
    [HttpPost]
    public async Task<IActionResult> Register (RegisterViewModel model)
    {
       if (!ModelState.IsValid)
       {
        return View (model);
       } 
       var user = new ApplicationUser
       {
           UserName = model.Email,
           Email = model.Email,
           FullName = model.FullName

       };

       var result = await _userManager.CreateAsync(user, model.Password);
       if (!result.Succeeded)
       {
       foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
            return View(model);

       }
       await _userManager.AddToRoleAsync(user, model.Role);
       if (model.Role == "Customer")
       {
        var customer = new Customer
        {
            UserId = user.Id,
            DateRegistered = DateTime.UtcNow,
            
        };
        _context.Customers.Add(customer);
       }
       else if (model.Role =="Technician")
        {
            var technician = new Technician
            {
                UserId = user.Id,
                DateRegistered = DateTime.UtcNow,
                IsAvailable = true
            };
            _context.Technicians.Add(technician);
        }
        await _context.SaveChangesAsync();
        await _signInManager.SignInAsync(user, isPersistent: false);
        return RedirectToAction ("Index", "Home");

    }
}