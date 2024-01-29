using JAS.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using JAS.Areas.Identity.Data;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using JAS.Models.Domain;
using System.Diagnostics.Metrics;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using Microsoft.EntityFrameworkCore;

namespace JAS.Controllers
{
    public class AccountController : Controller
    {
        private readonly ILogger<AccountController> _logger;
        private readonly UserManager<JASUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly JASContext _dBContext;
        private readonly IWebHostEnvironment _env;

        public AccountController(JASContext dBContext, ILogger<AccountController> logger, UserManager<JASUser> userManager, RoleManager<IdentityRole> roleManager, IWebHostEnvironment env)
        {
            _logger = logger;
            _userManager = userManager;
            _roleManager = roleManager;
            _dBContext = dBContext;
            _env = env;
        }

        public IActionResult AccountType(Guid userId)
        {
            ViewData["Title"] = "Account Type";
            ViewBag.UserId = userId;
            return View();
        }

        public async Task<IActionResult> SetSeeker(Guid userId)
        {
            await AssignRoleToUsers(userId, "JobSeeker");

            var viewModel = new JobSeeker()
            {
                jobSeekerId = userId.ToString(),
            };

            await _dBContext.JobSeeker.AddAsync(viewModel);
            await _dBContext.SaveChangesAsync();

            return RedirectToAction("Success", "Message");
        }

        public async Task<IActionResult> SetCompany(Guid userId)
        {
            await AssignRoleToUsers(userId, "Company");

            var viewModel = new Company()
            {
                companyId = userId.ToString(),
            };

            return RedirectToAction("AddCompany", viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> AddCompany(Company model)
        {
            var user = await _userManager.FindByIdAsync(model.companyId.ToString());

            if (user != null)
            {
                var viewModel = new Company()
                {
                    companyId = user.Id
                };

                ViewBag.CityList = await _dBContext.City.ToListAsync();

                return View("AddCompany", viewModel);
            }
            return View("Index");
        }

        [HttpPost]
        public async Task<IActionResult> AddCompanyOnPost(Company model, IFormFile filePath)
        {
            if (model == null)
            {
                return RedirectToAction("Oops", "Message");
            }

            var imagePath = await SaveImage(filePath);

            var company = new Company()
            {
                companyId = model.companyId,
                name = model.name,
                description = model.description,
                imagePath = imagePath,
                cityId = model.cityId
            };


            await _dBContext.Company.AddAsync(company);
            await _dBContext.SaveChangesAsync();
            return RedirectToAction("Success", "Message");
        }

        private async Task AssignRoleToUsers(Guid userId, string roleName)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());

            if (user != null)
            {
                var roleExists = await _roleManager.RoleExistsAsync(roleName);
                if (!roleExists)
                {
                    await _roleManager.CreateAsync(new IdentityRole(roleName));
                }

                await _userManager.AddToRoleAsync(user, roleName);
            }
        }

        private async Task<string> SaveImage(IFormFile file)
        {
            if (file != null)
            {
                var allowedExtensions = new[] { ".jpg", ".png" };
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

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}