using Microsoft.AspNetCore.Mvc;

namespace JAS.Controllers
{
    public class JobListingController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult JobCreation()
        {
            return View();
        }
    }
}
