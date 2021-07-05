using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
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

        [HttpDelete]
        public async Task<bool> DeleteAsync([FromBody] User user)
        {
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




    }
}
