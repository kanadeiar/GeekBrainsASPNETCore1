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
            _logger.LogInformation("Регистрация нового пользователя {0}", model.UserName);
            #endregion
            var user = new User
            {
                UserName = model.UserName,
            };
            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                #region Лог
                _logger.LogInformation("Пользователь {0} успешно зарегистрирован", user.UserName);
                #endregion
                await _userManager.AddToRoleAsync(user, Role.Users);
                #region Лог
                _logger.LogInformation("Пользователю {0} автоматически назначена роль {1}", user.UserName, Role.Users);
                #endregion
                await _signInManager.SignInAsync(user, false);
                #region Лог
                _logger.LogInformation("Пользователь {0} автоматически вошел в систему после регистрации", user.UserName);
                #endregion
                return RedirectToAction("Index", "Home");
            }

            var errors = result.Errors.Select(e => IdentityErrorCodes.GetDescription(e.Code)).ToArray();
            foreach (var error in errors)
            {
                ModelState.AddModelError("", error);
            }

            #region Лог
            _logger.LogError("{0} Ошибки при регистрации пользователя {1} в систему: {2}", DateTime.Now, user.UserName, string.Join(",", errors));
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
                _logger.LogInformation("Успешный вход в ситему Identity пользователя {0}", model.UserName);
                #endregion
                return LocalRedirect(model.ReturnUrl ?? "/");
            }

            ModelState.AddModelError("", "Ошибка в имени пользователя, либо в пароле при входе в систему Identity");
            #region Лог
            _logger.LogError("Ошибка при входе пользователя {0}, либо в пароле при входе в систему Identity", model.UserName);
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
            _logger.LogInformation("Выход пользователя {0} из системы Identity", username);
            #endregion
            return LocalRedirect(returnUrl ?? "/");
        }

        #endregion

        #region Тестирование WebAPI

        [AllowAnonymous]
        public async Task<IActionResult> IsNameFree(string UserName)
        {
            await Task.Delay(1000);
            var user = await _userManager.FindByNameAsync(UserName);
            return Json(user is null ? "true" : "Пользователь с таким имененем уже существует");
        }

        #endregion

        [AllowAnonymous]
        public IActionResult AccessDenied()
        {
            #region Лог
            _logger.LogError("В доступе оказано");
            #endregion
            return View();
        }
    }
}
