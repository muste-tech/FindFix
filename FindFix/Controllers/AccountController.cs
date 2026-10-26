using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using FindFix.Data;
using FindFix.Models;
using FindFix.Models.ViewModels;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore;
using FindFix.Services;
using System.ComponentModel;

namespace FindFix.Controllers;
public class AccountController : Controller
{
   private readonly UserManager <ApplicationUser> _userManager ;
   private readonly SignInManager <ApplicationUser> _signInManager;
   public readonly AppDbContext _context;
   public readonly CustomerQuestionBuilder _questionBuilder;
   public readonly TechnicianQuestionBuilder _technicianQuestionBuilder;
   public AccountController (UserManager <ApplicationUser> userManager,
    SignInManager <ApplicationUser> signInManager, AppDbContext context, 
    CustomerQuestionBuilder questionBuilder, 
    TechnicianQuestionBuilder technicianQuestionBuilder)
   {
        _userManager = userManager;
        _signInManager = signInManager;
        _context = context;
        _questionBuilder = questionBuilder;
        _technicianQuestionBuilder = technicianQuestionBuilder;
 
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
        if (model.Role == "Technician")
        {
           return RedirectToAction ("CompleteProfile", "Technician");  
        }
        return RedirectToAction ("Index", "Customer");

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
      [HttpGet]
      public  IActionResult ForgotPassword()
    {
        return View();
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
       public async Task< IActionResult > ForgotPassword(string email)
    {
        var user = await _userManager.FindByEmailAsync(email);
        // Don't reveal whether the account exists
        if (user == null)
        {
            return RedirectToAction("ForgotPasswordConfirmation");
        }
        List<SecurityQuestion>? questions = null;
        var customer = await _context.Customers.FirstOrDefaultAsync(c => c.UserId == user.Id);
        if (customer != null)
        {
            questions = await _questionBuilder.BuildAsync(customer);
        }
        else
        {
            var technician = await _context.Technicians.FirstOrDefaultAsync(c => c.UserId == user.Id);  
              if (technician != null)
        {
            questions = await _technicianQuestionBuilder.BuildAsync(technician);
        }
        }
        if (questions == null || questions.Count == 0)
        {
            return RedirectToAction("ForgotPasswordComfirmation");
        }

        // Store only what the server needs: the user, the questions, and the correct answer
        HttpContext.Session.SetString("ResetUserId", user.Id);
        HttpContext.Session.SetString("ResetQuestions", System.Text.Json.JsonSerializer.Serialize(questions));
        HttpContext.Session.SetInt32("ResetAttempts", 0);

        return RedirectToAction("AnswerQuestions");
    }
    public IActionResult ForgotPasswordConfirmation()
    {
        return View();
    }
    [HttpGet]
    public IActionResult AnswerQuestions()
    {
        var json = HttpContext.Session.GetString("ResetQuestions");
        if (json == null)
        {
            return RedirectToAction("ForgotPassword");
        }
        var questions = System.Text.Json.JsonSerializer.Deserialize<List<SecurityQuestion>>(json) ?? new();
        ViewBag.AttemptsLeft = 3 - (HttpContext.Session.GetInt32("ResetAttempts")?? 0);
        return View(questions);
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult AnswerQuestions(IFormCollection form)
    {
        var json = HttpContext.Session.GetString("ResetQuestions");
        if (json == null)
        {
            return RedirectToAction("ForgotPassword");
        }
        var questions = System.Text.Json.JsonSerializer.Deserialize<List<SecurityQuestion>>(json) ?? new();
        // Count correct answers. Each radio group is named q0, q1, q2.......
        int correct = 0;
        for (int i = 0; i < questions.Count; i++)
        {
            var chosen = form["q" + i].ToString();
            if (chosen == questions[i].CorrectAnswer)
            {
                correct++;
            }
        }
        // Need 2 correct or every question if there are fewer than 2
        int needed= Math.Min(2, questions.Count);
        if (correct >= needed)
        {
            HttpContext.Session.SetString("ResetVerified", "true");
            return RedirectToAction("ResetPassword");
        }
        var attempts = (HttpContext.Session.GetInt32("ResetAttempts") ?? 0) +1;
        if (attempts >= 3)
        {
            HttpContext.Session.Clear();
            TempData["Message"] = "Too many wrong answers, please start again";
            return RedirectToAction("ForgotPassword");
        }
        HttpContext.Session.SetInt32("ResetAttempts", attempts);
        ViewBag.AttemptsLeft = 3 - attempts;
        ViewBag.Error = "Some answers were wrong. Try again ";
        return View (questions);
    }    
        [HttpGet]
        public IActionResult ResetPassword()
       {
           if (HttpContext.Session.GetString("ResetVerified") != "true")
           {
             return RedirectToAction("ForgotPassword");
           }
           return View();
       }
       [HttpPost]
       [ValidateAntiForgeryToken]
       public async Task<IActionResult> ResetPassword(string password, string confirmPassword)
    {
        if (HttpContext.Session.GetString("ResetVerified") != "true")
           {
             return RedirectToAction("ForgotPassword");
           }
           if (password != confirmPassword)
           {
             ViewBag.Error ="The message do not match";
             return View();
           }
           var userId = HttpContext.Session.GetString("ResetUserId");
           var user = await _userManager.FindByIdAsync(userId?? "");
           if (user == null)
           {
             return RedirectToAction ("ForgotPassword");
           }
        //    Identity handles the reset token, so this is a directpassword change
         var token = await _userManager.GeneratePasswordResetTokenAsync(user);
         var result = await _userManager.ResetPasswordAsync(user, token, password);
         if (!result.Succeeded)
         {
            ViewBag.Error = string.Join(" ", result.Errors.Select(e => e.Description));
            return View();
         }
        //  Clear the reset session so it can't be reused
        HttpContext.Session.Clear();
        TempData ["Message"] ="Your password has been changed, You can log in now";
        return RedirectToAction("Login");
 }
}