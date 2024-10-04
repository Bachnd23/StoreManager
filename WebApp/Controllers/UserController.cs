using COCOApp.Models;
using COCOApp.Services;
using Microsoft.AspNetCore.Mvc;
using BCrypt.Net;
using COCOApp.Helpers;
using System.Diagnostics;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
namespace COCOApp.Controllers
{
    public class UserController : Controller
    {
        private readonly UserService _userService;
        private readonly UserDetailsService _userDetailsService;
        private readonly EmailService _emailService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public UserController(UserService userService, UserDetailsService userDetailsService, EmailService emailService, IHttpContextAccessor httpContextAccessor)
        {
            _userService = userService;
            _userDetailsService = userDetailsService;
            _emailService = emailService;
            _httpContextAccessor = httpContextAccessor;
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
        [HttpGet]
        public IActionResult GetUser(int userId)
        {
            User model = _userService.GetUserById(userId);
            if (model != null)
            {
                return View("/Views/User/UserDetail.cshtml", model);
            }
            else
            {
                return View("/Views/User/ListUsers.cshtml");
            }
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
        [HttpGet]
        public IActionResult ViewEdit(int userId)
        {
            User model = _userService.GetUserById(userId);
            if (model != null)
            {
                return View("/Views/User/EditUser.cshtml", model);
            }
            else
            {
                return View("/Views/User/ListUsers.cshtml");
            }
        }
        [HttpPost]
        public IActionResult EditUser(User model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    // Log the validation errors
                    var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                    string errorMessages = string.Join("; ", errors);

                    Debug.WriteLine(errorMessages);
                    // If the model state is not valid, return the same view with validation errors
                    return View("/Views/User/EditUser.cshtml", model);
                }
                Debug.WriteLine(model.Role);
                var user = new User
                {
                    Id = model.Id,
                    Email = model.Email,
                    Username = model.Username,
                    Role = model.Role,
                    Status = model.Status,
                    UpdatedAt = DateTime.Now
                };
                // Use the service to insert the customer
                _userService.UpdateUser(model.Id, user);
                HttpContext.Session.SetString("SuccessMsg", "Sửa khách hàng thành công!");
                // Redirect to the customer list or a success page
                return RedirectToAction("ViewList");
            }
            catch (ArgumentException ex)
            {
                HttpContext.Session.SetString("ErrorMsg", "Email hoặc tên đăng nhập đã được sử dụng!");
                return View("/Views/User/EditUser.cshtml", model);
            }
        }
        public async Task<IActionResult> ViewChangePassword(string email)
        {
            if (await _userService.CheckPasswordResetTokenAsync(email))
            {
                User user = _userService.GetActiveUserByEmail(email);
                return View("/Views/User/ChangePassword.cshtml", user);
            }
            else
            {
                HttpContext.Session.SetString("ErrorMsg", "Yêu cầu đã hết hạn hoặc không tồn tại!");
                return View("/Views/Home/SignIn.cshtml");
            }
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
                var userDetail = new BuyerDetail
                {
                    UserId = user.Id,
                    Fullname = "",
                    Address = "",
                    Phone = "",
                    Dob = DateTime.Now,
                    Gender = true,
                };
                _userDetailsService.AddUserDetails(userDetail);
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
        public async Task<IActionResult> Login(User user)
        {
            var authenticatedUser = _userService.GetUserByNameAndPass(user.Username, user.Password);

            if (authenticatedUser != null)
            {
                if (user.RememberToken != null)
                {
                    await _userService.UpdateRememberMeTokenAsync(user.Username);
                }

                // Create claims for the authenticated user
                var claims = new List<Claim>
                    {
                    new Claim(ClaimTypes.Name, authenticatedUser.Username),
                    new Claim(ClaimTypes.Email, authenticatedUser.Email),
                    new Claim(ClaimTypes.Role, GetRoleName((int)authenticatedUser.Role)) // Convert role ID to role name
                    };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

                // Sign in the user
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal);

                // Store additional data in session if needed
                if (authenticatedUser.Role == 2) // Seller
                {
                    HttpContext.Session.SetObjectInSession("user", authenticatedUser);
                    return RedirectToAction("Index", "Home");
                }
                else if (authenticatedUser.Role == 1) // Admin
                {
                    int sellerId = authenticatedUser.Id;
                    HttpContext.Session.SetObjectInSession("sellerId", sellerId);
                    authenticatedUser.Id = 0;
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

        // Helper method to convert role ID to role name
        private string GetRoleName(int roleId)
        {
            return roleId switch
            {
                1 => "Admin",
                2 => "Seller",
                _ => "Customer"
            };
        }

        public async Task<IActionResult> LogOut()
        {
            // Clear the session data
            HttpContext.Session.Clear();

            // Delete the remember me token and its expiration from the cookies
            HttpContext.Response.Cookies.Delete("RememberMeToken");
            HttpContext.Response.Cookies.Delete("RememberMeTokenTokenExpiration");

            // Sign out from the authentication scheme
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            // Redirect to the login page or home page after logout
            return RedirectToAction("ViewSignIn", "Home");
        }

        [HttpPost]
        public IActionResult UpdateUser(User model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    // Log the validation errors
                    var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                    string errorMessages = string.Join("; ", errors);

                    Debug.WriteLine(errorMessages);
                    // If the model state is not valid, return the same view with validation errors
                    return View("/Views/User/UserProfile.cshtml", model);
                }
                int sellerId = model.Id;
                if (sellerId == 0)
                {
                    sellerId = HttpContext.Session.GetCustomObjectFromSession<int>("sellerId");
                }
                // Convert the model to your domain entity
                var user = new User
                {
                    Id = sellerId,
                    Email = model.Email,
                    Username = model.Username,
                    Status = true,
                    UpdatedAt = DateTime.Now
                };
                // Use the service to insert the customer
                _userService.UpdateUser(sellerId, user);
                var userDetail = new BuyerDetail
                {
                    Fullname = model.BuyerDetail.Fullname,
                    Dob = model.BuyerDetail.Dob,
                    Gender = model.BuyerDetail.Gender,
                    Address = model.BuyerDetail.Address,
                    Phone = model.BuyerDetail.Phone,
                };
                _userDetailsService.UpdateUserDetails(sellerId, userDetail);
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
            User user = _userService.GetActiveUserByEmail(toEmail);
            if (user == null)
            {
                HttpContext.Session.SetString("ErrorMsg", "Tài khoản không tồn tại!");
                return View("/Views/User/ForgotPassword.cshtml");
            }
            var subject = "Yêu cầu đổi mật khẩu";
            string link = $"<a href='https://localhost:7178/User/ViewChangePassword?email={toEmail}'>Bấm vào đây</a>";
            String htmlMessage = "<html><body>" + "<p>Chúng tôi vừa nhận được yêu cầu đổi mật khẩu cho " + toEmail
                + "</p>" + "<p>Vui lòng " + link + " để thay đổi mật khẩu của bạn.</p>"
                + "<p>Vì sự bảo mật của bạn, link trên sẽ hết hạn trong 24 giờ hoặc ngay sau khi bạn thay đổi mật khẩu.</p>"
                + "<p>Cảm ơn vì đã sử dụng!<br/>CoCo Team.</p>" + "</body></html>";

            // Create a list of tasks to showcase asynchronous execution
            var tasks = new List<Task>
                {
                    // Example of sending an email asynchronously
                    _emailService.SendEmailAsync(toEmail, subject, htmlMessage),

                    // Example of performing another asynchronous operation (e.g., database update)
                    _userService.UpdateUserPasswordResetTokenAsync(toEmail)
                };

            // Await all tasks to complete
            await Task.WhenAll(tasks);
            HttpContext.Session.SetString("SuccessMsg", "Yêu cầu được chấp nhận, vui lòng kiểm tra email của bạn!");
            return RedirectToAction("ViewSignIn", "Home");
        }
        [HttpPost]
        public IActionResult UpdatePassword(User model)
        {
            // Hash the password using BCrypt
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(model.Password);

            // Use the service to update the user's password
            _userService.UpdateUserPassword(model.Id, hashedPassword);

            // Delete the password reset token and its expiration from the cookies
            HttpContext.Response.Cookies.Delete("PasswordResetToken");
            HttpContext.Response.Cookies.Delete("PasswordResetTokenExpiration");

            // Set a success message in the session
            HttpContext.Session.SetString("SuccessMsg", "Cập nhật mật khẩu thành công!");

            // Redirect to the sign-in view
            return RedirectToAction("ViewSignIn", "Home");
        }

    }
}
