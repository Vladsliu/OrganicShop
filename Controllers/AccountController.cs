using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using OrganicShop2.Data;
using OrganicShop2.Models.Data;
using OrganicShop2.Models.ViewModels.Account;
using System.Security.Claims;
using OrganicShop2.Models.ViewModels.Shop;
using Microsoft.AspNetCore.Authorization;

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

        [HttpGet]
        public ActionResult CreateAccount()
        
        
        {
            return View("CreateAccount");
        }

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
                return RedirectToAction("UserProfile");  
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

        [HttpGet]
        public IActionResult UserProfile()
        {
            var userName = User.Identity.Name;

            UserProfileVM model;

            UserDTO dto = _context.Users.FirstOrDefault(x => x.Username == userName);

            model = new UserProfileVM(dto);

            return View("UserProfile", model);
        }

        [HttpPost]
        public IActionResult UserProfile(UserProfileVM model)
        {
            bool userNameIsChanged = false;

            if (!ModelState.IsValid)
            {
                return View("Userprofile", model);
            }

            if (!string.IsNullOrWhiteSpace(model.Password))
            {
                if (!model.Password.Equals(model.ConfirmPassword))
                {
                    ModelState.AddModelError("", "Password do not match.");
                    return View("UserProfile", model);
                }
            }

            string userName = User.Identity.Name;

            if (userName != model.Username)
            {
                userName = model.Username;
                userNameIsChanged = true;
            }

            if (_context.Users.Where(x => x.Id != model.Id).Any(x => x.Username == userName)) 
            {
                ModelState.AddModelError("", $"UserName {model.Username} already exists");
                model.Username = "";
                return View("UserProfile", model);
            }

            UserDTO dto = _context.Users.Find(model.Id);

            dto.FirstName = model.FirstName;
            dto.LastName = model.LastName;
            dto.EmailAddress = model.EmailAddress;
            dto.Username = model.Username;

            if (!string.IsNullOrWhiteSpace(model.Password))
            {
                dto.Password = model.Password;
            }

            _context.SaveChanges();

            TempData["SM"] = "You have edited your profile";


            if (!userNameIsChanged)
                return View("UserProfile", model);

            else
                return RedirectToAction("Logout");
        }

        [HttpGet]
        public IActionResult Orders()
        {

            List<OrdersForUserVM> ordersForUser = new List<OrdersForUserVM>();

            var userName = User.Identity.Name;

            var userId = _context.Users.Where(x => x.Username == User.Identity.Name).Select(x => x.Id).FirstOrDefault();




            
            List<OrderVM> orders = _context.Orders.Where(x => x.UserId == userId).ToArray().Select(x => new OrderVM(x)).ToList();

            foreach (var order in orders)
            {
                Dictionary<string, int> productAndQty = new Dictionary<string, int>();

                decimal total = 0m;

                List<OrderDetailsDTO> orderDetailsList = _context.OrderDetails.Where(x => x.OrderId == order.OrderId).ToList();

                foreach (var orderDetails in orderDetailsList)
                {
                    ProductDTO product = _context.Products.FirstOrDefault(x => x.Id == orderDetails.ProductId);

                    decimal price = decimal.Parse(product.Price);

                    string productName = product.Name;

                    productAndQty.Add(productName, orderDetails.Quantity);

                    total += orderDetails.Quantity * price;
                }
                ordersForUser.Add(new OrdersForUserVM()
                {
                    OrderNumber = order.OrderId,
                    Total = total,
                    ProductsAndQty = productAndQty,
                    CreatedAt = order.CreatedAt
                }
                    );
            }

            return View(ordersForUser);
        }
    }
}





