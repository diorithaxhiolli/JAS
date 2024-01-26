using Microsoft.AspNetCore.Mvc;

namespace JAS.Controllers
{
    public class ApplicationController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult JobApplication()
        {
            return View();
        }
    }
}
