using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using WebStore.Domain.Identity;
using WebStore.Interfaces.Adresses;
using WebStore.Interfaces.Services.Identity;
using WebStore.WebAPI.Client.Base;

namespace WebStore.WebAPI.Client.Identity
{
    public class RoleApiClient : BaseClient, IRolesClient
    {
        public RoleApiClient(HttpClient client) : base(client, WebAPIInfo.Identity.ApiRole) { }

        #region IRoleStore<Role>
        
        public async Task<IdentityResult> CreateAsync(IdentityRole role, CancellationToken cancel)
        {
            var response = await PostAsync(Address, role, cancel).ConfigureAwait(false);
            var result = await response.Content.ReadFromJsonAsync<bool>(cancellationToken: cancel);
            return result ? IdentityResult.Success : IdentityResult.Failed();
        }

        public async Task<IdentityResult> UpdateAsync(IdentityRole role, CancellationToken cancel)
        {
            var response = await PutAsync(Address, role, cancel).ConfigureAwait(false);
            var result = await response.Content.ReadFromJsonAsync<bool>(cancellationToken: cancel);
            return result ? IdentityResult.Success : IdentityResult.Failed();
        }

        public async Task<IdentityResult> DeleteAsync(IdentityRole role, CancellationToken cancel)
        {
            var response = await DeleteAsync($"{Address}/{role.Id}", cancel).ConfigureAwait(false);
            var result = await response.Content.ReadFromJsonAsync<bool>(cancellationToken:cancel);
            return result ? IdentityResult.Success : IdentityResult.Failed();
        }

        public async Task<string> GetRoleIdAsync(IdentityRole role, CancellationToken cancel)
        {
            var response = await PostAsync($"{Address}/GetRoleId", role, cancel).ConfigureAwait(false);
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> GetRoleNameAsync(IdentityRole role, CancellationToken cancel)
        {
            var response = await PostAsync($"{Address}/GetRoleName", role, cancel).ConfigureAwait(false);
            return await response.Content.ReadAsStringAsync();
        }

        public async Task SetRoleNameAsync(IdentityRole role, string roleName, CancellationToken cancel)
        {
            var response = await PostAsync($"{Address}/SetRoleName/{roleName}", role, cancel).ConfigureAwait(false);
            role.Name = await response.Content.ReadAsStringAsync();
        }

        public async Task<string> GetNormalizedRoleNameAsync(IdentityRole role, CancellationToken cancel)
        {
            var response = await PostAsync($"{Address}/GetNormalizedRoleName", role, cancel).ConfigureAwait(false);
            return await response.Content.ReadAsStringAsync();
        }

        public async Task SetNormalizedRoleNameAsync(IdentityRole role, string normalizedName, CancellationToken cancel)
        {
            var response = await PostAsync($"{Address}/SetNormalizedRoleName/{normalizedName}", role, cancel)
                .ConfigureAwait(false);
            role.NormalizedName = await response.Content.ReadAsStringAsync();
        }

        public async Task<IdentityRole> FindByIdAsync(string roleId, CancellationToken cancel)
        {
            return await GetAsync<IdentityRole>($"{Address}/FindById/{roleId}", cancel).ConfigureAwait(false);
        }

        public async Task<IdentityRole> FindByNameAsync(string normalizedRoleName, CancellationToken cancel)
        {
            return await GetAsync<IdentityRole>($"{Address}/FindByName/{normalizedRoleName}", cancel);
        }

        #endregion IRoleStore<Role>
    }
}
