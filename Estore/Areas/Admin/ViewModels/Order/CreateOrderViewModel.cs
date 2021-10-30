using System;
using System.Collections.Generic;
using Estore.Areas.Admin.ViewModels.Product;

namespace Estore.Areas.Admin.ViewModels.Order
{
    public class CreateOrderViewModel
    {
        public List<OrderDetailViewModel> OrderDetails { get; set; }
        public OrderViewModel Order { get; set; }

        public CreateOrderViewModel(OrderViewModel order)
        {
            Order = order;
        }

        public string ErrorMessage { get; set; }

        public CreateOrderViewModel(List<OrderDetailViewModel> orderDetails, OrderViewModel order)
        {
            OrderDetails = orderDetails;
            Order = order;
        }

        public CreateOrderViewModel()
        {
        }
    }
}
