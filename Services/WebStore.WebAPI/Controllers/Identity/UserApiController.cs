using System;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Globalization;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using WebStore.Dal.Context;
using WebStore.Domain.DTO.Identity;
using WebStore.Domain.Identity;
using WebStore.Interfaces.Adresses;

namespace WebStore.WebAPI.Controllers.Identity
{
    [Route(WebAPIInfo.Identity.ApiUser), ApiController]
    public class UserApiController : ControllerBase
    {
        private readonly ILogger<UserApiController> _logger;
        private readonly UserStore<User> _userStore;
        public UserApiController(WebStoreContext context, ILogger<UserApiController> logger)
        {
            _logger = logger;
            _userStore = new UserStore<User>(context);
        }

        [HttpGet("All")]
        public async Task<IEnumerable<User>> GetAllUsers() => 
            await _userStore.Users.ToArrayAsync();

        #region Users

        [HttpPost("GetUserId")]
        public async Task<string> GetUserIdAsync([FromBody] User user)
        {
            return await _userStore.GetUserIdAsync(user);
        }

        [HttpPost("GetUserName")]
        public async Task<string> GetUserNameAsync([FromBody] User user)
        {
            return await _userStore.GetUserNameAsync(user);
        }

        [HttpPost("SetUserName/{name}")]
        public async Task<string> SetUserNameAsync([FromBody] User user, string name)
        {
            await _userStore.SetUserNameAsync(user, name);
            await _userStore.UpdateAsync(user);
            if (!string.Equals(user.UserName, name)) _logger.LogError($"Ошибка при изменении имени пользователя с {user.UserName} на {name}");
            return user.UserName;
        }

        [HttpPost("GetNormalizedUserName")]
        public async Task<string> GetNormalizedUserNameAsync([FromBody] User user)
        {
            return await _userStore.GetNormalizedUserNameAsync(user);
        }

        [HttpPost("SetNormalizedUserName/{name}")]
        public async Task<string> SetNormalizedUserNameAsync([FromBody] User user, string name)
        {
            await _userStore.SetNormalizedUserNameAsync(user, name);
            await _userStore.UpdateAsync(user);
            if (!string.Equals(user.NormalizedUserName, name)) _logger.LogError($"Ошибка при изменении нормализованного имени пользователя с {user.NormalizedUserName} на {name}");
            return user.NormalizedUserName;
        }

        [HttpPost]
        public async Task<bool> CrateAsync([FromBody] User user)
        {
            var result = await _userStore.CreateAsync(user);
            if (!result.Succeeded) _logger.LogError($"Ошибка при создании пользователя {user.UserName}");
            return result.Succeeded;
        }

        [HttpPut]
        public async Task<bool> UpdateAsync([FromBody] User user)
        {
            var result = await _userStore.UpdateAsync(user);
            if (!result.Succeeded) _logger.LogError($"Ошибка при изменении пользователя {user.UserName}");
            return result.Succeeded;
        }

        [HttpDelete("{userId}")]
        public async Task<bool> DeleteAsync(string userId)
        {
            var user = await _userStore.FindByIdAsync(userId);
            var result = await _userStore.DeleteAsync(user);
            return result.Succeeded;
        }

        [HttpGet("FindById/{id}")]
        public async Task<User> FindByIdAsync(string id)
        {
            return await _userStore.FindByIdAsync(id);
        }

        [HttpGet("FindByName/{name}")]
        public async Task<User> FindByName(string name)
        {
            return await _userStore.FindByNameAsync(name);
        }

        #endregion

        #region Roles

        [HttpPost("Role/{role}")]
        public async Task AddToRoleAsync([FromBody] User user, string role)
        {
            await _userStore.AddToRoleAsync(user, role);
            await _userStore.Context.SaveChangesAsync();
        }

        [HttpPost("Role/Delete/{role}")]
        public async Task RemoveFromRoleAsync([FromBody] User user, string role)
        {
            await _userStore.RemoveFromRoleAsync(user, role);
            await _userStore.Context.SaveChangesAsync();
        }

        [HttpPost("Role")]
        public async Task<IList<string>> GetRolesAsync([FromBody] User user)
        {
            return await _userStore.GetRolesAsync(user);
        }

        [HttpPost("IsInRole/{role}")]
        public async Task<bool> IsInRoleAsync([FromBody] User user, string role)
        {
            return await _userStore.IsInRoleAsync(user, role);
        }

