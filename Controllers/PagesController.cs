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
            //Оголошую модель пейджвиюмодел
            PageVM model;
            PagesDTO dto = new PagesDTO();
           
            //получаем страницу
            dto = _context.Pages.Find(id);

            //провереем доступна ли страница
            if (dto == null)
            {
                return Content("The page is not exist.");
            }
            //инициалізуєм модель данних
            model = new PageVM(dto);
            //повертаєм  модель у вью
            return View(model);
        }

        [HttpPost]
        public IActionResult EditPage(PageVM model)
        {
            //проверяем модель на валидность
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            //оголосим переменную краткого заоловка
            int id = model.Id;
            string slug = "home";
            //получаем страницу
            PagesDTO dto = _context.Pages.Find(id);
            //присвоить название из полученной ДТО
            dto.Title = model.Title;
            //проверяем краткий заголовок и присваиваем его если не обходимо
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
            //проверяем слаг и тайтл на уникальность
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
            //записиваем остальние значения в класДТО
            dto.Slug = slug;
            dto.Description = model.Description;
            dto.HasSidebar = model.HasSidebar;
            //зберігаєм зміни в бд
            _context.SaveChanges();
            //виводимо повідомлення що все зроблено успішно(темпдата)
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
        
    }
}
