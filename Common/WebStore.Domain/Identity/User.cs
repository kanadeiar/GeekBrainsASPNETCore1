using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using WebStore.Domain.Entities.Orders;

namespace WebStore.Domain.Identity
{
    /// <summary> Пользователь </summary>
    public class User : IdentityUser
    {
        /// <summary> Заказы </summary>
        public ICollection<Order> Orders { get; set; } = new List<Order>();
        /// <summary> Название администратора по умолчанию </summary>
        public const string Administrator = "Admin";
        /// <summary> Пароль администратора по умолчанию </summary>
        public const string DefaultAdministratorPassword = "12DBF150wifi@";
        //public const string DefaultAdministratorPassword = "123";
    }
}
