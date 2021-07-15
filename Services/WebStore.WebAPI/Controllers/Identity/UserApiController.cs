using System;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
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
using System.Text;

namespace WebStore.WebAPI.Controllers.Identity
{
    /// <summary> Управление пользователями </summary>
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

        /// <summary> Все пользователи </summary>
        /// <returns>Пользователи</returns>
        [HttpGet("All")]
        public async Task<IEnumerable<User>> GetAllUsers() => 
            await _userStore.Users.ToArrayAsync();

        #region Users

        /// <summary> Получние идентификатора пользователя </summary>
        /// <param name="user">Пользователь</param>
        /// <returns>Идентификатор</returns>
        [HttpPost("GetUserId")]
        public async Task<string> GetUserIdAsync([FromBody] User user)
        {
            return await _userStore.GetUserIdAsync(user);
        }

        /// <summary> Получение имени пользователя </summary>
        /// <param name="user">Пользователь</param>
        /// <returns>Название пользователя</returns>
        [HttpPost("GetUserName")]
        public async Task<string> GetUserNameAsync([FromBody] User user)
        {
            return await _userStore.GetUserNameAsync(user);
        }

        /// <summary> Установить имя пользователя </summary>
        /// <param name="user">Пользователь</param>
        /// <param name="name">Новое имя</param>
        /// <returns>Обновленное имя пользователя</returns>
        [HttpPost("SetUserName/{name}")]
        public async Task<string> SetUserNameAsync([FromBody] User user, string name)
        {
            await _userStore.SetUserNameAsync(user, name);
            await _userStore.UpdateAsync(user);
            #region Лог
            if (!string.Equals(user.UserName, name)) 
                _logger.LogError($"Ошибка при изменении имени пользователя с {user.UserName} на {name}");
            #endregion
            return user.UserName;
        }

        /// <summary> Получение нормализованного имени пользователя </summary>
        /// <param name="user">Пользователь</param>
        /// <returns>Нормализованное имя</returns>
        [HttpPost("GetNormalizedUserName")]
        public async Task<string> GetNormalizedUserNameAsync([FromBody] User user)
        {
            return await _userStore.GetNormalizedUserNameAsync(user);
        }

        /// <summary> Установка нормализованного имени пользователя </summary>
        /// <param name="user">Пользователь</param>
        /// <param name="name">Новое нормализованное имя пользователя</param>
        /// <returns>Обновленное имя пользователя</returns>
        [HttpPost("SetNormalizedUserName/{name}")]
        public async Task<string> SetNormalizedUserNameAsync([FromBody] User user, string name)
        {
            await _userStore.SetNormalizedUserNameAsync(user, name);
            await _userStore.UpdateAsync(user);
            #region Лог
            if (!string.Equals(user.NormalizedUserName, name)) 
                _logger.LogError($"Ошибка при изменении нормализованного имени пользователя с {user.NormalizedUserName} на {name}");
            #endregion
            return user.NormalizedUserName;
        }

        /// <summary> Создание пользователя </summary>
        /// <param name="user">Новый пользователь</param>
        /// <returns>Результат операции</returns>
        [HttpPost]
        public async Task<bool> CrateAsync([FromBody] User user)
        {
            var result = await _userStore.CreateAsync(user);
            #region Лог
            if (!result.Succeeded) 
                _logger.LogError($"Ошибка при создании пользователя {user.UserName}");
            #endregion
            return result.Succeeded;
        }

        /// <summary> Обновление пользователя </summary>
        /// <param name="user">Пользователь</param>
        /// <returns>Результат операции</returns>
        [HttpPut]
        public async Task<bool> UpdateAsync([FromBody] User user)
        {
            var result = await _userStore.UpdateAsync(user);
            #region Лог
            if (!result.Succeeded) 
                _logger.LogError($"Ошибка при изменении пользователя {user.UserName}");
            #endregion
            return result.Succeeded;
        }

        /// <summary> Удаление пользователя </summary>
        /// <param name="userId">Идентификатор пользователя</param>
        /// <returns>Результат операции</returns>
        [HttpDelete("{userId}")]
        public async Task<bool> DeleteAsync(string userId)
        {
            var user = await _userStore.FindByIdAsync(userId);
            var result = await _userStore.DeleteAsync(user);
            #region Лог
            if (!result.Succeeded)
                _logger.LogError($"Ошибка при удалении пользователя {user.UserName}");
            #endregion
            return result.Succeeded;
        }

