using JAS.Areas.Identity.Data;
using JAS.Models.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace JAS.Controllers
{
    public class JobListingController : Controller
    {
        private readonly JASContext jasContext;
        private readonly UserManager<JASUser> userManager;

        public JobListingController(JASContext jasContext, UserManager<JASUser> userManager)
        {
            this.jasContext = jasContext;
            this.userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var currentUser = await userManager.GetUserAsync(User);

            var jobListingList = await jasContext.JobListing
                .Where(cv => cv.companyId == currentUser.Id)
                .ToListAsync();

            return View(jobListingList);
        }

        [HttpGet]
        public IActionResult JobCreation()
        {
            var categories = jasContext.JobCategory.ToList(); // Fetch categories from the database
            ViewData["Categories"] = categories;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> JobCreation(JobListing addJobRequest)
        {
            // Get the currently logged-in user
            var user = await userManager.GetUserAsync(User);

            // Check if the user is not null before accessing properties
            if (user != null)
            {
                var job = new JobListing()
                {
                    title = addJobRequest.title,
                    categoryId = addJobRequest.categoryId,
                    companyId = user.Id.ToString(),
                };

                await jasContext.JobListing.AddAsync(job);
                await jasContext.SaveChangesAsync();
            }

            return RedirectToAction("JobCreation");
        }
    }
}
