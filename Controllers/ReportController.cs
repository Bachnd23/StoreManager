using COCOApp.Models;
using COCOApp.Services;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using COCOApp.Helpers;
using Microsoft.AspNetCore.Authorization;

namespace COCOApp.Controllers
{
    public class ReportController : Controller
    {
        private readonly ExportOrderService _orderService;
        private readonly ReportService _reportService;
        private readonly ReportsExportOrdersMappingService _reportsOrdersMappingService;

        public ReportController(ExportOrderService orderService, ReportService reportService, ReportsExportOrdersMappingService reportsOrdersMappingService)
        {
            _orderService = orderService;
            _reportService = reportService;
            _reportsOrdersMappingService = reportsOrdersMappingService;
        }
        public IActionResult ViewCreate()
        {
            User user = HttpContext.Session.GetCustomObjectFromSession<User>("user");
            ViewBag.Customers = _orderService.GetCustomersSelectList(user.Id);
            return View("/Views/Report/CreateReport.cshtml");
        }
        [HttpGet]
        public IActionResult GetOrders(int customerId, string daterange)
        {
            User user = HttpContext.Session.GetCustomObjectFromSession<User>("user");
            ViewBag.Customers = _orderService.GetCustomersSelectList(user.Id);
            List<ExportOrder> orders = _orderService.GetExportOrders(daterange, customerId, user.Id);
            return View("/Views/Report/CreateReport.cshtml", orders);
        }
        [HttpPost]
        public IActionResult CreateSummary(List<int> orderIds)
        {
            if (orderIds == null || orderIds.Count <= 0)
            {
                return View("/Views/Report/CreateReport.cshtml");
            }
            User user = HttpContext.Session.GetCustomObjectFromSession<User>("user");
            // Assuming _orderService can fetch orders by their IDs
            List<ExportOrder> orders = _orderService.GetExportOrdersByIds(orderIds, user.Id);

            return View("/Views/Report/ReportSummary.cshtml", orders);
        }
        [HttpPost]
        public IActionResult CreateInvoice(List<int> orderIds, List<decimal> costs)
        {
            User user = HttpContext.Session.GetCustomObjectFromSession<User>("user");
            List<ExportOrder> orders = _orderService.GetExportOrdersByIds(orderIds, user.Id);
            if (orders == null || orders.Count == 0)
            {
                return View("/Views/Report/CreateReport.cshtml");
            }
            decimal ordersTotalCost = 0;
            for (int i = 0; i < orders.Count; i++)
            {
                ExportOrder order = orders[i];
                //order.OrderProductCost = costs[i];
                //order.OrderTotal = costs[i] * order.Volume;
                ordersTotalCost += order.OrderTotal;
            }
            ViewBag.totalCost = ordersTotalCost;
            // Pass orders to the view
            return View("/Views/Report/Invoice.cshtml", orders);
        }
    }
}
