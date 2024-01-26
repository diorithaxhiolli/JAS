using Microsoft.AspNetCore.Mvc;

namespace JAS.Controllers
{
    public class EducationController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
