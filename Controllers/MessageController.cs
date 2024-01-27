using Microsoft.AspNetCore.Mvc;

namespace JAS.Controllers
{
    public class MessageController : Controller
    {
        public IActionResult Success()
        {
            return View();
        }

        public IActionResult Oops()
        {
            return View();
        }
    }
}
