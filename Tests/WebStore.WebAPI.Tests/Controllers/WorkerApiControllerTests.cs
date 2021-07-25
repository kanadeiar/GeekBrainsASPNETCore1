using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using WebStore.Domain.Entities;
using WebStore.Interfaces.Services;
using WebStore.WebAPI.Controllers;

namespace WebStore.WebAPI.Tests.Controllers
{
    [TestClass]
    public class WorkerApiControllerTests
    {
        #region Тестирование веб апи контроллера работников

        [TestMethod]
        public void GetAll_SendRequest_ShouldOkObject()
        {
            const int expectedId = 1;
            const string expectedFam = "Ivanov";
            const string expectedName = "Ivan";
            var workerDataMock = new Mock<IWorkerData>();
            workerDataMock
                .Setup(_ => _.GetAll())
                .ReturnsAsync(() =>
                {
                    Task.Delay(1000).Wait();
                    return new[]
                    {
                        new Worker
                        {
                            Id = expectedId,
                            LastName = expectedFam,
                            FirstName = expectedName,
                        },
                    };
                });
            var controller = new WorkerApiController(workerDataMock.Object);

            var result = controller.GetAll().Result;

            Assert
                .IsInstanceOfType(result, typeof(OkObjectResult));
            var objResult = (OkObjectResult) result;
            Assert
                .IsInstanceOfType(objResult.Value, typeof(IEnumerable<Worker>));
            var workers = (IEnumerable<Worker>)objResult.Value;
            Assert
                .AreEqual(expectedId, workers.FirstOrDefault().Id);
            Assert
                .AreEqual(expectedFam, workers.FirstOrDefault().LastName);
            Assert
                .AreEqual(expectedName, workers.FirstOrDefault().FirstName);
            workerDataMock
                .Verify(_ => _.GetAll(), Times.Once);
            workerDataMock
                .VerifyNoOtherCalls();
        }

        [TestMethod]
        public void GetById_SendRequest_ShouldOkObject()
        {
            const int expectedId = 1;
            const string expectedFam = "Ivanov";
            const string expectedName = "Ivan";
            var workerDataMock = new Mock<IWorkerData>();
            workerDataMock
                .Setup(_ => _.Get(It.IsAny<int>()))
                .ReturnsAsync((int i) =>
                {
                    Task.Delay(1000).Wait();
                    return new Worker
                    {
                        Id = i,
                        LastName = expectedFam,
                        FirstName = expectedName,
                    };
                });
            var controller = new WorkerApiController(workerDataMock.Object);

            var result = controller.GetById(expectedId).Result;

            Assert
                .IsInstanceOfType(result, typeof(OkObjectResult));
            var objResult = (OkObjectResult) result;
            Assert
                .IsInstanceOfType(objResult.Value, typeof(Worker));
            var worker = (Worker)objResult.Value;
            Assert
                .AreEqual(expectedId, worker.Id);
            Assert
                .AreEqual(expectedFam, worker.LastName);
            Assert
                .AreEqual(expectedName, worker.FirstName);
            workerDataMock
                .Verify(_ => _.Get(It.IsAny<int>()), Times.Once);
            workerDataMock
                .VerifyNoOtherCalls();
        }

        [TestMethod]
        public void Add_SendRequest_ShouldOkObject()
        {
            const int expectedId = 1;
            const string fam = "Ivanov";
            const string name = "Ivan";
            var workerDataMock = new Mock<IWorkerData>();
            workerDataMock
                .Setup(_ => _.Add(It.IsAny<Worker>()))
                .ReturnsAsync((Worker w) =>
                {
                    Task.Delay(1000).Wait();
                    return w.Id;
                });
            var worker = new Worker
            {
                Id = expectedId,
                LastName = fam,
                FirstName = name,
            };
            var controller = new WorkerApiController(workerDataMock.Object);
            
            var result = controller.Add(worker).Result;

            Assert
                .IsInstanceOfType(result, typeof(OkObjectResult));
            var objResult = (OkObjectResult) result;
            Assert
                .AreEqual(expectedId, (int)objResult.Value);
            workerDataMock
                .Verify(_ => _.Add(It.IsAny<Worker>()), Times.Once);
            workerDataMock
                .VerifyNoOtherCalls();
        }

        [TestMethod]
        public void Update_SendRequest_ShouldOk()
        {
            const int expectedId = 1;
            const string expectedNewFam = "Ivanov";
            const string expectedNewName = "Ivan";
            string callbackNewFam = default;
            string callbackNewName = default;
            var workerDataMock = new Mock<IWorkerData>();
            workerDataMock
                .Setup(_ => _.Update(It.IsAny<Worker>()))
                .Callback((Worker p) =>
                {
                    Task.Delay(1000).Wait();
                    callbackNewFam = p.LastName;
                    callbackNewName = p.FirstName;
                });
            var worker = new Worker()
            {
                Id = expectedId,
                LastName = expectedNewFam,
                FirstName = expectedNewName,
            };
            var controller = new WorkerApiController(workerDataMock.Object);

            var result = controller.Update(worker).Result;

            Assert
                .IsInstanceOfType(result, typeof(OkResult));
            Assert
                .AreEqual(expectedNewFam, callbackNewFam);
            Assert
                .AreEqual(expectedNewName, callbackNewName);
            workerDataMock
                .Verify(_ => _.Update(It.IsAny<Worker>()));
            workerDataMock
                .VerifyNoOtherCalls();
        }

        [TestMethod]
        public void DeleteOk_SendRequest_ShouldOkObject()
        {
            const int expectedId = 1;
            const bool expectedValue = true;
            var workerDataMock = new Mock<IWorkerData>();
            workerDataMock
                .Setup(_ => _.Delete(expectedId))
                .ReturnsAsync(true);
            var controller = new WorkerApiController(workerDataMock.Object);

            var result = controller.Delete(expectedId).Result;

            Assert
                .IsInstanceOfType(result, typeof(OkObjectResult));
            var objResult = (OkObjectResult) result;
            Assert
                .IsInstanceOfType(objResult.Value, typeof(bool));
            Assert
                .AreEqual(expectedValue, (bool)objResult.Value);
            workerDataMock
                .Verify(_ => _.Delete(expectedId));
            workerDataMock
                .VerifyNoOtherCalls();
        }

        [TestMethod]
        public void DeleteFailed_SendFakeRequest_ShouldNotFound()
        {
            const int expectedId = 1;
            const bool expectedValue = false;
            var workerDataMock = new Mock<IWorkerData>();
            workerDataMock
                .Setup(_ => _.Delete(expectedId))
                .ReturnsAsync(false);
            var controller = new WorkerApiController(workerDataMock.Object);

            var result = controller.Delete(expectedId).Result;

            Assert
                .IsInstanceOfType(result, typeof(NotFoundObjectResult));
            var objResult = (NotFoundObjectResult) result;
            Assert
                .IsInstanceOfType(objResult.Value, typeof(bool));
            Assert
                .AreEqual(expectedValue, (bool)objResult.Value);
            workerDataMock
                .Verify(_ => _.Delete(expectedId));
            workerDataMock
                .VerifyNoOtherCalls();
        }

        #endregion
    }
}
