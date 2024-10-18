using COCOApp.Models;
using COCOApp.Services;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using COCOApp.Helpers;
using Microsoft.AspNetCore.Authorization;
using AspNetCore.Reporting;
using AspNetCore.Reporting.ReportExecutionService;

namespace COCOApp.Controllers
{
    public class ReportController : Controller
    {
        private readonly ExportOrderService _orderService;
        private readonly UserService _userService;
        private readonly ReportService _reportService;
        private readonly ExportOrderItemService _itemService;
        private readonly UserDetailsService _userDetailsService;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ReportController(ExportOrderService orderService, UserService userService, ReportService reportService, ExportOrderItemService itemService, UserDetailsService userDetailsService, IWebHostEnvironment webHostEnvironment)
        {
            _orderService = orderService;
            _userService = userService;
            _reportService = reportService;
            _itemService = itemService;
            _userDetailsService = userDetailsService;
            _webHostEnvironment = webHostEnvironment;
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
        }
        [Authorize(Roles = "Admin,Seller")]
        public IActionResult ViewCreate()
        {
            User user = HttpContext.Session.GetCustomObjectFromSession<User>("user");
            ViewBag.Customers = _orderService.GetCustomersSelectList(user.Id);
            return View("/Views/Report/CreateReport.cshtml");
        }
        [Authorize(Roles = "Admin,Seller")]
        [HttpGet]
        public IActionResult GetOrders(int customerId, string daterange)
        {
            User user = HttpContext.Session.GetCustomObjectFromSession<User>("user");
            ViewBag.Customers = _orderService.GetCustomersSelectList(user.Id);
            List<ExportOrder> orders = _orderService.GetExportOrders(daterange, customerId, user.Id);
            if (orders.Count > 0)
            {
                string dateRange = orders[orders.Count - 1].OrderDate.ToString("MM / dd / yyyy") + " - " + orders[0].OrderDate.ToString("MM / dd / yyyy");
            }
            HttpContext.Session.SetObjectInSession("dateRange", daterange);
            return View("/Views/Report/CreateReport.cshtml", orders);
        }
        [Authorize(Roles = "Admin,Seller")]
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
            int sellerId = user.Id;
            if (sellerId == 0)
            {
                sellerId = HttpContext.Session.GetCustomObjectFromSession<int>("sellerId");
            }
            Report report = new Report()
            {
                CustomerId = orders[0].CustomerId,
                TotalPrice = 0,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                SellerId = sellerId,
            };
            _reportService.AddReport(report);
            foreach (var item in orderItems)
            {
                ReportDetail reportDetail = new ReportDetail()
                {
                    ReportId = report.Id,
                    ProductId = item.ProductId,
                    Volume = item.RealVolume,
                    TotalPrice = item.ProductPrice * item.Volume,
                    OrderDate = item.Order.OrderDate
                };
                _reportService.AddReportDetails(reportDetail);
            }
            List<ReportDetail> reportDetails = _reportService.GetReportDetails(report.Id);

            return View("/Views/Report/ReportSummary.cshtml", reportDetails);
        }
        [Authorize(Roles = "Admin,Seller")]
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
        [Authorize(Roles = "Admin,Seller")]
        public async Task<IActionResult> Print(List<ReportDetail> reportDetails)
        {
            if (reportDetails == null || !reportDetails.Any())
            {
                ModelState.AddModelError("", "No details provided.");
                return RedirectToAction("ViewCreate");
            }

            User user = HttpContext.Session.GetCustomObjectFromSession<User>("user");
            int sellerId = user.Id == 0
                ? HttpContext.Session.GetCustomObjectFromSession<int>("sellerId")
                : user.Id;

            user = _userService.GetUserById(sellerId);
            Report report = _reportService.GetReportById(reportDetails[0].ReportId, user.Id);
            string dateRange = HttpContext.Session.GetCustomObjectFromSession<string>("dateRange");
            List<ExportOrderItem> orderItems = _itemService.GetExportOrderItems(dateRange, report.Customer.Id, user.Id);

            int totalQuantity = orderItems.Sum(item => item.RealVolume);
            decimal totalCost = orderItems.Sum(item => item.Total);

            byte[] imageBytes = user.SellerDetail.ImageData;

            var reports = orderItems.Select(item => new
            {
                ProductName = item.Product.ProductName,
                product_price = item.Product.Cost + " VND",
                volume = item.RealVolume,
                MeasureUnit = item.Product.MeasureUnit,
                total = item.Total + " VND",
                orderDate = item.Order.OrderDate.ToString("dd-MM-yyyy"),
                ImageData = user.SellerDetail.ImageData != null
                ? Convert.ToBase64String(user.SellerDetail.ImageData)
                : string.Empty  // Provide a fallback if ImageData is null
            }).ToList();

            var parameters = new Dictionary<string, string>
                {
                    { "StoreNamePM", "Cửa hàng vlxd: " + user.SellerDetail.BusinessName },
                    { "StoreAddressPM", "Địa chỉ: " + user.SellerDetail.BusinessAddress },
                    { "StorePhonePM", "Số điện thoại: " + user.UserDetail.Phone },
                    { "DateRangePM", "Từ ngày: " + orderItems.First().Order.OrderDate.ToString("dd-MM-yyyy")
                                     + " đến ngày: " + orderItems.Last().Order.OrderDate.ToString("dd-MM-yyyy") },
                    { "CustomerPM", "Tên khách hàng: " + report.Customer.Name },
                    { "CustomerAddressPM", "Địa chỉ: " + report.Customer.Address },
                    { "TotalQuantityPM", "Tổng số lượng: " + totalQuantity },
                    { "TotalPricePM", "Tổng giá: " + totalCost + " VND" }
                };

            try
            {
                var path = Path.Combine(this._webHostEnvironment.ContentRootPath, "Reports", "OrderReport.rdlc");
                string mimetype = "";
                int extension = 1;
                LocalReport localReport = new LocalReport(path);

                localReport.AddDataSource("DataSet1", reports);

                var result = localReport.Execute(RenderType.Pdf, extension, parameters, mimetype);

                return File(result.MainStream, "application/pdf");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error generating report: {ex.Message}");
                ModelState.AddModelError("", "Failed to generate report.");
                return RedirectToAction("ViewCreate");
            }
        }


    }
}
