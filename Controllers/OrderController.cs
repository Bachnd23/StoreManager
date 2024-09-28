using COCOApp.Helpers;
using COCOApp.Models;
using COCOApp.Services;
using COCOApp.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace COCOApp.Controllers
{
    public class OrderController : Controller
    {
        private readonly ExportOrderService _orderService;
        private readonly ProductService _productService;
        private readonly IHubContext<OrderHub> _hubContext;
        private readonly ExportOrderItemService _itemService;

        public OrderController(ExportOrderService orderService, ProductService productService, IHubContext<OrderHub> hubContext, ExportOrderItemService itemService)
        {
            _orderService = orderService;
            _productService = productService;
            _hubContext = hubContext;
            _itemService = itemService;
        }
        private const int PageSize = 10;


        [HttpGet]
        public IActionResult GetOrdersList(string nameQuery, int pageNumber = 1)
        {
            User user = HttpContext.Session.GetCustomObjectFromSession<User>("user");
            var orders = _orderService.GetExportOrders(nameQuery, pageNumber, PageSize, user.Id);
            var totalOrders = _orderService.GetTotalExportOrders(nameQuery, user.Id);

            var response = new
            {
                orderResults = orders,
                pageNumber = pageNumber,
                totalPages = (int)Math.Ceiling(totalOrders / (double)PageSize)
            };

            return Json(response);
        }
        [HttpGet]
        public IActionResult GetOrderItemsList(int orderId,string nameQuery, int pageNumber = 1)
        {
            User user = HttpContext.Session.GetCustomObjectFromSession<User>("user");
            var orders = _itemService.GetExportOrderItems(orderId,nameQuery, pageNumber, PageSize, user.Id);
            var totalOrders = _itemService.GetTotalExportOrderItems(nameQuery, user.Id);

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
            ExportOrder model = _orderService.GetExportOrderById(orderId, user.Id); ;
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
            ExportOrder model = _orderService.GetExportOrderById(orderId, user.Id); ;
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
            ViewData["PageNumber"] = pageNumber;
            return View("/Views/Order/ListOrders.cshtml");
        }
        public IActionResult ViewOrderItemsList(int orderId,int pageNumber = 1)
        {
            ViewData["PageNumber"] = pageNumber;
            ViewData["OrderId"] = orderId;
            return View("/Views/Order/ListItems.cshtml");
        }

        public IActionResult Add()
        {
            User user = HttpContext.Session.GetCustomObjectFromSession<User>("user");
            ViewBag.Customers = _orderService.GetCustomersSelectList(user.Id);
            ViewBag.Products = _orderService.GetProductsSelectList(user.Id);
            var viewModel = new MultiOrderViewModel();
            return View("/Views/Order/AddOrder.cshtml", viewModel);
        }

        // POST: Order/CreateMultiple
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateMultiple(MultiOrderViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                // Retrieve the seller ID from session
                User user = HttpContext.Session.GetCustomObjectFromSession<User>("user");
                int sellerId = user?.Id ?? 0;
                if (sellerId == 0)
                {
                    sellerId = HttpContext.Session.GetCustomObjectFromSession<int>("sellerId");
                }

                // Sort Orders by CustomerId and OrderDate
                var sortedOrders = viewModel.Orders
                                            .OrderBy(order => order.CustomerId)
                                            .ThenBy(order => order.OrderDate)
                                            .ToList();

                int i = 0;
                Debug.WriteLine(sortedOrders.Count);
                while (i < sortedOrders.Count)
                {
                    var order = sortedOrders[i];
                    ExportOrder exportOrder=_orderService.GetExportOrderByCustomerAndDate(order.CustomerId, order.OrderDate);
                    if(exportOrder == null)
                    {
                        exportOrder = new ExportOrder
                        {
                            CustomerId = order.CustomerId,
                            OrderDate = order.OrderDate,
                            Complete = false,
                            OrderTotal = 0,
                            SellerId = sellerId,
                            CreatedAt = DateTime.Now,
                            UpdatedAt = DateTime.Now
                        };
                        _orderService.AddExportOrder(exportOrder);
                    }
                    // Add items for all orders with the same CustomerId and OrderDate
                    while (i < sortedOrders.Count && order.CustomerId == sortedOrders[i].CustomerId && order.OrderDate == sortedOrders[i].OrderDate)
                    {
                        var currentOrder = sortedOrders[i];
                        Product product = _productService.GetProductById(currentOrder.ProductId, user.Id);

                        var exportOrderItem = new ExportOrderItem
                        {
                            OrderId = exportOrder.Id,
                            ProductId = currentOrder.ProductId,
                            Volume = currentOrder.ProductVolume,
                            ProductPrice = product.Cost,
                            Total = currentOrder.ProductVolume * product.Cost,
                            SellerId = sellerId,
                            CreatedAt = DateTime.Now,
                            UpdatedAt = DateTime.Now
                        };
                        _itemService.AddExportOrderItem(exportOrderItem);

                        i++; // Increment index to process the next order
                    }
                }

                HttpContext.Session.SetString("SuccessMsg", "Thêm đơn hàng thành công!");
                return RedirectToAction("ViewList"); // Redirect to action "ViewList" if model state is valid
            }
            // Log the validation errors if any
            var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
            string errorMessages = string.Join("; ", errors);
            Debug.WriteLine(errorMessages);

            return RedirectToAction("Add"); // Redirect to action "Add" if model state is not valid
        }



        //[ValidateAntiForgeryToken]
        //[HttpPost]
        //public async Task<IActionResult> AddOrders(ExportOrder order,List<ExportOrderItem> orders)
        //{
        //    if (orders.Count == 0)
        //    {
        //        HttpContext.Session.SetString("ErrorMsg", "Không có đơn hàng nào!");
        //        return RedirectToAction("Add"); // Redirect to action "ViewList" if model state is not valid
        //    }
        //    User user = HttpContext.Session.GetCustomObjectFromSession<User>("user");
        //    foreach (var model in orders)
        //    {
        //        Product product = _productService.GetProductById(model.ProductId, user.Id);
        //        int sellerId = user.Id;
        //        if (sellerId == 0)
        //        {
        //            sellerId = HttpContext.Session.GetCustomObjectFromSession<int>("sellerId");
        //        }
        //        // Convert the model to your domain entity
        //        var order = new ExportOrder
        //        {
        //            CustomerId = model.CustomerId,
        //            ProductId = model.ProductId,
        //            Volume = model.Volume,
        //            OrderDate = model.OrderDate,
        //            Complete = false,
        //            OrderProductCost = product.Cost,
        //            OrderTotal = product.Cost * model.Volume,
        //            SellerId = sellerId,
        //            CreatedAt = DateTime.Now,
        //            UpdatedAt = DateTime.Now
        //        };

        //        _orderService.AddOrder(order);
        //        // Notify admins about changes to the order
        //        await _hubContext.Clients.Group("Admin").SendAsync("OrderUpdated", order);
        //    }

        //    HttpContext.Session.SetString("SuccessMsg", "Thêm đơn hàng thành công!");
        //    return RedirectToAction("ViewList"); // Redirect to action "ViewList" if model state is valid

        //}
        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> EditOrder(ExportOrder model)
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
            ExportOrder oldOrder = _orderService.GetExportOrderById(model.Id, user.Id);
            // Convert the model to your domain entity
            var order = new ExportOrder
            {
                CustomerId = model.CustomerId,
                OrderDate = model.OrderDate,
                SellerId = oldOrder.SellerId,
                UpdatedAt = DateTime.Now
            };

            // Use the service to edit the customer
            _orderService.EditExportOrder(model.Id, order);

            // Notify admins about changes to the order
            await _hubContext.Clients.Group("Admin").SendAsync("OrderUpdated", order);

            HttpContext.Session.SetString("SuccessMsg", "Sửa đơn hàng thành công!");
            // Redirect to the customer list or a success page
            return RedirectToAction("ViewList");
        }
    }
}
