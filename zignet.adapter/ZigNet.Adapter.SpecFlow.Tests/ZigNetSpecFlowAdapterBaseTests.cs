using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ZigNet.Adapter.SpecFlow.Utility;

namespace ZigNet.Adapter.SpecFlow.Tests
{
    
    public class ZigNetSpecFlowAdapterBaseTests
    {
        [TestClass]
        public class SaveTestResultMethod
        {
            [TestMethod]
            public void SavesPassedTest()
            {
                var zigNetApiHandlerMock = new Mock<IZigNetApiHandler>();
                var directoryServiceMock = new Mock<IDirectoryService>();
                var fileServiceMock = new Mock<IFileService>();
                var specFlowContextMock = new Mock<ISpecFlowContextWrapper>();

                directoryServiceMock.Setup(dsm => dsm.GetExecutingDirectory()).Returns(@"C:\executing\directory");
                fileServiceMock.Setup(fsm => fsm.ReadStringFromFile(@"C:\executing\directory\suiteResultId.txt")).Returns("1");
                specFlowContextMock.Setup(scm => scm.GetScenarioTitle()).Returns("scenario-title");
                specFlowContextMock.Setup(scm => scm.GetScenarioAndFeatureTags()).Returns(new string[] {"scenario-category-1"});
                specFlowContextMock.Setup(scm => scm.GetScenarioTestError()).Returns((Exception)null);

                var zigNetSpecFlowAdapterBase = new ZigNetSpecFlowAdapterBase(
                    zigNetApiHandlerMock.Object, directoryServiceMock.Object, 
                    fileServiceMock.Object, specFlowContextMock.Object);
                zigNetSpecFlowAdapterBase.SaveTestResult(DateTime.UtcNow);
            }

            [TestMethod]
            public void SavesFailedTestWithoutAssertion()
            {
                var zigNetApiHandlerMock = new Mock<IZigNetApiHandler>();
                var directoryServiceMock = new Mock<IDirectoryService>();
                var fileServiceMock = new Mock<IFileService>();
                var specFlowContextMock = new Mock<ISpecFlowContextWrapper>();

                directoryServiceMock.Setup(dsm => dsm.GetExecutingDirectory()).Returns(@"C:\executing\directory");
                fileServiceMock.Setup(fsm => fsm.ReadStringFromFile(@"C:\executing\directory\suiteResultId.txt")).Returns("1");
                specFlowContextMock.Setup(scm => scm.GetScenarioTitle()).Returns("scenario-title");
                specFlowContextMock.Setup(scm => scm.GetScenarioAndFeatureTags()).Returns(new string[] { "scenario-category-1" });
                specFlowContextMock.Setup(scm => scm.GetScenarioTestError()).Returns(new InvalidOperationException("invalid operation exception message"));

                var zigNetSpecFlowAdapterBase = new ZigNetSpecFlowAdapterBase(
                    zigNetApiHandlerMock.Object, directoryServiceMock.Object,
                    fileServiceMock.Object, specFlowContextMock.Object);
                zigNetSpecFlowAdapterBase.SaveTestResult(DateTime.UtcNow);
            }

            [TestMethod]
            public void SavesFailedTestWitAssertion()
            {
                var zigNetApiHandlerMock = new Mock<IZigNetApiHandler>();
                var directoryServiceMock = new Mock<IDirectoryService>();
                var fileServiceMock = new Mock<IFileService>();
                var specFlowContextMock = new Mock<ISpecFlowContextWrapper>();

                directoryServiceMock.Setup(dsm => dsm.GetExecutingDirectory()).Returns(@"C:\executing\directory");
                fileServiceMock.Setup(fsm => fsm.ReadStringFromFile(@"C:\executing\directory\suiteResultId.txt")).Returns("1");
                specFlowContextMock.Setup(scm => scm.GetScenarioTitle()).Returns("scenario-title");
                specFlowContextMock.Setup(scm => scm.GetScenarioAndFeatureTags()).Returns(new string[] { "scenario-category-1" });
                specFlowContextMock.Setup(scm => scm.GetScenarioTestError()).Returns(new AssertFailedException("assert failed exception message"));

                var zigNetSpecFlowAdapterBase = new ZigNetSpecFlowAdapterBase(
                    zigNetApiHandlerMock.Object, directoryServiceMock.Object,
                    fileServiceMock.Object, specFlowContextMock.Object);
                zigNetSpecFlowAdapterBase.SaveTestResult(DateTime.UtcNow);
            }