        /// <summary> Поиск пользователя по идентификатору </summary>
        /// <param name="id">Идентификатор</param>
        /// <returns>Пользователь</returns>
        [HttpGet("FindById/{id}")]
        public async Task<User> FindByIdAsync(string id)
        {
            return await _userStore.FindByIdAsync(id);
        }

        /// <summary> Поиск пользователя по имени </summary>
        /// <param name="name">Имя пользователя</param>
        /// <returns>Пользователь</returns>
        [HttpGet("FindByName/{name}")]
        public async Task<User> FindByName(string name)
        {
            return await _userStore.FindByNameAsync(name);
        }

        #endregion Users

        #region Roles

        /// <summary> Добавить пользователю роль </summary>
        /// <param name="user">Пользователь</param>
        /// <param name="role">Роль пользователя</param>
        /// <returns>Результат операции</returns>
        [HttpPost("Role/{role}")]
        public async Task AddToRoleAsync([FromBody] User user, string role)
        {
            await _userStore.AddToRoleAsync(user, role);
            await _userStore.Context.SaveChangesAsync();
            #region Лог
            if (!await _userStore.IsInRoleAsync(user, role))
                _logger.LogError($"Не удалось назначить роль {role} пользователю {user.UserName}");
            #endregion
        }

        /// <summary> Отобрать у пользователя роль </summary>
        /// <param name="user">Пользователь</param>
        /// <param name="role">Роль</param>
        /// <returns>Результат операции</returns>
        [HttpPost("Role/Delete/{role}")]
        public async Task RemoveFromRoleAsync([FromBody] User user, string role)
        {
            await _userStore.RemoveFromRoleAsync(user, role);
            await _userStore.Context.SaveChangesAsync();
            #region Лог
            if (await _userStore.IsInRoleAsync(user, role))
                _logger.LogError($"Не удалось удалить роль {role} у пользователя {user.UserName}");
            #endregion
        }

        /// <summary> Получить роли пользователя </summary>
        /// <param name="user">Пользователь</param>
        /// <returns>Его роли</returns>
        [HttpPost("Role")]
        public async Task<IList<string>> GetRolesAsync([FromBody] User user)
        {
            return await _userStore.GetRolesAsync(user);
        }

        /// <summary> Проверка роли у пользователя - в этой роли он или нет </summary>
        /// <param name="user">Пользователь</param>
        /// <param name="role">Роль</param>
        /// <returns>Пользователь в этой роли</returns>
        [HttpPost("IsInRole/{role}")]
        public async Task<bool> IsInRoleAsync([FromBody] User user, string role)
        {
            return await _userStore.IsInRoleAsync(user, role);
        }

        /// <summary> Получение пользователей в этой роли </summary>
        /// <param name="role">Роль</param>
        /// <returns>Пользователи</returns>
        [HttpGet("UsersInRole/{role}")]
        public async Task<IList<User>> GetUsersInRoleAsync(string role)
        {
            return await _userStore.GetUsersInRoleAsync(role);
        }

        #endregion Roles

        #region Passwords

        /// <summary> Установка хеша пароля пользователю </summary>
        /// <param name="hashDto">Дтошка с пользователем и хешем паролем</param>
        /// <returns>Новый хеш пароля</returns>
        [HttpPost("SetPasswordHash")]
        public async Task<string> SerPasswordHashAsync([FromBody] PasswordHashDTO hashDto)
        {
            await _userStore.SetPasswordHashAsync(hashDto.User, hashDto.Hash);
            await _userStore.UpdateAsync(hashDto.User);
            #region Лог
            if (!string.Equals(hashDto.User.PasswordHash, hashDto.Hash))
                _logger.LogError($"Ошибка при задании пароля пользователю {hashDto.User.UserName}");
            #endregion Лог
            return hashDto.User.PasswordHash;
        }

        /// <summary> Получение хеша пароля пользователя </summary>
        /// <param name="user">Пользователь</param>
        /// <returns>Хеш пароля</returns>
        [HttpPost("GetPasswordHash")]
        public async Task<string> GetPasswordHashAsync([FromBody] User user)
        {
            return await _userStore.GetPasswordHashAsync(user);
        }

