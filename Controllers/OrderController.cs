using COCOApp.Services;
using Microsoft.AspNetCore.Mvc;

namespace COCOApp.Controllers
{
    public class OrderController : Controller
    {
        private OrderService _orderService = new OrderService();
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
            return View("/Views/Order/AddOrder.cshtml");
        }
    }
}
