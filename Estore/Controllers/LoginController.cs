using System;
using Microsoft.AspNetCore.Mvc;

namespace Estore.Controllers
{
    public class Login : Controller
    {
        public Login()
        {
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
