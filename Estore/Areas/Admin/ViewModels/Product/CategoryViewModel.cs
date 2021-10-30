using System;
using System.ComponentModel.DataAnnotations;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Estore.Areas.Admin.ViewModels.Product
{
    public class CategoryViewModel
    {
        [Required]
        public string Id { get; set; }
        [Required]
        public string Title { get; set; }

        public CategoryViewModel(string id, string title)
        {
            Title = title;
            Id = id;
        }

        public CategoryViewModel()
        {

        }
    }
}
