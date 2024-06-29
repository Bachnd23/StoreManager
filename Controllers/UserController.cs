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
/*            if (!ModelState.IsValid)
            {
                return View("/Views/Products/AddProduct.cshtml", model);
            }*/

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

                return View("/Views/Home/Index.cshtml");
            }
            catch (ArgumentException ex)
            {
                // Handle the exception and show the error message
                ModelState.AddModelError(string.Empty, ex.Message);
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
                // Pass user object back to view on failed login attempt
                ModelState.AddModelError(string.Empty, "Invalid username or password.");
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
