using System;
using DataAccess.UnitOfWork;
using Estore.Areas.Admin.ViewModels.Member;
using Estore.Models;
using Microsoft.AspNetCore.Mvc;

namespace Estore.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class MembersController : Controller
    {
        private IUnitOfWorkFactory _unitOfWorkFactory;

        public MembersController(IUnitOfWorkFactory unitOfWorkFactory)
        {
            _unitOfWorkFactory = unitOfWorkFactory;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Update(string id)
        {
            using (var work = _unitOfWorkFactory.UnitOfWork)
            {
                var target = work.MemberRepository.GetById(id);

                if (target == null)
                {
                    return Json(new
                    {
                        success = false,
                        errorMessage = "Internal server error, member id can not be null"
                    });
                }
                UpdateMemberViewModel viewModel = new UpdateMemberViewModel(id, new MemberViewModel(target.Country, target.City, target.Password, target.Email, target.Name, target.CompanyName));
                return View(viewModel);
            }
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Update(UpdateMemberViewModel model)
        {
            using (var work = _unitOfWorkFactory.UnitOfWork)
            {
                if (work.MemberRepository.GetById(model.Id) == null)
                {
                    return Json(new
                    {
                        success = false,
                        errorMessage = "Internal server error, member id can not be null"
                    });
                }
                var target = model.Member;
                work.MemberRepository.Update(new BusinessObject.Member(target.Country, target.City, target.Password, target.Email, target.Name, model.Id, target.CompanyName));
                work.Save();
                var updatedModel = new UpdateMemberViewModel(model.Id, new MemberViewModel());
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        public IActionResult Create(CreateMemberViewModel model)
        {
            using (var work = _unitOfWorkFactory.UnitOfWork)
            {
                if (work.MemberRepository.GetByEmail(model.Member.Email) != null)
                {
                    CreateMemberViewModel viewModel = new CreateMemberViewModel("Your email already exist");
                    return View(viewModel);
                }

                var target = model.Member;
                work.MemberRepository.Add(target.Country, target.City, target.Password, target.Email, target.Name, target.CompanyName);
                work.Save();
                return RedirectToAction(nameof(Index));
            }
        }

        #region Api
        [HttpDelete]
        public IActionResult Delete(string id)
        {
            using (var work = _unitOfWorkFactory.UnitOfWork)
            {
                Console.WriteLine("Delete user" + id);
                var target = work.MemberRepository.GetById(id);
                if (target == null)
                {
                    return Json(new
                    {
                        success = false,
                        message = "Error while deleting"
                    });
                }
                work.MemberRepository.RemoveById(target.Id);
                work.Save();
                return Json(new
                {
                    success = true,
                    message = "Delete user success"
                });
            }
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            using (var work = _unitOfWorkFactory.UnitOfWork)
            {
                var allMembers = work.MemberRepository.GetAll();

                return Json(new
                {
                    data = allMembers
                });
            }
        }
    }

    #endregion
}
