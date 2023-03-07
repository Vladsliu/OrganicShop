using Microsoft.AspNetCore.Mvc;

namespace OrganicShop2.Controllers
{
    public class PagesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
