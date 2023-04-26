using OrganicShop2.Models.Data;
using Microsoft.AspNetCore.Mvc;
using OrganicShop2.Interfaces;
using OrganicShop2.Models.Data;
using OrganicShop2.Models.ViewModels.Pages;
using OrganicShop2.Data;
using OrganicShop2.Models.ViewModels.Shop;

namespace OrganicShop2.Controllers
{
    public class UserPagesController : Controller
    {
        private readonly Db _context;
        private readonly IPhotoService _photoService;
        public UserPagesController(Db context, IPhotoService photoService)
        {
            _context = context;
            _photoService = photoService;
        }
        public IActionResult Index(string page = "")

        {
            if (page == "")
                page = "apple-short";

            PageVM model;
            PagesDTO dto;

            if (!_context.Pages.Any(x => x.Slug.Equals(page)))
            {
                return NotFound();
                //return RedirectToAction("Index", new { page = "" });
            }

            dto = _context.Pages.Where(x => x.Slug == page).FirstOrDefault();

            ViewBag.PageTitle = dto.Title;

            if (dto.HasSidebar == true)
            {
                ViewBag.Sidebar = "Yes";
            }
            else
            {
                ViewBag.Sidebar = "No";
            }

            model = new PageVM(dto);


            ///
            SidebarVM modelSidebar;

            SidebarDTO dtoSidebar = _context.Sidebars.Find(1002);


            modelSidebar = new SidebarVM (dtoSidebar);


            ViewBag.SidebarModel = modelSidebar;

            ///
            List<PageVM> pageVMList;

            pageVMList = _context.Pages.ToArray().OrderBy(x => x.Sorting).Where(x => x.Slug != "002")
                         .Select(x => new PageVM(x)).ToList();
            ViewBag.ModelPagesMenuPartial = pageVMList;
			///
			List<CategoryVM> categoryVMList;

            categoryVMList = _context.Categories.ToArray().OrderBy(x => x.Sorting).Select(x => new CategoryVM(x)).ToList() ;

            ViewBag.CategoryVMList = categoryVMList;
			///

			return View(model);
        }

        
        public IActionResult PagesMenuPartial()
        {
            List<PageVM> pageVMList;

            pageVMList = _context.Pages.ToArray().OrderBy(x => x.Sorting).Where(x => x.Slug != "002")
                         .Select(x => new PageVM(x)).ToList();
            return PartialView(pageVMList);

        }

        public IActionResult SidebarPartial()
        {
            SidebarVM model;

            SidebarDTO dto = _context.Sidebars.Find(1002);

           
            model = new SidebarVM { Id = dto.id, Body = dto.Body };


            return PartialView("_SidebarPartial", model);
        }
    }
}
