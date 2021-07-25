using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebStore.Domain.Identity.ErrorCodes;

namespace WebStore.Domain.Tests.Identity.ErrorCodes
{
    [TestClass]
    public class IdentityErrorCodesTests
    {
        [TestMethod]
        public void GetDescription_1Description_ShouldCorrect()
        {
            var expectedValue = "Произошла неизвестная ошибка";

            var actual = IdentityErrorCodes.GetDescription(IdentityErrorCodes.DefaultError);

            Assert
                .AreEqual(actual, expectedValue);
        }
    }
}
