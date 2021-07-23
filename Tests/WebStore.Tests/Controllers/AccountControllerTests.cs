using System;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using WebStore.Controllers;
using WebStore.Domain.Identity;
using WebStore.Domain.WebModels.Account;

namespace WebStore.Tests.Controllers
{
    [TestClass]
    public class AccountControllerTests
    {
        #region Регистрация пользователя

        [TestMethod]
        public void Register_Returns_Correct()
        {
            var loggerStub = Mock
                .Of<ILogger<AccountController>>();
            var controller = new AccountController(new UserManagerMock(), new SignInManagerMock(), loggerStub);

            var result = controller.Register();

            Assert
                .IsInstanceOfType(result, typeof(ViewResult));
            var viewResult = (ViewResult)result;
            Assert
                .IsInstanceOfType(viewResult.Model, typeof(RegisterWebModel));
        }

        [TestMethod]
        public void RegisterModelInvalid_Returns_View()
        {
            var expectedName = "TestName";
            var expectedPassword = "123";
            var loggerStub = Mock
                .Of<ILogger<AccountController>>();
            var model = new RegisterWebModel
            {
                UserName = expectedName,
                Password = expectedPassword,
                PasswordConfirm = expectedPassword,
            };
            var controller = new AccountController(new UserManagerMock(), new SignInManagerMock(), loggerStub);
            controller.ModelState.AddModelError("error", "InvalidError");

            var result = controller.Register(model).Result;

            Assert
                .IsInstanceOfType(result, typeof(ViewResult));
            var viewResult = (ViewResult)result;
            Assert
                .IsInstanceOfType(viewResult.Model, typeof(RegisterWebModel));
        }

        [TestMethod]
        public void RegisterModelSucceeded_Returns_Correct()
        {
            var expectedName = "TestName";
            var expectedPassword = "123";
            var loggerStub = Mock
                .Of<ILogger<AccountController>>();
            var model = new RegisterWebModel
            {
                UserName = expectedName,
                Password = expectedPassword,
                PasswordConfirm = expectedPassword,
            };
            var userManagerMock = new Mock<UserManagerMock>();
            userManagerMock
                .Setup(u => u.CreateAsync(It.IsAny<User>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);
            userManagerMock
                .Setup(u => u.AddToRoleAsync(It.IsAny<User>(), It.IsAny<string>()));
            var signInManagerMock = new Mock<SignInManagerMock>();
            signInManagerMock
                .Setup(s => s.SignInAsync(It.IsAny<User>(), It.IsAny<bool>(), null));
            var controller = new AccountController(userManagerMock.Object, signInManagerMock.Object, loggerStub)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext
                    {
                        User = new ClaimsPrincipal(new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, expectedName) }))
                    }
                }
            };

            var result = controller.Register(model).Result;

            Assert.IsInstanceOfType(result, typeof(RedirectToActionResult));
            var redirectResult = (RedirectToActionResult)result;
            Assert.AreEqual("Home", redirectResult.ControllerName);
            Assert.AreEqual(nameof(HomeController.Index), redirectResult.ActionName);
            userManagerMock.Verify(u => u.CreateAsync(It.IsAny<User>(), It.IsAny<string>()));
            userManagerMock.Verify(u => u.AddToRoleAsync(It.IsAny<User>(), It.IsAny<string>()));
            userManagerMock.Verify();
            signInManagerMock.Verify(s => s.SignInAsync(It.IsAny<User>(), It.IsAny<bool>(), null));
            signInManagerMock.Verify();
        }

        [TestMethod]
        public void RegisterModelError_Returns_View()
        {
            var expectedTypeError = string.Empty;
            var expectedName = "TestName";
            var expectedPassword = "123";
            var expectedErrorCode = "test";
            var expectedErrorDescription = "TestDescription";
            var loggerStub = Mock
                .Of<ILogger<AccountController>>();
            var model = new RegisterWebModel
            {
                UserName = expectedName,
                Password = expectedPassword,
                PasswordConfirm = expectedPassword,
            };
            var userManagerMock = new Mock<UserManagerMock>();
            var errors = new[]
            {
                new IdentityError {Code = expectedErrorCode, Description = expectedErrorDescription}
            };
            userManagerMock
                .Setup(u => u.CreateAsync(It.IsAny<User>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Failed(errors));
            var controller = new AccountController(userManagerMock.Object, new SignInManagerMock(), loggerStub)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext
                    {
                        User = new ClaimsPrincipal(new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, expectedName) }))
                    }
                }
            };

            var result = controller.Register(model).Result;

            Assert.IsInstanceOfType(result, typeof(ViewResult));
            Assert
                .IsFalse(controller.ModelState.IsValid);
            var returnErrors = controller.ModelState[expectedTypeError].Errors;
            Assert
                .AreEqual(expectedErrorCode, errors.FirstOrDefault().Code);
            Assert
                .AreEqual(expectedErrorDescription, errors.FirstOrDefault().Description);
        }

        #endregion

        #region Вход пользователей

        [TestMethod]
        public void Login_Returns_Correct()
        {
            var expectedReturnUrl = "testUrl";
            var loggerStub = Mock
                .Of<ILogger<AccountController>>();
            var controller = new AccountController(new UserManagerMock(), new SignInManagerMock(), loggerStub);

            var result = controller.Login(expectedReturnUrl);

            Assert
                .IsInstanceOfType(result, typeof(ViewResult));
            var viewResult = (ViewResult)result;
            Assert
                .IsInstanceOfType(viewResult.Model, typeof(LoginWebModel));
            var loginModel = (LoginWebModel) viewResult.Model;
            Assert.AreEqual(expectedReturnUrl, loginModel.ReturnUrl);
        }



        #endregion
    }

    

    #region Моки подмены настоящих классов

    public class UserManagerMock : UserManager<User>
    {
        /// <summary> Мок подменяющий сервис управления пользователями </summary>
        public UserManagerMock()
            : base(new Mock<IUserStore<User>>().Object,
                new Mock<IOptions<IdentityOptions>>().Object,
                new Mock<IPasswordHasher<User>>().Object,
                new IUserValidator<User>[0],
                new IPasswordValidator<User>[0],
                new Mock<ILookupNormalizer>().Object,
                new Mock<IdentityErrorDescriber>().Object,
                new Mock<IServiceProvider>().Object,
                new Mock<ILogger<UserManager<User>>>().Object)
        {
        }
    }

    public class SignInManagerMock : SignInManager<User>
    {
        /// <summary> Мок подменяющий вход/выход пользователей </summary>
        public SignInManagerMock()
            : base(new Mock<UserManagerMock>().Object,
                new HttpContextAccessor(),
                new Mock<IUserClaimsPrincipalFactory<User>>().Object,
                new Mock<IOptions<IdentityOptions>>().Object,
                new Mock<ILogger<SignInManager<User>>>().Object,
                new Mock<IAuthenticationSchemeProvider>().Object,
                new Mock<IUserConfirmation<User>>().Object)
        {
        }
    }
    #endregion
}
