﻿using Microsoft.AspNetCore.Mvc;

namespace JAS.Controllers
{
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
