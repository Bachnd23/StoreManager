﻿using COCOApp.Models;
using COCOApp.Services;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using COCOApp.Helpers;
using Microsoft.AspNetCore.Authorization;
using AspNetCore.Reporting;

namespace COCOApp.Controllers
{
    public class ReportController : Controller
    {
        private readonly ExportOrderService _orderService;
        private readonly ReportService _reportService;
        private readonly ExportOrderItemService _itemService;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ReportController(ExportOrderService orderService, ReportService reportService, ExportOrderItemService itemService, IWebHostEnvironment webHostEnvironment)
        {
            _orderService = orderService;
            _reportService = reportService;
            _itemService = itemService;
            _webHostEnvironment = webHostEnvironment;
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
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
            User user = HttpContext.Session.GetCustomObjectFromSession<User>("user");
            if (orderIds == null || orderIds.Count <= 0)
            {
                ViewBag.Customers = _orderService.GetCustomersSelectList(user.Id);
                return View("/Views/Report/CreateReport.cshtml");
            }
            // Assuming _orderService can fetch orders by their IDs
            List<ExportOrder> orders = _orderService.GetExportOrdersByIds(orderIds, user.Id);
            // Assuming _orderService can fetch orders by their IDs
            List<ExportOrderItem> orderItems = _itemService.GetExportOrderItemsByIds(orderIds, user.Id);
            
            Report report = new Report()
            {
                CustomerId=orders[0].CustomerId,
                TotalPrice=0,
                CreatedAt= DateTime.Now,
                UpdatedAt= DateTime.Now,
                SellerId=user.Id,
            };
            _reportService.AddReport(report);
            foreach (var item in orderItems)
            {
                ReportDetail reportDetail = new ReportDetail()
                {
                    ReportId = report.Id,
                    ProductId = item.ProductId,
                    Volume = item.Volume,
                    TotalPrice = item.ProductPrice*item.Volume,
                    OrderDate=item.Order.OrderDate
                };
                _reportService.AddReportDetails(reportDetail);  
            }
            List<ReportDetail> reportDetails=_reportService.GetReportDetails(report.Id);

            return View("/Views/Report/ReportSummary.cshtml", reportDetails);
        }
        [HttpPost]
        public IActionResult CreateInvoice(List<int> orderIds)
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
        public async Task<IActionResult> Print(List<ReportDetail> reportDetails)
        {
            // Check if the model is valid
            if (reportDetails == null || !reportDetails.Any())
            {
                ModelState.AddModelError("", "No details provided.");
                return View("Error"); // Return an error view if the data is invalid
            }
            User user = HttpContext.Session.GetCustomObjectFromSession<User>("user");
            Report report = _reportService.GetReportById(reportDetails[0].ReportId,user.Id);
            string mimetype = "";
            int extension = 1;
            var path = $"{this._webHostEnvironment.ContentRootPath}\\Reports\\OrderReport.rdlc";

            int totalQuantity= 0;
            decimal totalCost = 0;
            for (int i = 0; i < reportDetails.Count();i++)
            {
                ReportDetail reportDetail = reportDetails[i];
                totalQuantity += reportDetail.Volume;
                totalCost += reportDetail.TotalPrice;
            }
            var reports=reportDetails.Select(item => new
            {
                ProductName = item.Product.ProductName,
                product_price = item.Product.Cost,
                volume = item.Volume,
                total = item.TotalPrice,
                orderDate = item.OrderDate
            }).ToList();

            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("CustomerPM", "Tên khách hàng: "+report.Customer.Name);
            parameters.Add("CustomerAddressPM", "Địa chỉ: " + report.Customer.Address);
            parameters.Add("TotalQuantityPM", "Tổng số lượng: "+totalQuantity);
            parameters.Add("TotalPricePM", "Tổng giá: "+totalCost);

            LocalReport localReport = new LocalReport(path);
            localReport.AddDataSource("DataSet1", reports);
            var result = localReport.Execute(RenderType.Pdf, extension, parameters, mimetype);

            return File(result.MainStream, "application/pdf");
        }
    }
}
