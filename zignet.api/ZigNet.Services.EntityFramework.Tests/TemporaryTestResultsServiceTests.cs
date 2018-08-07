using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Linq;
using ZigNet.Database.EntityFramework;
using ZigNet.Services.EntityFramework.Tests.Helpers;

namespace ZigNet.Services.EntityFramework.Tests
{
    public class TemporaryTestResultsServiceTests
    {
        [TestClass]
        public class DeleteAllWithSuiteIdMethod
        {
            [TestMethod]
            public void DeletesSingle()
            {
                var temporaryTestResults = new List<TemporaryTestResult>
                {
                    new TemporaryTestResult { SuiteId = 1 }
                };
                var mockDbSet = temporaryTestResults.ToDbSetMock();

                var mockContext = new Mock<ZigNetEntities>();
                mockContext.Setup(m => m.TemporaryTestResults).Returns(mockDbSet.Object);

                var zignetEntitiesWrapperMock = new Mock<IZigNetEntitiesWrapper>();
                zignetEntitiesWrapperMock.Setup(z => z.Get()).Returns(mockContext.Object);

                var temporaryTestResultsService = new TemporaryTestResultsService(zignetEntitiesWrapperMock.Object);
                temporaryTestResultsService.DeleteAll(1);

                mockDbSet.Verify(d => d.RemoveRange(temporaryTestResults), Times.Once);
            }

            [TestMethod]
            public void DeletesMultiple()
            {
                var temporaryTestResults = new List<TemporaryTestResult>
                {
                    new TemporaryTestResult { SuiteId = 1 },
                    new TemporaryTestResult { SuiteId = 1 }
                };
                var mockDbSet = temporaryTestResults.ToDbSetMock();

                var mockContext = new Mock<ZigNetEntities>();
                mockContext.Setup(m => m.TemporaryTestResults).Returns(mockDbSet.Object);

                var zignetEntitiesWrapperMock = new Mock<IZigNetEntitiesWrapper>();
                zignetEntitiesWrapperMock.Setup(z => z.Get()).Returns(mockContext.Object);

                var temporaryTestResultsService = new TemporaryTestResultsService(zignetEntitiesWrapperMock.Object);
                temporaryTestResultsService.DeleteAll(1);

                mockDbSet.Verify(d => d.RemoveRange(temporaryTestResults), Times.Once);
            }

            [TestMethod]
            public void DeletesOnlyWithSuiteId()
            {
                var temporaryTestResults = new List<TemporaryTestResult>
                {
                    new TemporaryTestResult { SuiteId = 1 },
                    new TemporaryTestResult { SuiteId = 1 },
                    new TemporaryTestResult { SuiteId = 2 }
                };
                var mockDbSet = temporaryTestResults.ToDbSetMock();

                var mockContext = new Mock<ZigNetEntities>();
                mockContext.Setup(m => m.TemporaryTestResults).Returns(mockDbSet.Object);

                var zignetEntitiesWrapperMock = new Mock<IZigNetEntitiesWrapper>();
                zignetEntitiesWrapperMock.Setup(z => z.Get()).Returns(mockContext.Object);

                var temporaryTestResultsService = new TemporaryTestResultsService(zignetEntitiesWrapperMock.Object);
                temporaryTestResultsService.DeleteAll(1);

                var temporaryTestResultsExpectedToBeDeleted = temporaryTestResults.Where(t => t.SuiteId == 1);
                mockDbSet.Verify(d => d.RemoveRange(temporaryTestResultsExpectedToBeDeleted), Times.Once);
            }

            [TestMethod]
            public void DoesNotThrowWhenNoneWithSuiteId()
            {
                var temporaryTestResults = new List<TemporaryTestResult>
                {
                    new TemporaryTestResult { SuiteId = 2 },
                    new TemporaryTestResult { SuiteId = 3 }
                };
                var mockDbSet = temporaryTestResults.ToDbSetMock();

                var mockContext = new Mock<ZigNetEntities>();
                mockContext.Setup(m => m.TemporaryTestResults).Returns(mockDbSet.Object);

                var zignetEntitiesWrapperMock = new Mock<IZigNetEntitiesWrapper>();
                zignetEntitiesWrapperMock.Setup(z => z.Get()).Returns(mockContext.Object);

                var temporaryTestResultsService = new TemporaryTestResultsService(zignetEntitiesWrapperMock.Object);
                temporaryTestResultsService.DeleteAll(1);

                var temporaryTestResultsExpectedToBeDeleted = new List<TemporaryTestResult>();
                mockDbSet.Verify(d => d.RemoveRange(temporaryTestResultsExpectedToBeDeleted), Times.Once);
            }
        }
    }
}
