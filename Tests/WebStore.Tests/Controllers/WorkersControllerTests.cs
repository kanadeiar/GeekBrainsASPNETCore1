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
        public void EditModelAndrey_Returns_ModelError()
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
                FirstName = "Ленин"
            };
            var controller = new WorkersController(workerDataStub, loggerStub);

            var result = controller.Edit(model).Result;

            Assert
                .IsInstanceOfType(result, typeof(ViewResult));
            var viewResult = (ViewResult)result;
            Assert
                .IsFalse(controller.ModelState.IsValid);
            var errors = controller.ModelState[expectedTypeError].Errors;
            Assert
                .AreEqual(expectedErrorsCount, errors.Count);
            Assert
                .AreEqual(expectedErrorMessage, errors.FirstOrDefault().ErrorMessage);
        }

        #endregion
    }
}
