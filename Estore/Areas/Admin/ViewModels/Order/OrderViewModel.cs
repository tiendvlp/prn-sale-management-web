using System;
using System.ComponentModel.DataAnnotations;

namespace Estore.Areas.Admin.ViewModels.Order
{
    public class OrderViewModel
    {
        [Required]
        public String MemberEmail { get; set; }
        [Required(ErrorMessage = "Your OrderDate can not be empty")]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime OrderDate { get; set; }
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        [Required(ErrorMessage = "Your required date can not be empty")]
        public DateTime RequiredDate { get; set; }
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        [Required(ErrorMessage = "Your shipped date can not be empty")]
        public DateTime ShippedDate { get; set; }
        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Your freight is invalid")]
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

        public OrderViewModel(DateTime orderDate, DateTime requiredDate, DateTime shippedDate, Double freight)
        {
            Freight = freight;
            OrderDate = orderDate;
            RequiredDate = requiredDate;
            ShippedDate = shippedDate;
        }
    }
}
