using FPT_BOOKMVC.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

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
    }
    
}
