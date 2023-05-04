using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using OrganicShop2.Models.ViewModels.Cart;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using OrganicShop2.Models.ViewModels.Cart;
using System.Collections.Generic;

namespace OrganicShop2.Controllers
{
	public class CartController : Controller
	{
		private readonly ISession _session;

		public CartController(IHttpContextAccessor httpContextAccessor)
		{
			_session = httpContextAccessor.HttpContext.Session;
		}

		public IActionResult Index()
		{
			var json = HttpContext.Session.GetString("cart");
			var cart = JsonConvert.DeserializeObject<List<CartVM>>(json) ?? new List<CartVM>();

			if (cart.Count == 0)//????
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

			decimal price =  0m;

			if (HttpContext.Session.TryGetValue("cart", out byte[] cartData) && cartData != null)
			{
				var list = System.Text.Json.JsonSerializer.Deserialize<List<CartVM>>(cartData);

				foreach (var item in list)
				{
					qty += item.Quantity;
					price += item.Quantity * item.Price;
				}
			}
			else 
			{
				model.Quantity = 0;
				model.Price = 0m;
			}
		return PartialView("_CartPartial", model);
		}
	}
}