        [HttpGet("UsersInRole/{role}")]
        public async Task<IList<User>> GetUsersInRoleAsync(string role)
        {
            return await _userStore.GetUsersInRoleAsync(role);
        }

        #endregion

        #region Passwords

        [HttpPost("SetPasswordHash")]
        public async Task<string> SerPasswordHashAsync([FromBody] PasswordHashDTO hashDto)
        {
            await _userStore.SetPasswordHashAsync(hashDto.User, hashDto.Hash);
            await _userStore.UpdateAsync(hashDto.User);
            if (!string.Equals(hashDto.User.PasswordHash, hashDto.Hash))
                _logger.LogError($"Ошибка при задании пароля пользователю {hashDto.User.UserName}");
            return hashDto.User.PasswordHash;
        }

        [HttpPost("GetPasswordHash")]
        public async Task<string> GetPasswordHashAsync([FromBody] User user)
        {
            return await _userStore.GetPasswordHashAsync(user);
        }

        [HttpPost("HasPassword")]
        public async Task<bool> HasPasswordAsync([FromBody] User user)
        {
            return await _userStore.HasPasswordAsync(user);
        }

        #endregion

        #region Email

        [HttpPost("SetEmail/{email}")]
        public async Task<string> SetEmailAsync([FromBody] User user, string email)
        {
            await _userStore.SetEmailAsync(user, email);
            await _userStore.UpdateAsync(user);
            return user.Email;
        }

        [HttpPost("GetEmail")]
        public async Task<string> GetEmailAsync([FromBody] User user)
        {
            return await _userStore.GetEmailAsync(user);
        }

        [HttpPost("GetEmailConfirmed")]
        public async Task<bool> GetEmailConfirmedAsync([FromBody] User user)
        {
            return await _userStore.GetEmailConfirmedAsync(user);
        }

        [HttpPost("SetEmailConfirmed/{enable:bool}")]
        public async Task<bool> SetEmailConfirmedAsync([FromBody] User user, bool enable)
        {
            await _userStore.SetEmailConfirmedAsync(user, enable);
            await _userStore.UpdateAsync(user);
            return user.EmailConfirmed;
        }

        [HttpGet("FindByEmail/{email}")]
        public async Task<User> FindByEmailAsync(string email)
        {
            return await _userStore.FindByEmailAsync(email);
        }

        [HttpPost("GetNormalizedEmail")]
        public async Task<string> GetNormalizedEmailAsync([FromBody] User user)
        {
            return await _userStore.GetNormalizedEmailAsync(user);
        }

        [HttpPost("SetNormalizedEmail/{email?}")]
        public async Task<string> SetNormalizedEmailAsync([FromBody] User user, string email)
        {
            await _userStore.SetNormalizedEmailAsync(user, email);
            await _userStore.UpdateAsync(user);
            return user.NormalizedEmail;
        }

        #endregion

        #region Phone

        [HttpPost("SetPhoneNumber/{phone}")]
        public async Task<string> SetPhoneNumberAsync([FromBody] User user, string phone)
        {
            await _userStore.SetPhoneNumberAsync(user, phone);
            await _userStore.UpdateAsync(user);
            return user.PhoneNumber;
        }

        [HttpPost("GetPhoneNumber")]
        public async Task<string> GetPhoneNumberAsync([FromBody] User user)
        {
            return await _userStore.GetPhoneNumberAsync(user);
        }

        [HttpPost("GetPhoneNumberConfirmed")]
        public async Task<bool> GetPhoneNumberConfirmedAsync([FromBody] User user)
        {
            return await _userStore.GetPhoneNumberConfirmedAsync(user);
        }

        [HttpPost("SetPhoneNumberConfirmed/{confirmed:bool}")]
        public async Task<bool> SetPhoneNumberConfirmedAsync([FromBody] User user, bool confirmed)
        {
            await _userStore.SetPhoneNumberConfirmedAsync(user, confirmed);
            await _userStore.UpdateAsync(user);
            return user.PhoneNumberConfirmed;
        }

        #endregion

        #region TwoFactor

        [HttpPost("SetTwoFactorEnabled/{enable:bool}")]
        public async Task<bool> SetTwoFactorEnabledAsync([FromBody] User user, bool enable)
        {
            await _userStore.SetTwoFactorEnabledAsync(user, enable);
            await _userStore.UpdateAsync(user);
            return user.TwoFactorEnabled;
        }

