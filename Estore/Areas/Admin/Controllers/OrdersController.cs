using System;
using System.Collections.Generic;
using System.Linq;
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

        public class CreateOrderRequestParam
        {
            public List<String> ProductIds { get; set; }
        }

        [HttpGet]
        public IActionResult Create(List<String> productIds)
        {
            using (var work = _unitOfWorkFactory.UnitOfWork)
            {
                List<OrderDetailViewModel> orderDetailViewModels = new List<OrderDetailViewModel>();
                double freight = 0;
                foreach (var id in productIds)
                {
                    Console.WriteLine("Add order detail");
                    var product = work.ProductRepository.GetById(id);

                    orderDetailViewModels.Add(new OrderDetailViewModel(product.Id, product.Name, 1, product.Price));
                    freight += product.Weight * 0.5;
                }

                DateTime orderDate = DateTime.Now;
                DateTime shippedDate = orderDate.AddDays(3);
                DateTime requiredDate = shippedDate;

                OrderViewModel orderViewModel = new OrderViewModel(orderDate, shippedDate, requiredDate, freight);

                CreateOrderViewModel viewModel = new CreateOrderViewModel(orderDetailViewModels, orderViewModel);
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
                    createOrderViewModel.ErrorMessage = "Your Member email does not exist";
                    return View(createOrderViewModel);
                }

                if (orderViewModel.OrderDate > orderViewModel.RequiredDate)
                {
                    createOrderViewModel.ErrorMessage = "Your required date have to be greater than order date";
                    return View(createOrderViewModel);
                }

                if (orderViewModel.OrderDate > orderViewModel.ShippedDate)
                {
                    createOrderViewModel.ErrorMessage = "Your shipped date have to be greater than order date";
                    return View(createOrderViewModel);
                }

                if (createOrderViewModel.OrderDetails == null || createOrderViewModel.OrderDetails.ToList().Count == 0)
                {
                    createOrderViewModel.ErrorMessage = "Your order details is empty";
                    return View(createOrderViewModel);
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

                work.Save();

                createOrderViewModel.IsSuccess = true;
                return View(createOrderViewModel);
            }
        }
        #endregion
    }
}
