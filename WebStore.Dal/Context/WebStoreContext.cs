using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebStore.Domain.Entities;
using WebStore.Domain.Identity;

namespace WebStore.Dal.Context
{
    /// <summary> Хранилище данных в базе данных </summary>
    public class WebStoreContext : IdentityDbContext<User, Role, string>
    {
        /// <summary> Категории </summary>
        public DbSet<Section> Sections { get; set; }
        /// <summary> Бренды </summary>
        public DbSet<Brand> Brands { get; set; }
        /// <summary> Товары </summary>
        public DbSet<Product> Products { get; set; }
        /// <summary> Работники </summary>
        public DbSet<Worker> Workers { get; set; }
        public WebStoreContext(DbContextOptions<WebStoreContext> options) : base(options)
        {
        }
    }
}
