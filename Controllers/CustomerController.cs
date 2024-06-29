using COCOApp.Models;
using COCOApp.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

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
            var customers = _customerService.GetCustomers(nameQuery, pageNumber, PageSize);
            var totalCustomers = _customerService.GetTotalCustomers(nameQuery);

            var response = new
            {
                customerResults = customers,
                pageNumber = pageNumber,
                totalPages = (int)Math.Ceiling(totalCustomers / (double)PageSize)
            };

            return Json(response);
        }
        public IActionResult ViewAdd()
        {
            return View("/Views/Customer/AddCustomer.cshtml");
        }
        public IActionResult ViewList()
        {
            return View("/Views/Customer/ListCustomers.cshtml");
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
            // Convert the model to your domain entity
            var customer = new Customer
            {
                Name = model.Name,
                Phone = model.Phone,
                Address = model.Address,
                Note = model.Note,  // Note property is nullable
                Status = model.Status,
                SellerId = 1,//to be updated
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
