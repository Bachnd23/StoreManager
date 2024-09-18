using COCOApp.Helpers;
using COCOApp.Models;
using COCOApp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR;
using System.Diagnostics;

namespace COCOApp.Controllers
{
    public class CustomerController : Controller
    {
        private readonly IHubContext<CustomerHub> _hubContext;
        private CustomerService _customerService;
        public CustomerController(CustomerService customerService, IHubContext<CustomerHub> hubContext)
        {
            _customerService = customerService;
            _hubContext = hubContext;
        }
        private const int PageSize = 10;

        [Authorize(Roles = "Admin,Seller")]
        [HttpGet]
        public IActionResult GetList(string nameQuery,int statusId, int pageNumber = 1)
        {
            User user = HttpContext.Session.GetCustomObjectFromSession<User>("user");
            var customers = _customerService.GetCustomers(nameQuery, pageNumber, PageSize, user.Id, statusId);
            var totalCustomers = _customerService.GetTotalCustomers(nameQuery, user.Id,statusId);

            var response = new
            {
                customerResults = customers,
                pageNumber = pageNumber,
                totalPages = (int)Math.Ceiling(totalCustomers / (double)PageSize)
            };

            return Json(response);
        }

        [Authorize(Roles = "Admin,Seller")]
        [HttpGet]
        public IActionResult GetCustomer(int customerId, int pageNumber = 1)
        {
            User user = HttpContext.Session.GetCustomObjectFromSession<User>("user");
            Customer model=_customerService.GetCustomerById(customerId,user.Id);
            ViewData["PageNumber"] = pageNumber;
            if (model != null)
            {
                return View("/Views/Customer/CustomerDetail.cshtml", model);
            }
            else
            {
                return View("/Views/Customer/ListCustomers.cshtml");
            }
        }
        [HttpGet]
        public IActionResult ViewEdit(int customerId, int pageNumber = 1)
        {
            User user = HttpContext.Session.GetCustomObjectFromSession<User>("user");
            Customer model = _customerService.GetCustomerById(customerId, user.Id);
            ViewData["PageNumber"] = pageNumber;
            if (model != null)
            {
                return View("/Views/Customer/EditCustomer.cshtml", model);
            }
            else
            {
                return View("/Views/Customer/ListCustomers.cshtml");
            }
        }

        [Authorize(Roles = "Admin,Seller")]
        public IActionResult ViewAdd()
        {
            return View("/Views/Customer/AddCustomer.cshtml");
        }

        [Authorize(Roles = "Admin,Seller")]
        public IActionResult ViewList(int pageNumber = 1)
        {
            ViewData["PageNumber"] = pageNumber;
            return View("/Views/Customer/ListCustomers.cshtml");
        }
        public IActionResult ViewDetail()
        {
            return View("/Views/Customer/CustomerDetail.cshtml");
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> AddCustomer(Customer model)
        {
            if (!ModelState.IsValid)
            {
                // Log the validation errors
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                string errorMessages = string.Join("; ", errors);

                Debug.WriteLine(errorMessages);
                // If the model state is not valid, return the same view with validation errors
                return View("/Views/Customer/AddCustomer.cshtml", model);
            }
            User user = HttpContext.Session.GetCustomObjectFromSession<User>("user");
            int sellerId = user.Id;
            if (sellerId == 0)
            {
                sellerId = HttpContext.Session.GetCustomObjectFromSession<int>("sellerId");
            }
            // Convert the model to your domain entity
            var customer = new Customer
            {
                Name = model.Name,
                Phone = model.Phone,
                Address = model.Address,
                Note = model.Note,  // Note property is nullable
                Status = model.Status,
                SellerId = sellerId,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };

            // Use the service to insert the customer
            _customerService.AddCustomer(customer);

            // Notify admins about changes to the customer
            await _hubContext.Clients.Group("Admin").SendAsync("CustomerUpdated", customer);

            HttpContext.Session.SetString("SuccessMsg", "Thêm khách hàng thành công!");
            // Redirect to the customer list or a success page
            return RedirectToAction("ViewList");
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> EditCustomer(Customer model)
        {

            if (!ModelState.IsValid)
            {
                // Log the validation errors
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                string errorMessages = string.Join("; ", errors);

                Debug.WriteLine(errorMessages);
                // If the model state is not valid, return the same view with validation errors
                return View("/Views/Customer/EditCustomer.cshtml", model);
            }
            User user = HttpContext.Session.GetCustomObjectFromSession<User>("user");
            Customer oldCustomer=_customerService.GetCustomerById(model.Id, user.Id);
            // Convert the model to your domain entity
            var customer = new Customer
            {
                Name = model.Name,
                Phone = model.Phone,
                Address = model.Address,
                Note = model.Note,  // Note property is nullable
                Status = model.Status,
                SellerId = oldCustomer.SellerId,
                UpdatedAt = DateTime.Now
            };

            // Use the service to edit the customer
            _customerService.EditCustomer(model.Id,customer);

            // Notify admins about changes to the customer
            await _hubContext.Clients.Group("Admin").SendAsync("CustomerUpdated", customer);

            HttpContext.Session.SetString("SuccessMsg", "Sửa khách hàng thành công!");
            // Redirect to the customer list or a success page
            return RedirectToAction("ViewList");
        }
    }

}
