using Microsoft.AspNetCore.Mvc;
using OrganicShop2.Data;
using OrganicShop2.Interfaces;
using OrganicShop2.Models.Data;
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

        //public IActionResult CategoryMenyPartial()
        //{
        //    List<CategoryVM> categoryVMList;

        //    categoryVMList = _context.Categories.ToArray().OrderBy(x => x.Sorting).Select(x => new CategoryVM()).ToList();
            
        //    return PartialView("CategoryMenuPartial", categoryVMList);
        //}

        public IActionResult Category()
        {
            List<ProductVM> productVMList;
            string name = Request.Query["name"].ToString();

            CategoryDTO categoryDTO = _context.Categories.Where(x => x.Slug == name).FirstOrDefault();

            int catID = categoryDTO.Id;

            productVMList = _context.Products.ToArray().Where(x => x.CategoryId == catID).Select(x => new ProductVM(x)).ToList();

            var productCat = _context.Products.Where(x => x.CategoryId == catID).FirstOrDefault();

            if (productCat == null)
            {
                var catName = _context.Categories.Where(x => x.Slug == name).Select(x => x.Name).FirstOrDefault();
                ViewBag.CategoryName = catName;
            }
            else
            {
                ViewBag.CategoryName = categoryDTO.Name;
            }
            return View(productVMList);
        }

       public IActionResult ProductDetails(string name)
        {
            ProductDTO dto;
            ProductVM model;
            string slug = Request.Query["slug"].ToString();

            if (!_context.Products.Any(x => x.Slug.Equals(slug)))
                {
                return RedirectToAction("Index", "UserShop");
                }

            dto = _context.Products.Where(x => x.Slug == slug).FirstOrDefault();

            model = new ProductVM(dto);

            return View("ProductDetails", model);
        }
    }
}
