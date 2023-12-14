using FPT_BOOKMVC.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;


namespace FPT_BOOKMVC
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
          : base(options)
        { }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Book> Books { get; set; }

        public DbSet<PublishCompany> PublicCompanies { get; set; }

        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }


        protected override void OnModelCreating(ModelBuilder modebuilder)
        {
            base.OnModelCreating(modebuilder);
            modebuilder.Entity<Category>().HasData(
                new Category { CategoryId = 1, Name = "Romansss", Description = "A lot of roman stories", IsApproved = true },
                new Category { CategoryId = 2, Name = "Action", Description = "Show you how is an action", IsApproved = true }
                );

            modebuilder.Entity<Book>().HasData(
                new Book
                {
                    BookId = 1,
                    Name = "Title",
                    Quantity = 1,
                    Price = 1,
                    Description = "Title",
                    UpdateDate = DateTime.Now,
                    Author = "ltn",
                    Image = "123",
                    CategoryId = 1,
                    PublishCompanyId = 1
                }
                );
            modebuilder.Entity<OrderDetail>().HasData(
                new OrderDetail
                {
                    OrderDetailId = 1,
                    Quantity = 2,
                    BookId = 1,
                    OrderId = 2
                    
                }
                ); 


            modebuilder.Entity<Book>()
        .   Property(b => b.Price)
            .HasColumnType("decimal(18, 2)");

            modebuilder.Entity<Cart>()
       .    Property(c => c.Total)
       .    HasColumnType("decimal(18, 2)");

            modebuilder.Entity<OrderDetail>()
        .   Property(od => od.Total)
        .   HasColumnType("decimal(18, 2)");
        }
    }
    
}
