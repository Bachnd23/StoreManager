using COCOApp.Models;
using COCOApp.Services;
using Microsoft.AspNetCore.Mvc;

namespace COCOApp.Controllers
{
    public class OrderController : Controller
    {
        private readonly OrderService _orderService=new OrderService();
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

        [HttpPost]
        public IActionResult AddOrders(List<Order> orders)
        {

                foreach (var model in orders)
                {
                    // Convert the model to your domain entity
                    var order = new Order
                    {
                        CustomerId = model.CustomerId,
                        ProductId = model.ProductId,
                        Volume = model.Volume,
                        Date = model.Date,
                        Complete = false,
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now
                    };
                    _orderService.AddOrder(order);
                }
                return RedirectToAction("ViewList"); // Redirect to action "ViewList" if model state is valid

        }
    }
}
