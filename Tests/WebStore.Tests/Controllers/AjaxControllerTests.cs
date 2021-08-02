using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WebStore.Controllers;

namespace WebStore.Tests.Controllers
{
    [TestClass]
    public class AjaxControllerTests
    {
        [TestMethod]
        public void GetHtml_SendRequest_ShouldPartialView()
        {
            int expectedId = 1;
            string expectedMessage = "Message";
            var controller = new AjaxController();

            var result = controller.GetHtml(expectedId, expectedMessage).Result;

            Assert
                .IsInstanceOfType(result, typeof(PartialViewResult));
            var partialView = (PartialViewResult)result;
            Assert
                .AreEqual("Partial/_AjaxDataViewPartial", partialView.ViewName);
        }

        [TestMethod]
        public void GetJson_SendRequest_ShouldJson()
        {
            int expectedId = 1;
            string expectedMessage = "Message";
            var controller = new AjaxController();

            var result = controller.GetJson(expectedId, expectedMessage).Result;
            
            Assert
                .IsInstanceOfType(result, typeof(JsonResult));
        }
    }
}
