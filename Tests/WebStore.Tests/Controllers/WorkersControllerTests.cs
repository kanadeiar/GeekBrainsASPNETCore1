using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using WebStore.Controllers;
using WebStore.Domain.Entities;
using WebStore.Domain.Models;
using WebStore.Domain.WebModels;
using WebStore.Interfaces.Services;

namespace WebStore.Tests.Controllers
{
    [TestClass]
    public class WorkersControllerTests
    {
        #region Отображение работников

        [TestMethod]
        public void Index_Returns_Correct()
        {
            const int expectedId = 1;
            const string expectedFam = "Иванов";
            const string expectedName = "Иван";
            const int expectedCount = 3;
            var workerDataMock = new Mock<IWorkerData>();
            workerDataMock
                .Setup(s => s.GetAll())
                .ReturnsAsync(() =>
                {
                    return Enumerable.Range(1, expectedCount)
                        .Select(i => new Worker
                        {
                            Id = i,
                            LastName = expectedFam,
                            FirstName = expectedName,
                        });
                });
            var loggerStub = Mock
                .Of<ILogger<WorkersController>>();
            var controller = new WorkersController(workerDataMock.Object, loggerStub);

            var result = controller.Index().Result;

            Assert
                .IsInstanceOfType(result, typeof(ViewResult));
            var viewResult = (ViewResult) result;
            Assert
                .IsInstanceOfType(viewResult.Model, typeof(IEnumerable<Worker>));
            var workers = (IEnumerable<Worker>) viewResult.Model;
            Assert
                .AreEqual(expectedCount, workers.Count());
            Assert
                .AreEqual(expectedId, workers.FirstOrDefault().Id);
            Assert
                .AreEqual(expectedFam, workers.FirstOrDefault().LastName);
            Assert
                .AreEqual(expectedName, workers.FirstOrDefault().FirstName);
            workerDataMock
                .Verify(s => s.GetAll(), Times.Once);
            workerDataMock
                .VerifyNoOtherCalls();
        }

        #endregion

        #region Детальное отображение работника

        [TestMethod]
        public void DetailsMinus_Return_BadRequest()
        {
            const int expectedId = -1;
            var workerDataStub = Mock
                .Of<IWorkerData>();
            var loggerStub = Mock
                .Of<ILogger<WorkersController>>();
            var controller = new WorkersController(workerDataStub, loggerStub);

            var result = controller.Details(expectedId).Result;

            Assert
                .IsInstanceOfType(result, typeof(BadRequestResult));
        }

        [TestMethod]
        public void DetailsNull_Return_NotFound()
        {
            const int expectedId = 1;
            var workerDataMock = new Mock<IWorkerData>();
            workerDataMock
                .Setup(w => w.Get(It.IsAny<int>()))
                .ReturnsAsync((Worker) null);
            var loggerStub = Mock
                .Of<ILogger<WorkersController>>();
            var controller = new WorkersController(workerDataMock.Object, loggerStub);

            var result = controller.Details(expectedId).Result;

            Assert
                .IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod]
        public void Details_Return_Correct()
        {
            const int expectedId = 1;
            const string expectedFam = "Иванов";
            const string expectedName = "Иван";
            var workerDataMock = new Mock<IWorkerData>();
            workerDataMock
                .Setup(w => w.Get(It.IsAny<int>()))
                .ReturnsAsync((int id) =>
                {
                    Task.Delay(1000).Wait();
                    return new Worker
                    {
                        Id = id,
                        LastName = expectedFam,
                        FirstName = expectedName,
                    };
                });
            var loggerStub = Mock
                .Of<ILogger<WorkersController>>();
            var controller = new WorkersController(workerDataMock.Object, loggerStub);

            var result = controller.Details(expectedId).Result;

            Assert
                .IsInstanceOfType(result, typeof(ViewResult));
            var viewResult = (ViewResult) result;
            Assert
                .IsInstanceOfType(viewResult.Model, typeof(EditWorkerWebModel));
            var workerModel = (EditWorkerWebModel) viewResult.Model;
            Assert.AreEqual(expectedId, workerModel.Id);
            Assert.AreEqual(expectedFam, workerModel.LastName);
            Assert.AreEqual(expectedName, workerModel.FirstName);
            workerDataMock.Verify(s => s.Get(expectedId), Times.Once);
            workerDataMock.VerifyNoOtherCalls();
        }

        #endregion

