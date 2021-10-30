using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Estore.Areas.Admin.ViewModels.Product
{
    public class ProductFilterViewModel
    {
        public string ProductId { get; set; }
        public string ProductName { get; set; }
        [Range(0, int.MaxValue, ErrorMessage = "Please enter a value bigger than {0}")]
        [DefaultValue(0)]
        public int MinQuantity { get; set; }
        [Range(0, int.MaxValue, ErrorMessage = "Please enter a value bigger than {0}")]
        [DefaultValue(200000)]
        public int MaxQuantity { get; set; }
        [Range(0, double.MaxValue, ErrorMessage = "Please enter a value bigger than {0}")]
        [DefaultValue(0)]
        public double MinPrice { get; set; }
        [Range(0, double.MaxValue, ErrorMessage = "Please enter a value bigger than {0}")]
        [DefaultValue(200000)]
        public double MaxPrice { get; set; }
        public bool IsIncludePrice { get; set; }
        public bool IsIncludeQuantity { get; set; }

        public ProductFilterViewModel()
        {
            IsIncludePrice = true;
            IsIncludeQuantity = true;
            MinQuantity = 0;
            MaxQuantity = 2000000;
            MinPrice = 0;
            MaxPrice = 2000000;

        }

        public ProductFilterViewModel(string productId, string productName, int minQuantity, int maxQuantity, double minPrice, double maxPrice)
        {
            ProductId = productId;
            ProductName = productName;
            MinQuantity = minQuantity;
            MaxQuantity = maxQuantity;
            MinPrice = minPrice;
            MaxPrice = maxPrice;
        }
    }
}
