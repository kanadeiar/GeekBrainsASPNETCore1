using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using WebStore.Domain.Entities;
using WebStore.Interfaces.Services;

namespace WebStore.Services.Services
{
    public class InCookiesCartStore : ICartStore
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly string _cartName;

        public Cart Cart
        {
            get
            {
                var context = _contextAccessor.HttpContext;
                if (!context!.Request.Cookies.ContainsKey(_cartName))
                {
                    var cart = new Cart();
                    context.Response.Cookies.Append(_cartName, JsonConvert.SerializeObject(cart));
                    return cart;
                }
                return JsonConvert.DeserializeObject<Cart>(context.Request.Cookies[_cartName]);
            }
            set => _contextAccessor.HttpContext!.Response.Cookies.Append(_cartName,
                JsonConvert.SerializeObject(value));
        }

        public InCookiesCartStore(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;

            var user = _contextAccessor.HttpContext!.User;
            var userName = user.Identity!.IsAuthenticated ? $"-{user.Identity.Name}" : null;

            _cartName = $"KanadeiarWebStore.Cart{userName}";
        }
    }
}
