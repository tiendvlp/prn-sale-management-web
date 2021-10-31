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
        public IActionResult Index(ManageOrderViewModel model)
        {
            using (var work = _unitOfWorkFactory.UnitOfWork)
            {
                model.errorMessage = "";

                List<OrderItemViewModel> orderItemViewModels = new List<OrderItemViewModel>();
                (List<OrderItemViewModel> orders, List<String> Ids, double totalEarn, long totalOrders) result;
                if (model.IsGetAll)
                {
                    result = _getOrderByTime(DateTime.MinValue, DateTime.MaxValue);
                }
                else
                {
                    result = _getOrderByTime(model.StartDate, model.EndDate);

                    if (model.StartDate > model.EndDate)
                    {
                        model.errorMessage = "Your start date have to before end date";
                        return View(model);
                    }

                }

                model.OrderIds = result.Ids;
                model.ListOfOrders = result.orders;
                model.TotalEarn = result.totalEarn;
                model.TotalOrders = result.totalOrders;

                return View(model);
            }
        }

        private (List<OrderItemViewModel> orders, List<String> Ids, double totalEarn, long totalOrders) _getOrderByTime(DateTime from, DateTime to)
        {
            List<OrderItemViewModel> orderItemViewModels = new List<OrderItemViewModel>();
            List<String> orderIds = new List<string>();
            double totalEarn = 0;
            long totalOrder = 0;
            using (var work = _unitOfWorkFactory.UnitOfWork)
            {
                var orders = work.OrderRepository.GetWithFilter(from, to);

                orders.ForEach(order =>
                {
                    totalOrder++;
                    List<OrderDetailViewModel> orderDetailViewModels = new List<OrderDetailViewModel>();

                    var orderDetails = work.OrderDetailRepository.GetByOrderId(order.Id);

                    orderDetails.ForEach(orderDetail =>
                    {
                        totalEarn += orderDetail.UnitPrice * orderDetail.Quantity;
                        // Wrong, we can not pass product name is null, but here we don't care @@
                        orderDetailViewModels.Add(new OrderDetailViewModel(orderDetail.ProductId, null, orderDetail.Quantity, orderDetail.UnitPrice));
                    });

                    OrderViewModel orderViewModel = new OrderViewModel(order.Member.Email, order.OrderDate, order.RequiredDate, order.ShippedDate, order.Freight);

                    orderItemViewModels.Add(new OrderItemViewModel(orderDetailViewModels, orderViewModel));
                    orderIds.Add(order.Id);
                });

                return (orderItemViewModels, orderIds, totalEarn, totalOrder);
            }
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

                Console.WriteLine("OrderDate: " + createOrderViewModel.Order.OrderDate.ToString("dd:MM:yyyy"));
                Console.WriteLine("ShippedDate: " + createOrderViewModel.Order.ShippedDate.ToString("dd:MM:yyyy"));
                Console.WriteLine("RequiredDate: " + createOrderViewModel.Order.RequiredDate.ToString("dd:MM:yyyy"));


                if (member == null)
                {
                    createOrderViewModel.ErrorMessage = "Your Member email does not exist";
                    return View(createOrderViewModel);
                }

                if (orderViewModel.OrderDate > orderViewModel.RequiredDate)
                {
                    createOrderViewModel.ErrorMessage = "Your required date can not go order date";
                    return View(createOrderViewModel);
                }

                if (orderViewModel.OrderDate > orderViewModel.ShippedDate)
                {
                    createOrderViewModel.ErrorMessage = "Your shipped date can not go before order date";
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
                int numberOfAddedOrderDetails = 0;
                foreach (var detailViewModel in createOrderViewModel.OrderDetails)
                {
                    if (detailViewModel.Quantity == 0)
                    {
                        continue;
                    }
                    numberOfAddedOrderDetails++;
                    work.OrderDetailRepository.Add(createdOrder.Id, detailViewModel.ProductId, detailViewModel.ProductPrice, detailViewModel.Quantity, 1);
                }

                if (numberOfAddedOrderDetails == 0)
                {
                    createOrderViewModel.AllProductsQuantitySetToNone = true;
                    return View(createOrderViewModel);
                }

                work.Save();
                createOrderViewModel.IsSuccess = true;
                return View(createOrderViewModel);
            }
        }

        [HttpPost]
        public IActionResult Delete([FromBody] string id)
        {
            Console.WriteLine("Delete: " + id);
            using (var work = _unitOfWorkFactory.UnitOfWork)
            {
                var details = work.OrderDetailRepository.GetByOrderId(id);
                if (details == null || details.Count == 0)
                {
                    return Json(new
                    {
                        success = true,
                        errorMessage = "There is no order with id: {" + id + "}"
                    });
                }
                work.OrderDetailRepository.RemoveByOrderId(id);
                work.OrderRepository.RemoveById(id);
                work.Save();
            }
            return Json(new
            {
                success = true
            });
        }
        #endregion
    }
}
