using System;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using WebStore.Controllers;
using WebStore.Domain.Identity;
using WebStore.Domain.WebModels.Account;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace WebStore.Tests.Controllers
{
    [TestClass]
    public class AccountControllerTests
    {
        #region Регистрация пользователя

        [TestMethod]
        public void Register_SendCorrectRequest_ShouldView()
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
        public void RegisterModel_SendInvalidModelRequest_ShouldView()
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
        public void RegisterModelSucceeded_SendSuccessRequest_ShouldView()
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
                .Setup(_ => _.CreateAsync(It.IsAny<User>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);
            userManagerMock
                .Setup(_ => _.AddToRoleAsync(It.IsAny<User>(), It.IsAny<string>()));
            var signInManagerMock = new Mock<SignInManagerMock>();
            signInManagerMock
                .Setup(_ => _.SignInAsync(It.IsAny<User>(), It.IsAny<bool>(), null));
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

            Assert
                .IsInstanceOfType(result, typeof(RedirectToActionResult));
            var redirectResult = (RedirectToActionResult)result;
            Assert
                .AreEqual("Home", redirectResult.ControllerName);
            Assert
                .AreEqual(nameof(HomeController.Index), redirectResult.ActionName);
            userManagerMock
                .Verify(_ => _.CreateAsync(It.IsAny<User>(), It.IsAny<string>()));
            userManagerMock
                .Verify(_ => _.AddToRoleAsync(It.IsAny<User>(), It.IsAny<string>()));
            userManagerMock
                .Verify();
            signInManagerMock
                .Verify(_ => _.SignInAsync(It.IsAny<User>(), It.IsAny<bool>(), null));
            signInManagerMock
                .Verify();
        }

        [TestMethod]
        public void RegisterModelError_SendAddErrorRequest_ShouldView()
        {
            var expectedTypeError = string.Empty;
            var expectedName = "TestName";
            var expectedPassword = "123";
            var expectedErrorCode = "Произошла неизвестная ошибка";
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
                .Setup(_ => _.CreateAsync(It.IsAny<User>(), It.IsAny<string>()))
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
                .AreEqual(expectedErrorCode, returnErrors.FirstOrDefault().ErrorMessage);
            userManagerMock
                .Verify(_  => _.CreateAsync(It.IsAny<User>(), It.IsAny<string>()));
            userManagerMock
                .Verify();
        }

        #endregion

        #region Вход пользователей

        [TestMethod]
        public void Login_SendCorrectRequest_ShouldView()
        {
            const string expectedReturnUrl = "testUrl";
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

        [TestMethod]
        public void LoginModel_SendInvalidRequest_ShouldView()
        {
            const string expectedErrorCode = "Test";
            const string expectedErrorMessage = "Message";
            var loggerStub = Mock
                .Of<ILogger<AccountController>>();
            var expectedModel = new LoginWebModel();
            var controller = new AccountController(new UserManagerMock(), new SignInManagerMock(), loggerStub);
            controller.ModelState.AddModelError(expectedErrorCode, expectedErrorMessage);

            var result = controller.Login(expectedModel).Result;

            Assert
                .IsInstanceOfType(result, typeof(ViewResult));
            var viewResult = (ViewResult)result;
            Assert
                .IsInstanceOfType(viewResult.Model, typeof(LoginWebModel));
        }

        [TestMethod]
        public void LoginModel_SendReturnUrlRequest_ShouldRedirect()
        {
            const string expectedUserName = "TestUser";
            const string expectedPassword = "123";
            const string expectedReturnUrl = "testUrl";
            var loggerStub = Mock
                .Of<ILogger<AccountController>>();
            var signInManagerMock = new Mock<SignInManagerMock>();
            signInManagerMock
                .Setup(_ => _.PasswordSignInAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(),
                    It.IsAny<bool>()))
                .ReturnsAsync(SignInResult.Success);
            var model = new LoginWebModel
            {
                UserName = expectedUserName,
                Password = expectedPassword,
                ReturnUrl = expectedReturnUrl,
            };
            var controller = new AccountController(new UserManagerMock(), signInManagerMock.Object, loggerStub);

            var result = controller.Login(model).Result;

            Assert
                .IsInstanceOfType(result, typeof(LocalRedirectResult));
            var redirectResult = (LocalRedirectResult) result;
            Assert
                .AreEqual(expectedReturnUrl, redirectResult.Url);
            signInManagerMock
                .Verify(_ => _.PasswordSignInAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(),
                It.IsAny<bool>()));
            signInManagerMock
                .Verify();
        }

        [TestMethod]
        public void LoginModel_SendRootUrlRequest_ShouldRedirect()
        {
            const string expectedUserName = "TestUser";
            const string expectedPassword = "123";
            const string expectedReturnUrl = "/";
            var loggerStub = Mock
                .Of<ILogger<AccountController>>();
            var signInManagerMock = new Mock<SignInManagerMock>();
            signInManagerMock
                .Setup(_ => _.PasswordSignInAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(),
                    It.IsAny<bool>()))
                .ReturnsAsync(SignInResult.Success);
            var model = new LoginWebModel
            {
                UserName = expectedUserName,
                Password = expectedPassword,
            };
            var controller = new AccountController(new UserManagerMock(), signInManagerMock.Object, loggerStub);

            var result = controller.Login(model).Result;

            Assert
                .IsInstanceOfType(result, typeof(LocalRedirectResult));
            var redirectResult = (LocalRedirectResult) result;
            Assert
                .AreEqual(expectedReturnUrl, redirectResult.Url);
            signInManagerMock
                .Verify(_ => _.PasswordSignInAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(),
                It.IsAny<bool>()));
            signInManagerMock
                .Verify();
        }

        [TestMethod]
        public void LoginModel_SendFailedRequest_ShouldView()
        {
            var expectedErrorCode = "Ошибка в имени пользователя, либо в пароле при входе в систему Identity";
            const string expectedUserName = "TestUser";
            const string expectedPassword = "123";
            var loggerStub = Mock
                .Of<ILogger<AccountController>>();
            var signInManagerMock = new Mock<SignInManagerMock>();
            signInManagerMock
                .Setup(_ => _.PasswordSignInAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(),
                    It.IsAny<bool>()))
                .ReturnsAsync(SignInResult.Failed);
            var model = new LoginWebModel
            {
                UserName = expectedUserName,
                Password = expectedPassword,
            };
            var controller = new AccountController(new UserManagerMock(), signInManagerMock.Object, loggerStub);

            var result = controller.Login(model).Result;

            Assert
                .IsInstanceOfType(result, typeof(ViewResult));
            Assert
                .IsFalse(controller.ModelState.IsValid);
            var returnErrors = controller.ModelState[string.Empty].Errors;
            Assert
                .AreEqual(expectedErrorCode, returnErrors.FirstOrDefault().ErrorMessage);
            signInManagerMock
                .Verify(_ => _.PasswordSignInAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(),
                It.IsAny<bool>()));
            signInManagerMock
                .Verify();
        }

        #endregion

        #region Выход из системы и доступ отказан

        [TestMethod]
        public void LogoutReturnUrl_SendRequest_ShouldRedirect()
        {
            const string expectedName = "TestUser";
            const string expectedReturnUrl = "testUrl";
            var loggerStub = Mock
                .Of<ILogger<AccountController>>();
            var signInManagerMock = new Mock<SignInManagerMock>();
            signInManagerMock
                .Setup(_ => _.SignOutAsync());
            var controller = new AccountController(new UserManagerMock(), signInManagerMock.Object, loggerStub)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext
                    {
                        User = new ClaimsPrincipal(new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, expectedName) }))
                    }
                }
            };

            var result = controller.Logout(expectedReturnUrl).Result;

            Assert
                .IsInstanceOfType(result, typeof(LocalRedirectResult));
            var redirectResult = (LocalRedirectResult) result;
            Assert
                .AreEqual(expectedReturnUrl, redirectResult.Url);
            signInManagerMock
                .Verify(_ => _.SignOutAsync());
            signInManagerMock
                .Verify();
        }

        [TestMethod]
        public void Logout_SendRootRequest_ShouldRedirect()
        {
            const string expectedName = "TestUser";
            const string expectedReturnUrl = "/";
            var loggerStub = Mock
                .Of<ILogger<AccountController>>();
            var signInManagerMock = new Mock<SignInManagerMock>();
            signInManagerMock
                .Setup(_ => _.SignOutAsync());
            var controller = new AccountController(new UserManagerMock(), signInManagerMock.Object, loggerStub)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext
                    {
                        User = new ClaimsPrincipal(new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, expectedName) }))
                    }
                }
            };

            var result = controller.Logout(null).Result;

            Assert
                .IsInstanceOfType(result, typeof(LocalRedirectResult));
            var redirectResult = (LocalRedirectResult) result;
            Assert
                .AreEqual(expectedReturnUrl, redirectResult.Url);
            signInManagerMock
                .Verify(_ => _.SignOutAsync());
            signInManagerMock
                .Verify();
        }

        [TestMethod]
        public void AccessDenied_SendRequest_ShouldView()
        {
            var loggerStub = Mock
                .Of<ILogger<AccountController>>();
            var controller = new AccountController(new UserManagerMock(), new SignInManagerMock(), loggerStub);

            var result = controller.AccessDenied();

            Assert
                .IsInstanceOfType(result, typeof(ViewResult));
        }

        #endregion

        #region Тестирование WebAPI

        [TestMethod]
        public void IsNameFree_SendRequest_ShouldView()
        {
            var expectedName = "TestName";
            var loggerStub = Mock
                .Of<ILogger<AccountController>>();
            var userManagerMock = new Mock<UserManagerMock>();
            userManagerMock
                .Setup(_ => _.FindByNameAsync(It.IsAny<string>()))
                .ReturnsAsync((User)null);
            var controller = new AccountController(userManagerMock.Object, new SignInManagerMock(), loggerStub);

            var result = controller.IsNameFree(expectedName).Result;

            Assert
                .IsInstanceOfType(result, typeof(JsonResult));
            var json = (JsonResult)result;
            userManagerMock
                .Verify(_ => _.FindByNameAsync(expectedName), Times.Once);
            userManagerMock
                .Verify();
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
