using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using OrganicShop2.Data;
using OrganicShop2.Models.Data;
using OrganicShop2.Models.ViewModels.Account;
using System.Security.Claims;

namespace OrganicShop2.Controllers
{
    public class AccountController : Controller
    {
        private readonly Db _context;

        public AccountController(Db context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return RedirectToAction("Login");
        }

        //[ActionName("create-account")]
        [HttpGet]
        public ActionResult CreateAccount()
        
        
        {
            return View("CreateAccount");
        }

        //[ActionName("create-account")]
        [HttpPost]
        public IActionResult CreateAccount(UserVM model)
        {
            if (!ModelState.IsValid)
                return View("CreateAccount", model);

            if (!model.Password.Equals(model.ConfirmPassword))
            {
                ModelState.AddModelError("", "Password do not match");
                return View("CreateAccount", model);
            }

            if (_context.Users.Any(x => x.Username.Equals(model.Username)))
            {
                ModelState.AddModelError("", $"Username {model.Username} already exist.");
                model.Username = "";
                return View("CreateAccount", model);
            }

            UserDTO userDTO = new UserDTO()
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Username = model.Username,
                Password = model.Password,
                EmailAddress = model.EmailAddress,
            };

            _context.Users.Add(userDTO);
            _context.SaveChanges();

            int id = userDTO.Id;

            UserRoleDTO userRoleDTO = new UserRoleDTO()
            {
                UserId = id,
                RoleId = 1,
            };

            _context.UserRoles.Add(userRoleDTO);
            _context.SaveChanges();

            TempData["SM"] = "You are now registered and can login";

            return RedirectToAction("Login");
        }

        public IActionResult Login()
        {
            string userName = User.Identity.Name;
            if (!string.IsNullOrEmpty(userName))
            {
                return RedirectToAction("user-profile");  
            }

            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginUserVM model)
        {
            if (!ModelState.IsValid) 
                return View(model);

            bool isValid = false;

            if (_context.Users.Any(x => x.Username.Equals(model.Username) && x.Password.Equals(model.Password)))
                isValid = true;

            if (!isValid) 
            {
                ModelState.AddModelError("", "Invalid username or password");
                return View(model);
            }
            else
            {

                var claims = new List<Claim>
                {
                 new Claim(ClaimTypes.Name, model.Username)
          
                };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = model.RememberMe
                 
                };

                 HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    authProperties
                );

                return RedirectToAction("Index", "Home");
            }
        }

        public IActionResult Logout()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("Login");
        }

        public IActionResult UserNavPartial()
        {
            var userName = User.Identity.Name;

            UserNavPartialVM model;

            UserDTO dto = _context.Users.FirstOrDefault(u => u.FirstName == userName);

            model = new UserNavPartialVM()
            { 
            FirstName = dto.FirstName,
            LastName = dto.LastName
            };
           
            return PartialView(model);
        }
    }
}