        /// <summary> Проверка что у пользователя есть хеш пароль </summary>
        /// <param name="user">Пользователь</param>
        /// <returns>Есть пароль</returns>
        [HttpPost("HasPassword")]
        public async Task<bool> HasPasswordAsync([FromBody] User user)
        {
            return await _userStore.HasPasswordAsync(user);
        }

        #endregion Passwords

        #region Email

        /// <summary> Установка адреса почты у пользователя </summary>
        /// <param name="user">Пользователь</param>
        /// <param name="email">электронная почта</param>
        /// <returns>Результат операции</returns>
        [HttpPost("SetEmail/{email}")]
        public async Task<string> SetEmailAsync([FromBody] User user, string email)
        {
            await _userStore.SetEmailAsync(user, email);
            await _userStore.UpdateAsync(user);
            #region Лог
            if (!string.Equals(user.Email, email))
                _logger.LogError($"Не удалось назначить адрес электронной почты {email} пользователю {user.UserName}");
            #endregion
            return user.Email;
        }

        /// <summary> Получение адреса электронной почты пользователя </summary>
        /// <param name="user">Пользователь</param>
        /// <returns>Результат операции</returns>
        [HttpPost("GetEmail")]
        public async Task<string> GetEmailAsync([FromBody] User user)
        {
            return await _userStore.GetEmailAsync(user);
        }

        /// <summary> Определение что адрес почты подтвержден </summary>
        /// <param name="user">Пользователь</param>
        /// <returns>Адрес подтвержден</returns>
        [HttpPost("GetEmailConfirmed")]
        public async Task<bool> GetEmailConfirmedAsync([FromBody] User user)
        {
            return await _userStore.GetEmailConfirmedAsync(user);
        }

        /// <summary> Установка, что адрес почты подтвержден </summary>
        /// <param name="user">Пользователь</param>
        /// <param name="enable">Статус подтверждения</param>
        /// <returns>Обновленный статус</returns>
        [HttpPost("SetEmailConfirmed/{enable:bool}")]
        public async Task<bool> SetEmailConfirmedAsync([FromBody] User user, bool enable)
        {
            await _userStore.SetEmailConfirmedAsync(user, enable);
            await _userStore.UpdateAsync(user);
            #region Лог
            if (await _userStore.GetEmailConfirmedAsync(user) != enable)
                _logger.LogError($"Не удалось установить статус подтверждения {enable} адреса электронной почты у пользователя {user.UserName}");
            #endregion
            return user.EmailConfirmed;
        }

        /// <summary> Поиск пользователя по электронной почте </summary>
        /// <param name="email">Электронная почта</param>
        /// <returns>Пользователь</returns>
        [HttpGet("FindByEmail/{email}")]
        public async Task<User> FindByEmailAsync(string email)
        {
            return await _userStore.FindByEmailAsync(email);
        }

        /// <summary> Получение нормализованного адреса электронной почты </summary>
        /// <param name="user">Пользователь</param>
        /// <returns>Нормализованное имя</returns>
        [HttpPost("GetNormalizedEmail")]
        public async Task<string> GetNormalizedEmailAsync([FromBody] User user)
        {
            return await _userStore.GetNormalizedEmailAsync(user);
        }

        /// <summary> Установка нормализованного адреса электронной почты </summary>
        /// <param name="user">Пользователь</param>
        /// <param name="email">Новый нормализованный адрес</param>
        /// <returns>Обновленный нормализованный адрес</returns>
        [HttpPost("SetNormalizedEmail/{email?}")]
        public async Task<string> SetNormalizedEmailAsync([FromBody] User user, string email)
        {
            await _userStore.SetNormalizedEmailAsync(user, email);
            await _userStore.UpdateAsync(user);
            #region Лог
            if (!string.Equals(user.NormalizedEmail, email))
                _logger.LogError($"Не удалось установить нормализованный адрес электронной почты {email} у пользователя {user.UserName}");
            #endregion
            return user.NormalizedEmail;
        }

        #endregion Email

        #region Phone

