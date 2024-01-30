using JAS.Models.Domain.CompositeModel;
using JAS.Models.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using JAS.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace JAS.Controllers
{
    public class JobCategoryController : Controller
    {
        private readonly JASContext _dBContext;
        private readonly UserManager<JASUser> _userManager;

        public JobCategoryController(JASContext context, UserManager<JASUser> userManager)
        {
            _dBContext = context;
            _userManager = userManager;
        }

        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Index()
        {
            JobCategoryComposite jobCategoryComposite = new JobCategoryComposite()
            {
                JobCategoryList = await _dBContext.JobCategory.ToListAsync(),
                JobCategory = new JobCategory()
            };

            return View(jobCategoryComposite);
        }

        [Authorize(Roles = "Administrator")]
        [HttpPost]
        public async Task<IActionResult> AddCategory(JobCategoryComposite compositeModel)
        {
            if (compositeModel == null)
            {
                return RedirectToAction("Index");
            }

            var category = new JobCategory()
            {
                name = compositeModel.JobCategory.name,
                description = compositeModel.JobCategory.description,
            };

            await _dBContext.JobCategory.AddAsync(category);
            await _dBContext.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Administrator")]
        [HttpGet]
        public async Task<IActionResult> ViewCategory(int categoryId)
        {
            var category = await _dBContext.JobCategory.FindAsync(categoryId);

            if (category != null)
            {
                var viewModel = new JobCategory()
                {
                    categoryId = category.categoryId,
                    name = category.name,
                    description = category.description,
                };

                return View("ViewCategory", viewModel);
            }
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Administrator")]
        [HttpPost]
        public async Task<IActionResult> ViewCategoryOnPost(JobCategory model)
        {
            var category = await _dBContext.JobCategory.FindAsync(model.categoryId);

            if (category != null)
            {
                category.name = model.name;
                category.description = model.description;

                await _dBContext.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> DeleteCategory(JobCategory model)
        {
            var category = await _dBContext.JobCategory.FindAsync(model.categoryId);

            if (category != null)
            {
                _dBContext.JobCategory.Remove(category);
                await _dBContext.SaveChangesAsync();

                return RedirectToAction("Index");
            }

            return RedirectToAction("Index");
        }
    }
}
