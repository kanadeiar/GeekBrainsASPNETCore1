﻿using WebStore.Controllers.API;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WebStore.Controllers.API.Tests
{
    [TestClass]
    public class ConsoleControllerTests
    {
        [TestMethod]
        public void WriteLine_Invoke_ShouldAdded()
        {
            var expectedText = "test";
            var controller = new ConsoleController();
            
            using (var consoleOutput = new ConsoleOutput())
            {
                controller.WriteLine(expectedText);

                Assert.AreEqual(expectedText + "\r\n", consoleOutput.GetOuput());
            }
        }
    }

    #region Вспомогательное

    public class ConsoleOutput : IDisposable
    {
        private StringWriter stringWriter;
        private TextWriter originalOutput;

        public ConsoleOutput()
        {
            stringWriter = new StringWriter();
            originalOutput = Console.Out;
            Console.SetOut(stringWriter);
        }

        public string GetOuput()
        {
            return stringWriter.ToString();
        }

        public void Dispose()
        {
            Console.SetOut(originalOutput);
            stringWriter.Dispose();
        }
    }

    #endregion
}

