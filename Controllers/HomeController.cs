using JAS.Areas.Identity.Data;
using JAS.Models;
using JAS.Models.Domain.CompositeModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace JAS.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private JASContext jasContext;
        private UserManager<JASUser> userManager;

        public HomeController(ILogger<HomeController> logger, JASContext jasContext, UserManager<JASUser> userManager)
        {
            _logger = logger;
            this.jasContext = jasContext;
            this.userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var jobListings = await jasContext.JobListing
                .Include(jc => jc.JobCategory)
                .Include(c => c.Company)
                 .ThenInclude(c => c.City)
                .Take(100)
                .ToListAsync();

            if (!jobListings.Any())
            {
                return RedirectToAction("Oops", "Message");
            }

            return View(jobListings);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}