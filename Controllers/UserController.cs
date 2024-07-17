using COCOApp.Models;
using COCOApp.Services;
using Microsoft.AspNetCore.Mvc;
using BCrypt.Net;
using COCOApp.Helpers;
using System.Diagnostics;
namespace COCOApp.Controllers
{
    public class UserController : Controller
    {
        private readonly UserService _userService;
        private readonly SellerDetailsService _sellerDetailsService;
        private readonly EmailService _emailService;
        public UserController(UserService userService, SellerDetailsService sellerDetailsService, EmailService emailService)
        {
            _userService = userService;
            _sellerDetailsService = sellerDetailsService;
            _emailService = emailService;
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
        public IActionResult ViewForgotPassword()
        {
            return View("/Views/User/ForgotPassword.cshtml");
        }
        public IActionResult ViewProfile()
        {
            User user = HttpContext.Session.GetCustomObjectFromSession<User>("user");
            return View("/Views/User/UserProfile.cshtml", user);
        }
        [HttpPost]
        public IActionResult RegisterUser(User model)
        {
            if (!ModelState.IsValid)
            {
                // Log the validation errors
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                string errorMessages = string.Join("; ", errors);

                Debug.WriteLine(errorMessages);
                // If the model state is not valid, return the same view with validation errors
                return View("/Views/Home/RegisterStore.cshtml", model);
            }
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
                    Status = true,
                    Role = 2,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                };
                // Use the service to insert the customer
                _userService.AddUser(user);
                var sellerDetail = new SellerDetail
                {
                    Id = user.Id,
                    Fullname = "",
                    Address = "",
                    Phone = "",
                    Dob = DateTime.Now,
                    Gender = true,
                };
                _sellerDetailsService.AddSellerDetails(sellerDetail);
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
                else if (authenticatedUser.Role == 1)// Admin
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
        [HttpPost]
        public IActionResult UpdateUser(User model)
        {
            try
            {
                // Convert the model to your domain entity
                var user = new User
                {
                    Email = model.Email,
                    Username = model.Username,
                    UpdatedAt = DateTime.Now
                };
                // Use the service to insert the customer
                _userService.UpdateUser(model.Id, user);
                var sellerDetail = new SellerDetail
                {
                    Fullname = model.SellerDetail.Fullname,
                    Dob = model.SellerDetail.Dob,
                    Gender = model.SellerDetail.Gender,
                    Address = model.SellerDetail.Address,
                    Phone = model.SellerDetail.Phone,
                };
                _sellerDetailsService.UpdateSellerDetails(model.Id, sellerDetail);
                HttpContext.Session.SetString("SuccessMsg", "Cập nhật tài khoản thành công!");
                return View("/Views/User/UserProfile.cshtml", model);
            }
            catch (ArgumentException ex)
            {
                HttpContext.Session.SetString("ErrorMsg", "Email hoặc tên đăng nhập đã được sử dụng!");
                return View("/Views/User/UserProfile.cshtml", model);
            }
        }
        [HttpPost]
        public async Task<IActionResult> ForgotPassword(string toEmail)
        {
            var subject = "Password Reset Request";
            var message = "Please click the link to reset your password.";

            // Create a list of tasks to showcase asynchronous execution
            var tasks = new List<Task>
    {
        // Example of sending an email asynchronously
        _emailService.SendEmailAsync(toEmail, subject, message),

        // Example of performing another asynchronous operation (e.g., database update)
/*        UpdateUserPasswordResetTokenAsync(toEmail)*/
    };

            // Await all tasks to complete
            await Task.WhenAll(tasks);

            return RedirectToAction("Index");
        }

    }
}
