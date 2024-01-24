using JAS.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using JAS.Areas.Identity.Data;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using JAS.Models.Domain;
using System.Diagnostics.Metrics;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace JAS.Controllers
{
    public class AccountController : Controller
    {
        private readonly ILogger<AccountController> _logger;
        private readonly UserManager<JASUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly JASContext _dBContext;

        public AccountController(JASContext dBContext, ILogger<AccountController> logger, UserManager<JASUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _logger = logger;
            _userManager = userManager;
            _roleManager = roleManager;
            _dBContext = dBContext;
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

            return View("AddCompany", viewModel);
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

                return View("AddCompany", viewModel);
            }
            return View("Index");
        }

        [HttpPost]
        public async Task<IActionResult> AddCompanyOnPost(Company model) 
        {
            var company = new Company()
            {
                companyId = model.companyId,
                name = model.name,
                description = model.description,
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

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}