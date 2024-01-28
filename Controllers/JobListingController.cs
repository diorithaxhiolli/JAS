using JAS.Areas.Identity.Data;
using JAS.Models.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using JAS.Models.Domain.CompositeModel;

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

        //create
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

        //read
        [HttpGet]
        public async Task<IActionResult> View(int id)
        {
            var job = await jasContext.JobListing
                .Include(j => j.JobCategory)  // Ensure JobCategory is included in the query
                .FirstOrDefaultAsync(x => x.positionId == id);

            if (job != null)
            {
                var viewJob = new HomePageComposite()
                {
                    Title = job.title,
                    CategoryName = job.JobCategory != null ? job.JobCategory.name : "N/A",
                };

                return View("View", viewJob);
            }

            return RedirectToAction("View"); // Redirect to a meaningful action, e.g., Index
        }


        //update
        [HttpPost]
        public async Task<IActionResult> View(HomePageComposite model)
        {
            var job = await jasContext.JobListing
                .Include(j => j.JobCategory)
                .FirstOrDefaultAsync(x => x.positionId == model.JobId);

            if (job != null)
            {
                job.title = model.Title;

                // Check if the job already has a JobCategory
                if (job.JobCategory == null)
                {
                    // Create a new JobCategory if it doesn't exist
                    job.JobCategory = new JobCategory { name = model.CategoryName };
                }
                else
                {
                    // Update the existing JobCategory
                    job.JobCategory.name = model.CategoryName;
                }

                await jasContext.SaveChangesAsync();

                return RedirectToAction("Index"); // Redirect to a meaningful action, e.g., Index
            }

            return RedirectToAction("Index"); // Redirect to a meaningful action, e.g., Index
        }

    }
}