        #region Отображение начальной фазы редактирования работника

        [TestMethod]
        public void EditIdIdNull_Returns_Correct()
        {
            int? id = null;
            var workerDataStub = Mock
                .Of<IWorkerData>();
            var loggerStub = Mock
                .Of<ILogger<WorkersController>>();
            var controller = new WorkersController(workerDataStub, loggerStub);

            var result = controller.Edit(id).Result;

            Assert
                .IsInstanceOfType(result, typeof(ViewResult));
            var viewResult = (ViewResult) result;
            Assert
                .IsInstanceOfType(viewResult.Model, typeof(EditWorkerWebModel));
            var workerModel = (EditWorkerWebModel) viewResult.Model;
            Assert
                .IsNull(workerModel.LastName);
            Assert
                .IsNull(workerModel.FirstName);
        }

        [TestMethod]
        public void EditIdMinus_Returns_BadRequest()
        {
            int? id = -1;
            var workerDataStub = Mock
                .Of<IWorkerData>();
            var loggerStub = Mock
                .Of<ILogger<WorkersController>>();
            var controller = new WorkersController(workerDataStub, loggerStub);

            var result = controller.Edit(id).Result;

            Assert
                .IsInstanceOfType(result, typeof(BadRequestResult));
        }

        [TestMethod]
        public void EditIdWorkerNull_Returns_NotFound()
        {
            int? id = 1;
            var workerDataMock = new Mock<IWorkerData>();
            workerDataMock
                .Setup(w => w.Get(It.IsAny<int>()))
                .ReturnsAsync((Worker) null);
            var loggerStub = Mock
                .Of<ILogger<WorkersController>>();
            var controller = new WorkersController(workerDataMock.Object, loggerStub);
            
            var result = controller.Edit(id).Result;

            Assert
                .IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod]
        public void EditId_Returns_Correct()
        {
            const int expectedId = 1;
            const string expectedFam = "Иванов";
            const string expectedName = "Иван";
            var workerDataMock = new Mock<IWorkerData>();
            workerDataMock
                .Setup(w => w.Get(It.IsAny<int>()))
                .ReturnsAsync((int id) =>
                {
                    Task.Delay(1000).Wait();
                    return new Worker
                    {
                        Id = id,
                        LastName = expectedFam,
                        FirstName = expectedName,
                    };
                });
            var loggerStub = Mock
                .Of<ILogger<WorkersController>>();
            var controller = new WorkersController(workerDataMock.Object, loggerStub);

            var result = controller.Edit(expectedId).Result;

            Assert
                .IsInstanceOfType(result, typeof(ViewResult));
            var viewResult = (ViewResult) result;
            Assert
                .IsInstanceOfType(viewResult.Model, typeof(EditWorkerWebModel));
            var workerModel = (EditWorkerWebModel) viewResult.Model;
            Assert
                .AreEqual(expectedId, workerModel.Id);
            Assert
                .AreEqual(expectedFam, workerModel.LastName);
            Assert
                .AreEqual(expectedName, workerModel.FirstName);
            workerDataMock
                .Verify(s => s.Get(expectedId), Times.Once);
            workerDataMock
                .VerifyNoOtherCalls();
        }

        #endregion

        #region Отображение завершающей фазы редактирования работника

        [TestMethod]
        public void EditModelNull_Returns_BadRequest()
        {            
            var workerDataStub = Mock
                .Of<IWorkerData>();
            var loggerStub = Mock
                .Of<ILogger<WorkersController>>();
            EditWorkerWebModel model = null;
            var controller = new WorkersController(workerDataStub, loggerStub);

            var result = controller.Edit(model).Result;

            Assert
                .IsInstanceOfType(result, typeof(BadRequestResult));
        }

        [TestMethod]
        public void EditModelLenin_Returns_ModelError()
        {
            var expectedTypeError = nameof(EditWorkerWebModel.FirstName);
            var expectedErrorsCount = 1;
            var expectedErrorMessage = "Ленин - плохое имя для работника!";
            var workerDataStub = Mock
                .Of<IWorkerData>();
            var loggerStub = Mock
                .Of<ILogger<WorkersController>>();
            var model = new EditWorkerWebModel
            {
                FirstName = "Ленин",
            };
            var controller = new WorkersController(workerDataStub, loggerStub);

            var result = controller.Edit(model).Result;

            Assert
                .IsInstanceOfType(result, typeof(ViewResult));
            Assert
                .IsFalse(controller.ModelState.IsValid);
            var errors = controller.ModelState[expectedTypeError].Errors;
            Assert
                .AreEqual(expectedErrorsCount, errors.Count);
            Assert
                .AreEqual(expectedErrorMessage, errors.FirstOrDefault().ErrorMessage);
        }

