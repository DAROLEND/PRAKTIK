using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;

namespace CoffeeFoodOrder.Models
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext() : base("DefaultConnection") { }

        public static ApplicationDbContext Create() => new ApplicationDbContext();

        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
    }
}
