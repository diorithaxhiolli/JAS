using Microsoft.AspNetCore.Mvc;

namespace JAS.Controllers
{
    public class JobController : Controller
    {
        public IActionResult JobApplication()
        {
            return View();
        }

        public IActionResult JobCreation()
        {
            return View();
        }

    }
}
