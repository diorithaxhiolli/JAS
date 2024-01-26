using Microsoft.AspNetCore.Mvc;

namespace JAS.Controllers
{
    public class JobCategoryController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
