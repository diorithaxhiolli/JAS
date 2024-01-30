using JAS.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using JAS.Models.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication;
using JAS.Models.Domain.CompositeModel;
using Microsoft.AspNetCore.Authorization;

namespace JAS.Controllers
{
    public class ApplicationController : Controller
    {
        private readonly JASContext _dBContext;
        private readonly UserManager<JASUser> _userManager;
        private readonly IWebHostEnvironment _env;
        public ApplicationController(JASContext _dBContext, UserManager<JASUser> _userManager, IWebHostEnvironment _env)
        {
            this._dBContext = _dBContext;
            this._userManager = _userManager;
            this._env = _env;
        }

        [Authorize(Roles = "JobSeeker")]
        [HttpGet]
        public async Task<IActionResult> Write(int positionId)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            var jobListingModel = await _dBContext.JobListing.FindAsync(positionId);

            ApplicationComposite compositeModel = new ApplicationComposite()
            {
                JASUser = currentUser,
                JobListing = jobListingModel,
            };

            List<CV> cvList = await _dBContext.CV
                .Where(x => x.jobSeekerId == currentUser.Id)
                .ToListAsync();

            if(cvList == null)
            {
                return RedirectToAction("Create", "CV");
            }

            ViewBag.CVs = cvList;
            return View(compositeModel);
        }


        [Authorize(Roles = "JobSeeker")]
        [HttpPost]
        public async Task<IActionResult> SendApplication(ApplicationComposite model, IFormFile filePath)
        {
            var filePDF = await SavePDF(filePath);

            var newCoverLetter = new CoverLetter()
            {
                filePath = filePDF.ToString(),
            };

            await _dBContext.AddAsync(newCoverLetter);
            await _dBContext.SaveChangesAsync();

            int newCoverLetterId = newCoverLetter.coverLetterId;


            var newApplication = new Application()
            {
                positionId = model.JobListing.positionId,
                jobSeekerId = model.JASUser.Id,
                cvId = model.CV.cvId,
                statusId = (await _dBContext.Status.FirstOrDefaultAsync(s => s.statusName == "New Applicant"))?.statusId ?? 0,
                coverLetterId = newCoverLetterId,
            };

            await _dBContext.AddAsync(newApplication);
            await _dBContext.SaveChangesAsync();

            return RedirectToAction("Index", "Status");
        }


        [Authorize(Roles = "JobSeeker")]
        public async Task<IActionResult> StatusApplication(Guid userId)
        {
            var applicationList = await _dBContext.Application
                .Include(jl => jl.JobListing)
                    .ThenInclude(jc => jc.JobCategory)
                .Include(jl => jl.JobListing)
                    .ThenInclude(co => co.Company)
                        .ThenInclude(us => us.User)
                .Include(cv => cv.CV)
                .Include(st => st.Status)
                .Include(cl => cl.CoverLetter)
                .Include(js => js.JobSeeker)
                    .ThenInclude(u => u.User)
                .Where(jl => jl.jobSeekerId == userId.ToString())
                .ToListAsync();

            return View(applicationList);
        }

        [Authorize(Roles = "JobSeeker")]
        public async Task<IActionResult> DownloadCV(int cvId)
        {
            var cv = await _dBContext.CV.FirstOrDefaultAsync(c => c.cvId == cvId);

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

        [Authorize(Roles = "JobSeeker")]
        public async Task<IActionResult> DownloadCoverLetter(int coverLetterId)
        {
            var coverLetter = await _dBContext.CoverLetter.FirstOrDefaultAsync(c => c.coverLetterId == coverLetterId);

            if (coverLetter == null || string.IsNullOrEmpty(coverLetter.filePath))
            {
                return NotFound();
            }

            var resolvedFilePath = coverLetter.filePath.Replace("~/", string.Empty);

            var filePath = Path.Combine(_env.WebRootPath, resolvedFilePath);

            return PhysicalFile(filePath, "application/pdf", Path.GetFileName(filePath));
        }

        [Authorize(Roles = "JobSeeker")]
        public IActionResult JobApplication()
        {
            return View();
        }

        [Authorize(Roles = "JobSeeker")]
        private async Task<string> SavePDF(IFormFile file)
        {
            if (file != null)
            {
                var allowedExtensions = new[] { ".pdf" };
                var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
                if (!allowedExtensions.Contains(extension))
                {
                    return "nofile";
                }
                var fileName = Guid.NewGuid().ToString() + extension;
                var filePath = Path.Combine(_env.WebRootPath, "uploads", fileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
                return "~/uploads/" + fileName;
            }
            return "nofile";
        }
    }
}
