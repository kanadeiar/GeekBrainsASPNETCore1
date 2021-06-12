using Microsoft.AspNetCore.Identity;

namespace WebStore.Domain.Identity
{
    public class Role : IdentityRole
    {
        public const string Administrators = "Administrators";
        public const string Users = "Users";
        public const string Clients = "Clients";
    }
}
