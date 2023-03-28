using CulinaryClub.Data;
using Microsoft.AspNetCore.Mvc;
using OrganicShop2.Interfaces;
using OrganicShop2.Models.ViewModels.Shop;

namespace OrganicShop2.Controllers
{
    public class ShopController : Controller
    {
        private readonly Db _context;
        public ShopController(Db context)
        {
            _context = context; 
        }
        public IActionResult Categories()
        {
       
            List<CategoryVM> categoryVMList;
           
            categoryVMList = _context.Categories.ToArray().OrderBy(x =>x.Sorting).Select(x => new CategoryVM(x)).ToList();
          
            return View(categoryVMList);
        }
    }
}
