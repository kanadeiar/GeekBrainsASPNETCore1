using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebStore.Domain.Entities;
using WebStore.Domain.Entities.Orders;
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
        /// <summary> Заказы </summary>
        public DbSet<Order> Orders { get; set; }
        /// <summary> Элементы заказов </summary>
        public DbSet<OrderItems> OrderItems { get; set; }

        public WebStoreContext(DbContextOptions<WebStoreContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Order>()
                .HasMany(o => o.Items)
                .WithOne(i => i.Order)
                .OnDelete(DeleteBehavior.Cascade);
            builder.Entity<User>()
                .HasMany(u => u.Orders)
                .WithOne(o => o.User)
                .OnDelete(DeleteBehavior.Cascade);

        }
    }
}
