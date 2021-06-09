using Microsoft.EntityFrameworkCore;
using WebStore.Domain.Entities;

namespace WebStore.Dal.Context
{
    public class WebStoreContext : DbContext
    {
        public DbSet<Section> Sections { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<Product> Products { get; set; }
        public WebStoreContext(DbContextOptions<WebStoreContext> options) : base(options)
        {
        }
    }
}
