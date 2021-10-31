using System;
using System.Collections.Generic;
using Estore.Models;

namespace Estore.Areas.Admin.ViewModels.Order
{
    public class ManageOrderViewModel
    {
        public List<OrderItemViewModel> ListOfOrders { get; set; }
        public List<String> OrderIds { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsGetAll { get; set; }
        public double TotalEarn { get; set; }
        public long TotalOrders { get; set; }
        public string errorMessage { get; set; }

        public ManageOrderViewModel()
        {
            ListOfOrders = new List<OrderItemViewModel>();
            TotalEarn = 0;
            TotalOrders = 0;
            StartDate = DateTime.Now.AddDays(-7);
            EndDate = DateTime.Now;
            IsGetAll = false;
        }

        public ManageOrderViewModel(List<OrderItemViewModel> listOfOrders, DateTime startDate, DateTime endDate, bool isGetAll, double totalEarn, long totalOrders, List<string> orderIds)
        {
            ListOfOrders = listOfOrders;
            StartDate = startDate;
            EndDate = endDate;
            IsGetAll = isGetAll;
            TotalEarn = totalEarn;
            TotalOrders = totalOrders;
            OrderIds = orderIds;
        }
    }
}
