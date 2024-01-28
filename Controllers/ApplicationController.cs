using JAS.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using JAS.Models.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication;
using JAS.Models.Domain.CompositeModel;

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

        [HttpGet]
        public async Task<IActionResult> Write()
        {
            //NEEDS CHANGE LATER, ONLY FOR TESTING
            int positionId = 10;
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

        public IActionResult JobApplication()
        {
            return View();
        }

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
