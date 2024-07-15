using COCOApp.Helpers;
using COCOApp.Models;
using COCOApp.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Diagnostics;

namespace COCOApp.Controllers
{
    public class CustomerController : Controller
    {
        private CustomerService _customerService;
        public CustomerController(CustomerService customerService)
        {
            _customerService = customerService;
        }
        private const int PageSize = 10;

        [HttpGet]
        public IActionResult GetList(string nameQuery, int pageNumber = 1)
        {
            User user = HttpContext.Session.GetCustomObjectFromSession<User>("user");
            var customers = _customerService.GetCustomers(nameQuery, pageNumber, PageSize, user.Id);
            var totalCustomers = _customerService.GetTotalCustomers(nameQuery, user.Id);

            var response = new
            {
                customerResults = customers,
                pageNumber = pageNumber,
                totalPages = (int)Math.Ceiling(totalCustomers / (double)PageSize)
            };

            return Json(response);
        }
        [HttpGet]
        public IActionResult GetCustomer(int customerId)
        {
            User user = HttpContext.Session.GetCustomObjectFromSession<User>("user");
            Customer model=_customerService.GetCustomerById(customerId,user.Id);
            Debug.WriteLine(model);
            if (model != null)
            {
                return View("/Views/Customer/CustomerDetail.cshtml", model);
            }
            else
            {
                return View("/Views/Customer/ListCustomers.cshtml");
            }
        }
        public IActionResult ViewAdd()
        {
            return View("/Views/Customer/AddCustomer.cshtml");
        }
        public IActionResult ViewList()
        {
            return View("/Views/Customer/ListCustomers.cshtml");
        }
        public IActionResult ViewDetail()
        {
            return View("/Views/Customer/CustomerDetail.cshtml");
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult AddCustomer(Customer model)
        {
            /*            if (!ModelState.IsValid)
                        {
                            // If the model state is not valid, return the same view with validation errors
                            return View("/Views/Customer/AddCustomer.cshtml", model);
                        }*/
            User user = HttpContext.Session.GetCustomObjectFromSession<User>("user");
            // Convert the model to your domain entity
            var customer = new Customer
            {
                Name = model.Name,
                Phone = model.Phone,
                Address = model.Address,
                Note = model.Note,  // Note property is nullable
                Status = model.Status,
                SellerId = user.Id,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };

            // Use the service to insert the customer
            _customerService.AddCustormer(customer);
            HttpContext.Session.SetString("SuccessMsg", "Thêm khách hàng thành công!");
            // Redirect to the customer list or a success page
            return RedirectToAction("ViewList");
        }

    }

}
