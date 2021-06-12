using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebStore.Domain.Entities;
using WebStore.Domain.Identity;

namespace WebStore.Dal.Context
{
    public class WebStoreContext : IdentityDbContext<User, Role, string>
    {
        public DbSet<Section> Sections { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<Product> Products { get; set; }
        public WebStoreContext(DbContextOptions<WebStoreContext> options) : base(options)
        {
        }
    }
}
