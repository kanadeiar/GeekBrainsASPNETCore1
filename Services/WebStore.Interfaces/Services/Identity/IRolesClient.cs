using Microsoft.AspNetCore.Identity;
using WebStore.Domain.Identity;

namespace WebStore.Interfaces.Services.Identity
{
    public interface IRolesClient : IRoleStore<Role>
    {
    }
}
