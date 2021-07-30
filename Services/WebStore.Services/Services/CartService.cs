using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Logging;
using WebStore.Domain.Entities;
using WebStore.Domain.Models;
using WebStore.Domain.WebModels;
using WebStore.Domain.WebModels.Cart;
using WebStore.Interfaces.Services;

namespace WebStore.Services.Services
{
    public class CartService : ICartService
    {
        private readonly ICartStore _cartStore;
        private readonly IProductData _productData;
        private readonly ILogger<CartService> _logger;
        private readonly Mapper _mapperProductToView = new (new MapperConfiguration(c => c.CreateMap<Product, ProductWebModel>()
            .ForMember("Section", o => o.MapFrom(p => p.Section.Name))
            .ForMember("Brand", o => o.MapFrom(p => p.Brand.Name))));

        public CartService(ICartStore cartStore, IProductData productData, ILogger<CartService> logger)
        {
            _cartStore = cartStore;
            _productData = productData;
            _logger = logger;
        }

        public void Add(int id)
        {
            var cart = _cartStore.Cart;

            var item = cart.Items.FirstOrDefault(i => i.ProductId == id);
            if (item is { })
                item.Quantity++;
            else
                cart.Items.Add(new CartItem{ ProductId = id });
            #region Лог
            _logger.LogInformation($"Успешно добавлен в корзину товар c идентификатором {id}");
            #endregion
            _cartStore.Cart = cart;
        }

        public void Subtract(int id)
        {
            var cart = _cartStore.Cart;

            var item = cart.Items.FirstOrDefault(c => c.ProductId == id);
            if (item is null) return;

            if (item.Quantity > 0)
                item.Quantity--;

            if (item.Quantity <= 0)
                cart.Items.Remove(item);
            #region Лог
            _logger.LogInformation($"Товар с идентификатором {id} успешно убавлен на еденицу в корзине");
            #endregion
            _cartStore.Cart = cart;
        }

        public void Remove(int id)
        {
            var cart = _cartStore.Cart;

            var item = cart.Items.FirstOrDefault(c => c.ProductId == id);
            if (item is null) return;

            cart.Items.Remove(item);
            #region Лог
            _logger.LogInformation($"Товар с идентификатором {id} успешно удален из корзины");
            #endregion
            _cartStore.Cart = cart;
        }

        public void Clear()
        {
            var cart = _cartStore.Cart;

            cart.Items.Clear();
            #region Лог
            _logger.LogInformation("Корзина успешно очищена от товаров");
            #endregion
            _cartStore.Cart = cart;
        }

        public async Task<CartWebModel> GetWebModel()
        {
            var products = (await _productData.GetProducts(new ProductFilter
            {
                Ids = _cartStore.Cart.Items.Select(i => i.ProductId).ToArray()
            }))?.Products;
            var productViews = _mapperProductToView
                .Map<IEnumerable<ProductWebModel>>(products).ToDictionary(p => p.Id);

            return new CartWebModel
            {
                Items = _cartStore.Cart.Items
                    .Where(p => productViews.ContainsKey(p.ProductId))
                    .Select(p => (productViews[p.ProductId], p.Quantity ))
            };
        }
    }
}
