using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebStore.Domain.Identity;
using WebStore.Domain.Identity.ErrorCodes;
using WebStore.Domain.WebModels.Account;

namespace WebStore.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly ILogger<AccountController> _logger;
        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager, ILogger<AccountController> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
        }

        #region Регистрация пользователя

        [AllowAnonymous]
        public IActionResult Register() => View(new RegisterWebModel());

        [HttpPost, ValidateAntiForgeryToken, AllowAnonymous]
        public async Task<IActionResult> Register(RegisterWebModel model)
        {
            if (!ModelState.IsValid)
                return View(model);
            #region Лог
            _logger.LogInformation($"Регистрация нового пользователя {model.UserName}");
            #endregion
            var user = new User
            {
                UserName = model.UserName,
            };
            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                #region Лог
                _logger.LogInformation($"Пользователь {user.UserName} успешно зарегистрирован");
                #endregion
                await _userManager.AddToRoleAsync(user, Role.Users);
                #region Лог
                _logger.LogInformation($"Пользователю {user.UserName} автоматически назначена роль {Role.Users}");
                #endregion
                await _signInManager.SignInAsync(user, false);
                #region Лог
                _logger.LogInformation($"Пользователь {user.UserName} автоматически вошел в систему после регистрации");
                #endregion
                return RedirectToAction("Index", "Home");
            }

            var errors = result.Errors.Select(e => IdentityErrorCodes.GetDescription(e.Code)).ToArray();
            foreach (var error in errors)
            {
                ModelState.AddModelError("", error);
            }

            #region Лог
            _logger.LogError($"{DateTime.Now} Ошибки при регистрации пользователя {user.UserName} в систему: " +
                             $"{string.Join(",", errors)}");
            #endregion
            return View(model);
        }

        #endregion

        #region Вход в систему

        [AllowAnonymous]
        public IActionResult Login(string returnUrl) => View(new LoginWebModel{ReturnUrl = returnUrl});

        [HttpPost, ValidateAntiForgeryToken, AllowAnonymous]
        public async Task<IActionResult> Login(LoginWebModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var result = await _signInManager.PasswordSignInAsync(
                model.UserName, 
                model.Password, 
                model.RememberMe, 
#if DEBUG
                false
#else
                true
#endif
            );

            if (result.Succeeded)
            {
                #region Лог
                _logger.LogInformation($"Успешный вход в ситему Identity пользователя {model.UserName}");
                #endregion
                return LocalRedirect(model.ReturnUrl ?? "/");
            }

            ModelState.AddModelError("", "Ошибка в имени пользователя, либо в пароле при входе в систему Identity");
            #region Лог
            _logger.LogError($"Ошибка при входе пользователя {model.UserName}, либо в пароле при входе в систему Identity");
            #endregion

            return View();
        }

        #endregion

        #region Выход из системы

        public async Task<IActionResult> Logout(string returnUrl)
        {
            var username = User.Identity!.Name;
            await _signInManager.SignOutAsync();
            #region Лог
            _logger.LogInformation($"Выход пользователя {username} из системы Identity");
            #endregion
            return LocalRedirect(returnUrl ?? "/");
        }

        #endregion

        [AllowAnonymous]
        public IActionResult AccessDenied()
        {
            #region Лог
            _logger.LogError($"В доступе оказано");
            #endregion
            return View();
        }
    }
}
