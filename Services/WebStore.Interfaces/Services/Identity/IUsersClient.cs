using Microsoft.AspNetCore.Identity;
using WebStore.Domain.Identity;

namespace WebStore.Interfaces.Services.Identity
{
    public interface IUsersClient : IUserRoleStore<User>, 
        IUserPasswordStore<User>, 
        IUserEmailStore<User>,
        IUserPhoneNumberStore<User>,
        IUserTwoFactorStore<User>,
        IUserLoginStore<User>,
        IUserLockoutStore<User>,
        IUserClaimStore<User>
    {
    }
}
