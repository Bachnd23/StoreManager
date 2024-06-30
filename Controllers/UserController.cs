using COCOApp.Models;
using COCOApp.Services;
using Microsoft.AspNetCore.Mvc;
using BCrypt.Net;
using COCOApp.Helpers;
namespace COCOApp.Controllers
{
    public class UserController : Controller
    {
        private readonly UserService _userService;

        public UserController(UserService userService)
        {
            _userService = userService;
        }

        private const int PageSize = 10;

        [HttpGet]
        public IActionResult GetList(string nameQuery, int pageNumber = 1)
        {
            var users = _userService.GetUsers(nameQuery, pageNumber, PageSize);
            var totalUsers = _userService.GetTotalUsers(nameQuery);

            var response = new
            {
                userResults = users,
                pageNumber = pageNumber,
                totalPages = (int)Math.Ceiling(totalUsers / (double)PageSize)
            };

            return Json(response);
        }
        public IActionResult ViewList()
        {
            return View("/Views/User/ListUsers.cshtml");
        }
        public IActionResult ViewAdd()
        {
            return View("/Views/User/AddUser.cshtml");
        }
        [HttpPost]
        public IActionResult RegisterUser(User model)
        {
            try
            {
                // Hash the password using BCrypt
                var hashedPassword = BCrypt.Net.BCrypt.HashPassword(model.Password);

                // Convert the model to your domain entity
                var user = new User
                {
                    Email = model.Email,
                    Username = model.Username,
                    Password = hashedPassword,
                    Status=true,
                    Role=2,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                };

                // Use the service to insert the customer
                _userService.AddUser(user);

                HttpContext.Session.SetString("SuccessMsg", "Đăng ký tài khoản thành công!");
                return View("/Views/Home/SignIn.cshtml");
            }
            catch (ArgumentException ex)
            {
                HttpContext.Session.SetString("ErrorMsg", "Email hoặc tên đăng nhập đã được sử dụng!");
                return View("/Views/Home/RegisterStore.cshtml", model);
            }
        }
        [ValidateAntiForgeryToken]
        public IActionResult Login(User user)
        {
            var authenticatedUser = _userService.GetUserByNameAndPass(user.Username, user.Password);

            if (authenticatedUser != null)
            {

                if (authenticatedUser.Role == 2) // Seller
                {
                    // Store authenticated user in session
                    HttpContext.Session.SetObjectInSession("user", authenticatedUser);
                    return RedirectToAction("Index", "Home");
                }
                else if(authenticatedUser.Role == 1)// Admin
                {
                    authenticatedUser.Id = 0;
                    // Store authenticated user in session
                    HttpContext.Session.SetObjectInSession("user", authenticatedUser);
                    return RedirectToAction("Index", "Home");
                } 
                else
                {
                    return View("/Views/Home/SignIn.cshtml", user);
                }
            }
            else
            {
                HttpContext.Session.SetString("ErrorMsg", "Tên đăng nhập hoặc mật khẩu sai");
                return View("/Views/Home/SignIn.cshtml", user);
            }
        }

        public IActionResult LogOut()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("ViewSignIn", "Home");
        }
    }
}
