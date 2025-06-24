using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Efood_Menu.Extensions;
using Efood_Menu.Models;
using Efood_Menu.Repositories;

namespace Efood_Menu.Controllers
{
	
	public class ShoppingCartController : Controller
	{
		private readonly IFoodItemRepository _foodItemRepository;
		private readonly ApplicationDbContext _context;
		private readonly UserManager<ApplicationUser> _userManager;

		public ShoppingCartController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IFoodItemRepository productRepository)
		{
			_foodItemRepository = productRepository;
			_context = context;
			_userManager = userManager;
		}

		public IActionResult Index()
		{
			var cart = HttpContext.Session.GetObjectFromJson<ShoppingCart>("Cart") ?? new ShoppingCart();
			return View(cart);
		}

		public IActionResult Checkout()
		{
			return View(new Order());
		}

		[HttpPost]
		public async Task<IActionResult> Checkout(Order order)
		{
			var cart = HttpContext.Session.GetObjectFromJson<ShoppingCart>("Cart");
			if (cart == null || !cart.Items.Any())
			{
				TempData["Error"] = "Giỏ hàng của bạn đang trống!";
				return RedirectToAction("Index");
			}

			var user = await _userManager.GetUserAsync(User);
			if (user == null)
			{
				return RedirectToAction("Login", "Account");
			}

			order.UserId = user.Id;
			order.OrderDate = DateTime.UtcNow;
			order.TotalAmount = cart.Items.Sum(i => i.Price * i.Quantity);
			order.DiscountApplied ??= 0;
			order.Status = "Pending";

			// Tạo danh sách OrderItem
			order.OrderItems = cart.Items.Select(i => new OrderItem
			{
				OrderId = i.ProductId,
				Quantity = i.Quantity,
				UnitPrice = i.Price,
			}).ToList();

			_context.Orders.Add(order);
			await _context.SaveChangesAsync();

			HttpContext.Session.Remove("Cart");

			return View("OrderCompleted", order.Id);
		}

		public async Task<IActionResult> AddToCart(int productId, int quantity)
		{
			var product = await GetProductFromDatabase(productId);
			if (product == null)
			{
				return NotFound();
			}

			var cartItem = new CartItem
			{
				ProductId = productId,
				Name = product.Name,
				Price = product.Price,
				Quantity = quantity
			};

			var cart = HttpContext.Session.GetObjectFromJson<ShoppingCart>("Cart") ?? new ShoppingCart();
			cart.AddItem(cartItem);
			HttpContext.Session.SetObjectAsJson("Cart", cart);

			return RedirectToAction("Index");
		}

		public IActionResult RemoveFromCart(int productId)
		{
			var cart = HttpContext.Session.GetObjectFromJson<ShoppingCart>("Cart");

			if (cart != null)
			{
				cart.RemoveItem(productId);
				HttpContext.Session.SetObjectAsJson("Cart", cart);
			}

			return RedirectToAction("Index");
		}

		private async Task<FoodItem> GetProductFromDatabase(int productId)
		{
			return await _foodItemRepository.GetByIdAsync(productId);
		}
	}
}
