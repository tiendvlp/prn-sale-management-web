using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace Estore.Areas.Admin.ViewModels.Product
{
    public class CreateProductViewModel
    {
        [BindProperty]
        public ProductViewModel Product { get; set; }
        public string ErrorMessage { get; set; }
        public IEnumerable<CategoryViewModel> Categories { get; set; }
        public IEnumerable<String> WeightUnits { get; set; }

        public CreateProductViewModel(IEnumerable<CategoryViewModel> categories, IEnumerable<string> weightUnits)
        {
            Categories = categories;
            WeightUnits = weightUnits;
        }

        public CreateProductViewModel(string errorMessage)
        {
            ErrorMessage = errorMessage;
        }

        public CreateProductViewModel()
        {
        }
    }
}
