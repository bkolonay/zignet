using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ZigNet.Services.DTOs;
using ZigNet.Database.EntityFramework;
using ZigNet.Services.EntityFramework.Tests.Helpers;
using DbLatestTestResult = ZigNet.Database.EntityFramework.LatestTestResult;
using TestResultType = ZigNet.Domain.Test.TestResultType;

namespace ZigNet.Services.EntityFramework.Tests
{
    public class LatestTestResultsServiceTests
    {
        [TestClass]
        public class Save
        {
            [TestMethod]
            public void CreatesNew()
            {
                var utcNow = DateTime.UtcNow;

                var mockLatestTestResults = new List<DbLatestTestResult>().ToDbSetMock();

                var mockContext = new Mock<ZigNetEntities>();
                mockContext.Setup(m => m.LatestTestResults).Returns(mockLatestTestResults.Object);

                var mockZignetEntitiesWrapper = new Mock<IZigNetEntitiesWrapper>();
                mockZignetEntitiesWrapper.Setup(z => z.Get()).Returns(mockContext.Object);

                var latestTestResultDto = new LatestTestResultDto
                {
                    TestResultID = 4,
                    SuiteId = 2,
                    TestId = 0,
                    TestName = "first time failing test",
                    SuiteName = "suite-name"
                };

                var latestTestResultsService = new LatestTestResultsService(mockZignetEntitiesWrapper.Object);
                var savedLatestTestResultDto = latestTestResultsService.Save(latestTestResultDto, TestResultType.Fail, utcNow);

                Assert.AreEqual(2, savedLatestTestResultDto.SuiteId);
                Assert.AreEqual(0, savedLatestTestResultDto.TestId);
                Assert.AreEqual("first time failing test", savedLatestTestResultDto.TestName);
                Assert.AreEqual("suite-name", savedLatestTestResultDto.SuiteName);
                Assert.AreEqual(4, savedLatestTestResultDto.TestResultID);
                Assert.AreEqual(utcNow, savedLatestTestResultDto.FailingFromDate);
                Assert.AreEqual(null, savedLatestTestResultDto.PassingFromDate);
                Assert.AreEqual(0, savedLatestTestResultDto.LatestTestResultID);
            }

            [TestMethod]
            public void CreatesNewWhenSuiteIdDoesNotMatch()
            {
                var utcNow = DateTime.UtcNow;

                var mockLatestTestResults = new List<DbLatestTestResult> {
                    new DbLatestTestResult { LatestTestResultID = 5, SuiteId = 7, TestId = 1 }
                }.ToDbSetMock();

                var mockContext = new Mock<ZigNetEntities>();
                mockContext.Setup(m => m.LatestTestResults).Returns(mockLatestTestResults.Object);

                var mockZignetEntitiesWrapper = new Mock<IZigNetEntitiesWrapper>();
                mockZignetEntitiesWrapper.Setup(z => z.Get()).Returns(mockContext.Object);

                var latestTestResultDto = new LatestTestResultDto
                {
                    TestResultID = 4,
                    SuiteId = 2,
                    TestId = 1,
                    TestName = "existing test",
                    SuiteName = "suite-name"
                };

                var latestTestResultsService = new LatestTestResultsService(mockZignetEntitiesWrapper.Object);
                var savedLatestTestResultDto = latestTestResultsService.Save(latestTestResultDto, TestResultType.Fail, utcNow);

                Assert.AreEqual(2, savedLatestTestResultDto.SuiteId);
                Assert.AreEqual(1, savedLatestTestResultDto.TestId);
                Assert.AreEqual("existing test", savedLatestTestResultDto.TestName);
                Assert.AreEqual("suite-name", savedLatestTestResultDto.SuiteName);
                Assert.AreEqual(4, savedLatestTestResultDto.TestResultID);
                Assert.AreEqual(utcNow, savedLatestTestResultDto.FailingFromDate);
                Assert.AreEqual(null, savedLatestTestResultDto.PassingFromDate);
                Assert.AreEqual(0, savedLatestTestResultDto.LatestTestResultID);
            }

