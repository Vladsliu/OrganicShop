using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using OrganicShop2.Data;
using OrganicShop2.Interfaces;
using OrganicShop2.Models.Data;
using OrganicShop2.Models.ViewModels;
using OrganicShop2.Models.ViewModels.Pages;
using OrganicShop2.Models.ViewModels.Shop;

namespace OrganicShop2.Controllers
{
    public class PagesController : Controller
    {

        private readonly Db _context;
        private readonly IPhotoService _photoService;
        public PagesController(Db context, IPhotoService photoService)
        {
            _context = context;
            _photoService = photoService;
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
        public async Task<IActionResult> AddPage(CreatePagesDTOViewModel pagesDTOVM)
        {
            if (!ModelState.IsValid)
            {
                return View(pagesDTOVM);
            }

            var result = await _photoService.AddPhotoAsync(pagesDTOVM.Image);

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
            dto.Image = result.Url.ToString();
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
        public async Task<IActionResult> EditPage(CreatePagesDTOViewModel pagesDTOVM)
        {
            if (!ModelState.IsValid)
            {
                return View(pagesDTOVM);
            }


            int id = pagesDTOVM.Id;
            string slug = "home";

            PagesDTO dto = _context.Pages.Find(id);


            var result = await _photoService.AddPhotoAsync(pagesDTOVM.Image);

            if (dto.Image != null && dto.Image.Length > 0)
            {
                await _photoService.DeletePhotoAsync(dto.Image);
            }
            else
            {
                TempData["SM"] = "Error somethyngs, try again";
                return View(pagesDTOVM);

            }

            dto.Title = pagesDTOVM.Title;
           
            if (pagesDTOVM.Slug != "home")
            {
                if (string.IsNullOrWhiteSpace(pagesDTOVM.Slug))  
                {
                    slug = pagesDTOVM.Title.Replace(" ", "-").ToLower();
                }
                else
                {
                    slug = pagesDTOVM.Slug.Replace(" ", "-").ToLower();
                }
            }
            if (_context.Pages.Where(x => x.Id != id).Any(x => x.Title == pagesDTOVM.Title))
            {
                ModelState.AddModelError("", "That title already exist");
                return View(pagesDTOVM);
            }
            else if (_context.Pages.Where(x => x.Id != id).Any(x => x.Slug == slug))
            {
                ModelState.AddModelError("", "That slug already exist");
                return View(pagesDTOVM);
            }
            dto.Slug = slug;
            dto.Description = pagesDTOVM.Description;
            dto.HasSidebar = pagesDTOVM.HasSidebar;
            dto.Image = result.Url.ToString();

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

        public IActionResult Sidebar()
        {
            List<SidebarVM> sidebarVMList;

            sidebarVMList = _context.Sidebars.ToArray().Select(x => new SidebarVM(x)).ToList();
            return View(sidebarVMList);
        }
        public IActionResult EditSidebar(int id)
        {
           
            SidebarVM model;
           
            var dto = _context.Sidebars.Find(id);
            
            model = new SidebarVM(dto);
            
            return View(model);

        }

        [HttpPost]
        public IActionResult EditSidebar(SidebarVM model)
        {
            
            SidebarDTO dto = _context.Sidebars.Find(model.Id);

            dto.Body = model.Body;
            
            _context.SaveChanges();
           
            TempData["SM"] = "You have updated a sidebar!";

            return RedirectToAction("Sidebar");
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
            return RedirectToAction("Sidebar");

        }

        public IActionResult DeleteSidebar(int id) 
        {
            SidebarDTO dto = new SidebarDTO();

            dto = _context.Sidebars.Find(id);

            _context.Remove(dto);
            _context.SaveChanges();

            TempData["SM"] = "One element of sidebar deleted";

            return RedirectToAction("Sidebar");
        }
 
    }
}
