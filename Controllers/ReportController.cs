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
/*        public IActionResult GetReports(int customerId, string daterange)
        {
            ViewBag.Customers = _orderService.GetCustomersSelectList();
            List<Order> rom = _reportsOrdersMappingService.GetGetReportsOrdersMapping(daterange,customerId);
            return View("/Views/Report/CreateReport.cshtml", rom);
        }*/

    }
}
