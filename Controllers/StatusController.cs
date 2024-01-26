using Microsoft.AspNetCore.Mvc;

namespace JAS.Controllers
{
    public class StatusController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
