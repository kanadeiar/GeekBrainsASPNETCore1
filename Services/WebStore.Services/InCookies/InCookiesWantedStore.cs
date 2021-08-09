using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using WebStore.Domain.Models;
using WebStore.Interfaces.Services;

namespace WebStore.Services.InCookies
{
    /// <summary> Хранение в куках списка желаемых товаров </summary>
    public class InCookiesWantedStore : IWantedStore
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly string _storageName;
        /// <summary> Желаемые товары </summary>
        public Wanted Wanted
        {
            get
            {
                var context = _contextAccessor.HttpContext;
                if (!context.Request.Cookies.ContainsKey(_storageName))
                {
                    var wanted = new Wanted();
                    context.Response.Cookies.Append(_storageName, JsonConvert.SerializeObject(wanted));
                    return wanted;
                }
                return JsonConvert.DeserializeObject<Wanted>(context.Request.Cookies[_storageName]);
            }
            set => _contextAccessor.HttpContext!.Response.Cookies.Append(_storageName,
                JsonConvert.SerializeObject(value));
        }

        public InCookiesWantedStore(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;

            var user = _contextAccessor.HttpContext!.User;
            var userName = user.Identity!.IsAuthenticated ? $"-{user.Identity.Name}" : null;

            _storageName = $"KanadeiarWebStore.Wanted{userName}";
        }
    }
}
