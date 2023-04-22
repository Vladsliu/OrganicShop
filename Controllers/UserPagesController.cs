using OrganicShop2.Models.Data;
using Microsoft.AspNetCore.Mvc;
using OrganicShop2.Interfaces;
using OrganicShop2.Models.Data;
using OrganicShop2.Models.ViewModels.Pages;
using OrganicShop2.Data;

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
                page = "003";

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


            //kostil

            List<PageVM> pageVMList = _context.Pages.ToArray().OrderBy(x => x.Sorting).Where(x => x.Slug != "002")
                         .Select(x => new PageVM(x)).ToList();

            ViewBag.PageVMList = pageVMList;

            ///kostil2

            SidebarVM modelSB;

            SidebarDTO dtoSB = _context.Sidebars.Find(1002);

            modelSB = new SidebarVM(dtoSB);

            ViewBag.SidebarModel = modelSB;



            //


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

            model = new SidebarVM(dto);

            return PartialView(model);
        

           
        }
    }
}
