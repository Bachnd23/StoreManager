using COCOApp.Helpers;
using COCOApp.Models;
using COCOApp.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Diagnostics;

namespace COCOApp.Controllers
{
    public class OrderController : Controller
    {
        private readonly OrderService _orderService;
        private readonly ProductService _productService;
        private readonly IHubContext<OrderHub> _hubContext;

        public OrderController(OrderService orderService, ProductService productService, IHubContext<OrderHub> hubContext)
        {
            _orderService = orderService;
            _productService = productService;
            _hubContext = hubContext;
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
        public IActionResult ViewDetail(int orderId, int pageNumber = 1)
        {
            User user = HttpContext.Session.GetCustomObjectFromSession<User>("user");
            Order model = _orderService.GetOrderById(orderId, user.Id); ;
            ViewData["PageNumber"] = pageNumber;
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
        public IActionResult ViewEdit(int orderId, int pageNumber = 1)
        {
            User user = HttpContext.Session.GetCustomObjectFromSession<User>("user");
            Order model = _orderService.GetOrderById(orderId, user.Id); ;
            ViewBag.Customers = _orderService.GetCustomersSelectList(user.Id);
            ViewBag.Products = _orderService.GetProductsSelectList(user.Id);
            ViewData["PageNumber"] = pageNumber;
            if (model != null)
            {
                return View("/Views/Order/EditOrder.cshtml", model);
            }
            else
            {
                return View("/Views/Order/ListOrders.cshtml");
            }
        }
        public IActionResult ViewList(int pageNumber = 1)
        {
            Debug.WriteLine(pageNumber);
            ViewData["PageNumber"] = pageNumber;
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
        public async Task<IActionResult> AddOrders(List<Order> orders)
        {
            if (orders.Count == 0)
            {
                HttpContext.Session.SetString("ErrorMsg", "Không có đơn hàng nào!");
                return RedirectToAction("Add"); // Redirect to action "ViewList" if model state is not valid
            }
            User user = HttpContext.Session.GetCustomObjectFromSession<User>("user");
            foreach (var model in orders)
            {
                Product product = _productService.GetProductById(model.ProductId, user.Id);
                int sellerId=user.Id;
                if (sellerId == 0)
                {
                    sellerId= HttpContext.Session.GetCustomObjectFromSession<int>("sellerId");
                }
                // Convert the model to your domain entity
                var order = new Order
                {
                    CustomerId = model.CustomerId,
                    ProductId = model.ProductId,
                    Volume = model.Volume,
                    OrderDate = model.OrderDate,
                    Complete = false,
                    OrderProductCost = product.Cost,
                    OrderTotal = product.Cost * model.Volume,
                    SellerId = sellerId,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                };

                _orderService.AddOrder(order);
                // Notify admins about changes to the order
                await _hubContext.Clients.Group("Admin").SendAsync("OrderUpdated", order);
            }

            HttpContext.Session.SetString("SuccessMsg", "Thêm đơn hàng thành công!");
            return RedirectToAction("ViewList"); // Redirect to action "ViewList" if model state is valid

        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> EditOrder(Order model)
        {
            User user = HttpContext.Session.GetCustomObjectFromSession<User>("user");
            if (!ModelState.IsValid)
            {
                // Log the validation errors
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                string errorMessages = string.Join("; ", errors);

                Debug.WriteLine(errorMessages);
                // If the model state is not valid, return the same view with validation errors
                ViewBag.Customers = _orderService.GetCustomersSelectList(user.Id);
                ViewBag.Products = _orderService.GetProductsSelectList(user.Id);
                return View("/Views/Order/EditOrder.cshtml", model);
            }
            Order oldOrder = _orderService.GetOrderById(model.Id, user.Id);
            // Convert the model to your domain entity
            var order = new Order
            {
                CustomerId = model.CustomerId,
                ProductId = model.ProductId,
                Volume = model.Volume,
                OrderDate = model.OrderDate,
                SellerId = oldOrder.SellerId,
                UpdatedAt = DateTime.Now
            };

            // Use the service to edit the customer
            _orderService.EditOrder(model.Id, order);

            // Notify admins about changes to the order
            await _hubContext.Clients.Group("Admin").SendAsync("OrderUpdated",order);

            HttpContext.Session.SetString("SuccessMsg", "Sửa đơn hàng thành công!");
            // Redirect to the customer list or a success page
            return RedirectToAction("ViewList");
        }
    }
}
