using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using WebStore.Domain.Entities;
using WebStore.Domain.Infrastructure.Filters;
using WebStore.Services.Interfaces;
using WebStore.WebModels;

namespace WebStore.Services
{
    /// <summary> Сервис корзины в куках браузера </summary>
    public class InCookiesCartService : ICartService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IProductData _productData;
        private readonly string _cartName;
        private readonly Mapper _mapperProductToView = new (new MapperConfiguration(c => c.CreateMap<Product, ProductWebModel>()
            .ForMember("Section", o => o.MapFrom(p => p.Section.Name))
            .ForMember("Brand", o => o.MapFrom(p => p.Brand.Name))));

        private Cart Cart
        {
            get
            {
                var context = _httpContextAccessor.HttpContext;
                if (!context!.Request.Cookies.ContainsKey(_cartName))
                {
                    var cart = new Cart();
                    context.Response.Cookies.Append(_cartName, JsonConvert.SerializeObject(cart));
                    return cart;
                }
                return JsonConvert.DeserializeObject<Cart>(context.Request.Cookies[_cartName]);
            }
            set => _httpContextAccessor.HttpContext!.Response.Cookies.Append(_cartName,
                JsonConvert.SerializeObject(value));
        }
        #region Этот вариант больше не используется

        //private Cart Cart
        //{
        //    get
        //    {
        //        var cookies = context!.Response.Cookies;
        //        var cartCookie = context.Request.Cookies[_cartName];
        //        if (cartCookie is null)
        //        {
        //            var cart = new Cart();
        //            cookies.Append(_cartName, JsonConvert.SerializeObject(cart));
        //            return cart;
        //        }
        //        ReplaceCookies(cookies, cartCookie);
        //        return JsonConvert.DeserializeObject<Cart>(cartCookie);
        //    }
        //    set => ReplaceCookies(_httpContextAccessor.HttpContext!.Response.Cookies, JsonConvert.SerializeObject(value));
        //}

        //private int _cookieHash;
        //private void ReplaceCookies(IResponseCookies cookies, string cookie)
        //{
        //    if (_cookieHash == cookie.GetHashCode())
        //        return;
        //    _cookieHash = cookie.GetHashCode();
        //    cookies.Delete(_cartName);
        //    cookies.Append(_cartName, cookie);
        //}

        #endregion

        public InCookiesCartService(IHttpContextAccessor contextAccessor, IProductData productData)
        {
            _httpContextAccessor = contextAccessor;
            _productData = productData;

            var user = _httpContextAccessor.HttpContext!.User;
            var userName = user.Identity!.IsAuthenticated ? $"-{user.Identity.Name}" : null;

            _cartName = $"KanadeiarWebStore.Cart{userName}";
        }
        public void Add(int id)
        {
            var cart = Cart;

            var item = cart.Items.FirstOrDefault(i => i.ProductId == id);
            if (item is { })
                item.Quantity++;
            else
                cart.Items.Add(new CartItem{ ProductId = id });

            Cart = cart;
        }

        public void Subtract(int id)
        {
            var cart = Cart;

            var item = cart.Items.FirstOrDefault(c => c.ProductId == id);
            if (item is null) return;

            if (item.Quantity > 0)
                item.Quantity--;

            if (item.Quantity <= 0)
                cart.Items.Remove(item);

            Cart = cart;
        }

        public void Remove(int id)
        {
            var cart = Cart;

            var item = cart.Items.FirstOrDefault(c => c.ProductId == id);
            if (item is null) return;

            cart.Items.Remove(item);

            Cart = cart;
        }

        public void Clear()
        {
            var cart = Cart;

            cart.Items.Clear();

            Cart = cart;
        }

        public CartWebModel GetWebModel()
        {
            var products = _productData.GetProducts(new ProductFilter
            {
                Ids = Cart.Items.Select(i => i.ProductId).ToArray()
            });
            var productViews = _mapperProductToView
                .Map<IEnumerable<ProductWebModel>>(products).ToDictionary(p => p.Id);

            return new CartWebModel
            {
                Items = Cart.Items
                    .Where(p => productViews.ContainsKey(p.ProductId))
                    .Select(p => (productViews[p.ProductId], p.Quantity, productViews[p.ProductId].Price * p.Quantity))
            };
        }
    }
}
