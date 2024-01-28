using JAS.Areas.Identity.Data;
using JAS.Models.Domain.CompositeModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using JAS.Models.Domain;

namespace JAS.Controllers
{
    public class StatusController : Controller
    {
        private readonly JASContext _dBContext;
        private readonly UserManager<JASUser> _userManager;

        public StatusController(JASContext context, UserManager<JASUser> userManager)
        {
            _dBContext = context;
            _userManager = userManager;
        }


        public async Task<IActionResult> Index()
        {
            StatusComposite statusComposite = new StatusComposite()
            {
                StatusList = await _dBContext.Status.ToListAsync(),
                Status = new Status()
            };

            return View(statusComposite);
        }

        public async Task<IActionResult> AddStatusOnPost(StatusComposite model)
        {
            if(model == null)
            {
                return RedirectToAction("Index");
            }

            var status = new Status()
            {
                statusName = model.Status.statusName,
            };

            await _dBContext.Status.AddAsync(status);
            await _dBContext.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> View(int statusId)
        {
            var status = await _dBContext.Status.FindAsync(statusId);

            if (status != null)
            {
                var viewModel = new Status()
                {
                    statusId = status.statusId,
                    statusName = status.statusName,
                };

                return View("View", viewModel);
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> ViewStatusOnPost(Status model)
        {
            var status = await _dBContext.Status.FindAsync(model.statusId);

            if (status != null)
            {
                status.statusName = model.statusName;

                await _dBContext.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> DeleteStatus(Status model)
        {
            var status = await _dBContext.Status.FindAsync(model.statusId);

            if (status != null)
            {
                _dBContext.Status.Remove(status);
                await _dBContext.SaveChangesAsync();

                return RedirectToAction("Index");
            }

            return RedirectToAction("Index");
        }
    }
}