            [TestMethod]
            public void CreatesNewWhenTestIdDoesNotMatch()
            {
                var utcNow = DateTime.UtcNow;

                var mockLatestTestResults = new List<DbLatestTestResult> {
                    new DbLatestTestResult { LatestTestResultID = 5, SuiteId = 2, TestId = 7 }
                }.ToDbSetMock();

                var mockContext = new Mock<ZigNetEntities>();
                mockContext.Setup(m => m.LatestTestResults).Returns(mockLatestTestResults.Object);

                var mockZignetEntitiesWrapper = new Mock<IZigNetEntitiesWrapper>();
                mockZignetEntitiesWrapper.Setup(z => z.Get()).Returns(mockContext.Object);

                var latestTestResultDto = new LatestTestResultDto
                {
                    TestResultID = 4,
                    SuiteId = 2,
                    TestId = 1,
                    TestName = "existing test",
                    SuiteName = "suite-name"
                };

                var latestTestResultsService = new LatestTestResultsService(mockZignetEntitiesWrapper.Object);
                var savedLatestTestResultDto = latestTestResultsService.Save(latestTestResultDto, TestResultType.Fail, utcNow);

                Assert.AreEqual(2, savedLatestTestResultDto.SuiteId);
                Assert.AreEqual(1, savedLatestTestResultDto.TestId);
                Assert.AreEqual("existing test", savedLatestTestResultDto.TestName);
                Assert.AreEqual("suite-name", savedLatestTestResultDto.SuiteName);
                Assert.AreEqual(4, savedLatestTestResultDto.TestResultID);
                Assert.AreEqual(utcNow, savedLatestTestResultDto.FailingFromDate);
                Assert.AreEqual(null, savedLatestTestResultDto.PassingFromDate);
                Assert.AreEqual(0, savedLatestTestResultDto.LatestTestResultID);
            }

            [TestMethod]
            public void UpdatesExisting()
            {
                var utcNow = DateTime.UtcNow;

                var mockLatestTestResults = new List<DbLatestTestResult> {
                    new DbLatestTestResult {
                        LatestTestResultID = 5,
                        SuiteId = 2,
                        TestId = 1,
                        TestName = "existing test",
                        SuiteName = "suite-name"
                    }
                }.ToDbSetMock();

                var mockContext = new Mock<ZigNetEntities>();
                mockContext.Setup(m => m.LatestTestResults).Returns(mockLatestTestResults.Object);

                var mockZignetEntitiesWrapper = new Mock<IZigNetEntitiesWrapper>();
                mockZignetEntitiesWrapper.Setup(z => z.Get()).Returns(mockContext.Object);

                var latestTestResultDto = new LatestTestResultDto
                {
                    TestResultID = 4,
                    SuiteId = 2,
                    TestId = 1,
                    SuiteName = "suite-name"
                };

                var latestTestResultsService = new LatestTestResultsService(mockZignetEntitiesWrapper.Object);
                var savedLatestTestResultDto = latestTestResultsService.Save(latestTestResultDto, TestResultType.Fail, utcNow);

                Assert.AreEqual(2, savedLatestTestResultDto.SuiteId);
                Assert.AreEqual(1, savedLatestTestResultDto.TestId);
                Assert.AreEqual("existing test", savedLatestTestResultDto.TestName);
                Assert.AreEqual("suite-name", savedLatestTestResultDto.SuiteName);
                Assert.AreEqual(4, savedLatestTestResultDto.TestResultID);
                Assert.AreEqual(utcNow, savedLatestTestResultDto.FailingFromDate);
                Assert.AreEqual(null, savedLatestTestResultDto.PassingFromDate);
                Assert.AreEqual(5, savedLatestTestResultDto.LatestTestResultID);
            }