        /// <summary> Установка номера телефона пользователю </summary>
        /// <param name="user">Пользователь</param>
        /// <param name="phone">Номер телефона</param>
        /// <returns>Обновленный номер телефона</returns>
        [HttpPost("SetPhoneNumber/{phone}")]
        public async Task<string> SetPhoneNumberAsync([FromBody] User user, string phone)
        {
            await _userStore.SetPhoneNumberAsync(user, phone);
            await _userStore.UpdateAsync(user);
            #region Лог
            if (!string.Equals(user.PhoneNumber, phone))
                _logger.LogError($"Не удалось установить номер телефона {phone} у пользователя {user.UserName}");
            #endregion
            return user.PhoneNumber;
        }

        /// <summary> Получение номера телефона пользователя </summary>
        /// <param name="user">Пользователь</param>
        /// <returns>Результат операции</returns>
        [HttpPost("GetPhoneNumber")]
        public async Task<string> GetPhoneNumberAsync([FromBody] User user)
        {
            return await _userStore.GetPhoneNumberAsync(user);
        }

        /// <summary> Получение статуса подтверждения номера телефона </summary>
        /// <param name="user">Пользователь</param>
        /// <returns>Телефон подтвержден</returns>
        [HttpPost("GetPhoneNumberConfirmed")]
        public async Task<bool> GetPhoneNumberConfirmedAsync([FromBody] User user)
        {
            return await _userStore.GetPhoneNumberConfirmedAsync(user);
        }

        /// <summary> Установка статуса подтвержедния номера телефона </summary>
        /// <param name="user">Пользователь</param>
        /// <param name="confirmed">Статус подтверждения</param>
        /// <returns>Обновленный статус подтверждения</returns>
        [HttpPost("SetPhoneNumberConfirmed/{confirmed:bool}")]
        public async Task<bool> SetPhoneNumberConfirmedAsync([FromBody] User user, bool confirmed)
        {
            await _userStore.SetPhoneNumberConfirmedAsync(user, confirmed);
            await _userStore.UpdateAsync(user);
            #region Лог
            if (user.PhoneNumberConfirmed != confirmed)
                _logger.LogError($"Не удалось установить статус подтверждения {confirmed} у номера телефона у пользователя {user.UserName}");
            #endregion
            return user.PhoneNumberConfirmed;
        }

        #endregion Phone

        #region TwoFactor

        /// <summary> Установка двухфакторного входа </summary>
        /// <param name="user">Пользователь</param>
        /// <param name="enable">Статус установки</param>
        /// <returns>Текущий обновленный статус</returns>
        [HttpPost("SetTwoFactorEnabled/{enable:bool}")]
        public async Task<bool> SetTwoFactorEnabledAsync([FromBody] User user, bool enable)
        {
            await _userStore.SetTwoFactorEnabledAsync(user, enable);
            await _userStore.UpdateAsync(user);
            #region Лог
            if (user.TwoFactorEnabled != enable)
                _logger.LogError($"Не удалось установить статус двухфакторного входа {enable} у пользователя {user.UserName}");
            #endregion
            return user.TwoFactorEnabled;
        }

        /// <summary> Получение двухфакторного входа </summary>
        /// <param name="user">Пользователь</param>
        /// <returns>Статус установки</returns>
        [HttpPost("GetTwoFactorEnabled")]
        public async Task<bool> GetTwoFactorEnabledAsync([FromBody] User user)
        {
            return await _userStore.GetTwoFactorEnabledAsync(user);
        }

        #endregion TwoFactor

        #region Login

        /// <summary> Добавление логина </summary>
        /// <param name="loginDto">Дтошка добавления логина пользователя</param>
        /// <returns>Результат операции</returns>
        [HttpPost("AddLogin")]
        public async Task AddLoginAsync([FromBody] AddLoginDTO loginDto)
        {
            await _userStore.AddLoginAsync(loginDto.User, loginDto.UserLoginInfo);
            await _userStore.Context.SaveChangesAsync();
            #region Лог
            if (!(await _userStore.GetLoginsAsync(loginDto.User)).Contains(loginDto.UserLoginInfo))
                _logger.LogError($"Не удалось добавить логин {loginDto.UserLoginInfo.LoginProvider} {loginDto.UserLoginInfo.ProviderKey} пользователю {loginDto.User.UserName}");
            #endregion
        }

