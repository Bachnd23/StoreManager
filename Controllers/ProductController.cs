using COCOApp.Models;
using COCOApp.Services;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using COCOApp.Helpers;

namespace COCOApp.Controllers
{
    public class ProductController : Controller
    {
        private ProductService _productService;
        public ProductController(ProductService productService)
        {
            _productService = productService;
        }
        private const int PageSize = 10;

        [HttpGet]
        public IActionResult GetList(string nameQuery,int statusId, int pageNumber = 1)
        {
            User user = HttpContext.Session.GetCustomObjectFromSession<User>("user");
            var products = _productService.GetProducts(nameQuery, pageNumber, PageSize,user.Id,statusId);
            var totalProducts = _productService.GetTotalProducts(nameQuery,user.Id,statusId);

            var response = new
            {
                productResults = products,
                pageNumber = pageNumber,
                totalPages = (int)Math.Ceiling(totalProducts / (double)PageSize)
            };

            return Json(response);
        }
        [HttpGet]
        public IActionResult GetProduct(int productId)
        {
            User user = HttpContext.Session.GetCustomObjectFromSession<User>("user");
            Product model = _productService.GetProductById(productId, user.Id);
            if (model != null)
            {
                return View("/Views/Products/ProductDetail.cshtml", model);
            }
            else
            {
                return View("/Views/Products/ListProducts.cshtml");
            }
        }
        [HttpGet]
        public IActionResult ViewEdit(int productId)
        {
            User user = HttpContext.Session.GetCustomObjectFromSession<User>("user");
            Product model = _productService.GetProductById(productId, user.Id);;
            if (model != null)
            {
                return View("/Views/Products/EditProduct.cshtml", model);
            }
            else
            {
                return View("/Views/Products/ListProducts.cshtml");
            }
        }
        public IActionResult ViewList()
        {
            return View("/Views/Products/ListProducts.cshtml");
        }
        public IActionResult ViewAdd()
        {
            return View("/Views/Products/AddProduct.cshtml");
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult AddProduct(Product model)
        {
            User user = HttpContext.Session.GetCustomObjectFromSession<User>("user");
            model.SellerId = user.Id;

            if (!ModelState.IsValid)
            {
                // Log the validation errors
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                string errorMessages = string.Join("; ", errors);

                Debug.WriteLine(errorMessages);

                // Return the same view with validation errors
                return View("/Views/Products/AddProduct.cshtml", model);
            }
            int sellerId = model.Id;
            if (sellerId == 0)
            {
                sellerId = HttpContext.Session.GetCustomObjectFromSession<int>("sellerId");
            }
            // Convert the model to your domain entity
            var product = new Product
            {
                ProductName = model.ProductName,
                MeasureUnit = model.MeasureUnit,
                Cost = model.Cost,
                Status = model.Status,
                SellerId = sellerId,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };

            // Use the service to insert the customer
            _productService.AddProduct(product);

            // On success
            HttpContext.Session.SetString("SuccessMsg", "Thêm sản phẩm thành công!");

            // Redirect to the customer list or a success page
            return RedirectToAction("ViewList");
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult EditProduct(Product model)
        {
            if (!ModelState.IsValid)
            {
                // Log the validation errors
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                string errorMessages = string.Join("; ", errors);

                Debug.WriteLine(errorMessages);

                // Return the same view with validation errors
                return View("/Views/Products/EditProduct.cshtml", model);
            }

            User user = HttpContext.Session.GetCustomObjectFromSession<User>("user");
            Product oldProduct=_productService.GetProductById(model.Id, user.Id);
            // Convert the model to your domain entity
            var product = new Product
            {
                ProductName = model.ProductName,
                MeasureUnit = model.MeasureUnit,
                Cost = model.Cost,
                Status = model.Status,
                SellerId = oldProduct.SellerId,
                UpdatedAt = DateTime.Now
            };

            // Use the service to edit the customer
            _productService.EditProduct(model.Id, product);
            HttpContext.Session.SetString("SuccessMsg", "Sửa hàng thành công!");
            // Redirect to the customer list or a success page
            return RedirectToAction("ViewList");
        }
    }
}
