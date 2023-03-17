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
        [HttpGet]
        public IActionResult EditPage(int id)
        {
            PageVM model;
            PagesDTO dto = new PagesDTO();
           
            dto = _context.Pages.Find(id);

            if (dto == null)
            {
                return Content("The page is not exist.");
            }

            model = new PageVM(dto);
            return View(model);
        }

        [HttpPost]
        public IActionResult EditPage(PageVM model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            int id = model.Id;
            string slug = "home";

            PagesDTO dto = _context.Pages.Find(id);
           
            dto.Title = model.Title;
           
            if (model.Slug != "home")
            {
                if (string.IsNullOrWhiteSpace(model.Slug))
                {
                    slug = model.Title.Replace(" ", "-").ToLower();
                }
                else
                {
                    slug = model.Slug.Replace(" ", "-").ToLower();
                }
            }
            if (_context.Pages.Where(x => x.Id != id).Any(x => x.Title == model.Title))//??
            {
                ModelState.AddModelError("", "That title already exist");
                return View(model);
            }
            else if (_context.Pages.Where(x => x.Id != id).Any(x => x.Slug == slug))
            {
                ModelState.AddModelError("", "That slug already exist");
                return View(model);
            }
            dto.Slug = slug;
            dto.Description = model.Description;
            dto.HasSidebar = model.HasSidebar;
           
            _context.SaveChanges();
            
            TempData["SM"] = "You have adited the page.";

            return RedirectToAction("EditPage");
        }

        public IActionResult PageDetails(int id)
        {
            PageVM model;

            PagesDTO dto = _context.Pages.Find(id);

            if (dto == null) 
            {
                return Content("The page does not exist.");
            }

            model = new PageVM(dto);
            return View(model);
        }
        
        public IActionResult DeletePage(int id)
        {
            PagesDTO dto = _context.Pages.Find(id);

            _context.Pages.Remove(dto);
            _context.SaveChanges();

            TempData["SM"] = "You have deleted a page!";

            return RedirectToAction("Index");
        }

        public IActionResult EditSidebar()
        {
           
            SidebarVM model;
           
            var dto = _context.Sidebars.Find(1);//not good use "1"
            
            model = new SidebarVM(dto);
            
            return View(model);

        }

        [HttpPost]
        public IActionResult EditSidebar(SidebarVM model)
        {
            
            SidebarDTO dto = _context.Sidebars.Find(1);
            
            dto.Body = model.Body;
            
            _context.SaveChanges();
           
            TempData["SM"] = "You have updated a sidebar!";

            return RedirectToAction("EditSidebar");
        }

        public IActionResult CreateSidebar()
        
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreateSidebar(SidebarVM model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            SidebarDTO dto = new SidebarDTO();
           
            dto.Body = model.Body;

            _context.Sidebars.Add(dto);
            _context.SaveChanges();

            TempData["SM"] = "Added new sidebar page!";
            return RedirectToAction("CreateSidebar");

        }
    }
}
