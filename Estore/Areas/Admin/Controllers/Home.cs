using System;
using Microsoft.AspNetCore.Mvc;

namespace Estore.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class Home : Controller
    {
        public Home()
        {
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
