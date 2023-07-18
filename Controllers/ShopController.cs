using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using OrganicShop2.Data;
using OrganicShop2.Interfaces;
using OrganicShop2.Models.Data;
using OrganicShop2.Models.ViewModels;
using OrganicShop2.Models.ViewModels.Pages;
using OrganicShop2.Models.ViewModels.Shop;
using OrganicShop2.Services;
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
            
            model.Categories = new SelectList(_context.Categories.ToList(), "Id", "Name");

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

        public IActionResult EditProduct(int id)
        {
            CreateProductDTOViewModel model = new CreateProductDTOViewModel();

            ProductDTO dto = _context.Products.Find(id);

            if (dto == null)
            {
                return Content("That product does not exist");
            }

            model = new CreateProductDTOViewModel(dto);

            model.Categories = new SelectList(_context.Categories.ToList(), "Id", "Name");

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditProduct(CreateProductDTOViewModel model)
        {
            int id = model.Id;

            if (!ModelState.IsValid)
            {
                TempData["SM"] = "Error somethyngs, try again";
                model.Categories = new SelectList(_context.Categories.ToList(), "Id", "Name", model.CategoryId);
                return View(model);
            }
            if (_context.Products.Where(x => x.Id != id).Any(x => x.Name == model.Name))
            {
                TempData["SM"] = "Error Name, try again";
                model.Categories = new SelectList(_context.Categories.ToList(), "Id", "Name", model.CategoryId);
                return View(model);
            }

            ProductDTO dto;
            dto = _context.Products.Find(id);

            dto.Name = model.Name;
            dto.Slug = model.Name.Replace(" ", "-").ToLower(); 
            dto.Description = model.Description;
            dto.Price = model.Price;
            dto.CategoryId = model.CategoryId;

            CategoryDTO catDTO = _context.Categories.FirstOrDefault(x => x.Id == model.CategoryId);
            dto.CategoryName = catDTO.Name;


            if (dto.Image != null && dto.Image.Length > 0)
            {
                await _photoService.DeletePhotoAsync(dto.Image);
            }
            else
            {
                TempData["SM"] = "Error somethyngs, try again";
                model.Categories = new SelectList(_context.Categories.ToList(), "Id", "Name", model.CategoryId);
                return View(model);
            
            }

            var result = await _photoService.AddPhotoAsync(model.Image);
            dto.Image = result.Url.ToString();
            
            _context.Products.Update(dto);
            _context.SaveChanges();
            TempData["SM"] = "Product updated";


            return RedirectToAction("Products");
        }

        public async Task<IActionResult> DeleteProduct(int id)
        {
            ProductDTO dto = _context.Products.Find(id);
            _context.Products.Remove(dto);

            if (dto.Image != null && dto.Image.Length > 0)
            {
                await _photoService.DeletePhotoAsync(dto.Image);
                TempData["SM"] = "One product deleted";
            }
            else
            {
                TempData["SM"] = "Error somethyngs, try again";
                return RedirectToAction("products");
            }
            _context.SaveChanges();

            return RedirectToAction("products");
        }

        public IActionResult Orders()
        {
            List<OrdersForUserVM> ordersForAdmin = new List<OrdersForUserVM>();

            List<OrderVM> orders = _context.Orders.ToArray().Select(x => new OrderVM(x)).ToList();

            foreach (var order in orders)
            {
                Dictionary<string, int> productAndQty = new Dictionary<string, int>();

                decimal total = 0m;

                List<OrderDetailsDTO> orderDetailsList = _context.OrderDetails.Where(x => x.OrderId == order.OrderId).ToList();

                UserDTO user = _context.Users.FirstOrDefault(x => x.Id == order.UserId);
                string username = user.Username;

                foreach (var orderDetails in orderDetailsList)
                {
                    ProductDTO product = _context.Products.FirstOrDefault(x => x.Id == orderDetails.ProductId);

                    decimal price = decimal.Parse(product.Price);

                    string productName = product.Name;

                    productAndQty.Add(productName, orderDetails.Quantity);

                    total += orderDetails.Quantity * price;
                }
                ordersForAdmin.Add(new OrdersForUserVM()
                {
                    OrderNumber = order.OrderId,
                    UserName = username,
                    Total = total,
                    ProductsAndQty = productAndQty,
                    CreatedAt = order.CreatedAt
                }
                    );
            }

            return View(ordersForAdmin);
        }
    }
}
