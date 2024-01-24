using Microsoft.AspNetCore.Mvc;

namespace JAS.Controllers
{
    public class Message : Controller
    {
        public IActionResult Success()
        {
            return View();
        }
    }
}
