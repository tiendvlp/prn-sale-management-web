using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using DataAccess.UnitOfWork;
using Estore.Areas.Admin.ViewModels;
using Estore.Areas.Admin.ViewModels.Product;
using Microsoft.AspNetCore.Mvc;

namespace Estore.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductsController : Controller
    {
        private IUnitOfWorkFactory _unitOfWorkFactory;

        public ProductsController(IUnitOfWorkFactory unitOfWorkFactory)
        {
            _unitOfWorkFactory = unitOfWorkFactory;
        }

        public IActionResult Index(ManageProductsViewModel model)
        {
            if (model == null)
            {
                model = new ManageProductsViewModel();
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult Update(string id)
        {
            using (var work = _unitOfWorkFactory.UnitOfWork)
            {
                var target = work.ProductRepository.GetById(id);
                if (target == null)
                {
                    Console.WriteLine("Date product, id can not be null");
                    return View();
                }
                List<CategoryViewModel> categoryViewModels = new List<CategoryViewModel>();
                foreach (var category in work.CategoryRepository.GetAll())
                {
                    categoryViewModels.Add(new CategoryViewModel(category.Id, category.Name));
                }
                var weightUnits = Enum.GetValues(typeof(BusinessObject.WeightUnit)).Cast<BusinessObject.WeightUnit>();
                List<String> weightUnitValues = new List<string>();
                foreach (var unit in weightUnits)
                {
                    weightUnitValues.Add(unit.ToString());
                }
                var viewModel = new UpdateProductViewModel(
                    id,
                    new ProductViewModel(new CategoryViewModel(target.Id, target.Category.Name), target.Name, target.Weight, target.Price, (int)target.Quantity, target.Unit),
                    categoryViewModels, weightUnitValues);
                return View(viewModel);
            }
        }

        [HttpGet]
        public IActionResult Create()
        {
            using (var work = _unitOfWorkFactory.UnitOfWork)
            {
                List<CategoryViewModel> categoryViewModels = new List<CategoryViewModel>();
                foreach (var category in work.CategoryRepository.GetAll())
                {
                    categoryViewModels.Add(new CategoryViewModel(category.Id, category.Name));
                }
                var weightUnits = Enum.GetValues(typeof(BusinessObject.WeightUnit)).Cast<BusinessObject.WeightUnit>();
                List<String> weightUnitValues = new List<string>();
                foreach (var unit in weightUnits)
                {
                    weightUnitValues.Add(unit.ToString());
                }
                var viewModel = new CreateProductViewModel(
                    categoryViewModels, weightUnitValues);
                return View(viewModel);
            }
        }

        [HttpPost]
        public IActionResult Create(CreateProductViewModel model)
        {
            using (var work = _unitOfWorkFactory.UnitOfWork)
            {
                BusinessObject.WeightUnit weightUnit = _getWeightUnitInEnum(model.Product.Unit);

                var target = model.Product;



                work.ProductRepository.Add(target.Category.Id,
                        target.Name, target.Weight, weightUnit, target.Quantity, target.Price);
                work.Save();
                return RedirectToAction(nameof(Index));
            }
        }

        private BusinessObject.WeightUnit _getWeightUnitInEnum(string unit)
        {
            if (unit.Equals(BusinessObject.WeightUnit.GRAM.ToString()))
            {
                return BusinessObject.WeightUnit.GRAM;
            }
            else if (unit.Equals(BusinessObject.WeightUnit.KILOGRAM.ToString()))
            {
                return BusinessObject.WeightUnit.KILOGRAM;
            }
            else if (unit.Equals(BusinessObject.WeightUnit.MILLIGRAM.ToString()))
            {
                return BusinessObject.WeightUnit.MILLIGRAM;
            }
            else if (unit.Equals(BusinessObject.WeightUnit.TON.ToString()))
            {
                return BusinessObject.WeightUnit.TON;
            }
            throw new Exception("Invalid weight unit value: " + unit);
        }

        [HttpPost]
        public IActionResult Update(UpdateProductViewModel model)
        {
            using (var work = _unitOfWorkFactory.UnitOfWork)
            {
                Console.WriteLine("Update product with id: " + model.ProductId);
                if (work.ProductRepository.GetById(model.ProductId) == null)
                {
                    return Json(new
                    {
                        success = false,
                        errorMessage = "Internal server error, product id can not be null"
                    });
                }

                var target = model.Product;
                BusinessObject.WeightUnit weightUnit = _getWeightUnitInEnum(target.Unit);

                work.ProductRepository.Update(new BusinessObject.Product(
                        model.ProductId,
                        new BusinessObject.Category(target.Category.Id, target.Category.Title),
                        target.Name, target.Weight, weightUnit, target.Quantity, target.Price));
                work.Save();
                return RedirectToAction(nameof(Index));
            }
        }


        #region api
        [HttpDelete]
        public IActionResult Delete(string id)
        {
            using (var work = _unitOfWorkFactory.UnitOfWork)
            {
                var target = work.ProductRepository.GetById(id);
                if (target == null)
                {
                    return Json(new
                    {
                        success = false,
                        message = "Error while deleting"
                    });
                }
                work.OrderDetailRepository.RemoveByProductId(id);
                work.ProductRepository.RemoveById(id);
                work.Save();
            }

            using (var work = _unitOfWorkFactory.UnitOfWork)
            {
                work.OrderRepository.RemoveOrderContainsNoOrderDetails();
                work.Save();
            }
            Console.WriteLine("Delete product" + id);

            return Json(new
            {
                success = true,
                message = "Delete product success"
            });
        }

        [HttpPost()]
        public IActionResult GetAll([FromBody()] ProductFilterViewModel model)
        {
            Console.WriteLine("Getall product action");

            if (model != null)
            {
                Console.WriteLine("Product name: " + model.ProductName);
            }

            using (var work = _unitOfWorkFactory.UnitOfWork)
            {
                IEnumerable<BusinessObject.Product> queryResult;
                if (model == null)
                {
                    queryResult = work.ProductRepository.GetAll();
                    Console.WriteLine("Filter is null");
                }
                else
                {
                    var minPrice = model.MinPrice;
                    var maxPrice = model.MaxPrice;
                    var maxQuantity = model.MaxQuantity;
                    var minQuantity = model.MinQuantity;

                    if (!model.IsIncludePrice)
                    {
                        minPrice = -1;
                        maxPrice = -1;
                    }
                    if (!model.IsIncludeQuantity)
                    {
                        maxQuantity = -1;
                        minQuantity = -1;
                    }
                    queryResult = work.ProductRepository.GetWIthFilter(model.ProductId, model.ProductName, maxQuantity, minQuantity, maxPrice, minPrice);
                }

                List<dynamic> results = new List<dynamic>();
                foreach (var product in queryResult)
                {
                    dynamic data = new
                    {
                        id = product.Id,
                        name = product.Name,
                        categoryName = product.Category.Name,
                        categoryId = product.CategoryId,
                        quantity = product.Quantity,
                        unit = product.Unit,
                        price = product.Price,
                        weight = product.Weight
                    };
                    results.Add(data);
                }

                return Json(new { success = true, data = results });
            }

        }
        #endregion
    }

}
