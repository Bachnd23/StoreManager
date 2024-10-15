using COCOApp.Repositories;
using COCOApp.Services;
using Microsoft.AspNetCore.Mvc;

namespace COCOApp.Controllers
{
    public class DashboardController : Controller
    {
        private readonly ProductStatisticService _statisticService;

        public DashboardController(ProductStatisticService statisticService)
        {
            _statisticService = statisticService;
        }

        public async Task<IActionResult> Index()
        {
            var endDate = DateTime.Now;
            var startDate = endDate.AddDays(-30);
            var viewModel = await _statisticService.GetDashboardDataAsync(startDate, endDate);
            return View(viewModel);
        }
    }
}
