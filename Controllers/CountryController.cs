using Microsoft.AspNetCore.Mvc;

namespace JAS.Controllers
{
    public class CountryController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
