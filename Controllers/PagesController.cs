using CulinaryClub.Data;
using Microsoft.AspNetCore.Mvc;
using OrganicShop2.Models.Data;
using OrganicShop2.Models.ViewModels.Pages;

namespace OrganicShop2.Controllers
{
    public class PagesController : Controller
    {

        private readonly Db _context;
        public PagesController(Db context)
        {
            _context = context;
        }

        public IActionResult Index()
        
        { 
            List<PageVM> pageList = _context.Pages.ToArray().OrderBy(x => x.Sorting).Select(x => new PageVM(x)).ToList();
            return View(pageList);
        }



        public IActionResult AddPage()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddPage(PageVM model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            PagesDTO dto = new PagesDTO();
            dto.Title = model.Title.ToUpper();
            string slug;
            if (string.IsNullOrWhiteSpace(model.Slug))
            {
                slug = model.Title.Replace(" ", "-").ToLower();
            }
            else
            {
                slug = model.Slug.Replace(" ", "-").ToLower();
            }
            if (_context.Pages.Any(x => x.Title == model.Title))
            {
                ModelState.AddModelError("", "That title already exist.");
                return View(model);
            }
            else if (_context.Pages.Any(x => x.Slug == model.Slug))
            {
                ModelState.AddModelError("", "That slug already exist.");
            }
            dto.Slug = slug;
            dto.Description = model.Description;
            dto.HasSidebar = model.HasSidebar;
            dto.Sorting = 100;
            _context.Pages.Add(dto);
            _context.SaveChanges();

            TempData["SM"] = "Added new page!";
            return RedirectToAction("Index");
        }
        
    }
}
