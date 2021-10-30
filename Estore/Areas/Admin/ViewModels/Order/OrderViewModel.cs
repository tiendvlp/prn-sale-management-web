using System;
using System.ComponentModel.DataAnnotations;

namespace Estore.Areas.Admin.ViewModels.Order
{
    public class OrderViewModel
    {
        [Required]
        public String MemberEmail { get; set; }
        [Required]
        public DateTime OrderDate { get; set; }
        [Required]
        public DateTime RequiredDate { get; set; }
        [Required]
        public DateTime ShippedDate { get; set; }
        [Required]
        [Range(0, Double.MaxValue, ErrorMessage = "The frieght have to greater than value {0}")]
        public Double Freight { get; set; }


        public OrderViewModel()
        {
        }

        public OrderViewModel(string memberEmail, DateTime orderDate, DateTime requiredDate, DateTime shippedDate, double freight)
        {
            MemberEmail = memberEmail;
            OrderDate = orderDate;
            RequiredDate = requiredDate;
            ShippedDate = shippedDate;
            Freight = freight;
        }
    }
}