        /// <summary> Удалить логин пользователя </summary>
        /// <param name="user">Пользователь</param>
        /// <param name="loginProvider">Провайдер</param>
        /// <param name="providerKey">Ключ</param>
        /// <returns>Результат операции</returns>
        [HttpPost("RemoveLogin/{loginProvider}/{providerKey}")]
        public async Task RemoveLoginAsync([FromBody] User user, string loginProvider, string providerKey)
        {
            await _userStore.RemoveLoginAsync(user, loginProvider, providerKey);
            await _userStore.Context.SaveChangesAsync();
            #region Лог
            var logins = await _userStore.GetLoginsAsync(user);
            foreach (var login in logins)
                if (login.LoginProvider == loginProvider && login.ProviderKey == providerKey)
                {
                    _logger.LogError($"Не удалось удалить логин {loginProvider} {providerKey} у пользователя {user}");
                    break;
                }
            #endregion
        }

        /// <summary> Получение всех логинов пользователя </summary>
        /// <param name="user">Пользователь</param>
        /// <returns>Его логины</returns>
        [HttpPost("GetLogins")]
        public async Task<IList<UserLoginInfo>> GetLoginsAsync([FromBody] User user)
        {
            return await _userStore.GetLoginsAsync(user);
        }

        /// <summary> Получение логина пользователя </summary>
        /// <param name="loginProvider"></param>
        /// <param name="providerKey"></param>
        /// <returns>Пользователь</returns>
        [HttpGet("FindByLogin/{loginProvider}/{providerKey}")]
        public async Task<User> FindByLoginAsync(string loginProvider, string providerKey)
        {
            return await _userStore.FindByLoginAsync(loginProvider, providerKey);
        }

        #endregion Login

        #region Lockout

        /// <summary> Получение даты окончания блокировки </summary>
        /// <param name="user">Пользователь</param>
        /// <returns>Дата</returns>
        [HttpPost("GetLockoutEndDate")]
        public async Task<DateTimeOffset?> GetLockoutEndDateAsync([FromBody] User user)
        {
            return await _userStore.GetLockoutEndDateAsync(user);
        }

        /// <summary> Установка даты окончания блокировки </summary>
        /// <param name="lockoutDto">Дтошка для пользователя и даты</param>
        /// <returns>Обновленная дата окончания блокировки</returns>
        [HttpPost("SetLockoutEndDate")]
        public async Task<DateTimeOffset?> SetLockoutEndDateAsync([FromBody] SetLockoutDTO lockoutDto)
        {
            await _userStore.SetLockoutEndDateAsync(lockoutDto.User, lockoutDto.LockoutEnd);
            await _userStore.UpdateAsync(lockoutDto.User);
            #region Лог
            if (!(lockoutDto.User.LockoutEnd == lockoutDto.LockoutEnd))
                _logger.LogError($"Не удалось установить дату окончания блокировки {lockoutDto.LockoutEnd} у пользователя {lockoutDto.User.UserName}");
            #endregion
            return lockoutDto.User.LockoutEnd;
        }

        /// <summary> Увеличение количества неверных входов </summary>
        /// <param name="user">Пользователь</param>
        /// <returns>Текущее количество</returns>
        [HttpPost("IncrementAccessFailedCount")]
        public async Task<int> IncrementAccessFailedCountAsync([FromBody] User user)
        {
            var oldCount = await _userStore.GetAccessFailedCountAsync(user);
            var count = await _userStore.IncrementAccessFailedCountAsync(user);
            await _userStore.UpdateAsync(user);
            #region Лог
            if (count != oldCount + 1)
                _logger.LogError($"Не удалось увеличить на еденицу количество неверных входов {oldCount} у пользователя {user.UserName} ");
            #endregion
            return count;
        }

        /// <summary> Сброс количества неверных входов </summary>
        /// <param name="user">Пользователь</param>
        /// <returns>Текущее количество</returns>
        [HttpPost("ResetAccessFailedCount")]
        public async Task<int> ResetAccessFailedCountAsync([FromBody] User user)
        {
            await _userStore.ResetAccessFailedCountAsync(user);
            await _userStore.UpdateAsync(user);
            #region Лог
            if (await _userStore.GetAccessFailedCountAsync(user) != 0)
                _logger.LogError($"Не удалось сбросить количество неверных входов у пользователя {user.UserName}");
            #endregion
            return user.AccessFailedCount;
        }

        /// <summary> Получение количества неверных входов </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost("GetAccessFailedCount")]
        public async Task<int> GetAccessFailedCountAsync([FromBody] User user)
        {
            return await _userStore.GetAccessFailedCountAsync(user);
        }

