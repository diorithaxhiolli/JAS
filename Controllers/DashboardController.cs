using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JAS.Controllers
{
    public class DashboardController : Controller
    {
        [Authorize]
        public IActionResult Control()
        {
            if (User.IsInRole("Administrator"))
            {
                // Display admin dashboard layout
                return RedirectToAction("AdminDashboard");
            }
            else if (User.IsInRole("JobSeeker"))
            {
                // Display user dashboard layout
                return RedirectToAction("JobSeekerDashboard");
            }
            else if (User.IsInRole("Company"))
            {
                // Default dashboard layout for other roles or unauthenticated users
                return RedirectToAction("CompanyDashboard");
            }

            return RedirectToAction("Oops", "Message");
        }

        [Authorize(Roles = "Company")]
        public IActionResult CompanyDashboard()
        {
            return View();
        }

        [Authorize(Roles = "Administrator")]
        public IActionResult AdminDashboard()
        {
            return View();
        }

        [Authorize(Roles = "JobSeeker")]
        public IActionResult JobSeekerDashboard()
        {
            return View();
        }
    }
}
