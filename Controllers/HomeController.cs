using Microsoft.AspNetCore.Mvc;
using SchoolSystem.Models;

namespace SchoolSystem.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            if (!User.Identity?.IsAuthenticated ?? false)
                return RedirectToAction("Login", "Account");

            var role = User.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value;

            return role switch
            {
                "Admin" => RedirectToAction("Dashboard", "Admin"),
                "Teacher" => RedirectToAction("Dashboard", "Teacher"),
                "Student" => RedirectToAction("Dashboard", "Student"),
                _ => RedirectToAction("Login", "Account")
            };
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = System.Diagnostics.Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
