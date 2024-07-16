using COCOApp.Helpers;
using COCOApp.Models;
using COCOApp.Services;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

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
            User user = HttpContext.Session.GetCustomObjectFromSession<User>("user");
            var orders = _orderService.GetOrders(nameQuery, pageNumber, PageSize, user.Id);
            var totalOrders = _orderService.GetTotalOrders(nameQuery, user.Id);

            var response = new
            {
                orderResults = orders,
                pageNumber = pageNumber,
                totalPages = (int)Math.Ceiling(totalOrders / (double)PageSize)
            };

            return Json(response);
        }
        [HttpGet]
        public IActionResult ViewDetail(int orderId)
        {
            User user = HttpContext.Session.GetCustomObjectFromSession<User>("user");
            Order model = _orderService.GetOrderById(orderId, user.Id); ;
            if (model != null)
            {
                return View("/Views/Order/OrderDetail.cshtml", model);
            }
            else
            {
                return View("/Views/Order/ListOrders.cshtml");
            }
        }
        [HttpGet]
        public IActionResult ViewEdit(int orderId)
        {
            User user = HttpContext.Session.GetCustomObjectFromSession<User>("user");
            Order model = _orderService.GetOrderById(orderId, user.Id); ;
            ViewBag.Customers = _orderService.GetCustomersSelectList(user.Id);
            ViewBag.Products = _orderService.GetProductsSelectList(user.Id);
            if (model != null)
            {
                return View("/Views/Order/EditOrder.cshtml", model);
            }
            else
            {
                return View("/Views/Order/ListOrders.cshtml");
            }
        }
        public IActionResult ViewList()
        {
            return View("/Views/Order/ListOrders.cshtml");
        }

        public IActionResult Add()
        {
            User user = HttpContext.Session.GetCustomObjectFromSession<User>("user");
            ViewBag.Customers = _orderService.GetCustomersSelectList(user.Id);
            ViewBag.Products = _orderService.GetProductsSelectList(user.Id);
            return View("/Views/Order/AddOrder.cshtml");
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult AddOrders(List<Order> orders)
        {
            User user = HttpContext.Session.GetCustomObjectFromSession<User>("user");
            foreach (var model in orders)
            {
                Product product = _productService.GetProductById(model.ProductId, user.Id);
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
                    SellerId = user.Id,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                };

                _orderService.AddOrder(order);
            }
            HttpContext.Session.SetString("SuccessMsg", "Thêm đơn hàng thành công!");
            return RedirectToAction("ViewList"); // Redirect to action "ViewList" if model state is valid

        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult EditOrder(Order model)
        {
            /*            if (!ModelState.IsValid)
                        {
                            // If the model state is not valid, return the same view with validation errors
                            return View("/Views/Customer/AddCustomer.cshtml", model);
                        }*/
            User user = HttpContext.Session.GetCustomObjectFromSession<User>("user");
            Order oldOrder = _orderService.GetOrderById(model.Id, user.Id);
            // Convert the model to your domain entity
            var order = new Order
            {
                CustomerId = model.CustomerId,
                ProductId = model.ProductId,
                Volume = model.Volume,
                Date = model.Date,
                SellerId = oldOrder.SellerId,
                UpdatedAt = DateTime.Now
            };

            // Use the service to edit the customer
            _orderService.EditOrder(model.Id, order);
            HttpContext.Session.SetString("SuccessMsg", "Sửa đơn hàng thành công!");
            // Redirect to the customer list or a success page
            return RedirectToAction("ViewList");
        }
    }
}
