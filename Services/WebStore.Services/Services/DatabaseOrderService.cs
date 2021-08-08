using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using WebStore.Dal.Context;
using WebStore.Domain.Entities.Orders;
using WebStore.Domain.Identity;
using WebStore.Domain.WebModels.Cart;
using WebStore.Interfaces.Services;

namespace WebStore.Services.Services
{
    /// <summary> Сервис заказов в базе данных </summary>
    public class DatabaseOrderService : IOrderService
    {
        private readonly WebStoreContext _context;
        private readonly UserManager<User> _userManager;
        private readonly ILogger<DatabaseOrderService> _logger;
        public DatabaseOrderService(WebStoreContext context, UserManager<User> userManager, ILogger<DatabaseOrderService> logger)
        {
            _context = context;
            _userManager = userManager;
            _logger = logger;
        }
        public async Task<IEnumerable<Order>> GetUserOrders(string userName)
        {
            return await _context.Orders
                .Include(o => o.User)
                .Include(o => o.Items)
                .ThenInclude(i => i.Product)
                .Where(o => o.User.UserName == userName).ToArrayAsync();
        }
        public async Task<IEnumerable<Order>> GetAllOrders()
        {
            return await _context.Orders
                .Include(o => o.User)
                .Include(o => o.Items)
                .ThenInclude(i => i.Product).ToArrayAsync();
        }
        public async Task<Order> GetOrderById(int id)
        {
            return await _context.Orders
                .Include(o => o.User)
                .Include(o => o.Items)
                .ThenInclude(i => i.Product)
                .FirstOrDefaultAsync(o => o.Id == id);
        }
        public async Task<Order> CreateOrder(string userName, CartWebModel cart, CreateOrderWebModel model)
        {
            var user = await _userManager.FindByNameAsync(userName);
            if (user is null)
            {
                #region Лог
                _logger.LogError("Пользователь {0} отсутствует в базе данных", userName);
                #endregion
                throw new InvalidOperationException(nameof(user) + " этот пользователь отсутствует в базе данных");
            }

            #region Лог
            _logger.LogInformation($" Начато формирование заказа");
            #endregion
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
            order.Items = orderItems;
            
            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();

            await transaction.CommitAsync();

            #region Лог
            _logger.LogInformation($"Заказ успешно сформирован и добавлен в базу данных");
            #endregion

            return order;
        }
    }
}
