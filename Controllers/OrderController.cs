using Microsoft.AspNetCore.Mvc;

namespace COCOApp.Controllers
{
    public class OrderController : Controller
    {
        public IActionResult List()
        {
            return View("/Views/Order/ListOrders.cshtml");
        }
        public IActionResult Add()
        {
            return View("/Views/Order/AddOrder.cshtml");
        }
    }
}