            [TestMethod]
            public void UpdatesSuiteName()
            {
                var utcNow = DateTime.UtcNow;

                var mockLatestTestResults = new List<DbLatestTestResult> {
                    new DbLatestTestResult {
                        LatestTestResultID = 5,
                        SuiteId = 2,
                        TestId = 1,
                        TestName = "existing test",
                        SuiteName = "existing suite name"
                    }
                }.ToDbSetMock();

                var mockContext = new Mock<ZigNetEntities>();
                mockContext.Setup(m => m.LatestTestResults).Returns(mockLatestTestResults.Object);

                var mockZignetEntitiesWrapper = new Mock<IZigNetEntitiesWrapper>();
                mockZignetEntitiesWrapper.Setup(z => z.Get()).Returns(mockContext.Object);

                var latestTestResultDto = new LatestTestResultDto
                {
                    TestResultID = 4,
                    SuiteId = 2,
                    TestId = 1,
                    SuiteName = "new suite name"
                };

                var latestTestResultsService = new LatestTestResultsService(mockZignetEntitiesWrapper.Object);
                var savedLatestTestResultDto = latestTestResultsService.Save(latestTestResultDto, TestResultType.Fail, utcNow);

                Assert.AreEqual(2, savedLatestTestResultDto.SuiteId);
                Assert.AreEqual(1, savedLatestTestResultDto.TestId);
                Assert.AreEqual("existing test", savedLatestTestResultDto.TestName);
                Assert.AreEqual("new suite name", savedLatestTestResultDto.SuiteName);
                Assert.AreEqual(4, savedLatestTestResultDto.TestResultID);
                Assert.AreEqual(utcNow, savedLatestTestResultDto.FailingFromDate);
                Assert.AreEqual(null, savedLatestTestResultDto.PassingFromDate);
                Assert.AreEqual(5, savedLatestTestResultDto.LatestTestResultID);
            }

            [TestMethod]
            public void UpdatesPassingFromDate()
            {
                var utcNow = DateTime.UtcNow;

                var mockLatestTestResults = new List<DbLatestTestResult> {
                    new DbLatestTestResult {
                        LatestTestResultID = 5,
                        SuiteId = 2,
                        TestId = 1,
                        TestName = "existing test",
                        SuiteName = "suite-name",
                        FailingFromDateTime = utcNow.AddDays(-3)
                    }                    
                }.ToDbSetMock();

                var mockContext = new Mock<ZigNetEntities>();
                mockContext.Setup(m => m.LatestTestResults).Returns(mockLatestTestResults.Object);

                var mockZignetEntitiesWrapper = new Mock<IZigNetEntitiesWrapper>();
                mockZignetEntitiesWrapper.Setup(z => z.Get()).Returns(mockContext.Object);

                var latestTestResultDto = new LatestTestResultDto
                {
                    TestResultID = 4,
                    SuiteId = 2,
                    TestId = 1,
                    SuiteName = "suite-name"
                };

                var latestTestResultsService = new LatestTestResultsService(mockZignetEntitiesWrapper.Object);
                var savedLatestTestResultDto = latestTestResultsService.Save(latestTestResultDto, TestResultType.Pass, utcNow);

                Assert.AreEqual(2, savedLatestTestResultDto.SuiteId);
                Assert.AreEqual(1, savedLatestTestResultDto.TestId);
                Assert.AreEqual("existing test", savedLatestTestResultDto.TestName);
                Assert.AreEqual("suite-name", savedLatestTestResultDto.SuiteName);
                Assert.AreEqual(4, savedLatestTestResultDto.TestResultID);
                Assert.AreEqual(null, savedLatestTestResultDto.FailingFromDate);
                Assert.AreEqual(utcNow, savedLatestTestResultDto.PassingFromDate);
                Assert.AreEqual(5, savedLatestTestResultDto.LatestTestResultID);
            }

            [TestMethod]
            public void UpdatesFailingFromDate()
            {
                var utcNow = DateTime.UtcNow;

                var mockLatestTestResults = new List<DbLatestTestResult> {
                    new DbLatestTestResult {
                        LatestTestResultID = 5,
                        SuiteId = 2,
                        TestId = 1,
                        TestName = "existing test",
                        SuiteName = "suite-name",
                        PassingFromDateTime = utcNow.AddDays(-3)
                    }
                }.ToDbSetMock();

                var mockContext = new Mock<ZigNetEntities>();
                mockContext.Setup(m => m.LatestTestResults).Returns(mockLatestTestResults.Object);

                var mockZignetEntitiesWrapper = new Mock<IZigNetEntitiesWrapper>();
                mockZignetEntitiesWrapper.Setup(z => z.Get()).Returns(mockContext.Object);

                var latestTestResultDto = new LatestTestResultDto
                {
                    TestResultID = 4,
                    SuiteId = 2,
                    TestId = 1,
                    SuiteName = "suite-name"
                };

                var latestTestResultsService = new LatestTestResultsService(mockZignetEntitiesWrapper.Object);
                var savedLatestTestResultDto = latestTestResultsService.Save(latestTestResultDto, TestResultType.Fail, utcNow);

                Assert.AreEqual(2, savedLatestTestResultDto.SuiteId);
                Assert.AreEqual(1, savedLatestTestResultDto.TestId);
                Assert.AreEqual("existing test", savedLatestTestResultDto.TestName);
                Assert.AreEqual("suite-name", savedLatestTestResultDto.SuiteName);
                Assert.AreEqual(4, savedLatestTestResultDto.TestResultID);
                Assert.AreEqual(utcNow, savedLatestTestResultDto.FailingFromDate);
                Assert.AreEqual(null, savedLatestTestResultDto.PassingFromDate);
                Assert.AreEqual(5, savedLatestTestResultDto.LatestTestResultID);
            }

