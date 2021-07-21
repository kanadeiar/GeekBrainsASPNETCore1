using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebStore.Domain.DTO;
using WebStore.Domain.Entities;
using WebStore.Interfaces.Services;
using WebStore.WebAPI.Controllers;

namespace WebStore.WebAPI.Tests.Controllers
{
    [TestClass]
    public class ProductApiConrollerTests
    {
        [TestMethod]
        public void GetSections_Returns_Correct()
        {
            const int expectedId = 1;
            const string expectedName = "TestSection";

            var productDataMock = new Mock<IProductData>();
            productDataMock
                .Setup(p => p.GetSections())
                .ReturnsAsync(() =>
                {
                    Task.Delay(1000).Wait();
                    return new[]
                    {
                        new Section
                        {
                            Id = expectedId,
                            Name = expectedName,
                            Products = Array.Empty<Product>(),
                        },
                    };
                });
            var controller = new ProductApiController(productDataMock.Object);

            var result = controller.GetSections().Result;
            Assert
                .IsInstanceOfType(result, typeof(OkObjectResult));
            var viewResult = (OkObjectResult) result;
            Assert
                .IsInstanceOfType(viewResult.Value, typeof(IEnumerable<SectionDTO>));
            var sections = (IEnumerable<SectionDTO>)viewResult.Value;
            Assert
                .AreEqual(expectedId, sections.FirstOrDefault().Id);
            Assert
                .AreEqual(expectedName, sections.FirstOrDefault().Name);
            productDataMock
                .Verify(p => p.GetSections(), Times.Once);
            productDataMock
                .VerifyNoOtherCalls();
        }
    }
}
