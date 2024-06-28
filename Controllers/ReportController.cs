﻿using COCOApp.Models;
using COCOApp.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Diagnostics;

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
            if(orderIds==null || orderIds.Count <= 0)
            {
                return View("/Views/Report/CreateReport.cshtml");
            }
            // Assuming _orderService can fetch orders by their IDs
            List<Order> orders = _orderService.GetOrdersByIds(orderIds);

            // Pass orders to the view
            return View("/Views/Report/ReportSummary.cshtml", orders);
        }
        [HttpPost]
        public IActionResult CreateInvoice(List<int> orderIds, List<decimal> costs)
        {
            List<Order> orders = _orderService.GetOrdersByIds(orderIds);
            if(orders==null|| orders.Count == 0) {
                return View("/Views/Report/CreateReport.cshtml");
            }
            decimal ordersTotalCost = 0;
            for (int i=0;i<orders.Count;i++)
            {
                Order order = orders[i];
                order.OrderProductCost = costs[i];
                order.OrderTotal= costs[i]*order.Volume;
                ordersTotalCost+= order.OrderTotal;
            }
            ViewBag.totalCost=ordersTotalCost;
            // Pass orders to the view
            return View("/Views/Report/Invoice.cshtml", orders);
        }
    }
}
