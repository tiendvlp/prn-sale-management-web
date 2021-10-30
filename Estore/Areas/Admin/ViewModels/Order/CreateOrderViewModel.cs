using System;
using System.Collections.Generic;
using Estore.Areas.Admin.ViewModels.Product;

namespace Estore.Areas.Admin.ViewModels.Order
{
    public class CreateOrderViewModel
    {
        public IEnumerable<OrderDetailViewModel> OrderDetails { get; set; }
        public OrderViewModel Order { get; set; }

        public CreateOrderViewModel(OrderViewModel order)
        {
            Order = order;
        }

        public string ErrorMessage { get; set; }

        public CreateOrderViewModel(IEnumerable<OrderDetailViewModel> orderDetails)
        {
            OrderDetails = orderDetails;
        }

        public CreateOrderViewModel()
        {
        }
    }
}
