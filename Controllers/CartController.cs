using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using OrganicShop2.Models.ViewModels.Cart;
using OrganicShop2.Data;
using OrganicShop2.Models.Data;
using Microsoft.AspNetCore.Http;
using System.Net.Mail;
using System.Net;

namespace OrganicShop2.Controllers
{
	public class CartController : Controller
	{
		private readonly IHttpContextAccessor _session;
        private readonly Db _context;

        public CartController(IHttpContextAccessor httpContextAccessor, Db context)
		{
			_session = httpContextAccessor;
            _context = context;
        }

		public IActionResult Index()
		{
			var json = HttpContext.Session.GetString("cart");
           

            var cart = new List<CartVM>();
            if (!string.IsNullOrEmpty(json))
            {
                cart = JsonConvert.DeserializeObject<List<CartVM>>(json);
            }

            if (cart.Count == 0 || cart == null)
			{
				ViewBag.Message = "Your cart is empty.";
				return View();
			}

			decimal total = 0m;

			foreach (var item in cart)
			{
				total += item.Total;
			}

			ViewBag.GrandTotal = total;

            return View(cart);
		}

		public IActionResult CartPartial()
		{
			CartVM model = new CartVM();

			int qty = 0;

			decimal price = 0m;

			if (HttpContext.Session.TryGetValue("cart", out byte[] cartData) && cartData != null)
			{
				var list = System.Text.Json.JsonSerializer.Deserialize<List<CartVM>>(cartData);

				foreach (var item in list)
				{
					qty += item.Quantity;
					price += item.Quantity * item.Price;

				}

				model.Quantity = qty;
				model.Price = price;
            }
			else
			{
				model.Quantity = 0;
				model.Price = 0m;
			}

            return PartialView("_CartPartial", model);
		}
		public IActionResult AddToCartPartial(int id)
		{
			var json = HttpContext.Session.GetString("cart");


			var cart = new List<CartVM>();
			if (!string.IsNullOrEmpty(json))
			{
				cart = JsonConvert.DeserializeObject<List<CartVM>>(json);
			}

			CartVM model = new CartVM();

			ProductDTO product = _context.Products.Find(id);

			var productInCart = cart.FirstOrDefault(x => x.ProductId == id);

			if (productInCart == null)
			{
				cart.Add(new CartVM()
				{
					ProductId = product.Id,
					ProductName = product.Name,
					Quantity = 1,
					Price = decimal.Parse(product.Price),
					Image = product.Image
				});
			}
			else
			{
				productInCart.Quantity++;
			}

			int qty = 0;
			decimal price = 0m;

			foreach (var item in cart)
			{
				qty += item.Quantity;
				price += item.Quantity * item.Price ;
			}

			model.Quantity = qty;
			model.Price = price;

			HttpContext.Session.SetString("cart", JsonConvert.SerializeObject(cart));

			return PartialView("_AddToCartPartial", model);
		}

		public JsonResult IncrementProduct(int productId)
		{
			var json = HttpContext.Session.GetString("cart");

			var cart = new List<CartVM>();
			if (!string.IsNullOrEmpty(json))
			{
				cart = JsonConvert.DeserializeObject<List<CartVM>>(json);
			}

			CartVM model = cart.FirstOrDefault(x => x.ProductId == productId);

			model.Quantity++;

			var result = new { qty = model.Quantity, price = model.Price };

            HttpContext.Session.SetString("cart", JsonConvert.SerializeObject(cart));

            return new JsonResult(result);
		}

		public IActionResult DecrementProduct(int productId) 
		{
            var json = HttpContext.Session.GetString("cart");

            var cart = new List<CartVM>();
            if (!string.IsNullOrEmpty(json))
            {
                cart = JsonConvert.DeserializeObject<List<CartVM>>(json);
            }

            CartVM model = cart.FirstOrDefault(x => x.ProductId == productId);

			if (model.Quantity > 1)
			{
				model.Quantity--;
			}
			else
			{
				model.Quantity = 0;
				cart.Remove(model);
			}

            var result = new { qty = model.Quantity, price = model.Price };

            HttpContext.Session.SetString("cart", JsonConvert.SerializeObject(cart));

            return new JsonResult(result);
        }

		public void RemoveProduct(int productId) 
		{
            var json = HttpContext.Session.GetString("cart");

            var cart = new List<CartVM>();
            if (!string.IsNullOrEmpty(json))
            {
                cart = JsonConvert.DeserializeObject<List<CartVM>>(json);
            }

			CartVM model = cart.FirstOrDefault(x => x.ProductId == productId);

			cart.Remove(model);
            HttpContext.Session.SetString("cart", JsonConvert.SerializeObject(cart));
        }

		public IActionResult PaypalPartial()
		{
            //List<CartVM> cart = Session["cart"] as List<CartVM>;

            var cartJson = HttpContext.Session.GetString("cart");
            List<CartVM> cart = JsonConvert.DeserializeObject<List<CartVM>>(cartJson);


            return PartialView(cart);
		}

		[HttpPost]
		public void PlaceOrder()
		{
            var cartJson = HttpContext.Session.GetString("cart");
            List<CartVM> cart = JsonConvert.DeserializeObject<List<CartVM>>(cartJson);

			string userName = User.Identity.Name;

			int orderId = 0;

			OrderDTO orderDTO = new OrderDTO();

			var q = _context.Users.FirstOrDefault(x => x.Username == userName);
			int userId = q.Id;

			orderDTO.UserId = userId;
			orderDTO.CreateAt = DateTime.Now;
			

			_context.Add(orderDTO);
			_context.SaveChanges();

			orderId = orderDTO.OrderId;


			foreach (var item in cart)
			{ 
			OrderDetailsDTO orderDetailsDTO = new OrderDetailsDTO();

				orderDetailsDTO.OrderId = orderId;
				orderDetailsDTO.UserId = userId;
				orderDetailsDTO.ProductId = item.ProductId;
				orderDetailsDTO.Quantity = item.Quantity;

				_context.OrderDetails.Add(orderDetailsDTO);
				_context.SaveChanges();
			}

            var client = new SmtpClient("sandbox.smtp.mailtrap.io", 25)
            {
                Credentials = new NetworkCredential("35cec9d75ea6cf", "17a307a80844de"),
                EnableSsl = true
            };
			client.Send("shop@example.com", "admin@example.com", "New Order", $"You have a new order. Order number: {orderId}");

			var a = cart;

            HttpContext.Session.Remove("cart");
        }
    }
}
