using COCOApp.Models;
using COCOApp.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace COCOApp.Controllers
{
    public class CustomerController : Controller
    {
        private CustomerService _customerService = new CustomerService();
        private const int PageSize = 10;

        [HttpGet]
        public IActionResult List(string nameQuery, int pageNumber = 1)
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

        [HttpGet]
        public IActionResult ViewList()
        {
            return View("/Views/Customer/ListCustomers.cshtml");
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View("/Views/Customer/AddCustomer.cshtml");
        }
    }

}
