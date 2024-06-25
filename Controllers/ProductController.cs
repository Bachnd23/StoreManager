using COCOApp.Services;
using Microsoft.AspNetCore.Mvc;

namespace COCOApp.Controllers
{
    public class ProductController : Controller
    {
        private ProductService _productService = new ProductService();
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
        public IActionResult Add()
        {
            return View("/Views/Products/AddProduct.cshtml");
        }
    }
}
