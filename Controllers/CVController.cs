using Microsoft.AspNetCore.Mvc;

namespace JAS.Controllers
{
    public class CVController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
