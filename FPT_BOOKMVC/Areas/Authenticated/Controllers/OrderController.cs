using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using FPT_BOOKMVC.Utils;
using FPT_BOOKMVC.Data;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using FPT_BOOKMVC.Models;
using Microsoft.AspNetCore.Hosting;
using System;
using FPT_BOOKMVC.Data.Migrations;

namespace FPT_BOOKMVC.Controllers
{
	[Area(SD.AuthenticatedArea)]
	[Authorize(Roles = SD.StoreOwnerRole + "," + SD.CustomerRole)]
	public class OrderController : Controller
    {
        private readonly ApplicationDbContext context;
        private readonly IWebHostEnvironment webHostEnvironment;

        public OrderController(ApplicationDbContext context, IWebHostEnvironment webHost)
        {
            this.context = context;
            webHostEnvironment = webHost;
        }

        [HttpGet]
        public async Task<IActionResult> OrderIndex()
        {
            //tabble join
            var order_customer_detail = await context.OrderDetails.Include(_ => _.Book).Include(_ => _.Order).ThenInclude(_ => _.User).ToListAsync();
            return View(order_customer_detail);
        }
		[HttpGet]
		public async Task<IActionResult> Historyorder()
		{
			var order_customer_detail = await context.OrderDetails.Include(_ => _.Book).Include(_ => _.Order).ThenInclude(_ => _.User).ToListAsync();
			return View(order_customer_detail);
		}

		public async Task<IActionResult> OrderBook()
        {

            var identity = (ClaimsIdentity)User.Identity;
            var claims = identity.FindFirst(ClaimTypes.NameIdentifier);

            var user = await context.ApplicationUsers.FindAsync(claims.Value);

            var customer = new Customer()
            {
                UserId = user.Id,
                Name  = user.FullName,
                Email = user.Email,
                Address = user.Address,
                User = user
            };

            if (!context.Customers.Contains(customer))
            {
                await context.Customers.AddAsync(customer);
                await context.SaveChangesAsync();
            }

            var order = new Order()
            {
                UserId = user.Id
            };

            await context.Orders.AddAsync(order);
            await context.SaveChangesAsync();


            var order_list = await context.Carts.Include(x => x.Book).ToListAsync();

            foreach (var book in order_list)
            {
                var book_object = await context.Carts.Include(x => x.Book).FirstOrDefaultAsync(x => x.BookId == book.BookId);
                var orderDetail = new OrderDetail()
                {
                    Quantity = book.Quantity,
                    BookId = book.BookId,
                    OrderId = order.OrderId,
                    Book = book_object.Book,
                    Total = book.Total
                };

                await context.AddAsync(orderDetail);
                await context.SaveChangesAsync();
            }
            foreach (var book in order_list)
            {
                context.Carts.Remove(book);
                await context.SaveChangesAsync();
            }

            foreach (var order_book in order_list)
            {
                var stored_book = await context.Books.FirstOrDefaultAsync(x => x.BookId == order_book.BookId);
                int book_quantity_inStored = stored_book.Quantity;
                book_quantity_inStored -= order_book.Quantity;
                stored_book.Quantity = book_quantity_inStored;
                if (stored_book.Quantity == 0)
                {
                    context.Books.Remove(stored_book);
                }
                await context.SaveChangesAsync();
            }
            await context.SaveChangesAsync();
            return RedirectToAction("BookProduct", "Book");
        }

        public async Task<IActionResult> DeleteAllOrders()
        {
            var order_customer_detail = await context.OrderDetails.Include(_ => _.Book).Include(_ => _.Order).ThenInclude(_ => _.User).ToListAsync();

            if (order_customer_detail == null)
            {
                return NotFound();
            }
            foreach (var orderDetail in order_customer_detail)
            {
                context.OrderDetails.Remove(orderDetail);
            }
            await context.SaveChangesAsync();

            return RedirectToAction(nameof(OrderIndex));
        }
    }
}
