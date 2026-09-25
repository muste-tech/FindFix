using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace FindFix.Controllers;
 [Authorize (Roles = "Customer")]
 public class CustomerController : Controller
{
    public IActionResult Index ()
    {
         return View();
    }
   
}