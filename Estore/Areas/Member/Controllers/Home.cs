using System;
using Microsoft.AspNetCore.Mvc;

namespace Estore.Areas.Member.Controllers
{
    [Area("Member")]
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
