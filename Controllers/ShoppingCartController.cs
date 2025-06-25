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
        [HttpGet]
        public IActionResult Checkout()
        {
            var tables = _context.Tables.ToList();
            ViewBag.Tables = tables;
            return View(new Order());
        }

        [HttpPost]
		public async Task<IActionResult> Checkout(Order order, int? NumberOfGuests, DateTime? ReservationDateTime, string? TableNumber)
		{
            var cart = HttpContext.Session.GetObjectFromJson<ShoppingCart>("Cart");
            if (cart == null || !cart.Items.Any())
            {
                TempData["Error"] = "Giỏ hàng của bạn đang trống!";
                return RedirectToAction("Index");
            }

            var user = await _userManager.GetUserAsync(User);
            

            order.UserId = user?.Id;
            order.OrderDate = DateTime.UtcNow;
            order.TotalAmount = cart.Items.Sum(i => i.Price * i.Quantity);
            order.Status = "Pending";

            order.OrderItems = cart.Items.Select(i => new OrderItem
            {
                FoodItemId = i.ProductId,
                Quantity = i.Quantity,
                UnitPrice = i.Price
            }).ToList();

            // Nếu là ăn tại chỗ thì tạo Reservation
            if (order.OrderType == OrderType.DineIn)
            {
                if (NumberOfGuests == null || ReservationDateTime == null || string.IsNullOrEmpty(TableNumber))
                {
                    ModelState.AddModelError("", "Vui lòng nhập đầy đủ thông tin đặt bàn.");
                    var tables = _context.Tables.ToList();
                    ViewBag.Tables = tables;
                    return View(order);
                }

                // Tìm TableId từ TableNumber
                var table = _context.Tables.FirstOrDefault(t => t.TableNumber == TableNumber);
                if (table == null)
                {
                    ModelState.AddModelError("", "Bàn không hợp lệ.");
                    var tables = _context.Tables.ToList();
                    ViewBag.Tables = tables;
                    return View(order);
                }

                var reservation = new Reservation
                {
                    FullName = order.User?.FullName ?? "",
                    PhoneNumber = order.User?.PhoneNumber ?? "",
                    ReservationDateTime = ReservationDateTime.Value,
                    NumberOfGuests = NumberOfGuests.Value,
                    TableId = table.Id,
                    Note = order.Notes,
                    UserId = user?.Id
                };

                _context.Reservations.Add(reservation);
                // Có thể cập nhật trạng thái bàn nếu cần
                table.IsAvailable = false;
            }

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
				Quantity = quantity,
				Image = product.ImageUrl ?? "default-image-url.jpg" // Thay thế bằng URL ảnh mặc định nếu không có ảnh
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
