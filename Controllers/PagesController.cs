using CulinaryClub.Data;
using Microsoft.AspNetCore.Mvc;
using OrganicShop2.Models.Data;
using OrganicShop2.Models.ViewModels.Pages;

namespace OrganicShop2.Controllers
{
    public class PagesController : Controller
    {
        public IActionResult Index()
        {
            //оголошую список для вью (пейдж вью модел)
            List<PageVM> pageList;
            //ініціалізую список з БД
            using (Db db = new Db())
            {
                pageList = db.Pages.ToArray().OrderBy(x => x.Sorting).Select(x => new PageVM(x)).ToList();
                /*pageList = new List<PageVM>() {};*///-заглушка
            }
                //повертаю список в вью
                return View(pageList);
        }

        public IActionResult AddPage()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddPage(PageVM model)
        {
            //перевірка моделі на валідність
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            using (Db db = new Db()) 
            {
                //оголошуємо змінну для короткого опису
                string slug;
                //ініціалізіція класу PageDTO
                PagesDTO dto = new PagesDTO();
                //присвоюю заголовок для моделі
                dto.Title = model.Title.ToUpper();
                //Перевірка наявності короткого опису товару, якшо немає додаємо
                if (string.IsNullOrWhiteSpace(model.Slug))
                {
                    slug = model.Title.Replace(" ", "-").ToLower();
                }
                else
                {
                    slug = model.Slug.Replace(" ", "-").ToLower();
                }
                //Впевнюємось шо заголовок унікальний
                if (db.Pages.Any(x => x.Title == model.Title))
                {
                    ModelState.AddModelError("", "That title already exist.");
                    return View(model);
                }
                else if (db.Pages.Any(x => x.Slug == model.Slug))
                {
                    ModelState.AddModelError("", "That slug already exist.");
                }
                //присвоємо інші значення нашій моделі
                dto.Slug = slug;
                dto.Description = model.Description;
                dto.HasSidebar = model.HasSidebar;
                dto.Sorting = 100;
                //зберігаємо модель в бд
                db.Pages.Add(dto);
                db.SaveChanges();
            }

            //передаємо повідомлення через TempData
            TempData["SM"] = "Added new page!";
            //переадресовуємо користувача на метод індекс
            return RedirectToAction("Index");
        }
    }
}