            [TestMethod]
            public void UpdatesFailingFromDateIfInconclusive()
            {
                var utcNow = DateTime.UtcNow;

                var mockLatestTestResults = new List<DbLatestTestResult> {
                    new DbLatestTestResult {
                        LatestTestResultID = 5,
                        SuiteId = 2,
                        TestId = 1,
                        TestName = "existing test",
                        SuiteName = "suite-name",
                        PassingFromDateTime = utcNow.AddDays(-3)
                    }
                }.ToDbSetMock();

                var mockContext = new Mock<ZigNetEntities>();
                mockContext.Setup(m => m.LatestTestResults).Returns(mockLatestTestResults.Object);

                var mockZignetEntitiesWrapper = new Mock<IZigNetEntitiesWrapper>();
                mockZignetEntitiesWrapper.Setup(z => z.Get()).Returns(mockContext.Object);

                var latestTestResultDto = new LatestTestResultDto
                {
                    TestResultID = 4,
                    SuiteId = 2,
                    TestId = 1,
                    SuiteName = "suite-name"
                };

                var latestTestResultsService = new LatestTestResultsService(mockZignetEntitiesWrapper.Object);
                var savedLatestTestResultDto = latestTestResultsService.Save(latestTestResultDto, TestResultType.Inconclusive, utcNow);

                Assert.AreEqual(2, savedLatestTestResultDto.SuiteId);
                Assert.AreEqual(1, savedLatestTestResultDto.TestId);
                Assert.AreEqual("existing test", savedLatestTestResultDto.TestName);
                Assert.AreEqual("suite-name", savedLatestTestResultDto.SuiteName);
                Assert.AreEqual(4, savedLatestTestResultDto.TestResultID);
                Assert.AreEqual(utcNow, savedLatestTestResultDto.FailingFromDate);
                Assert.AreEqual(null, savedLatestTestResultDto.PassingFromDate);
                Assert.AreEqual(5, savedLatestTestResultDto.LatestTestResultID);
            }

            [TestMethod]
            public void IgnoresPassingFromDate()
            {
                var utcNow = DateTime.UtcNow;

                var mockLatestTestResults = new List<DbLatestTestResult> {
                    new DbLatestTestResult {
                        LatestTestResultID = 5,
                        SuiteId = 2,
                        TestId = 1,
                        TestName = "existing test",
                        SuiteName = "suite-name",
                        PassingFromDateTime = utcNow.AddDays(-3)
                    }
                }.ToDbSetMock();

                var mockContext = new Mock<ZigNetEntities>();
                mockContext.Setup(m => m.LatestTestResults).Returns(mockLatestTestResults.Object);

                var mockZignetEntitiesWrapper = new Mock<IZigNetEntitiesWrapper>();
                mockZignetEntitiesWrapper.Setup(z => z.Get()).Returns(mockContext.Object);

                var latestTestResultDto = new LatestTestResultDto
                {
                    TestResultID = 4,
                    SuiteId = 2,
                    TestId = 1,
                    SuiteName = "suite-name"
                };

                var latestTestResultsService = new LatestTestResultsService(mockZignetEntitiesWrapper.Object);
                var savedLatestTestResultDto = latestTestResultsService.Save(latestTestResultDto, TestResultType.Pass, utcNow);

                Assert.AreEqual(2, savedLatestTestResultDto.SuiteId);
                Assert.AreEqual(1, savedLatestTestResultDto.TestId);
                Assert.AreEqual("existing test", savedLatestTestResultDto.TestName);
                Assert.AreEqual("suite-name", savedLatestTestResultDto.SuiteName);
                Assert.AreEqual(0, savedLatestTestResultDto.TestResultID);
                Assert.AreEqual(null, savedLatestTestResultDto.FailingFromDate);
                Assert.AreEqual(utcNow.AddDays(-3), savedLatestTestResultDto.PassingFromDate);
                Assert.AreEqual(5, savedLatestTestResultDto.LatestTestResultID);
            }

