using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using WebStore.Domain.DTO.Identity;
using WebStore.Domain.Identity;
using WebStore.Interfaces.Adresses;
using WebStore.Interfaces.Services.Identity;
using WebStore.WebAPI.Client.Base;

namespace WebStore.WebAPI.Client.Identity
{
    public class UserApiClient : BaseClient, IUsersClient
    {
        public UserApiClient(HttpClient client) : base(client, WebAPIInfo.Identity.ApiUser) { }

        #region IUserStore<User>

        public async Task<string> GetUserIdAsync(User user, CancellationToken cancel)
        {
            var response = await PostAsync($"{Address}/GetUserId", user, cancel).ConfigureAwait(false);
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> GetUserNameAsync(User user, CancellationToken cancel)
        {
            var response = await PostAsync($"{Address}/GetUserName", user, cancel).ConfigureAwait(false);
            return await response.Content.ReadAsStringAsync();
        }

        public async Task SetUserNameAsync(User user, string userName, CancellationToken cancel)
        {
            var response = await PostAsync($"{Address}/SetUserName/{userName}", user, cancel).ConfigureAwait(false);
            user.UserName = await response.Content.ReadAsStringAsync();
        }

        public async Task<string> GetNormalizedUserNameAsync(User user, CancellationToken cancel)
        {
            var response = await PostAsync($"{Address}/GetNormalizedUserName", user, cancel).ConfigureAwait(false);
            return await response.Content.ReadAsStringAsync();
        }

        public async Task SetNormalizedUserNameAsync(User user, string normalizedName, CancellationToken cancel)
        {
            var response = await PostAsync($"{Address}/SetNormalizedUserName/{normalizedName}", user, cancel)
                .ConfigureAwait(false);
            user.NormalizedUserName = await response.Content.ReadAsStringAsync();
        }

        public async Task<IdentityResult> CreateAsync(User user, CancellationToken cancel)
        {
            var response = await PostAsync(Address, user, cancel).ConfigureAwait(false);
            var result = await response.Content.ReadFromJsonAsync<bool>(cancellationToken: cancel);
            return result ? IdentityResult.Success : IdentityResult.Failed();
        }

        public async Task<IdentityResult> UpdateAsync(User user, CancellationToken cancel)
        {
            var response = await PutAsync(Address, user, cancel).ConfigureAwait(false);
            var result = await response.Content.ReadFromJsonAsync<bool>(cancellationToken: cancel);
            return result ? IdentityResult.Success : IdentityResult.Failed();
        }

        public async Task<IdentityResult> DeleteAsync(User user, CancellationToken cancel)
        {
            var response = await DeleteAsync(Address, cancel).ConfigureAwait(false);
            var result = await response.Content.ReadFromJsonAsync<bool>(cancellationToken: cancel);
            return result ? IdentityResult.Success : IdentityResult.Failed();
        }

        public async Task<User> FindByIdAsync(string userId, CancellationToken cancel)
        {
            return await GetAsync<User>($"{Address}/FindById/{userId}", cancel).ConfigureAwait(false);
        }

        public async Task<User> FindByNameAsync(string normalizedUserName, CancellationToken cancel)
        {
            return await GetAsync<User>($"{Address}/FindByName/{normalizedUserName}", cancel).ConfigureAwait(false);
        }

        #endregion

        #region IUserRoleStore<User>

        public async Task AddToRoleAsync(User user, string roleName, CancellationToken cancel)
        {
            await PostAsync($"{Address}/Role/{roleName}", user, cancel).ConfigureAwait(false);
        }

        public async Task RemoveFromRoleAsync(User user, string roleName, CancellationToken cancel)
        {
            await PostAsync($"{Address}/Role/Delete/{roleName}", user, cancel).ConfigureAwait(false);
        }

        public async Task<IList<string>> GetRolesAsync(User user, CancellationToken cancel)
        {
            var response = await PostAsync($"{Address}/Role", user, cancel).ConfigureAwait(false);
            return await response.Content.ReadFromJsonAsync<IList<string>>(cancellationToken: cancel);
        }

        public async Task<bool> IsInRoleAsync(User user, string roleName, CancellationToken cancel)
        {
            var response = await PostAsync($"{Address}/IsInRole/{roleName}", user, cancel).ConfigureAwait(false);
            return await response.Content.ReadFromJsonAsync<bool>(cancellationToken: cancel);
        }

        public async Task<IList<User>> GetUsersInRoleAsync(string roleName, CancellationToken cancel)
        {
            return await GetAsync<List<User>>($"{Address}/UsersInRole/{roleName}", cancel).ConfigureAwait(false);
        }


        #endregion

        #region IUserPasswordStore<User>

        public async Task SetPasswordHashAsync(User user, string passwordHash, CancellationToken cancel)
        {
            var response = await PostAsync($"{Address}/SetPasswordHash",
                new PasswordHashDTO {User = user, Hash = passwordHash}, cancel).ConfigureAwait(false);
            user.PasswordHash = await response.Content.ReadAsStringAsync();
        }

        public async Task<string> GetPasswordHashAsync(User user, CancellationToken cancel)
        {
            var response = await PostAsync($"{Address}/GetPasswordHash", user, cancel).ConfigureAwait(false);
            return await response.Content.ReadFromJsonAsync<string>(cancellationToken: cancel);
        }

        public async Task<bool> HasPasswordAsync(User user, CancellationToken cancel)
        {
            var response = await PostAsync($"{Address}/HasPassword", user, cancel).ConfigureAwait(false);
            return await response.Content.ReadFromJsonAsync<bool>(cancellationToken: cancel);
        }

        #endregion

        #region IUserEmailStore<User>

        public async Task SetEmailAsync(User user, string email, CancellationToken cancel)
        {
            var response = await PostAsync($"{Address}/SetEmail/{email}", user, cancel).ConfigureAwait(false);
            user.Email = await response.Content.ReadAsStringAsync();
        }

        public async Task<string> GetEmailAsync(User user, CancellationToken cancel)
        {
            var response = await PostAsync($"{Address}/GetEmail", user, cancel).ConfigureAwait(false);
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<bool> GetEmailConfirmedAsync(User user, CancellationToken cancel)
        {
            var response = await PostAsync($"{Address}/GetEmailConfirmed", user, cancel).ConfigureAwait(false);
            return await response.Content.ReadFromJsonAsync<bool>(cancellationToken: cancel);
        }

        public async Task SetEmailConfirmedAsync(User user, bool confirmed, CancellationToken cancel)
        {
            var response = await PostAsync($"{Address}/SetEmailConfirmed/{confirmed}", user, cancel)
                .ConfigureAwait(false);
            user.EmailConfirmed = await response.Content.ReadFromJsonAsync<bool>(cancellationToken: cancel);
        }

        public async Task<User> FindByEmailAsync(string normalizedEmail, CancellationToken cancel)
        {
            return await GetAsync<User>($"{Address}/FindByEmail/{normalizedEmail}", cancel).ConfigureAwait(false);
        }

        public async Task<string> GetNormalizedEmailAsync(User user, CancellationToken cancel)
        {
            var response = await PostAsync($"{Address}/GetNormalizedEmail", user, cancel).ConfigureAwait(false);
            return await response.Content.ReadAsStringAsync();
        }

        public async Task SetNormalizedEmailAsync(User user, string normalizedEmail, CancellationToken cancel)
        {
            var response = await PostAsync($"{Address}/SetNormalizedEmail/{normalizedEmail}", user, cancel)
                .ConfigureAwait(false);
            user.NormalizedEmail = await response.Content.ReadAsStringAsync();
        }

        #endregion

        #region IUserPhoneNumberStore<User>

        public async Task SetPhoneNumberAsync(User user, string phoneNumber, CancellationToken cancel)
        {
            var response = await PostAsync($"{Address}/SetPhoneNumber/{phoneNumber}", user, cancel)
                .ConfigureAwait(false);
            user.PhoneNumber = await response.Content.ReadAsStringAsync();
        }

        public async Task<string> GetPhoneNumberAsync(User user, CancellationToken cancel)
        {
            var response = await PostAsync($"{Address}/GetPhoneNumber", user, cancel).ConfigureAwait(false);
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<bool> GetPhoneNumberConfirmedAsync(User user, CancellationToken cancel)
        {
            var response = await PostAsync($"{Address}/GetPhoneNumberConfirmed", user, cancel).ConfigureAwait(false);
            return await response.Content.ReadFromJsonAsync<bool>(cancellationToken: cancel);
        }

        public async Task SetPhoneNumberConfirmedAsync(User user, bool confirmed, CancellationToken cancel)
        {
            var response = await PostAsync($"{Address}/SetPhoneNumberConfirmed/{confirmed}", user, cancel)
                .ConfigureAwait(false);
            user.PhoneNumberConfirmed = await response.Content.ReadFromJsonAsync<bool>(cancellationToken: cancel);
        }

        #endregion

        #region IUserTwoFactorStore<User>

        public async Task SetTwoFactorEnabledAsync(User user, bool enabled, CancellationToken cancel)
        {
            var response = await PostAsync($"{Address}/SetTwoFactorEnabled/{enabled}", user, cancel)
                .ConfigureAwait(false);
            user.TwoFactorEnabled = await response.Content.ReadFromJsonAsync<bool>(cancellationToken: cancel);
        }

        public async Task<bool> GetTwoFactorEnabledAsync(User user, CancellationToken cancel)
        {
            var response = await PostAsync($"{Address}/GetTwoFactorEnabled", user, cancel).ConfigureAwait(false);
            return await response.Content.ReadFromJsonAsync<bool>(cancellationToken: cancel);
        }

        #endregion

        #region IUserLoginStore<User>

        public Task AddLoginAsync(User user, UserLoginInfo login, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task RemoveLoginAsync(User user, string loginProvider, string providerKey, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<IList<UserLoginInfo>> GetLoginsAsync(User user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<User> FindByLoginAsync(string loginProvider, string providerKey, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IUserLockoutStore<User>

        public Task<DateTimeOffset?> GetLockoutEndDateAsync(User user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task SetLockoutEndDateAsync(User user, DateTimeOffset? lockoutEnd, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<int> IncrementAccessFailedCountAsync(User user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task ResetAccessFailedCountAsync(User user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<int> GetAccessFailedCountAsync(User user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<bool> GetLockoutEnabledAsync(User user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task SetLockoutEnabledAsync(User user, bool enabled, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IUserClaimStore<User>

        public Task<IList<Claim>> GetClaimsAsync(User user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task AddClaimsAsync(User user, IEnumerable<Claim> claims, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task ReplaceClaimAsync(User user, Claim claim, Claim newClaim, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task RemoveClaimsAsync(User user, IEnumerable<Claim> claims, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<IList<User>> GetUsersForClaimAsync(Claim claim, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
