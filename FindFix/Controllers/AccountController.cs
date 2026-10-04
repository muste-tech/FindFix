using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using FindFix.Data;
using FindFix.Models;
using FindFix.Models.ViewModels;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore;

namespace FindFix.Controllers;
public class AccountController : Controller
{
   private readonly UserManager <ApplicationUser> _userManager ;
   private readonly SignInManager <ApplicationUser> _signInManager;
   public readonly AppDbContext _context;
   public AccountController (UserManager <ApplicationUser> userManager, SignInManager <ApplicationUser> signInManager, AppDbContext context )
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
    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }
        var result = await _signInManager.PasswordSignInAsync(
            model.Email, model.Password, model.RememberMe, lockoutOnFailure: false );
            if (!result.Succeeded)
        {
            ModelState.AddModelError(string.Empty, "Invalid Email or Password");
            return View(model);
        }
        var user = await _userManager.FindByEmailAsync(model.Email);
        if (await _userManager.IsInRoleAsync(user!, "Customer"))
        {
            return RedirectToAction("Index", "Customer");
        }
        else if (await _userManager.IsInRoleAsync(user!, "Technician"))
        {
          var technician = await _context.Technicians.FirstOrDefaultAsync(t => t.UserId == user!.Id);
          if (technician != null && !technician.IsProfileComplete)
          {
            return RedirectToAction("CompleteProfile", "Technician");
          }
            return RedirectToAction("Index", "Technician");
        }
         else if (await _userManager.IsInRoleAsync(user!, "Admin"))
        {
            return RedirectToAction("Admin", "Admin");
        }
        return RedirectToAction("Index", "Home");
    }
      public async Task<IActionResult> Logout ()
      {
        await _signInManager.SignOutAsync();
        return RedirectToAction ("Index", "Home");
      }
    }