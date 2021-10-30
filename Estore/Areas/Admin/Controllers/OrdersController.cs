using System;
using System.Collections.Generic;
using DataAccess.UnitOfWork;
using Estore.Areas.Admin.ViewModels.Order;
using Estore.Areas.Admin.ViewModels.Product;
using Microsoft.AspNetCore.Mvc;

namespace Estore.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class OrdersController : Controller
    {
        private IUnitOfWorkFactory _unitOfWorkFactory;

        public OrdersController(IUnitOfWorkFactory unitOfWorkFactory)
        {
            _unitOfWorkFactory = unitOfWorkFactory;
        }

        [HttpGet]
        public IActionResult Index(IEnumerable<string> productIds)
        {
            using (var work = _unitOfWorkFactory.UnitOfWork)
            {
                List<OrderDetailViewModel> orderDetailViewModels = new List<OrderDetailViewModel>();
                foreach (var id in productIds)
                {
                    var product = work.ProductRepository.GetById(id);
                    orderDetailViewModels.Add(new OrderDetailViewModel(product.Id, product.Name, (int)product.Quantity, product.Price));
                }

                CreateOrderViewModel viewModel = new CreateOrderViewModel(orderDetailViewModels);
                return View(viewModel);
            }
        }


        #region Api
        [HttpPost]
        public IActionResult Create(CreateOrderViewModel createOrderViewModel)
        {
            using (var work = _unitOfWorkFactory.UnitOfWork)
            {
                var member = work.MemberRepository.GetByEmail(createOrderViewModel.Order.MemberEmail);
                var orderViewModel = createOrderViewModel.Order;

                if (member == null)
                {
                    return Json(new
                    {
                        message = "Your member email does not exist",
                        success = false
                    });
                }

                if (orderViewModel.OrderDate > orderViewModel.RequiredDate)
                {
                    return Json(new
                    {
                        message = "Your required date have to be greater than order date",
                        success = false
                    });
                }

                if (orderViewModel.OrderDate > orderViewModel.ShippedDate)
                {
                    return Json(new
                    {
                        message = "Your shipped date have to be greater than order date",
                        success = false
                    });
                }

                BusinessObject.Order createdOrder = work.OrderRepository.Add(member.Id,
                                                                             orderViewModel.OrderDate,
                                                                             orderViewModel.RequiredDate,
                                                                             orderViewModel.ShippedDate,
                                                                             orderViewModel.Freight);
                foreach (var detailViewModel in createOrderViewModel.OrderDetails)
                {
                    work.OrderDetailRepository.Add(createdOrder.Id, detailViewModel.ProductId, detailViewModel.ProductPrice, detailViewModel.Quantity, 1);
                }

            }

            return Json(new
            {
                message = "Your order has been created successfully !!",
                success = true
            });
        }

    }
    #endregion
}
