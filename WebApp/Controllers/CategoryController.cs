using COCOApp.Helpers;
using COCOApp.Hubs;
using COCOApp.Models;
using COCOApp.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Diagnostics;

namespace COCOApp.Controllers
{
    public class CategoryController : Controller
    {

        private readonly IHubContext<CategoryHub> _hubContext;
        private CategoryService _categoryService;
        public CategoryController(CategoryService categoryService, IHubContext<CategoryHub> hubContext)
        {
            _categoryService = categoryService;
            _hubContext = hubContext;
        }
        private const int PageSize = 10;


        [HttpGet]
        public IActionResult GetList(string nameQuery, int pageNumber = 1)
        {
            User user = HttpContext.Session.GetCustomObjectFromSession<User>("user");
            var categories = _categoryService.GetCategories(nameQuery, pageNumber, PageSize, user.Id);
            var totalCategories = _categoryService.GetTotalCategories(nameQuery, user.Id);

            var response = new
            {
                categoryResults = categories,
                pageNumber = pageNumber,
                totalPages = (int)Math.Ceiling(totalCategories / (double)PageSize)
            };

            return Json(response);
        }

        [HttpGet]
        public IActionResult GetCategory(int categoryId, int pageNumber = 1)
        {
            User user = HttpContext.Session.GetCustomObjectFromSession<User>("user");
            Category model = _categoryService.GetCategoryById(categoryId, user.Id);
            ViewData["PageNumber"] = pageNumber;
            if (model != null)
            {
                return View("/Views/Category/CategoryDetail.cshtml", model);
            }
            else
            {
                return View("/Views/Category/ListCategorys.cshtml");
            }
        }
        public IActionResult ViewList(int pageNumber = 1)
        {
            ViewData["PageNumber"] = pageNumber;
            return View("/Views/Category/ListCategorys.cshtml");
        }
        public IActionResult ViewAdd(int pageNumber = 1)
        {
            ViewData["PageNumber"] = pageNumber;
            return View("/Views/Category/AddCategory.cshtml");
        }
        [HttpGet]
        public IActionResult ViewEdit(int categoryId, int pageNumber = 1)
        {
            User user = HttpContext.Session.GetCustomObjectFromSession<User>("user");
            Category model = _categoryService.GetCategoryById(categoryId, user.Id);
            ViewData["PageNumber"] = pageNumber;

            if (model != null)
            {
                return View("/Views/Category/EditCategory.cshtml", model);
            }
            else
            {
                return View("/Views/Category/ListCategorys.cshtml");
            }
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> AddCategory(Category model)
        {
            User user = HttpContext.Session.GetCustomObjectFromSession<User>("user");
            //model.SellerId = user.Id;

            if (!ModelState.IsValid)
            {
                // Log the validation errors
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                string errorMessages = string.Join("; ", errors);

                Debug.WriteLine(errorMessages);

                // Return the same view with validation errors
                return View("/Views/Categories/AddCategory.cshtml", model);
            }
            int sellerId = user.Id;
            if (sellerId == 0)
            {
                sellerId = HttpContext.Session.GetCustomObjectFromSession<int>("sellerId");
            }

            // Convert the model to your domain entity
            var category = new Category
            {
                CategoryName = model.CategoryName,
                Description = model.Description,
            };

            // Use the service to insert the product
            _categoryService.AddCategory(category);

            // Notify admins about changes to the product
            await _hubContext.Clients.Group("Admin").SendAsync("CategoryUpdated", category);

            // On success
            HttpContext.Session.SetString("SuccessMsg", "Thêm danh mục thành công!");

            // Redirect to the product list or a success page
            return RedirectToAction("ViewList");
        }


        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> EditCategory(Category model)
        {
            if (!ModelState.IsValid)
            {
                // Log the validation errors
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                string errorMessages = string.Join("; ", errors);

                Debug.WriteLine(errorMessages);

                // Return the same view with validation errors
                return View("/Views/Categories/EditCategory.cshtml", model);
            }

            User user = HttpContext.Session.GetCustomObjectFromSession<User>("user");
            Category oldCategory = _categoryService.GetCategoryById(model.Id, user.Id);
            // Convert the model to your domain entity
            var category = new Category
            {
                CategoryName = model.CategoryName,
                Description = model.Description,
            };

            // Use the service to edit the product
            _categoryService.EditCategory(model.Id, category);

            // Notify admins about changes to the product
            await _hubContext.Clients.Group("Admin").SendAsync("CategoryUpdated", category);

            HttpContext.Session.SetString("SuccessMsg", "Sửa danh mục thành công!");
            // Redirect to the customer list or a success page
            return RedirectToAction("ViewList");
        }

    }
}
