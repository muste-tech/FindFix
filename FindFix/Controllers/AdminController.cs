using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace FindFix.Controllers;
 [Authorize (Roles = "Admin")]
 public class AdminController : Controller
{
    public IActionResult Index ()
    {
         return View();
    }
   
}