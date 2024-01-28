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
            var currentUser = await userManager.GetUserAsync(User);

            // Get the positionId from the JobListing model
            var jobPositionId = await jasContext.JobListing
                .Where(cv => cv.companyId == currentUser.Id)
                .Select(j => j.positionId)
                .FirstOrDefaultAsync();

            // Use jobPositionId in your logic as needed

            var jobListingList = await jasContext.JobListing
                .Where(cv => cv.companyId == currentUser.Id)
                .Include(j => j.Company)
                .Include(j => j.JobCategory)
                .Select(j => new HomePageComposite
                {
                    JobId = j.positionId,
                    Title = j.title,
                    CompanyName = j.Company != null ? j.Company.name : "N/A",
                    CategoryName = j.JobCategory != null ? j.JobCategory.name : "N/A"
                })
                .ToListAsync();

            return View(jobListingList);
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