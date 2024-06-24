using Microsoft.AspNetCore.Mvc;

namespace COCOApp.Controllers
{
    public class ProductController : Controller
    {
        public IActionResult List()
        {
            return View("/Views/Products/ListProducts.cshtml");
        }
        public IActionResult Add()
        {
            return View("/Views/Products/AddProduct.cshtml");
        }
    }
}
