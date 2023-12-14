using FPT_BOOKMVC.Data;
using FPT_BOOKMVC.Models;
using FPT_BOOKMVC.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FPT_BOOKMVC.Areas.Authenticated.Controllers
{
	[Area(SD.AuthenticatedArea)]
	[Authorize(Roles = SD.StoreOwnerRole + "," + SD.CustomerRole)]
	public class CartController : Controller
	{
		private readonly ApplicationDbContext context;
		private readonly IWebHostEnvironment webHostEnvironment;

		public CartController(ApplicationDbContext context, IWebHostEnvironment webHost)
		{
			this.context = context;
			webHostEnvironment = webHost;
		}

		[HttpGet]
		public async Task<IActionResult> CartIndex()
		{
			var cartitem = await context.Carts.Include(_ => _.Book).ToListAsync();
			return View(cartitem);
		}
		public async Task<IActionResult> Plus(int cartId)
		{
			var cart = await context.Carts.Include(p => p.Book).FirstOrDefaultAsync(c => c.CartId == cartId);
			var book = context.Books.FirstOrDefault(x => x.BookId == cart.BookId);

			if (cart == null)
			{
				return NotFound();
			}

			if (cart.Quantity < book.Quantity)
			{
				cart.Quantity += 1;
			}

			cart.Total = cart.Quantity * cart.Book.Price;
			await context.SaveChangesAsync();
			return RedirectToAction(nameof(CartIndex));

		}
		public async Task<IActionResult> Minus(int cartId)
		{
			var cart = await context.Carts.Include(p => p.Book).FirstOrDefaultAsync(c => c.CartId == cartId);

			if (cart.Quantity == 1)
			{
				var cnt = context.Carts.ToList().Count;
				context.Carts.Remove(cart);
				await context.SaveChangesAsync();
			}
			else
			{
				cart.Quantity -= 1;
				cart.Total = cart.Quantity * cart.Book.Price;
				await context.SaveChangesAsync();
			}

			return RedirectToAction(nameof(CartIndex));
		}

		public async Task<IActionResult> AddToCart(int id)
		{
			var book = await context.Books.FirstOrDefaultAsync(x => x.BookId == id);
			if (book != null)
			{
				var cart_item = new Cart()
				{
					BookId = book.BookId,
					Book = book,
					Quantity = 1
				};

				cart_item.Total = cart_item.Quantity * cart_item.Book.Price;

				foreach (var item in context.Carts.Include(_ => _.Book).ToList())
				{
					if (item.BookId == cart_item.BookId)
					{
						return RedirectToAction("CartIndex");
					}
				}

				await context.Carts.AddAsync(cart_item);
				await context.SaveChangesAsync();

				Thread.Sleep(2500);
				return RedirectToAction("BookProduct", "Book");

			}

			return RedirectToAction("CartIndex");
		}

		public async Task<IActionResult> DeleteCartItem(int id)
		{
			var cart_item = await context.Carts.FirstOrDefaultAsync(_ => _.BookId == id);

			if (cart_item != null)
			{
				context.Carts.Remove(cart_item);


				await context.SaveChangesAsync();

				return RedirectToAction("CartIndex");
			}

			return RedirectToAction("CartIndex");
		}
	}
}
