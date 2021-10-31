using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Estore.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Estore.Areas.Member.ViewModels.OrderHistory
{
    public class OrderHistoryViewModel
    {
        public List<OrderItemViewModel> ListOfOrders { get; set; }

        public OrderHistoryViewModel()
        {
            ListOfOrders = new List<OrderItemViewModel>();
        }

        public OrderHistoryViewModel(List<OrderItemViewModel> listOfOrders)
        {
            ListOfOrders = listOfOrders;
        }
    }
}
