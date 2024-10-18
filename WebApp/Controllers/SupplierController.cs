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
    public class SupplierController : Controller
    {
        private readonly IHubContext<SupplierHub> _hubContext;
        private SupplierService _supplierService;
        public SupplierController(SupplierService supplierService, IHubContext<SupplierHub> hubContext)
        {
            _supplierService = supplierService;
            _hubContext = hubContext;
        }
        private const int PageSize = 10;

        [Authorize(Roles = "Admin,Seller")]
        [HttpGet]
        public IActionResult GetList(string nameQuery,int statusId, int pageNumber = 1)
        {
            User user = HttpContext.Session.GetCustomObjectFromSession<User>("user");
            var suppliers = _supplierService.GetSuppliers(nameQuery, pageNumber, PageSize, user.Id, statusId);
            var totalSuppliers = _supplierService.GetTotalSuppliers(nameQuery, user.Id,statusId);

            var response = new
            {
                supplierResults = suppliers,
                pageNumber = pageNumber,
                totalPages = (int)Math.Ceiling(totalSuppliers / (double)PageSize)
            };

            return Json(response);
        }

        [Authorize(Roles = "Admin,Seller")]
        [HttpGet]
        public IActionResult GetSupplier(int supplierId, int pageNumber = 1)
        {
            User user = HttpContext.Session.GetCustomObjectFromSession<User>("user");
            Supplier model =_supplierService.GetSupplierById(supplierId,user.Id);
            ViewData["PageNumber"] = pageNumber;
            if (model != null)
            {
                return View("/Views/Supplier/SupplierDetail.cshtml", model);
            }
            else
            {
                return View("/Views/Supplier/ListSuppliers.cshtml");
            }
        }
        [Authorize(Roles = "Admin,Seller")]
        [HttpGet]
        public IActionResult ViewEdit(int supplierId, int pageNumber = 1)
        {
            User user = HttpContext.Session.GetCustomObjectFromSession<User>("user");
            Supplier model = _supplierService.GetSupplierById(supplierId, user.Id);
            ViewData["PageNumber"] = pageNumber;
            if (model != null)
            {
                return View("/Views/Supplier/EditSupplier.cshtml", model);
            }
            else
            {
                return View("/Views/Supplier/ListSuppliers.cshtml");
            }
        }

        [Authorize(Roles = "Admin,Seller")]
        public IActionResult ViewAdd()
        {
            return View("/Views/Supplier/AddSupplier.cshtml");
        }

        [Authorize(Roles = "Admin,Seller")]
        public IActionResult ViewList(int pageNumber = 1)
        {
            ViewData["PageNumber"] = pageNumber;
            return View("/Views/Supplier/ListSuppliers.cshtml");
        }
        [Authorize(Roles = "Admin,Seller")]
        public IActionResult ViewDetail()
        {
            return View("/Views/Supplier/SupplierDetail.cshtml");
        }
        [Authorize(Roles = "Admin,Seller")]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> AddSupplier(Supplier model)
        {
            if (!ModelState.IsValid)
            {
                // Log the validation errors
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                string errorMessages = string.Join("; ", errors);

                Debug.WriteLine(errorMessages);
                // If the model state is not valid, return the same view with validation errors
                return View("/Views/Supplier/AddSupplier.cshtml", model);
            }
            User user = HttpContext.Session.GetCustomObjectFromSession<User>("user");
            int sellerId = user.Id;
            if (sellerId == 0)
            {
                sellerId = HttpContext.Session.GetCustomObjectFromSession<int>("sellerId");
            }
            // Convert the model to your domain entity
            var supplier = new Supplier
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

            // Use the service to insert the supplier
            _supplierService.AddSupplier(supplier);

            // Notify admins about changes to the supplier
            await _hubContext.Clients.Group("Admin").SendAsync("SupplierUpdated", supplier);

            HttpContext.Session.SetString("SuccessMsg", "Thêm nhà cung cấp thành công!");
            // Redirect to the supplier list or a success page
            return RedirectToAction("ViewList");
        }
        [Authorize(Roles = "Admin,Seller")]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> EditSupplier(Supplier model)
        {

            if (!ModelState.IsValid)
            {
                // Log the validation errors
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                string errorMessages = string.Join("; ", errors);

                Debug.WriteLine(errorMessages);
                // If the model state is not valid, return the same view with validation errors
                return View("/Views/Supplier/EditSupplier.cshtml", model);
            }
            User user = HttpContext.Session.GetCustomObjectFromSession<User>("user");
            Supplier oldSupplier=_supplierService.GetSupplierById(model.Id, user.Id);
            // Convert the model to your domain entity
            var supplier = new Supplier
            {
                Name = model.Name,
                Phone = model.Phone,
                Address = model.Address,
                Note = model.Note,  // Note property is nullable
                Status = model.Status,
                SellerId = oldSupplier.SellerId,
                UpdatedAt = DateTime.Now
            };

            // Use the service to edit the supplier
            _supplierService.EditSupplier(model.Id,supplier);

            // Notify admins about changes to the supplier
            await _hubContext.Clients.Group("Admin").SendAsync("SupplierUpdated", supplier);

            HttpContext.Session.SetString("SuccessMsg", "Sửa nhà cung cấp  thành công!");
            // Redirect to the supplier list or a success page
            return RedirectToAction("ViewList");
        }
    }

}
