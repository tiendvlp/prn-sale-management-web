using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace Estore.Areas.Admin.ViewModels.Product
{
    public class UpdateProductViewModel
    {
        public string ProductId { get; set; }
        [BindProperty]
        public ProductViewModel Product { get; set; }
        public string ErrorMessage { get; set; }
        public IEnumerable<CategoryViewModel> Categories { get; set; }
        public IEnumerable<String> WeightUnits { get; set; }

        public UpdateProductViewModel(string productId, ProductViewModel product, IEnumerable<CategoryViewModel> categories, IEnumerable<string> weightUnits)
        {
            ProductId = productId;
            Product = product;
            Categories = categories;
            WeightUnits = weightUnits;
        }

        public UpdateProductViewModel(string errorMessage)
        {
            ErrorMessage = errorMessage;
        }

        public UpdateProductViewModel()
        {
        }
    }
}
