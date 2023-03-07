using Microsoft.AspNetCore.Mvc;

namespace OrganicShop2.Controllers
{
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
