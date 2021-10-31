using System;
using System.Collections.Generic;
using Estore.Areas.Admin.ViewModels.Product;

namespace Estore.Areas.Admin.ViewModels.Order
{
    public class UpdateOrderViewModel
    {
        public List<OrderDetailViewModel> OrderDetails { get; set; }
        public string OrderId { get; set; }
        public OrderViewModel Order { get; set; }
        public string ErrorMessage { get; set; }
        public bool IsSuccess { get; set; }
        public bool AllProductsQuantitySetToNone { get; set; }

        public UpdateOrderViewModel(OrderViewModel order, string orderId)
        {
            IsSuccess = false;
            Order = order;
            OrderId = orderId;
        }


        public UpdateOrderViewModel(List<OrderDetailViewModel> orderDetails, OrderViewModel order, string orderId)
        {
            OrderDetails = orderDetails;
            Order = order;
            OrderId = orderId;
        }

        public UpdateOrderViewModel()
        {
        }
    }
}
