using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace Estore.Areas.Admin.ViewModels.Product
{
    public class ManageProductsViewModel
    {
        [BindProperty]
        public ProductFilterViewModel Filter { get; set; }

        public ManageProductsViewModel(ProductFilterViewModel filter)
        {
            Filter = filter;
        }

        public ManageProductsViewModel()
        {
            Filter = new ProductFilterViewModel();
        }

    }
}
