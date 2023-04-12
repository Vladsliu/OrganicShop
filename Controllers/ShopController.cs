using CulinaryClub.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OrganicShop2.Interfaces;
using OrganicShop2.Models.Data;
using OrganicShop2.Models.ViewModels;
using OrganicShop2.Models.ViewModels.Pages;
using OrganicShop2.Models.ViewModels.Shop;
using OrganicShop2.Services;
//using PagedList;
using X.PagedList;

namespace OrganicShop2.Controllers
{
    public class ShopController : Controller
    {
        private readonly Db _context;
        private readonly IPhotoService _photoService;
        public ShopController(Db context, IPhotoService photoService)
        {
            _context = context;
            _photoService = photoService;
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
            if (_context.Categories.Any(x => x.Name == model.Name))
            {
                TempData["SM"] = "This category already exist!";
                return View("AddCategories");
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

        public IActionResult DeleteCategory(int id)
        {
            CategoryDTO dto = _context.Categories.Find(id);

            _context.Categories.Remove(dto);
            _context.SaveChanges();

            TempData["SM"] = "You have deleted a category!";

            return RedirectToAction("Categories");
        }

        public IActionResult UpdateCategory(int id)
        {
            CategoryVM model;

            var dto = _context.Categories.Find(id);

            model = new CategoryVM(dto);

            return View(model);
        }

        [HttpPost]
        public IActionResult UpdateCategory(CategoryVM model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            if (_context.Categories.Any(x => x.Name == model.Name))
            {
                TempData["SM"] = "This category already exist!";
                return View(model);
            }

            CategoryDTO dto = _context.Categories.Find(model.Id);
           
            dto.Name = model.Name;
            dto.Slug = model.Name.Replace(" ", "-").ToLower();
            dto.Sorting = 100;

            _context.Categories.Update(dto);
            _context.SaveChanges();

            TempData["SM"] = "Category updated!";

            return RedirectToAction("Categories");
        }

        public IActionResult AddProduct()
        { 

            ProductVM model = new ProductVM();
            
            model.Categories = new SelectList(_context.Categories.ToList(), dataValueField: "Id", dataTextField: "Name");

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddProduct(CreateProductDTOViewModel model)
        {
            if (!ModelState.IsValid)
            {
                TempData["SM"] = "Error. Please, try again!";
                return RedirectToAction("AddProduct");
            }

            if (_context.Products.Any(x => x.Name == model.Name))
            {
                TempData["SM"] = "Error, name already exist!";
                return RedirectToAction("AddProduct");
            }
            var result = await _photoService.AddPhotoAsync(model.Image);

            ProductDTO product = new ProductDTO();
            product.Name = model.Name;
            product.Slug = model.Name.Replace(" ", "-").ToLower();
            product.Description = model.Description;
            product.Price = model.Price;
            product.CategoryId = model.CategoryId;
            product.Image = result.Url.ToString();

            CategoryDTO catDTO = _context.Categories.FirstOrDefault(x => x.Id == model.CategoryId);
            product.CategoryName = catDTO.Name;

            _context.Products.Add(product);
            _context.SaveChanges();

            TempData["SM"] = "You have added a product!";

        return RedirectToAction ("AddProduct");
        }

        public IActionResult Products(int? page, int? catId)
        {
            List<ProductVM> listOfProductVM;

            var pageNumber = page ?? 1;

            listOfProductVM = _context.Products.ToArray()
                              .Where(x => catId == null || catId == 0 || x.CategoryId == catId)
                              .Select(x => new ProductVM(x))
                              .ToList();

            ViewBag.Categories = new SelectList(_context.Categories.ToList(), "Id", "Name");
            ViewBag.SelectedCat = catId.ToString();

            var onePageOfProducts = listOfProductVM.ToPagedList(pageNumber, 3);
            ViewBag.onePageOfProducts = onePageOfProducts;

            return View(listOfProductVM);
        }
    }
}
