using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebStore.Dal.Context;
using WebStore.Domain.Entities.Orders;
using WebStore.Domain.Identity;
using WebStore.Services.Interfaces;
using WebStore.WebModels;

namespace WebStore.Services
{
    /// <summary> Сервис заказов в базе данных </summary>
    public class DatabaseOrderService : IOrderService
    {
        private readonly WebStoreContext _context;
        private readonly UserManager<User> _userManager;
        public DatabaseOrderService(WebStoreContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public async Task<IEnumerable<Order>> GetUserOrders(string userName)
        {
            return await _context.Orders
                .Include(o => o.User)
                .Include(o => o.Items)
                .Where(o => o.User.UserName == userName).ToArrayAsync();
        }
        public async Task<Order> GetOrderById(int id)
        {
            return await _context.Orders
                .Include(o => o.User)
                .Include(o => o.Items)
                .FirstOrDefaultAsync(o => o.Id == id);
        }
        public async Task<Order> CreateOrder(string userName, CartWebModel cart, CreateOrderViewModel model)
        {
            var user = await _userManager.FindByNameAsync(userName);
            if (user is null)
                throw new InvalidOperationException(nameof(user) + " этот пользователь отсутствует в базе данных");

            await using var transaction = await _context.Database.BeginTransactionAsync();

            var order = new Order
            {
                Name = model.Name,
                User = user,
                Phone = model.Phone,
                Address = model.Address,
            };
            var productsIds = cart.Items
                .Select(i => i.Product.Id).ToArray();
            var cartProducts = await _context.Products
                .Where(p => productsIds.Contains(p.Id)).ToArrayAsync();
            var orderItems = cart.Items
                .Join(cartProducts, i => i.Product.Id, p => p.Id, (i, p) => new OrderItem
                {
                    Order = order,
                    Product = p,
                    Price = p.Price,
                    Quantity = i.Quantity
                }).ToArray();
            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();

            await transaction.CommitAsync();

            return order;
        }
    }
}
