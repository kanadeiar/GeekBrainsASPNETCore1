using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using WebStore.Domain.Models;
using WebStore.Interfaces.Services;

namespace WebStore.Services.InCookies
{
    /// <summary> Хранение в куках списка сравниваемых товаров </summary>
    public class InCookiesCompareStore : ICompareStore
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly string _storageName;

        /// <summary> Сравниваемые товары </summary>
        public Compare Compare
        {
            get
            {
                var context = _contextAccessor.HttpContext;
                if (!context.Request.Cookies.ContainsKey(_storageName))
                {
                    var compare = new Compare();
                    context.Response.Cookies.Append(_storageName, JsonConvert.SerializeObject(compare));
                    return compare;
                }
                return JsonConvert.DeserializeObject<Compare>(context.Request.Cookies[_storageName]);
            }
            set => _contextAccessor.HttpContext!.Response.Cookies.Append(_storageName,
                JsonConvert.SerializeObject(value));
        }

        public InCookiesCompareStore(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
            _contextAccessor = contextAccessor;

            var user = _contextAccessor.HttpContext!.User;
            var userName = user.Identity!.IsAuthenticated ? $"-{user.Identity.Name}" : null;

            _storageName = $"KanadeiarWebStore.Compare{userName}";
        }
    }
}
