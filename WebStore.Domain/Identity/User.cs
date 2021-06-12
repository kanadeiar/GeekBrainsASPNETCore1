using Microsoft.AspNetCore.Identity;

namespace WebStore.Domain.Identity
{
    public class User : IdentityUser
    {
        private const string Administrator = "Admin";
        public const string DefaultAdministratorPassword = "123";
    }
}