        /// <summary> Получение значения что пользователь заблокирован </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost("GetLockoutEnabled")]
        public async Task<bool> GetLockoutEnabledAsync([FromBody] User user)
        {
            return await _userStore.GetLockoutEnabledAsync(user);
        }

        /// <summary> Установка, что пользователь заблокирован </summary>
        /// <param name="user"></param>
        /// <param name="enabled"></param>
        /// <returns></returns>
        [HttpPost("SetLockoutEnabled/{enabled:bool}")]
        public async Task<bool> SetLockoutEnabledAsync([FromBody] User user, bool enabled)
        {
            await _userStore.SetLockoutEnabledAsync(user, enabled);
            await _userStore.UpdateAsync(user);
            #region Лог
            if (user.LockoutEnabled != enabled)
                _logger.LogError($"Не удалось установить статус блокировки {enabled} у пользователя {user.UserName}");
            #endregion
            return user.LockoutEnabled;
        }

        #endregion Lockout

        #region Claims

        /// <summary> Получение прав пользователя </summary>
        /// <param name="user">Пользователь</param>
        /// <returns>Права его</returns>
        [HttpPost("GetClaims")]
        public async Task<IList<Claim>> GetClaimsAsync([FromBody] User user)
        {
            return await _userStore.GetClaimsAsync(user);
        }

        /// <summary> Добавление прав пользователю </summary>
        /// <param name="claimDto">Дотшка для добавления прав</param>
        /// <returns>Результат операции</returns>
        [HttpPost("AddClaims")]
        public async Task AddClaimsAsync([FromBody] AddClaimDTO claimDto)
        {
            await _userStore.AddClaimsAsync(claimDto.User, claimDto.Cliams);
            await _userStore.Context.SaveChangesAsync();
            #region Лог
            var claims = await _userStore.GetClaimsAsync(claimDto.User);
            var containsAll = true;
            foreach (var claim in claimDto.Cliams)
            {
                if (claims.Contains(claim))
                    continue;
                else
                {
                    containsAll = false;
                    break;
                }
            }
            if (!containsAll)
                _logger.LogError($"Не удалось добавить права пользователю {claimDto.User}");
            #endregion
        }

        /// <summary> Изменение права пользователя </summary>
        /// <param name="claimDto">Дтошка для пользователя, права и нового права</param>
        /// <returns>Результат операции</returns>
        [HttpPost("ReplaceClaim")]
        public async Task ReplaceClaimAsync([FromBody] ReplaceClaimDTO claimDto)
        {
            await _userStore.ReplaceClaimAsync(claimDto.User, claimDto.Claim, claimDto.NewClaim);
            await _userStore.Context.SaveChangesAsync();
            #region Лог
            var claims = await _userStore.GetClaimsAsync(claimDto.User);
            if (!claims.Contains(claimDto.NewClaim))
                _logger.LogError($"Не удалось изменить права пользователя с {claimDto.Claim} {claimDto.NewClaim} на у пользователя {claimDto.User}");
            #endregion
        }

        /// <summary> Отбирание права у пользователя </summary>
        /// <param name="claimDto">Дтошка для пользователя, права</param>
        /// <returns>Результат операции</returns>
        [HttpPost("RemoveClaims")]
        public async Task RemoveClaimsAsync([FromBody] RemoveClaimDTO claimDto)
        {
            await _userStore.RemoveClaimsAsync(claimDto.User, claimDto.Cliams);
            await _userStore.Context.SaveChangesAsync();
            #region Лог
            var claims = await _userStore.GetClaimsAsync(claimDto.User);
            var containsAny = false;
            foreach (var claim in claimDto.Cliams)
            {
                if (claims.Contains(claim))
                {
                    containsAny = true;
                    break;
                }
                else
                    continue; 
            }
            if (containsAny)
                _logger.LogError($"Не удалось отобрать права у пользователя {claimDto.User}");
            #endregion
        }

        /// <summary> Получение пользователей с таким правом </summary>
        /// <param name="claim">Право</param>
        /// <returns>Пользователи</returns>
        [HttpPost("GetUsersForClaim")]
        public async Task<IList<User>> GetUserForClaimAsync([FromBody] Claim claim)
        {
            return await _userStore.GetUsersForClaimAsync(claim);
        }

        #endregion Claims
    }
}
