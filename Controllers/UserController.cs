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
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                };

                // Use the service to insert the customer
                _userService.AddUser(user);

                HttpContext.Session.SetString("SuccessMsg", "Đăng ký tài khoản thành công!");
                return View("/Views/Home/Index.cshtml");
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
                // Store authenticated user in session
                HttpContext.Session.SetObjectInSession("user", authenticatedUser);

                if (authenticatedUser.Role == 2) // Seller
                {
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