        [TestMethod]
        public void EditModelVladimirPutin_Returns_ModelError()
        {
            var expectedTypeError = string.Empty;
            var expectedErrorsCount = 1;
            var expectedErrorMessage = "Нельзя иметь фамилию & имя тестового работника Владимир & Путин!";
            var workerDataStub = Mock
                .Of<IWorkerData>();
            var loggerStub = Mock
                .Of<ILogger<WorkersController>>();
            var model = new EditWorkerWebModel
            {
                LastName = "Путин",
                FirstName = "Владимир",
            };
            var controller = new WorkersController(workerDataStub, loggerStub);

            var result = controller.Edit(model).Result;

            Assert
                .IsInstanceOfType(result, typeof(ViewResult));
            Assert
                .IsFalse(controller.ModelState.IsValid);
            var errors = controller.ModelState[expectedTypeError].Errors;
            Assert
                .AreEqual(expectedErrorsCount, errors.Count);
            Assert
                .AreEqual(expectedErrorMessage, errors.FirstOrDefault().ErrorMessage);
        }

        [TestMethod]
        public void EditModelId0_Add_And_Returns_Correct()
        {
            const int expectedId = 0;
            const string expectedFam = "Иванов";
            const string expectedName = "Иван";
            var workerDataMock = new Mock<IWorkerData>();
            workerDataMock
                .Setup(w => w.Add(It.IsAny<Worker>()))
                .ReturnsAsync((Worker w) => 
                { 
                    return w.Id; 
                });
            var loggerStub = Mock
                .Of<ILogger<WorkersController>>();
            var model = new EditWorkerWebModel
            {
                Id = expectedId,
                LastName = expectedFam,
                FirstName = expectedName,
            };
            var controller = new WorkersController(workerDataMock.Object, loggerStub);

            var result = controller.Edit(model).Result;

            Assert
                .IsInstanceOfType(result, typeof(RedirectToActionResult));
            var redirectResult = (RedirectToActionResult)result;
            Assert
                .AreEqual(nameof(WorkersController.Index), redirectResult.ActionName);
            Assert
                .IsNull(redirectResult.ControllerName);
            workerDataMock
                .Verify(w => w.Add(It.IsAny<Worker>()));
            workerDataMock
                .VerifyNoOtherCalls();
        }

        [TestMethod]
        public void EditModelId1_Update_And_Returns_Correct()
        {
            const int expectedId = 1;
            const string expectedFam = "Иванов";
            const string expectedName = "Иван";
            string callbackFam = default;
            string callbackName = default;
            var workerDataMock = new Mock<IWorkerData>();
            workerDataMock
                .Setup(w => w.Update(It.IsAny<Worker>()))
                .Callback((Worker w) =>
                {
                    callbackFam = expectedFam;
                    callbackName = expectedName;
                });
            var loggerStub = Mock
                .Of<ILogger<WorkersController>>();
            var model = new EditWorkerWebModel
            {
                Id = expectedId,
                LastName = expectedFam,
                FirstName = expectedName,
            };
            var controller = new WorkersController(workerDataMock.Object, loggerStub);

            var result = controller.Edit(model).Result;

            Assert
                .IsInstanceOfType(result, typeof(RedirectToActionResult));
            var redirectResult = (RedirectToActionResult)result;
            Assert
                .AreEqual(nameof(WorkersController.Index), redirectResult.ActionName);
            Assert
                .IsNull(redirectResult.ControllerName);
            Assert
                .AreEqual(expectedFam, callbackFam);
            Assert
                .AreEqual(expectedName, callbackName);
            workerDataMock
                .Verify(w => w.Update(It.IsAny<Worker>()));
            workerDataMock
                .VerifyNoOtherCalls();
        }

        #endregion

        #region Отображение начальной фазы удаления работника

        [TestMethod]
        public void DeleteIdMinus_Returns_BadRequest()
        {
            const int id = -1;
            var workerDataStub = Mock
                .Of<IWorkerData>();
            var loggerStub = Mock
                .Of<ILogger<WorkersController>>();
            var controller = new WorkersController(workerDataStub, loggerStub);

            var result = controller.Delete(id).Result;

            Assert
                .IsInstanceOfType(result, typeof(BadRequestResult));
        }