            [TestMethod]
            public void IgnoresFailingFromDate()
            {
                var utcNow = DateTime.UtcNow;

                var mockLatestTestResults = new List<DbLatestTestResult> {
                    new DbLatestTestResult {
                        LatestTestResultID = 5,
                        SuiteId = 2,
                        TestId = 1,
                        TestName = "existing test",
                        SuiteName = "suite-name",
                        FailingFromDateTime = utcNow.AddDays(-3)
                    }
                }.ToDbSetMock();

                var mockContext = new Mock<ZigNetEntities>();
                mockContext.Setup(m => m.LatestTestResults).Returns(mockLatestTestResults.Object);

                var mockZignetEntitiesWrapper = new Mock<IZigNetEntitiesWrapper>();
                mockZignetEntitiesWrapper.Setup(z => z.Get()).Returns(mockContext.Object);

                var latestTestResultDto = new LatestTestResultDto
                {
                    TestResultID = 4,
                    SuiteId = 2,
                    TestId = 1,
                    SuiteName = "suite-name"
                };

                var latestTestResultsService = new LatestTestResultsService(mockZignetEntitiesWrapper.Object);
                var savedLatestTestResultDto = latestTestResultsService.Save(latestTestResultDto, TestResultType.Fail, utcNow);

                Assert.AreEqual(2, savedLatestTestResultDto.SuiteId);
                Assert.AreEqual(1, savedLatestTestResultDto.TestId);
                Assert.AreEqual("existing test", savedLatestTestResultDto.TestName);
                Assert.AreEqual("suite-name", savedLatestTestResultDto.SuiteName);
                Assert.AreEqual(0, savedLatestTestResultDto.TestResultID);
                Assert.AreEqual(utcNow.AddDays(-3), savedLatestTestResultDto.FailingFromDate);
                Assert.AreEqual(null, savedLatestTestResultDto.PassingFromDate);
                Assert.AreEqual(5, savedLatestTestResultDto.LatestTestResultID);
            }

            [TestMethod]
            public void IgnoresFailingFromDateIfInconclusive()
            {
                var utcNow = DateTime.UtcNow;

                var mockLatestTestResults = new List<DbLatestTestResult> {
                    new DbLatestTestResult {
                        LatestTestResultID = 5,
                        SuiteId = 2,
                        TestId = 1,
                        TestName = "existing test",
                        SuiteName = "suite-name",
                        FailingFromDateTime = utcNow.AddDays(-3)
                    }
                }.ToDbSetMock();

                var mockContext = new Mock<ZigNetEntities>();
                mockContext.Setup(m => m.LatestTestResults).Returns(mockLatestTestResults.Object);

                var mockZignetEntitiesWrapper = new Mock<IZigNetEntitiesWrapper>();
                mockZignetEntitiesWrapper.Setup(z => z.Get()).Returns(mockContext.Object);

                var latestTestResultDto = new LatestTestResultDto
                {
                    TestResultID = 4,
                    SuiteId = 2,
                    TestId = 1,
                    SuiteName = "suite-name"
                };

                var latestTestResultsService = new LatestTestResultsService(mockZignetEntitiesWrapper.Object);
                var savedLatestTestResultDto = latestTestResultsService.Save(latestTestResultDto, TestResultType.Inconclusive, utcNow);

                Assert.AreEqual(2, savedLatestTestResultDto.SuiteId);
                Assert.AreEqual(1, savedLatestTestResultDto.TestId);
                Assert.AreEqual("existing test", savedLatestTestResultDto.TestName);
                Assert.AreEqual("suite-name", savedLatestTestResultDto.SuiteName);
                Assert.AreEqual(0, savedLatestTestResultDto.TestResultID);
                Assert.AreEqual(utcNow.AddDays(-3), savedLatestTestResultDto.FailingFromDate);
                Assert.AreEqual(null, savedLatestTestResultDto.PassingFromDate);
                Assert.AreEqual(5, savedLatestTestResultDto.LatestTestResultID);
            }
        }
    }
}
