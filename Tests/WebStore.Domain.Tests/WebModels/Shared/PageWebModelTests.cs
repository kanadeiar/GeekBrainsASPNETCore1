using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebStore.Domain.WebModels.Shared;

namespace WebStore.Domain.Tests.WebModels.Shared
{
    [TestClass]
    public class PageWebModelTests
    {
        [TestMethod]
        public void Create_Correct24Elements_ShouldCorrect()
        {
            const int count = 24;
            const int expectedPageNumber = 1;
            const int pageSize = 6;
            const int expectedStartNumber = 1;
            const int expectedPageCount = 4;

            var model = new PageWebModel(count, expectedPageNumber, pageSize);

            Assert
                .IsInstanceOfType(model, typeof(PageWebModel));
            Assert
                .AreEqual(expectedPageNumber, model.Page);
            Assert
                .AreEqual(expectedStartNumber, model.StartNumber);
            Assert
                .AreEqual(expectedPageCount, model.TotalPages);
            Assert
                .IsTrue(model.HasNextPage);
            Assert
                .IsTrue(model.HasNextNextPage);
            Assert
                .IsTrue(model.HasLastPage);
            Assert
                .IsFalse(model.HasFirstPage);
            Assert
                .IsFalse(model.HasPreviousPage);
            Assert
                .IsFalse(model.HasPrevPreviousPage);
        }

        [TestMethod]
        public void Create_Correct14_ShouldCorrect()
        {
            const int count = 14;
            const int expectedPageNumber = 1;
            const int pageSize = 9;
            const int expectedStartNumber = 1;
            const int expectedPageCount = 2;

            var model = new PageWebModel(count, expectedPageNumber, pageSize);

            Assert
                .IsInstanceOfType(model, typeof(PageWebModel));
            Assert
                .AreEqual(expectedPageNumber, model.Page);
            Assert
                .AreEqual(expectedStartNumber, model.StartNumber);
            Assert
                .AreEqual(expectedPageCount, model.TotalPages);
            Assert
                .IsTrue(model.HasNextPage);
            Assert
                .IsFalse(model.HasNextNextPage);
            Assert
                .IsFalse(model.HasLastPage);
            Assert
                .IsFalse(model.HasFirstPage);
            Assert
                .IsFalse(model.HasPreviousPage);
            Assert
                .IsFalse(model.HasPrevPreviousPage);
        }

        [TestMethod]
        public void CreateLast_Page4_ShouldCorrect()
        {
            const int count = 24;
            const int expectedPageNumber = 4;
            const int pageSize = 6;
            const int expectedStartNumber = 19;
            const int expectedPageCount = 4;

            var model = new PageWebModel(count, expectedPageNumber, pageSize);

            Assert
                .IsInstanceOfType(model, typeof(PageWebModel));
            Assert
                .AreEqual(expectedPageNumber, model.Page);
            Assert
                .AreEqual(expectedStartNumber, model.StartNumber);
            Assert
                .AreEqual(expectedPageCount, model.TotalPages);
        }

        [TestMethod]
        public void Create_MaxSize_ShouldCorrect()
        {
            const int count = Int32.MaxValue;
            const int pageSize = 999;
            const int expectedPageNumber = (count / pageSize / 2);
            const int expectedStartNumber = 1073740186;
            const int expectedPageCount = 2149634;

            var model = new PageWebModel(count, expectedPageNumber, pageSize);

            Assert
                .IsInstanceOfType(model, typeof(PageWebModel));
            Assert
                .AreEqual(expectedPageNumber, model.Page);
            Assert
                .AreEqual(expectedStartNumber, model.StartNumber);
            Assert
                .AreEqual(expectedPageCount, model.TotalPages);
            Assert
                .IsTrue(model.HasNextPage);
            Assert
                .IsTrue(model.HasNextNextPage);
            Assert
                .IsTrue(model.HasLastPage);
            Assert
                .IsTrue(model.HasPreviousPage);
            Assert
                .IsTrue(model.HasPrevPreviousPage);
            Assert
                .IsTrue(model.HasFirstPage);
        }

        [TestMethod]
        public void Create_MinSize_ShouldCorrect()
        {
            const int count = 2;
            const int pageSize = 9;
            const int expectedPageNumber = 1;
            const int expectedStartNumber = 1;
            const int expectedPageCount = 1;

            var model = new PageWebModel(count, expectedPageNumber, pageSize);

            Assert
                .IsInstanceOfType(model, typeof(PageWebModel));
            Assert
                .AreEqual(expectedPageNumber, model.Page);
            Assert
                .AreEqual(expectedStartNumber, model.StartNumber);
            Assert
                .AreEqual(expectedPageCount, model.TotalPages);
            Assert
                .IsFalse(model.HasNextPage);
            Assert
                .IsFalse(model.HasNextNextPage);
            Assert
                .IsFalse(model.HasLastPage);
            Assert
                .IsFalse(model.HasPreviousPage);
            Assert
                .IsFalse(model.HasPrevPreviousPage);
            Assert
                .IsFalse(model.HasFirstPage);
        }

        [TestMethod]
        public void Create_ZeroSize_ShouldCorrect()
        {
            const int count = 0;
            const int pageSize = 9;
            const int expectedPageNumber = 1;
            const int expectedStartNumber = 1;
            const int expectedPageCount = 1;

            var model = new PageWebModel(count, expectedPageNumber, pageSize);

            Assert
                .IsInstanceOfType(model, typeof(PageWebModel));
            Assert
                .AreEqual(expectedPageNumber, model.Page);
            Assert
                .AreEqual(expectedStartNumber, model.StartNumber);
            Assert
                .AreEqual(expectedPageCount, model.TotalPages);
            Assert
                .IsFalse(model.HasNextPage);
            Assert
                .IsFalse(model.HasNextNextPage);
            Assert
                .IsFalse(model.HasLastPage);
            Assert
                .IsFalse(model.HasPreviousPage);
            Assert
                .IsFalse(model.HasPrevPreviousPage);
            Assert
                .IsFalse(model.HasFirstPage);
        }
    }
}
