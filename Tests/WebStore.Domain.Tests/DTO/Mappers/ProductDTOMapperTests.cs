using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebStore.Domain.DTO;
using WebStore.Domain.DTO.Mappers;
using WebStore.Domain.Entities;

namespace WebStore.Domain.Tests.DTO.Mappers
{
    [TestClass]
    public class ProductDTOMapperTests
    {
        [TestMethod]
        public void ToDTO_ProductNull_ShouldNull()
        {
            Product product = null;

            var actual = product.ToDTO();

            Assert
                .IsNull(actual);
        }

        [TestMethod]
        public void FromDTO_DTONull_ShouldNull()
        {
            ProductDTO product = null;

            var actual = product.FromDTO();

            Assert
                .IsNull(actual);
        }

        [TestMethod]
        public void ToDTO_CorrectProduct_ShouldCorrect()
        {
            const int expectedId = 1;
            const string expectedName = "Test Name";
            const int expectedOrder = 1;
            const int expectedBrandId = 1;
            const int expectedSectionId = 1;
            const string expectedImageUrl = "test.jpg";
            const decimal expectedPrice = 12m;
            var product = new Product
            {
                Id = expectedId,
                Name = expectedName,
                Order = expectedOrder,
                BrandId = expectedBrandId,
                Brand = new Brand {Id = expectedBrandId},
                SectionId = expectedSectionId,
                Section = new Section {Id = expectedSectionId},
                ImageUrl = expectedImageUrl,
                Price = expectedPrice,
            };

            var actual = product.ToDTO();

            Assert
                .IsInstanceOfType(actual, typeof(ProductDTO));
            Assert
                .AreEqual(expectedId, actual.Id);
            Assert
                .AreEqual(expectedName, actual.Name);
            Assert
                .AreEqual(expectedOrder, actual.Order);
            Assert
                .AreEqual(expectedBrandId, actual.BrandId);
            Assert
                .AreEqual(expectedBrandId, actual.Brand.Id);
            Assert
                .AreEqual(expectedSectionId, actual.SectionId);
            Assert
                .AreEqual(expectedSectionId, actual.Section.Id);
            Assert
                .AreEqual(expectedImageUrl, actual.ImageUrl);
            Assert
                .AreEqual(expectedPrice, actual.Price);
        }

        [TestMethod]
        public void ToDTO_WithNullProduct_ShouldCorrect()
        {
            const int expectedId = 1;
            const string expectedName = "Test Name";

            var product = new Product
            {
                Id = expectedId,
                Name = expectedName,
                BrandId = null,
                Brand = null,
            };

            var actual = product.ToDTO();

            Assert
                .IsInstanceOfType(actual, typeof(ProductDTO));
            Assert
                .AreEqual(expectedId, actual.Id);
            Assert
                .AreEqual(expectedName, actual.Name);
            Assert
                .IsNull(actual.BrandId);
            Assert
                .IsNull(actual.Brand);
        }

        [TestMethod]
        public void FromDTO_CorrectDTO_ShouldCorrect()
        {
            const int expectedId = 1;
            const string expectedName = "Test Name";
            const int expectedOrder = 1;
            const int expectedBrandId = 1;
            const int expectedSectionId = 1;
            const string expectedImageUrl = "test.jpg";
            const decimal expectedPrice = 12m;
            var product = new ProductDTO
            {
                Id = expectedId,
                Name = expectedName,
                Order = expectedOrder,
                BrandId = expectedBrandId,
                Brand = new BrandDTO {Id = expectedBrandId},
                SectionId = expectedSectionId,
                Section = new SectionDTO {Id = expectedSectionId},
                ImageUrl = expectedImageUrl,
                Price = expectedPrice,
            };

            var actual = product.FromDTO();

            Assert
                .IsInstanceOfType(actual, typeof(Product));
            Assert
                .AreEqual(expectedId, actual.Id);
            Assert.
                AreEqual(expectedName, actual.Name);
            Assert
                .AreEqual(expectedOrder, actual.Order);
            Assert
                .AreEqual(expectedBrandId, actual.BrandId);
            Assert
                .AreEqual(expectedBrandId, actual.Brand.Id);
            Assert
                .AreEqual(expectedSectionId, actual.SectionId);
            Assert
                .AreEqual(expectedSectionId, actual.Section.Id);
            Assert
                .AreEqual(expectedImageUrl, actual.ImageUrl);
            Assert
                .AreEqual(expectedPrice, actual.Price);
        }

        [TestMethod]
        public void FromDTO_WithNullDTO_ShouldCorrect()
        {
            const int expectedId = 1;
            const string expectedName = "Test Name";
            var product = new ProductDTO
            {
                Id = expectedId,
                Name = expectedName,
                BrandId = null,
                Brand = null,
            };

            var actual = product.FromDTO();

            Assert
                .IsInstanceOfType(actual, typeof(Product));
            Assert
                .AreEqual(expectedId, actual.Id);
            Assert
                .AreEqual(expectedName, actual.Name);
            Assert
                .IsNull(actual.BrandId);
            Assert
                .IsNull(actual.Brand);
        }
    }
}