            [TestMethod]
            [ExpectedException(typeof(InvalidOperationException))]
            public void ThrowsIfTextFileNotFound()
            {
                var zigNetApiHandlerMock = new Mock<IZigNetApiHandler>();
                var directoryServiceMock = new Mock<IDirectoryService>();
                var fileServiceMock = new Mock<IFileService>();
                var specFlowContextMock = new Mock<ISpecFlowContextWrapper>();

                directoryServiceMock.Setup(dsm => dsm.GetExecutingDirectory()).Returns(@"C:\executing\directory");
                fileServiceMock.Setup(fsm => fsm.ReadStringFromFile(@"C:\executing\directory\suiteResultId.txt")).Throws(new InvalidOperationException());

                var zigNetSpecFlowAdapterBase = new ZigNetSpecFlowAdapterBase(
                    zigNetApiHandlerMock.Object, directoryServiceMock.Object,
                    fileServiceMock.Object, specFlowContextMock.Object);
                zigNetSpecFlowAdapterBase.SaveTestResult(DateTime.UtcNow);
            }

            [TestMethod]
            [ExpectedException(typeof(FormatException))]
            public void ThrowsIfTextFromFileNotInteger()
            {
                var zigNetApiHandlerMock = new Mock<IZigNetApiHandler>();
                var directoryServiceMock = new Mock<IDirectoryService>();
                var fileServiceMock = new Mock<IFileService>();
                var specFlowContextMock = new Mock<ISpecFlowContextWrapper>();

                directoryServiceMock.Setup(dsm => dsm.GetExecutingDirectory()).Returns(@"C:\executing\directory");
                fileServiceMock.Setup(fsm => fsm.ReadStringFromFile(@"C:\executing\directory\suiteResultId.txt")).Returns("not-an-integer");

                var zigNetSpecFlowAdapterBase = new ZigNetSpecFlowAdapterBase(
                    zigNetApiHandlerMock.Object, directoryServiceMock.Object,
                    fileServiceMock.Object, specFlowContextMock.Object);
                zigNetSpecFlowAdapterBase.SaveTestResult(DateTime.UtcNow);
            }

            [TestMethod]
            public void DoesNotThrowWhenScenarioHasZeroCategories()
            {
                var zigNetApiHandlerMock = new Mock<IZigNetApiHandler>();
                var directoryServiceMock = new Mock<IDirectoryService>();
                var fileServiceMock = new Mock<IFileService>();
                var specFlowContextMock = new Mock<ISpecFlowContextWrapper>();

                directoryServiceMock.Setup(dsm => dsm.GetExecutingDirectory()).Returns(@"C:\executing\directory");
                fileServiceMock.Setup(fsm => fsm.ReadStringFromFile(@"C:\executing\directory\suiteResultId.txt")).Returns("1");
                specFlowContextMock.Setup(scm => scm.GetScenarioTitle()).Returns("scenario-title");
                specFlowContextMock.Setup(scm => scm.GetScenarioAndFeatureTags()).Returns(new string[0]);
                specFlowContextMock.Setup(scm => scm.GetScenarioTestError()).Returns((Exception)null);

                var zigNetSpecFlowAdapterBase = new ZigNetSpecFlowAdapterBase(
                    zigNetApiHandlerMock.Object, directoryServiceMock.Object,
                    fileServiceMock.Object, specFlowContextMock.Object);
                zigNetSpecFlowAdapterBase.SaveTestResult(DateTime.UtcNow);
            }

            [TestMethod]
            public void DoesNotThrowWhenScenarioNameEmpty()
            {
                var zigNetApiHandlerMock = new Mock<IZigNetApiHandler>();
                var directoryServiceMock = new Mock<IDirectoryService>();
                var fileServiceMock = new Mock<IFileService>();
                var specFlowContextMock = new Mock<ISpecFlowContextWrapper>();

                directoryServiceMock.Setup(dsm => dsm.GetExecutingDirectory()).Returns(@"C:\executing\directory");
                fileServiceMock.Setup(fsm => fsm.ReadStringFromFile(@"C:\executing\directory\suiteResultId.txt")).Returns("1");
                specFlowContextMock.Setup(scm => scm.GetScenarioTitle()).Returns("");
                specFlowContextMock.Setup(scm => scm.GetScenarioAndFeatureTags()).Returns(new string[0]);
                specFlowContextMock.Setup(scm => scm.GetScenarioTestError()).Returns((Exception)null);

                var zigNetSpecFlowAdapterBase = new ZigNetSpecFlowAdapterBase(
                    zigNetApiHandlerMock.Object, directoryServiceMock.Object,
                    fileServiceMock.Object, specFlowContextMock.Object);
                zigNetSpecFlowAdapterBase.SaveTestResult(DateTime.UtcNow);
            }
        }
    }
}
