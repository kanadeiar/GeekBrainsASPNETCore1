using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebStore.Domain.Identity;
using WebStore.Domain.Identity.ErrorCodes;
using WebStore.WebModels;

namespace WebStore.Controllers
{
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

        public IActionResult Register() => View(new RegisterWebModel());
        [HttpPost]
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

                _logger.LogInformation($"Ползователь {user.UserName} успешно зарегистрирован");

                #endregion
                await _userManager.AddToRoleAsync(user, Role.Clients);
                #region Лог

                _logger.LogInformation($"Пользователю {user.UserName} автоматически назначена роль {Role.Clients}");

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

            _logger.LogError($"Ошибки при регистрации пользователя {user.UserName} в систему: " +
                             $"{string.Join(",", errors)}");

            #endregion
            return View(model);
        }

        #endregion

        #region Вход в систему

        public IActionResult Login(string returnUrl) => View(new LoginWebModel{ReturnUrl = returnUrl});

        [HttpPost, ValidateAntiForgeryToken]
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
                return LocalRedirect(model.ReturnUrl ?? "/");

            ModelState.AddModelError("", "Ошибка в имени пользователя, либо в пароле");

            return View();
        }


        #endregion

        public IActionResult Logout()
        {
            return RedirectToAction("Index", "Home");
        }

        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
