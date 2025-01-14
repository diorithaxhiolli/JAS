﻿using JAS.Areas.Identity.Data;
using JAS.Models.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using JAS.Models.Domain.CompositeModel;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.CodeAnalysis.CSharp.Formatting;
using Microsoft.AspNetCore.Authorization;

namespace JAS.Controllers
{
    public class JobListingController : Controller
    {
        private readonly JASContext jasContext;
        private readonly UserManager<JASUser> userManager;
        private readonly IWebHostEnvironment _env;

        public JobListingController(JASContext jasContext, UserManager<JASUser> userManager, IWebHostEnvironment _env)
        {
            this.jasContext = jasContext;
            this.userManager = userManager;
            this._env = _env;
        }

        [Authorize(Roles = "Company")]
        public async Task<IActionResult> Index()
        {
            var currentUser = await userManager.GetUserAsync(User);

            var jobListingList = await jasContext.JobListing
                .Where(cv => cv.companyId == currentUser.Id)
                .ToListAsync();

            var jobListingComposite = new List<JobListingComposite>();

            foreach (var jobListing in jobListingList)
            {
                var applicationCount = await jasContext.Application
                           .Where(app => app.positionId == jobListing.positionId)
                           .CountAsync();

                var jobListingModel = new JobListingComposite
                {
                    JobListing = jobListing,
                    ApplicationCount = applicationCount
                };

                jobListingComposite.Add(jobListingModel);
            }

            return View(jobListingComposite);
        }

        [Authorize(Roles = "Company")]
        public async Task<IActionResult> Create()
        {
            var categories = jasContext.JobCategory.ToList();
            ViewData["Categories"] = categories;

            return View();
        }


