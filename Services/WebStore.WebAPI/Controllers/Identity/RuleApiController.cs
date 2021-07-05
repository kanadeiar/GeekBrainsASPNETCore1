using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using WebStore.Dal.Context;
using WebStore.Domain.Identity;
using WebStore.Interfaces.Adresses;

namespace WebStore.WebAPI.Controllers.Identity
{
    [Route(WebAPIInfo.Identity.ApiRole), ApiController]
    public class RuleApiController : ControllerBase
    {
        private readonly ILogger<RuleApiController> _logger;
        private readonly RoleStore<Role> _roleStore;

        public RuleApiController(WebStoreContext context, ILogger<RuleApiController> logger)
        {
            _logger = logger;
            _roleStore = new RoleStore<Role>(context);
        }

        [HttpGet("All")]
        public async Task<IEnumerable<Role>> GetAllRolesAsync() =>
            await _roleStore.Roles.ToArrayAsync();

        [HttpPost]
        public async Task<bool> CreateAsync([FromBody] Role role)
        {
            var result = await _roleStore.CreateAsync(role);
            if (!result.Succeeded) _logger.LogError($"Ошибка создания роли пользователей {role.Name}");
            return result.Succeeded;
        }

        [HttpPut]
        public async Task<bool> UpdateAsync([FromBody] Role role)
        {
            var result = await _roleStore.UpdateAsync(role);
            if (!result.Succeeded) _logger.LogError($"Ошибка обновления роли пользователей {role.Name}");
            return result.Succeeded;
        }

        [HttpDelete]
        public async Task<bool> DeleteAsync(Role role)
        {
            var result = await _roleStore.DeleteAsync(role);
            if (!result.Succeeded) _logger.LogError($"Ошибка удаления роли пользователей {role.Name}");
            return result.Succeeded;
        }

        [HttpPost("GetRoleId")]
        public async Task<string> GetRoleIdAsync([FromBody] Role role)
        {
            return await _roleStore.GetRoleIdAsync(role);
        }

        [HttpPost("GetRoleName")]
        public async Task<string> GetRoleNameAsync([FromBody] Role role)
        {
            return await _roleStore.GetRoleNameAsync(role);
        }

        [HttpPost("SetRoleName/{name}")]
        public async Task<string> SetRoleNameAsync([FromBody] Role role, string name)
        {
            await _roleStore.SetRoleNameAsync(role, name);
            await _roleStore.UpdateAsync(role);
            if (!string.Equals(role.Name, name)) _logger.LogError($"Ошибка при изменении имени роли с {role.Name} на {name}");
            return role.Name;
        }

        [HttpPost("GetNormalizedRoleName")]
        public async Task<string> GetNormalizedRoleNameAsync([FromBody] Role role)
        {
            return await _roleStore.GetNormalizedRoleNameAsync(role);
        }

        [HttpPost("SetNormalizedRoleName/{name}")]
        public async Task<string> SetNormalizedRoleNameAsync([FromBody] Role role, string name)
        {
            await _roleStore.SetNormalizedRoleNameAsync(role, name);
            await _roleStore.UpdateAsync(role);
            if (!string.Equals(role.NormalizedName, name)) _logger.LogError($"Ошибка при изменении нормализованного имени роли с {role.NormalizedName} на {name}");
            return role.NormalizedName;
        }

        [HttpGet("FindById/{id}")]
        public async Task<Role> FindBuIdAsync(string id)
        {
            return await _roleStore.FindByIdAsync(id);
        }

        [HttpGet("FindByName/{name}")]
        public async Task<Role> FindByNameAsync(string name)
        {
            return await _roleStore.FindByNameAsync(name);
        }
    }
}
