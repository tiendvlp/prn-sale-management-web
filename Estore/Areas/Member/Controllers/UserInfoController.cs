using System;
using DataAccess.UnitOfWork;
using Estore.Areas.Member.ViewModels.UserInfo;
using Estore.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Estore.Areas.Member.Controllers
{
    [Area("Member")]
    public class UserInfoController : Controller
    {
        private IUnitOfWorkFactory _unitOfWorkFactory;

        public UserInfoController(IUnitOfWorkFactory unitOfWorkFactory)
        {
            _unitOfWorkFactory = unitOfWorkFactory;
        }


        [HttpGet]
        public IActionResult Index()
        {
            using (var work = _unitOfWorkFactory.UnitOfWork)
            {
                String userId = HttpContext.Session.GetString("UserId");

                var target = work.MemberRepository.GetById(userId);
                if (target == null)
                {
                    return Json(new
                    {
                        success = false,
                        errorMessage = "Internal server error, member id can not be null"
                    });
                }
                UserInfoViewModel viewModel = new UserInfoViewModel(new MemberViewModel(target.Country, target.City, target.Password, target.Email, target.Name, target.CompanyName));
                return View(viewModel);
            }
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Update(UserInfoViewModel model)
        {
            using (var work = _unitOfWorkFactory.UnitOfWork)
            {
                String userId = HttpContext.Session.GetString("UserId");
                if (work.MemberRepository.GetById(userId) == null)
                {
                    return Json(new
                    {
                        success = false,
                        errorMessage = "Internal server error, member does not exist"
                    });
                }
                var target = model.Member;
                work.MemberRepository.Update(new BusinessObject.Member(target.Country, target.City, target.Password, target.Email, target.Name, userId, target.CompanyName));
                work.Save();
                HttpContext.Session.SetString("WelcomeString", target.Name);
                return RedirectToAction(nameof(Index));
            }
        }
    }
}
