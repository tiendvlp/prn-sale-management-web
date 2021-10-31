using System;
using System.Collections.Generic;
using Estore.Areas.Admin.ViewModels.Product;
using Estore.Models;

namespace Estore.Areas.Admin.ViewModels.Order
{
    public class CreateOrderViewModel
    {
        public List<OrderDetailViewModel> OrderDetails { get; set; }
        public OrderViewModel Order { get; set; }
        public string ErrorMessage { get; set; }
        public bool IsSuccess { get; set; }
        public bool AllProductsQuantitySetToNone { get; set; }

        public CreateOrderViewModel(OrderViewModel order)
        {
            IsSuccess = false;
            Order = order;
        }


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