        [HttpPost("GetTwoFactorEnabled")]
        public async Task<bool> GetTwoFactorEnabledAsync([FromBody] User user)
        {
            return await _userStore.GetTwoFactorEnabledAsync(user);
        }

        #endregion

        #region Login

        [HttpPost("AddLogin")]
        public async Task AddLoginAsync([FromBody] AddLoginDTO loginDto)
        {
            await _userStore.AddLoginAsync(loginDto.User, loginDto.UserLoginInfo);
            await _userStore.Context.SaveChangesAsync();
        }

        [HttpPost("RemoveLogin/{loginProvider}/{providerKey}")]
        public async Task RemoveLoginAsync([FromBody] User user, string loginProvider, string providerKey)
        {
            await _userStore.RemoveLoginAsync(user, loginProvider, providerKey);
            await _userStore.Context.SaveChangesAsync();
        }

        [HttpPost("GetLogins")]
        public async Task<IList<UserLoginInfo>> GetLoginsAsync([FromBody] User user)
        {
            return await _userStore.GetLoginsAsync(user);
        }

        [HttpGet("FindByLogin/{loginProvider}/{providerKey}")]
        public async Task<User> FindByLoginAsync(string loginProvider, string providerKey)
        {
            return await _userStore.FindByLoginAsync(loginProvider, providerKey);
        }

        #endregion

        #region Lockout

        [HttpPost("GetLockoutEndDate")]
        public async Task<DateTimeOffset?> GetLockoutEndDateAsync([FromBody] User user)
        {
            return await _userStore.GetLockoutEndDateAsync(user);
        }

        [HttpPost("SetLockoutEndDate")]
        public async Task<DateTimeOffset?> SetLockoutEndDateAsync([FromBody] SetLockoutDTO lockoutDto)
        {
            await _userStore.SetLockoutEndDateAsync(lockoutDto.User, lockoutDto.LockoutEnd);
            await _userStore.UpdateAsync(lockoutDto.User);
            return lockoutDto.User.LockoutEnd;
        }

        [HttpPost("IncrementAccessFailedCount")]
        public async Task<int> IncrementAccessFailedCountAsync([FromBody] User user)
        {
            var count = await _userStore.IncrementAccessFailedCountAsync(user);
            await _userStore.UpdateAsync(user);
            return count;
        }

        [HttpPost("ResetAccessFailedCount")]
        public async Task<int> ResetAccessFailedCountAsync([FromBody] User user)
        {
            await _userStore.ResetAccessFailedCountAsync(user);
            await _userStore.UpdateAsync(user);
            return user.AccessFailedCount;
        }

        [HttpPost("GetAccessFailedCount")]
        public async Task<int> GetAccessFailedCountAsync([FromBody] User user)
        {
            return await _userStore.GetAccessFailedCountAsync(user);
        }

        [HttpPost("SetLockoutEnabled/{enabled:bool}")]
        public async Task<bool> GetLockoutEnabledAsync([FromBody] User user, bool enabled)
        {
            await _userStore.SetLockoutEnabledAsync(user, enabled);
            await _userStore.UpdateAsync(user);
            return user.LockoutEnabled;
        }

        #endregion

        #region Claims

        [HttpPost("GetClaims")]
        public async Task<IList<Claim>> GetClaimsAsync([FromBody] User user)
        {
            return await _userStore.GetClaimsAsync(user);
        }

        [HttpPost("AddClaims")]
        public async Task AddClaimsAsync([FromBody] AddClaimDTO claimDto)
        {
            await _userStore.AddClaimsAsync(claimDto.User, claimDto.Cliams);
            await _userStore.Context.SaveChangesAsync();
        }

        [HttpPost("ReplaceClaim")]
        public async Task ReplaceClaimAsync([FromBody] ReplaceClaimDTO claimDto)
        {
            await _userStore.ReplaceClaimAsync(claimDto.User, claimDto.Claim, claimDto.NewClaim);
            await _userStore.Context.SaveChangesAsync();
        }

        [HttpPost("RemoveClaims")]
        public async Task RemoveClaimsAsync([FromBody] RemoveClaimDTO claimDto)
        {
            await _userStore.RemoveClaimsAsync(claimDto.User, claimDto.Cliams);
            await _userStore.Context.SaveChangesAsync();
        }

        [HttpPost("GetUsersForClaim")]
        public async Task<IList<User>> GetUserForClaimAsync([FromBody] Claim claim)
        {
            return await _userStore.GetUsersForClaimAsync(claim);
        }

        #endregion
    }
}
