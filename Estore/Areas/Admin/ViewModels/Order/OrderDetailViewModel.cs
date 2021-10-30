using System;
using System.ComponentModel.DataAnnotations;

namespace Estore.Areas.Admin.ViewModels.Order
{
    public class OrderDetailViewModel
    {
        public string ProductId { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public Double ProductPrice { get; set; }

        public OrderDetailViewModel()
        {
        }

        public OrderDetailViewModel(string productId, string productName, int quantity, double productPrice)
        {
            ProductId = productId;
            ProductName = productName;
            Quantity = quantity;
            ProductPrice = productPrice;
        }
    }
}
