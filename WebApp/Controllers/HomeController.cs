using COCOApp.Helpers;
using COCOApp.Models;
using COCOApp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace COCOApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserService _userService;

        public HomeController(ILogger<HomeController> logger,UserService userService)
        {
            _logger = logger;
            _userService = userService;
        }
        [Authorize(Roles = "Admin,Seller")]
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult ViewRegisterStore()
        {
            return View("/Views/Home/RegisterStore.cshtml");
        }
        public async Task<IActionResult> ViewSignIn()
        {
            User authenticatedUser =await _userService.CheckRememberMeTokenAsync();

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
                    int sellerId = authenticatedUser.Id;
                    HttpContext.Session.SetObjectInSession("sellerId", sellerId);
                    authenticatedUser.Id = 0;
                    // Store authenticated user in session
                    HttpContext.Session.SetObjectInSession("user", authenticatedUser);
                    return RedirectToAction("Index", "Home");
                }
            }
            return View("/Views/Home/SignIn.cshtml");
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [AllowAnonymous]
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
