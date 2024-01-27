using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JAS.Controllers
{
    public class DashboardController : Controller
    {
        public IActionResult Control()
        {
            if (User.IsInRole("Administrator"))
            {
                // Display admin dashboard layout
                return View("AdminDashboard");
            }
            else if (User.IsInRole("JobSeeker"))
            {
                // Display user dashboard layout
                return View("JobSeekerDashboard");
            }
            else if (User.IsInRole("Company"))
            {
                // Default dashboard layout for other roles or unauthenticated users
                return View("CompanyDashboard");
            }

            return RedirectToAction("Index", "Home");
        }
    }
}