        [TestMethod]
        public void DeleteIdNull_Returns_NotFound()
        {
            const int id = 1;
            var workerDataMock = new Mock<IWorkerData>();
            workerDataMock
                .Setup(w => w.Get(It.IsAny<int>()))
                .ReturnsAsync((Worker) null);
            var loggerStub = Mock
                .Of<ILogger<WorkersController>>();
            var controller = new WorkersController(workerDataMock.Object, loggerStub);

            var result = controller.Delete(id).Result;

            Assert
                .IsInstanceOfType(result, typeof(NotFoundResult));
            workerDataMock
                .Verify(w => w.Get(It.IsAny<int>()));
            workerDataMock
                .VerifyNoOtherCalls();
        }

        [TestMethod]
        public void DeleteId_Returns_Correct()
        {
            const int expectedId = 1;
            const string expectedFam = "Иванов";
            const string expectedName = "Иван";
            var workerDataMock = new Mock<IWorkerData>();
            workerDataMock
                .Setup(w => w.Get(It.IsAny<int>()))
                .ReturnsAsync((int id) =>
                {
                    Task.Delay(1000).Wait();
                    return new Worker
                    {
                        Id = id,
                        LastName = expectedFam,
                        FirstName = expectedName,
                    };
                });
            var loggerStub = Mock
                .Of<ILogger<WorkersController>>();
            var controller = new WorkersController(workerDataMock.Object, loggerStub);

            var result = controller.Delete(expectedId).Result;

            Assert
                .IsInstanceOfType(result, typeof(ViewResult));
            var viewResult = (ViewResult) result;
            Assert
                .IsInstanceOfType(viewResult.Model, typeof(EditWorkerWebModel));
            var workerModel = (EditWorkerWebModel) viewResult.Model;
            Assert
                .AreEqual(expectedId, workerModel.Id);
            Assert
                .AreEqual(expectedFam, workerModel.LastName);
            Assert
                .AreEqual(expectedName, workerModel.FirstName);
            workerDataMock
                .Verify(s => s.Get(expectedId), Times.Once);
            workerDataMock
                .VerifyNoOtherCalls();
        }

        #endregion

        #region Отображение завершающей фазы удаления работника

        [TestMethod]
        public void DeleteConfirmedMinus1_Returns_Correct()
        {
            int id = -1;
            var workerDataStub = Mock
                .Of<IWorkerData>();
            var loggerStub = Mock
                .Of<ILogger<WorkersController>>();
            var controller = new WorkersController(workerDataStub, loggerStub);

            var result = controller.DeleteConfirmed(id).Result;

            Assert
                .IsInstanceOfType(result, typeof(BadRequestResult));
        }

        [TestMethod]
        public void DeleteConfirmedFalse_Returns_BadRequest()
        {
            int id = 1;
            var workerDataMock = new Mock<IWorkerData>();
            workerDataMock
                .Setup(w => w.Delete(1))
                .ReturnsAsync(false);
            var loggerStub = Mock
                .Of<ILogger<WorkersController>>();
            var controller = new WorkersController(workerDataMock.Object, loggerStub);

            var result = controller.DeleteConfirmed(id).Result;

            Assert
                .IsInstanceOfType(result, typeof(BadRequestResult));
            workerDataMock
                .Verify(w => w.Delete(1));
            workerDataMock
                .VerifyNoOtherCalls();
        }

        [TestMethod]
        public void DeleteConfirmed_Returns_Correct()
        {
            int id = 1;
            var workerDataMock = new Mock<IWorkerData>();
            workerDataMock
                .Setup(w => w.Delete(1))
                .ReturnsAsync(true);
            var loggerStub = Mock
                .Of<ILogger<WorkersController>>();
            var controller = new WorkersController(workerDataMock.Object, loggerStub);

            var result = controller.DeleteConfirmed(id).Result;

            Assert
                .IsInstanceOfType(result, typeof(RedirectToActionResult));
            var redirectResult = (RedirectToActionResult)result;
            Assert
                .AreEqual(nameof(WorkersController.Index), redirectResult.ActionName);
            Assert
                .IsNull(redirectResult.ControllerName);
            workerDataMock
                .Verify(w => w.Delete(1));
            workerDataMock
                .VerifyNoOtherCalls();
        }

        #endregion
    }
}