        [Authorize(Roles = "Company")]
        [HttpGet]
        public async Task<IActionResult> View(int positionId)
        {
            var jobListing = await jasContext.JobListing
               .Include(d => d.JobCategory)
               .FirstOrDefaultAsync(d => d.positionId == positionId);

            string categoryName = jobListing.JobCategory.name;
            ViewData["categoryName"] = categoryName;

            var categories = jasContext.JobCategory.ToList();
            ViewData["Categories"] = categories;

            if (jobListing != null)
            {
                var viewModel = new JobListing()
                {
                    title = jobListing.title,
                    salary = jobListing.salary,
                    companyId = jobListing.companyId,
                    categoryId = jobListing.categoryId,
                    description = jobListing.description,
                };

                return View("View", viewModel);
            }
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Company")]
        [HttpPost]
        public async Task<IActionResult> UpdateJobListingOnPost(JobListing model)
        {
            var jobListing = await jasContext.JobListing.FindAsync(model.positionId);

            if (jobListing != null)
            {
                jobListing.title = model.title;
                jobListing.salary = model.salary;
                jobListing.categoryId = model.categoryId;
                jobListing.description = model.description;

                await jasContext.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Company")]
        public async Task<IActionResult> AddJobListingOnPost(JobListing model)
        {
            if (model == null)
            {
                return RedirectToAction("Index");
            }

            var currentUser = await userManager.GetUserAsync(User);

            var jobListing = new JobListing()
            {
                title = model.title,
                categoryId = model.categoryId,
                salary = model.salary,
                companyId = currentUser.Id,
                description = model.description,
            };

            await jasContext.JobListing.AddAsync(jobListing);
            await jasContext.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Company")]
        public async Task<IActionResult> ViewApplicationsJob(int positionId)
        {
            if (positionId == null)
            {
                return RedirectToAction("Index");
            }

            var applicationData = await jasContext.Application
                .Include(a => a.CoverLetter)
                .Include(a => a.Status)
                .Include(a => a.CV)
                    .ThenInclude(cv => cv.Education)
                .Include(a => a.CV)
                    .ThenInclude(cv => cv.Experience)
                .Include(a => a.JobListing)
                .Include(a => a.JobSeeker)
                    .ThenInclude(js => js.User)
                .Where(app => app.positionId == positionId)
                .ToListAsync();

            return View(applicationData);
        }

        [Authorize(Roles = "Company")]
        [HttpGet]
        public async Task<IActionResult> ViewUserCV(int cvId, int applicationId)
        {
            var status = jasContext.Status.ToList();
            ViewData["Statuses"] = status;

            var applicationData = await jasContext.Application
                .Include(cv => cv.CV)
                    .ThenInclude(cv => cv.Education)
                .Include(cv => cv.CV)
                    .ThenInclude(cv => cv.Experience)
                .Include(st => st.Status)
                .Include(cl => cl.CoverLetter)
                .Include(jl => jl.JobListing)
                .Include(js => js.JobSeeker)
                    .ThenInclude(js => js.User)
                .Where(app => app.cvId == cvId)
                .FirstOrDefaultAsync();

            ViewBag.appId = applicationId;

            if (applicationData == null)
            {
                return RedirectToAction("Index");
            }

            return View("ViewUserCV", applicationData);
        }

        [Authorize(Roles = "Company")]
        [HttpPost]
        public async Task<IActionResult> ChangeStatus(int cvId, int statusId, int applicationId)
        {
            var application = await jasContext.Application.FindAsync(applicationId);

            if (application == null)
            {
                return RedirectToAction("Index");
            }

            application.statusId = statusId;
            await jasContext.SaveChangesAsync();

            return await ViewUserCV(cvId, applicationId);
        }


        [HttpGet]
        public async Task<IActionResult> ViewJobListing(int positionId)
        {
            var jobListingModel = await jasContext.JobListing
                .Include(jc => jc.JobCategory)
                .Include(c => c.Company)
                    .ThenInclude(c => c.City)
                .Include(c => c.Company)
                    .ThenInclude(c => c.User)
                .Where(jl => jl.positionId == positionId)
                .FirstOrDefaultAsync();

            return View(jobListingModel);
        }

        [Authorize(Roles = "Company")]
        public async Task<IActionResult> DownloadCV(int cvId)
        {
            var cv = await jasContext.CV.FirstOrDefaultAsync(c => c.cvId == cvId);

            if (cv == null || string.IsNullOrEmpty(cv.filePath))
            {
                // Handle the case where the CV with the given ID is not found or file path is empty
                return NotFound();
            }

            // Resolve the tilde (~) in the file path
            var resolvedFilePath = cv.filePath.Replace("~/", string.Empty);

            // Construct the full physical path
            var filePath = Path.Combine(_env.WebRootPath, resolvedFilePath);

            // Return the file as a downloadable content
            return PhysicalFile(filePath, "application/pdf", Path.GetFileName(filePath));
        }

        [Authorize(Roles = "Company")]
        public async Task<IActionResult> DownloadCoverLetter(int coverLetterId)
        {
            var coverLetter = await jasContext.CoverLetter.FirstOrDefaultAsync(c => c.coverLetterId == coverLetterId);

            if (coverLetter == null || string.IsNullOrEmpty(coverLetter.filePath))
            {
                return NotFound();
            }

            var resolvedFilePath = coverLetter.filePath.Replace("~/", string.Empty);

            var filePath = Path.Combine(_env.WebRootPath, resolvedFilePath);

            return PhysicalFile(filePath, "application/pdf", Path.GetFileName(filePath));
        }

        public async Task<IActionResult> SearchJobs(int? categoryId)
        {
            if(categoryId == null)
            {
                return RedirectToAction("Oops", "Message");
            }

            var jobListings = await jasContext.JobListing
                .Include(jc => jc.JobCategory)
                .Include(c => c.Company)
                    .ThenInclude(c => c.City)
                .Where(jl => jl.categoryId == categoryId)
                .Take(100)
                .ToListAsync();


            return View("SearchResult", jobListings);
        }


        public IActionResult GetSearchValue(string search)
        {
            List<JobCategory> allsearch = jasContext.JobCategory
                .Where(x => x.name.Contains(search))
                .Select(x => new JobCategory
                {
                    categoryId = x.categoryId,
                    name = x.name
                })
                .ToList();

            return Json(allsearch);
        }

        [HttpPost]
        public async Task<IActionResult> GetPostSearchValue(string search)
        {
            if (string.IsNullOrWhiteSpace(search))
            {
                return RedirectToAction("Oops", "Message");
            }

            var jobCat = await jasContext.JobCategory.FirstOrDefaultAsync(x => x.name.ToLower() == search.ToLower());

            int categoryId;

            if (jobCat != null)
            {
                categoryId = jobCat.categoryId;
                return RedirectToAction("SearchJobs", "JobListing", new { categoryId = jobCat.categoryId});
            }

            return RedirectToAction("Oops", "Message");
        }


        [HttpGet]
        public IActionResult GetCategoryNames(string searchTerm)
        {
            var categoryNames = jasContext.JobCategory
                .Where(c => c.name.Contains(searchTerm))
                .Select(c => new { label = c.name, value = c.categoryId }) // Return both label and value
                .ToList();

            return Json(categoryNames);
        }


    }
}

