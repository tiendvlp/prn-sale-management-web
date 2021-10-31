using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Estore.Models
{
    public class OrderItemViewModel
    {
        public List<OrderDetailViewModel> OrderDetails { get; set; }
        public OrderViewModel Info { get; set; }

        public OrderItemViewModel()
        {

        }
        public OrderItemViewModel(List<OrderDetailViewModel> orderDetails, OrderViewModel order)
        {
            OrderDetails = orderDetails;
            Info = order;
        }
    }
}