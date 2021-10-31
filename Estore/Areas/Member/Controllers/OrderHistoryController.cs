using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataAccess.UnitOfWork;
using Estore.Areas.Member.ViewModels.OrderHistory;
using Estore.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Estore.Areas.Member.Controllers
{
    [Area("Member")]
    public class OrderHistoryController : Controller
    {
        private IUnitOfWorkFactory _unitOfWorkFactory;

        public OrderHistoryController(IUnitOfWorkFactory unitOfWorkFactory)
        {
            _unitOfWorkFactory = unitOfWorkFactory;
        }

        [HttpGet]
        public IActionResult Index()
        {

            List<OrderItemViewModel> orderItemViewModels = new List<OrderItemViewModel>();
            using (var work = _unitOfWorkFactory.UnitOfWork)
            {
                String userId = HttpContext.Session.GetString("UserId");
                var orders = work.OrderRepository.GetByMemberId(userId);

                orders.ForEach(order =>
                {
                    List<OrderDetailViewModel> orderDetailViewModels = new List<OrderDetailViewModel>();

                    var orderDetails = work.OrderDetailRepository.GetByOrderId(order.Id);

                    orderDetails.ForEach(orderDetail =>
                    {
                        orderDetailViewModels.Add(new OrderDetailViewModel(orderDetail.ProductId, orderDetail.Product.Name, orderDetail.Quantity, orderDetail.UnitPrice));
                    });

                    OrderViewModel orderViewModel = new OrderViewModel(order.Member.Email, order.OrderDate, order.RequiredDate, order.ShippedDate, order.Freight);

                    orderItemViewModels.Add(new OrderItemViewModel(orderDetailViewModels, orderViewModel));
                });

                OrderHistoryViewModel viewModel = new OrderHistoryViewModel(orderItemViewModels);
                return View(viewModel);
            }
        }
    }
}
