using Microsoft.AspNetCore.Mvc;
using OrganicShop2.Models.ViewModels.Cart;

namespace OrganicShop2.Controllers
{
	public class CartController : Controller
	{
		private readonly ISession _session;

		public CartController(IHttpContextAccessor httpContextAccessor)
		{
			_session = httpContextAccessor.HttpContext.Session;
		}

		//del?
		public IActionResult Index()
		{
			return View();
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
