using CulinaryClub.Data;
using Microsoft.AspNetCore.Mvc;
using OrganicShop2.Interfaces;
using OrganicShop2.Models.Data;
using OrganicShop2.Models.ViewModels;
using OrganicShop2.Models.ViewModels.Pages;
using OrganicShop2.Models.ViewModels.Shop;
using OrganicShop2.Services;

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

        public IActionResult AddNewCategory()

        {
            return View();
        }


        [HttpPost]
        public IActionResult AddNewCategory(CategoryVM model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            CategoryDTO dto = new CategoryDTO();

            dto.Name = model.Name;
            dto.Slug = model.Name.Replace(" ", "-").ToLower();
            dto.Sorting = 100;

            _context.Categories.Add(dto);
            _context.SaveChanges();

            TempData["SM"] = "Added new category page!";
            return RedirectToAction("Categories");

        }

        public IActionResult AddCategories()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddCategories(CreatePagesDTOViewModel pagesDTOVM)
        {
            if (!ModelState.IsValid)
            {
                return View(pagesDTOVM);
            }

            PagesDTO dto = new PagesDTO();
            dto.Title = pagesDTOVM.Title.ToUpper();
            string slug;
            if (string.IsNullOrWhiteSpace(pagesDTOVM.Slug))
            {
                slug = pagesDTOVM.Title.Replace(" ", "-").ToLower();
            }
            else
            {
                slug = pagesDTOVM.Slug.Replace(" ", "-").ToLower();
            }
            if (_context.Pages.Any(x => x.Title == pagesDTOVM.Title))
            {
                ModelState.AddModelError("", "That title already exist.");
                return View(pagesDTOVM);
            }
            else if (_context.Pages.Any(x => x.Slug == pagesDTOVM.Slug))
            {
                ModelState.AddModelError("", "That slug already exist.");
            }
            dto.Slug = slug;
            dto.Description = pagesDTOVM.Description;
            dto.HasSidebar = pagesDTOVM.HasSidebar;
            dto.Sorting = 100;
            _context.Pages.Add(dto);
            _context.SaveChanges();

            TempData["SM"] = "Added new cat!";
            return RedirectToAction("Categories");
        }


    }
}
