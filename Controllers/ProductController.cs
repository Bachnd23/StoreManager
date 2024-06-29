﻿using COCOApp.Models;
using COCOApp.Services;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

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
        public IActionResult GetList(string nameQuery, int pageNumber = 1)
        {
            var products = _productService.GetProducts(nameQuery, pageNumber, PageSize);
            var totalProducts = _productService.GetTotalProducts(nameQuery);

            var response = new
            {
                productResults = products,
                pageNumber = pageNumber,
                totalPages = (int)Math.Ceiling(totalProducts / (double)PageSize)
            };

            return Json(response);
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
/*            if (!ModelState.IsValid)
            {
                // If the model state is not valid, return the same view with validation errors

                // On error
                HttpContext.Session.SetString("ErrorMsg", "Something went wrong!");
                return View("/Views/Products/AddProduct.cshtml", model);
            }*/

            // Convert the model to your domain entity
            var product = new Product
            {
                ProductName = model.ProductName,
                MeasureUnit = model.MeasureUnit,
                Cost = model.Cost,
                Status = model.Status,
                SellerId = 1,//to be updated
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
    }
}
