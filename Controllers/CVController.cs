using JAS.Areas.Identity.Data;
using JAS.Models.Domain.CompositeModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using JAS.Models.Domain;
using NuGet.Packaging.Signing;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.AspNetCore.Authorization;

namespace JAS.Controllers
{
    public class CVController : Controller
    {
        private readonly JASContext _dBContext;
        private readonly IWebHostEnvironment _env;
        private readonly UserManager<JASUser> _userManager;


        public CVController(JASContext context, IWebHostEnvironment env, UserManager<JASUser> userManager)
        {
            _dBContext = context;
            _env = env;
            _userManager = userManager;
        }

        [Authorize(Roles = "JobSeeker")]
        public async Task<IActionResult> Index()
        {
            var currentUser = await _userManager.GetUserAsync(User);

            var cvList = await _dBContext.CV
                .Where(cv => cv.jobSeekerId == currentUser.Id)
                .ToListAsync();

            return View(cvList);
        }

        [Authorize(Roles = "JobSeeker")]
        public IActionResult Create()
        {
            return View();
        }

        [Authorize(Roles = "JobSeeker")]
        [HttpPost]
        public async Task<IActionResult> AddCVOnPost(CVComposite postModel, IFormFile filePath)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            var cvModel = postModel.CV;
            var educationModel = postModel.Education;
            var experienceModel = postModel.Experience;

            var filePDF = await SavePDF(filePath);

            var newCv = new CV()
            {
                name = cvModel.name,
                summary = cvModel.summary,
                jobSeekerId = currentUser.Id,
                filePath = filePDF.ToString(),
            };

            await _dBContext.CV.AddAsync(newCv);
            await _dBContext.SaveChangesAsync();

            int newCvId = newCv.cvId;

            var newEducation = new Education()
            {
                institution = educationModel.institution,
                degree = educationModel.degree,
                field = educationModel.field,
                graduationDate = educationModel.graduationDate,
                cvId = newCvId
            };

            var newExperience = new Experience()
            {
                title = experienceModel.title,
                description = experienceModel.description,
                cvId = newCvId
            };

            await _dBContext.Education.AddAsync(newEducation);
            await _dBContext.Experience.AddAsync(newExperience);
            await _dBContext.SaveChangesAsync();

            return RedirectToAction("Index", "CV");
        }

        [Authorize(Roles = "JobSeeker")]
        [HttpGet]
        public async Task<IActionResult> View(int cvId)
        {
            var cvWithRelatedData = _dBContext.CV
            .Include(cv => cv.Education)
            .Include(cv => cv.Experience)
            .FirstOrDefault(cv => cv.cvId == cvId);

            var cvModel = cvWithRelatedData;
            var educationModel = cvWithRelatedData.Education.ToList().FirstOrDefault();
            var experienceModel = cvWithRelatedData.Experience.ToList().FirstOrDefault();

            if (cvWithRelatedData != null)
            {
                var cvViewModel = new CV()
                {
                    cvId = cvModel.cvId,
                    name = cvModel.name,
                    jobSeekerId = cvModel.jobSeekerId,
                    summary = cvModel.summary,
                    filePath = cvModel.filePath,
                };

                var educationViewModel = new Education()
                {
                    educationId = educationModel.educationId,
                    institution = educationModel.institution,
                    degree = educationModel.degree,
                    field = educationModel.field,
                    graduationDate = educationModel.graduationDate,
                };

                var experienceViewModel = new Experience()
                {
                    experienceId = experienceModel.experienceId,
                    title = experienceModel.title,
                    description = experienceModel.description,
                };

                var cvCompositeViewModel = new CVComposite
                {
                    CV = cvViewModel,
                    Education = educationViewModel,
                    Experience = experienceViewModel,
                };

                return View("View", cvCompositeViewModel);
            }
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "JobSeeker")]
        [HttpPost]
        public async Task<IActionResult> UpdateCVOnPost(CVComposite model, IFormFile filePath)
        {
            var updatedCv = await UpdateCV(model.CV, filePath);
            var updatedEducation = await UpdateEducation(model.Education);
            var updatedExperience = await UpdateExperience(model.Experience);

            if(updatedCv != false && updatedEducation != false && updatedExperience != false)
            {
                return RedirectToAction("Index");
            }


            return RedirectToAction("Index");
        }

        [Authorize(Roles = "JobSeeker")]
        public async Task<IActionResult> DeleteCV(CVComposite model)
        {
            var isDeleted = await DeleteCVWithRelatedData(model.CV, model.Education, model.Experience);

            if (isDeleted != null)
            {
                return RedirectToAction("Index");
            }

            return RedirectToAction("Index");
        }

        [Authorize(Roles = "JobSeeker")]
        public async Task<Boolean> DeleteCVWithRelatedData(CV cvModel, Education eduModel, Experience expModel)
        {
            var cv = await _dBContext.CV.FindAsync(cvModel.cvId);
            var education = await _dBContext.Education.FindAsync(eduModel.educationId);
            var experience = await _dBContext.Experience.FindAsync(expModel.experienceId);

            if (education == null || experience == null || cv == null)
            {
                return false;
            }

            _dBContext.Education.Remove(education);
            _dBContext.Experience.Remove(experience);
            await _dBContext.SaveChangesAsync();

            _dBContext.CV.Remove(cv);
            await _dBContext.SaveChangesAsync();

            return true;
        }

        [Authorize(Roles = "JobSeeker")]
        public async Task<Boolean> UpdateCV(CV model, IFormFile filePath)
        {
            if (model == null)
            {
                return false;
            }

            var cvData = await _dBContext.CV.FindAsync(model.cvId);

            cvData.name = model.name;
            cvData.summary = model.summary;
            if (filePath != null)
            {
                var filePDF = await SavePDF(filePath);
                cvData.filePath = filePDF.ToString();
            }

            await _dBContext.SaveChangesAsync();

            return true;
        }

        [Authorize(Roles = "JobSeeker")]
        public async Task<Boolean> UpdateEducation(Education model)
        {
            if (model == null)
            {
                return false;
            }

            var educationData = await _dBContext.Education.FindAsync(model.educationId);

            educationData.institution = model.institution;
            educationData.degree = model.degree;
            educationData.field = model.field;
            educationData.graduationDate = model.graduationDate;

            await _dBContext.SaveChangesAsync();

            return true;
        }

        [Authorize(Roles = "JobSeeker")]
        public async Task<Boolean> UpdateExperience(Experience model)
        {
            if (model == null)
            {
                return false;
            }

            var experienceData = await _dBContext.Experience.FindAsync(model.experienceId);

            experienceData.title = model.title;
            experienceData.description = model.description;

            await _dBContext.SaveChangesAsync();

            return true;
        }


        [Authorize(Roles = "JobSeeker")]
        public async Task<IActionResult> DownloadFile(int cvId)
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
