using Microsoft.AspNetCore.Mvc;
using OrganicShop2.Data;
using OrganicShop2.Interfaces;
using OrganicShop2.Models.ViewModels.Shop;

namespace OrganicShop2.Controllers
{
    public class UserShopController : Controller
    {
        private readonly Db _context;
        public UserShopController(Db context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return RedirectToAction("Index", "UserPages");
        }

        public IActionResult CategoryMenyPartial()
        {
            List<CategoryVM> categoryVMList;

            categoryVMList = _context.Categories.ToArray().OrderBy(x => x.Sorting).Select(x => new CategoryVM()).ToList();
            
            return PartialView("CategoryMenuPartial", categoryVMList);
        }
    }
}
