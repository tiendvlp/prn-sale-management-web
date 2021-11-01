using System;
using DataAccess.UnitOfWork;
using Desktop.common;
using Estore.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Estore.Controllers
{
    public class AuthController : Controller
    {
        private IUnitOfWorkFactory _unitOfWorkFactory;
        private AppSetting _appSetting;

        public AuthController(IUnitOfWorkFactory unitOfWorkFactory, AppSetting appSetting)
        {
            _appSetting = appSetting;
            _unitOfWorkFactory = unitOfWorkFactory;
        }

        public IActionResult Signout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction(nameof(Login));
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                AdminAccount adminAccount = _appSetting.Admins.Find(item => item.Email.Equals(model.Input.Email));
                if (adminAccount != null && adminAccount.Password.Equals(model.Input.Password))
                {
                    HttpContext.Session.SetString("Role", "Admin");
                    HttpContext.Session.SetString("WelcomeString", "Admin");
                    return RedirectToAction("Index", "Members", new { area = "Admin" });
                }
                using (var work = _unitOfWorkFactory.UnitOfWork)
                {
                    var user = work.MemberRepository.GetByEmail(model.Input.Email);
                    if (user != null)
                    {
                        HttpContext.Session.SetString("UserId", user.Id);
                        HttpContext.Session.SetString("Role", "Member");
                        HttpContext.Session.SetString("WelcomeString", user.Name);
                        Console.WriteLine("Login success");
                        return RedirectToAction("Index", "UserInfo", new { area = "Member" });

                    }
                    Console.WriteLine("Login Failed");
                    LoginViewModel errorModel = new LoginViewModel();
                    errorModel.ErrorMessage = "Your user name or password is incorrect";
                    return View(errorModel);
                }
            }
            return View();
        }
    }
}
