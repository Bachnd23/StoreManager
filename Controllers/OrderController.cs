using COCOApp.Models;
using COCOApp.Services;
using Microsoft.AspNetCore.Mvc;

namespace COCOApp.Controllers
{
    public class OrderController : Controller
    {
        private readonly OrderService _orderService;
        private readonly ProductService _productService;

        public OrderController(OrderService orderService, ProductService productService)
        {
            _orderService = orderService;
            _productService = productService;
        }
        private const int PageSize = 10;


        [HttpGet]
        public IActionResult GetList(string nameQuery, int pageNumber = 1)
        {
            var orders = _orderService.GetOrders(nameQuery, pageNumber, PageSize);
            var totalOrders = _orderService.GetTotalOrders(nameQuery);

            var response = new
            {
                orderResults = orders,
                pageNumber = pageNumber,
                totalPages = (int)Math.Ceiling(totalOrders / (double)PageSize)
            };

            return Json(response);
        }

        public IActionResult ViewList()
        {
            return View("/Views/Order/ListOrders.cshtml");
        }

        public IActionResult Add()
        {
            ViewBag.Customers = _orderService.GetCustomersSelectList();
            ViewBag.Products = _orderService.GetProductsSelectList();
            return View("/Views/Order/AddOrder.cshtml");
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult AddOrders(List<Order> orders)
        {

            foreach (var model in orders)
            {
                Product product = _productService.GetProductById(model.ProductId);
                // Convert the model to your domain entity
                var order = new Order
                {
                    CustomerId = model.CustomerId,
                    ProductId = model.ProductId,
                    Volume = model.Volume,
                    Date = model.Date,
                    Complete = false,
                    OrderProductCost = product.Cost,
                    OrderTotal = product.Cost * model.Volume,
                    SellerId = 1,//to be updated
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                };

                _orderService.AddOrder(order);
            }
            return RedirectToAction("ViewList"); // Redirect to action "ViewList" if model state is valid

        }
        public IActionResult GetOrders(int customerId, string daterange)
        {
            ViewBag.Customers = _orderService.GetCustomersSelectList();
            List<Order> orders = _orderService.GetOrders(daterange, customerId);
            return View("/Views/Report/CreateReport.cshtml", orders);
        }

    }
}
