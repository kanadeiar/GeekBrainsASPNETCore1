using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using WebStore.Domain.Entities.Orders;

namespace WebStore.Domain.Identity
{
    public class User : IdentityUser
    {
        public ICollection<Order> Orders { get; set; } = new List<Order>();

        
        public const string Administrator = "Admin";
        //public const string DefaultAdministratorPassword = "12DBF150wifi@";
        public const string DefaultAdministratorPassword = "123";
    }
}
