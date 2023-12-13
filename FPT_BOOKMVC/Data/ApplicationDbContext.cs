using FPT_BOOKMVC.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FPT_BOOKMVC.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
<<<<<<< Updated upstream
            : base(options)
        {
        }
        public DbSet<Category> Categories { get; set; }
=======
          : base(options)
        { }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<Customer> Customers { get; set; }
        
        public DbSet<PublishCompany> PublicCompanies { get; set; }
>>>>>>> Stashed changes

        public DbSet<Book> Books { get; set; }
        public DbSet<PublishCompany> PublishCompanies { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }

    }
}
