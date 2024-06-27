using COCOApp.Models;
using COCOApp.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace COCOApp.Controllers
{
    public class ReportController : Controller
    {
        private readonly OrderService _orderService = new OrderService();
        private readonly ReportService _reportService = new ReportService();
        private readonly ReportsOrdersMappingService _reportsOrdersMappingService = new ReportsOrdersMappingService();
        public IActionResult ViewCreate()
        {
            ViewBag.Customers = _orderService.GetCustomersSelectList();
            return View("/Views/Report/CreateReport.cshtml");
        }
        [HttpPost]
        public IActionResult CreateSummary(List<int> orderIds)
        {
            // Assuming _orderService can fetch orders by their IDs
            List<Order> orders = _orderService.GetOrdersByIds(orderIds);

            // Fetch customers select list for dropdown
            ViewBag.Customers = _orderService.GetCustomersSelectList();

            // Pass orders to the view
            return View("/Views/Report/ReportSummary.cshtml", orders);
        }
    }
}
