using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace Estore.Areas.Admin.ViewModels.Product
{
    public class ProductViewModel
    {
        [BindProperty]
        [Required]
        public CategoryViewModel Category { get; set; }
        [Required]
        public String Name { get; set; }
        [Required]
        public Double Weight { get; set; }
        [Required]
        public Double Price { get; set; }
        [Required]
        public int Quantity { get; set; }
        [Required]
        public string Unit { get; set; }

        public ProductViewModel()
        {

        }

        public ProductViewModel(CategoryViewModel category, string name, double weight, double price, int quantity, string unit)
        {
            Category = category;
            Name = name;
            Weight = weight;
            Price = price;
            Quantity = quantity;
            Unit = unit;
        }
    }
}
