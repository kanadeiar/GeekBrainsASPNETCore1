using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using WebStore.Dal.Context;
using WebStore.Interfaces.Adresses;

namespace WebStore.WebAPI.Controllers.Identity
{
    /// <summary> Управление ролями пользователей </summary>
    [Route(WebAPIInfo.Identity.ApiRole), ApiController]
    public class RoleApiController : ControllerBase
    {
        private readonly ILogger<RoleApiController> _logger;
        private readonly RoleStore<IdentityRole> _roleStore;

        public RoleApiController(WebStoreContext context, ILogger<RoleApiController> logger)
        {
            _logger = logger;
            _roleStore = new RoleStore<IdentityRole>(context);
        }

        /// <summary> Все роли пользователей </summary>
        /// <returns>Роли пользователей</returns>
        [HttpGet("All")]
        public async Task<IEnumerable<IdentityRole>> GetAllRolesAsync() =>
            await _roleStore.Roles.ToArrayAsync();

        #region Roles

        /// <summary> Создание роли пользователей </summary>
        /// <param name="role">Новая роль пользователей</param>
        /// <returns>Результат операции</returns>
        [HttpPost]
        public async Task<bool> CreateAsync([FromBody] IdentityRole role)
        {
            var result = await _roleStore.CreateAsync(role);
            #region Лог
            if (!result.Succeeded) 
                _logger.LogError($"Ошибка создания роли пользователей {role.Name}");
            #endregion
            return result.Succeeded;
        }

        /// <summary> Обновление роли пользователей </summary>
        /// <param name="role">Роль пользователей</param>
        /// <returns>Результат операции</returns>
        [HttpPut]
        public async Task<bool> UpdateAsync([FromBody] IdentityRole role)
        {
            var result = await _roleStore.UpdateAsync(role);
            #region Лог
            if (!result.Succeeded) 
                _logger.LogError($"Ошибка обновления роли пользователей {role.Name}");
            #endregion
            return result.Succeeded;
        }

        /// <summary> Удаление роли пользователей </summary>
        /// <param name="roleId">Идентификатор роли пользователей</param>
        /// <returns>Результат операции</returns>
        [HttpDelete("{roleId}")]
        public async Task<bool> DeleteAsync(string roleId)
        {
            var role = await _roleStore.FindByIdAsync(roleId);
            var result = await _roleStore.DeleteAsync(role);
            #region Лог
            if (!result.Succeeded) 
                _logger.LogError($"Ошибка удаления роли пользователей {role.Name}");
            #endregion
            return result.Succeeded;
        }

        /// <summary> Получение идентификатора роли пользователей по идентификатору </summary>
        /// <param name="role">Роль пользователей</param>
        /// <returns>Идентификатор</returns>
        [HttpPost("GetRoleId")]
        public async Task<string> GetRoleIdAsync([FromBody] IdentityRole role)
        {
            return await _roleStore.GetRoleIdAsync(role);
        }

        /// <summary> Получение название роли </summary>
        /// <param name="role">Роль</param>
        /// <returns>Ее название</returns>
        [HttpPost("GetRoleName")]
        public async Task<string> GetRoleNameAsync([FromBody] IdentityRole role)
        {
            return await _roleStore.GetRoleNameAsync(role);
        }

        /// <summary> Установка названия роли </summary>
        /// <param name="role">Роль</param>
        /// <param name="name">Новое название</param>
        /// <returns>Обновленное название роли</returns>
        [HttpPost("SetRoleName/{name}")]
        public async Task<string> SetRoleNameAsync([FromBody] IdentityRole role, string name)
        {
            await _roleStore.SetRoleNameAsync(role, name);
            await _roleStore.UpdateAsync(role);
            #region Лог
            if (!string.Equals(role.Name, name))
                _logger.LogError($"Ошибка при изменении имени роли с {role.Name} на {name}");
            #endregion
            return role.Name;
        }

        /// <summary> Получить нормализованное название роли пользователей </summary>
        /// <param name="role">Роль</param>
        /// <returns>Нормализованное название</returns>
        [HttpPost("GetNormalizedRoleName")]
        public async Task<string> GetNormalizedRoleNameAsync([FromBody] IdentityRole role)
        {
            return await _roleStore.GetNormalizedRoleNameAsync(role);
        }

        /// <summary> Установка нормализованного названия роли пользователей </summary>
        /// <param name="role">Роль</param>
        /// <param name="name">Новое нормализованное название</param>
        /// <returns>Обновленное название роли</returns>
        [HttpPost("SetNormalizedRoleName/{name}")]
        public async Task<string> SetNormalizedRoleNameAsync([FromBody] IdentityRole role, string name)
        {
            await _roleStore.SetNormalizedRoleNameAsync(role, name);
            await _roleStore.UpdateAsync(role);
            #region Лог
            if (!string.Equals(role.NormalizedName, name))
                _logger.LogError($"Ошибка при изменении нормализованного имени роли с {role.NormalizedName} на {name}");
            #endregion
            return role.NormalizedName;
        }

        /// <summary> Получение роли по идентификатору </summary>
        /// <param name="id">Идентификатор</param>
        /// <returns>Роль</returns>
        [HttpGet("FindById/{id}")]
        public async Task<IdentityRole> FindByIdAsync(string id)
        {
            return await _roleStore.FindByIdAsync(id);
        }

        /// <summary> Получние роли по названию </summary>
        /// <param name="name">Название</param>
        /// <returns>Роль</returns>
        [HttpGet("FindByName/{name}")]
        public async Task<IdentityRole> FindByNameAsync(string name)
        {
            return await _roleStore.FindByNameAsync(name);
        }
        
        #endregion Roles
    }
}
