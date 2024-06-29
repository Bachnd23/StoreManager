﻿using COCOApp.Models;
using COCOApp.Services;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using COCOApp.Helpers;

namespace COCOApp.Controllers
{
    public class ReportController : Controller
    {
        private readonly OrderService _orderService;
        private readonly ReportService _reportService;
        private readonly ReportsOrdersMappingService _reportsOrdersMappingService;

        public ReportController(OrderService orderService, ReportService reportService, ReportsOrdersMappingService reportsOrdersMappingService)
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
            List<Order> orders = _orderService.GetOrders(daterange, customerId, user.Id);
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
            List<Order> orders = _orderService.GetOrdersByIds(orderIds, user.Id);

            return View("/Views/Report/ReportSummary.cshtml", orders);
        }
        [HttpPost]
        public IActionResult CreateInvoice(List<int> orderIds, List<decimal> costs)
        {
            User user = HttpContext.Session.GetCustomObjectFromSession<User>("user");
            List<Order> orders = _orderService.GetOrdersByIds(orderIds,user.Id);
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