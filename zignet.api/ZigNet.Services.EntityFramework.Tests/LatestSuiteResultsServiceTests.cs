using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using ZigNet.Services.DTOs;
using ZigNet.Database.EntityFramework;
using ZigNet.Services.EntityFramework.Tests.Helpers;

namespace ZigNet.Services.EntityFramework.Tests
{
    public class LatestSuiteResultsServiceTests
    {
        [TestClass]
        public class GetLatest
        {
            [TestMethod]
            public void SingleSuite()
            {
                var now = DateTime.Now;

                var suiteDtos = new List<SuiteDto> {
                    new SuiteDto { SuiteID = 1, Name = "Services", ApplicationNameAbbreviation = "LN", EnvironmentNameAbbreviation = "TSM" }
                };

                var mockSuiteService = new Mock<ISuiteService>();
                mockSuiteService.Setup(s => s.GetAll()).Returns(suiteDtos);

                var suiteResult = new SuiteResult { SuiteResultEndDateTime = now };
                var temporaryTestResults = new List<TemporaryTestResult>
                {
                    new TemporaryTestResult { SuiteId = 1, TestResultTypeId = 1, SuiteResult = suiteResult },
                    new TemporaryTestResult { SuiteId = 1, TestResultTypeId = 2, SuiteResult = suiteResult },
                    new TemporaryTestResult { SuiteId = 1, TestResultTypeId = 3, SuiteResult = suiteResult }
                };
                var mockTemporaryTestResults = temporaryTestResults.ToDbSetMock();
                mockTemporaryTestResults.Setup(m => m.AsNoTracking()).Returns(mockTemporaryTestResults.Object);

                var mockContext = new Mock<ZigNetEntities>();
                mockContext.Setup(m => m.TemporaryTestResults).Returns(mockTemporaryTestResults.Object);

                var zignetEntitiesWrapperMock = new Mock<IZigNetEntitiesWrapper>();
                zignetEntitiesWrapperMock.Setup(z => z.Get()).Returns(mockContext.Object);

                var latestSuiteResultsService = new LatestSuiteResultsService(zignetEntitiesWrapperMock.Object, mockSuiteService.Object);
                var suiteSummaries = latestSuiteResultsService.GetLatest().ToList();

                Assert.AreEqual(1, suiteSummaries.Count);
                Assert.AreEqual("LN Services (TSM)", suiteSummaries[0].SuiteName);
                Assert.AreEqual(1, suiteSummaries[0].SuiteIds.Count);
                Assert.AreEqual(1, suiteSummaries[0].SuiteIds[0]);
                Assert.AreEqual(now, suiteSummaries[0].SuiteEndTime);
                Assert.AreEqual(1, suiteSummaries[0].TotalFailedTests);
                Assert.AreEqual(1, suiteSummaries[0].TotalInconclusiveTests);
                Assert.AreEqual(1, suiteSummaries[0].TotalPassedTests);
            }

            [TestMethod]
            public void MultipleSuites()
            {
                var now = DateTime.Now;

                var suiteDtos = new List<SuiteDto> {
                    new SuiteDto { SuiteID = 1, Name = "Services", ApplicationNameAbbreviation = "LN", EnvironmentNameAbbreviation = "TSM" },
                    new SuiteDto { SuiteID = 2, Name = "UI", ApplicationNameAbbreviation = "LN", EnvironmentNameAbbreviation = "TSR" }
                };

                var mockSuiteService = new Mock<ISuiteService>();
                mockSuiteService.Setup(s => s.GetAll()).Returns(suiteDtos);

                var servicesSuiteResult = new SuiteResult { SuiteResultEndDateTime = now };
                var uiSuiteResult = new SuiteResult { SuiteResultEndDateTime = now.AddDays(1) };
                var temporaryTestResults = new List<TemporaryTestResult>
                {
                    new TemporaryTestResult { SuiteId = 1, TestResultTypeId = 1, SuiteResult = servicesSuiteResult },
                    new TemporaryTestResult { SuiteId = 1, TestResultTypeId = 2, SuiteResult = servicesSuiteResult },
                    new TemporaryTestResult { SuiteId = 1, TestResultTypeId = 2, SuiteResult = servicesSuiteResult },
                    new TemporaryTestResult { SuiteId = 1, TestResultTypeId = 3, SuiteResult = servicesSuiteResult },
                    new TemporaryTestResult { SuiteId = 1, TestResultTypeId = 3, SuiteResult = servicesSuiteResult },
                    new TemporaryTestResult { SuiteId = 1, TestResultTypeId = 3, SuiteResult = servicesSuiteResult },

                    new TemporaryTestResult { SuiteId = 2, TestResultTypeId = 1, SuiteResult = uiSuiteResult },
                    new TemporaryTestResult { SuiteId = 2, TestResultTypeId = 2, SuiteResult = uiSuiteResult }
                };
                var mockTemporaryTestResults = temporaryTestResults.ToDbSetMock();
                mockTemporaryTestResults.Setup(m => m.AsNoTracking()).Returns(mockTemporaryTestResults.Object);

                var mockContext = new Mock<ZigNetEntities>();
                mockContext.Setup(m => m.TemporaryTestResults).Returns(mockTemporaryTestResults.Object);

                var zignetEntitiesWrapperMock = new Mock<IZigNetEntitiesWrapper>();
                zignetEntitiesWrapperMock.Setup(z => z.Get()).Returns(mockContext.Object);

                var latestSuiteResultsService = new LatestSuiteResultsService(zignetEntitiesWrapperMock.Object, mockSuiteService.Object);
                var suiteSummaries = latestSuiteResultsService.GetLatest().OrderBy(s => s.SuiteName).ToList();

                Assert.AreEqual(2, suiteSummaries.Count);

                Assert.AreEqual("LN Services (TSM)", suiteSummaries[0].SuiteName);
                Assert.AreEqual(1, suiteSummaries[0].SuiteIds.Count);
                Assert.AreEqual(1, suiteSummaries[0].SuiteIds[0]);
                Assert.AreEqual(now, suiteSummaries[0].SuiteEndTime);
                Assert.AreEqual(1, suiteSummaries[0].TotalFailedTests);
                Assert.AreEqual(2, suiteSummaries[0].TotalInconclusiveTests);
                Assert.AreEqual(3, suiteSummaries[0].TotalPassedTests);

                Assert.AreEqual("LN UI (TSR)", suiteSummaries[1].SuiteName);
                Assert.AreEqual(1, suiteSummaries[1].SuiteIds.Count);
                Assert.AreEqual(2, suiteSummaries[1].SuiteIds[0]);
                Assert.AreEqual(now.AddDays(1), suiteSummaries[1].SuiteEndTime);
                Assert.AreEqual(1, suiteSummaries[1].TotalFailedTests);
                Assert.AreEqual(1, suiteSummaries[1].TotalInconclusiveTests);
                Assert.AreEqual(0, suiteSummaries[1].TotalPassedTests);
            }

            [TestMethod]
            public void ZeroSuites()
            {
                var now = DateTime.Now;

                var suiteDtos = new List<SuiteDto>();

                var mockSuiteService = new Mock<ISuiteService>();
                mockSuiteService.Setup(s => s.GetAll()).Returns(suiteDtos);

                var temporaryTestResults = new List<TemporaryTestResult>();
                var mockTemporaryTestResults = temporaryTestResults.ToDbSetMock();
                mockTemporaryTestResults.Setup(m => m.AsNoTracking()).Returns(mockTemporaryTestResults.Object);

                var mockContext = new Mock<ZigNetEntities>();
                mockContext.Setup(m => m.TemporaryTestResults).Returns(mockTemporaryTestResults.Object);

                var zignetEntitiesWrapperMock = new Mock<IZigNetEntitiesWrapper>();
                zignetEntitiesWrapperMock.Setup(z => z.Get()).Returns(mockContext.Object);

                var latestSuiteResultsService = new LatestSuiteResultsService(zignetEntitiesWrapperMock.Object, mockSuiteService.Object);
                var suiteSummaries = latestSuiteResultsService.GetLatest().ToList();

                Assert.AreEqual(0, suiteSummaries.Count);
            }

            [TestMethod]
            public void ZeroTestResultsForSuite()
            {
                var now = DateTime.Now;

                var suiteDtos = new List<SuiteDto> {
                    new SuiteDto { SuiteID = 1, Name = "Services", ApplicationNameAbbreviation = "LN", EnvironmentNameAbbreviation = "TSM" }
                };
                var mockSuiteService = new Mock<ISuiteService>();
                mockSuiteService.Setup(s => s.GetAll()).Returns(suiteDtos);

                var suiteResult = new SuiteResult { SuiteResultEndDateTime = now };
                var temporaryTestResults = new List<TemporaryTestResult>();
                var mockTemporaryTestResults = temporaryTestResults.ToDbSetMock();
                mockTemporaryTestResults.Setup(m => m.AsNoTracking()).Returns(mockTemporaryTestResults.Object);

                var mockContext = new Mock<ZigNetEntities>();
                mockContext.Setup(m => m.TemporaryTestResults).Returns(mockTemporaryTestResults.Object);

                var zignetEntitiesWrapperMock = new Mock<IZigNetEntitiesWrapper>();
                zignetEntitiesWrapperMock.Setup(z => z.Get()).Returns(mockContext.Object);

                var latestSuiteResultsService = new LatestSuiteResultsService(zignetEntitiesWrapperMock.Object, mockSuiteService.Object);
                var suiteSummaries = latestSuiteResultsService.GetLatest().ToList();

                Assert.AreEqual(1, suiteSummaries.Count);
                Assert.AreEqual("LN Services (TSM)", suiteSummaries[0].SuiteName);
                Assert.AreEqual(1, suiteSummaries[0].SuiteIds.Count);
                Assert.AreEqual(1, suiteSummaries[0].SuiteIds[0]);
                Assert.IsNull(suiteSummaries[0].SuiteEndTime);
                Assert.AreEqual(0, suiteSummaries[0].TotalFailedTests);
                Assert.AreEqual(0, suiteSummaries[0].TotalInconclusiveTests);
                Assert.AreEqual(0, suiteSummaries[0].TotalPassedTests);
            }

            [TestMethod]
            public void OnlyPassedTests()
            {
                var now = DateTime.Now;

                var suiteDtos = new List<SuiteDto> {
                    new SuiteDto { SuiteID = 1, Name = "Services", ApplicationNameAbbreviation = "LN", EnvironmentNameAbbreviation = "TSM" }
                };
                var mockSuiteService = new Mock<ISuiteService>();
                mockSuiteService.Setup(s => s.GetAll()).Returns(suiteDtos);

                var suiteResult = new SuiteResult { SuiteResultEndDateTime = now };
                var temporaryTestResults = new List<TemporaryTestResult>
                {
                    new TemporaryTestResult { SuiteId = 1, TestResultTypeId = 3, SuiteResult = suiteResult },
                    new TemporaryTestResult { SuiteId = 1, TestResultTypeId = 3, SuiteResult = suiteResult },
                    new TemporaryTestResult { SuiteId = 1, TestResultTypeId = 3, SuiteResult = suiteResult }
                };
                var mockTemporaryTestResults = temporaryTestResults.ToDbSetMock();
                mockTemporaryTestResults.Setup(m => m.AsNoTracking()).Returns(mockTemporaryTestResults.Object);

                var mockContext = new Mock<ZigNetEntities>();
                mockContext.Setup(m => m.TemporaryTestResults).Returns(mockTemporaryTestResults.Object);

                var zignetEntitiesWrapperMock = new Mock<IZigNetEntitiesWrapper>();
                zignetEntitiesWrapperMock.Setup(z => z.Get()).Returns(mockContext.Object);

                var latestSuiteResultsService = new LatestSuiteResultsService(zignetEntitiesWrapperMock.Object, mockSuiteService.Object);
                var suiteSummaries = latestSuiteResultsService.GetLatest().ToList();

                Assert.AreEqual(1, suiteSummaries.Count);
                Assert.AreEqual("LN Services (TSM)", suiteSummaries[0].SuiteName);
                Assert.AreEqual(1, suiteSummaries[0].SuiteIds.Count);
                Assert.AreEqual(1, suiteSummaries[0].SuiteIds[0]);
                Assert.AreEqual(now, suiteSummaries[0].SuiteEndTime);
                Assert.AreEqual(0, suiteSummaries[0].TotalFailedTests);
                Assert.AreEqual(0, suiteSummaries[0].TotalInconclusiveTests);
                Assert.AreEqual(3, suiteSummaries[0].TotalPassedTests);
            }

            [TestMethod]
            public void NoEndTime()
            {
                var now = DateTime.Now;

                var suiteDtos = new List<SuiteDto> {
                    new SuiteDto { SuiteID = 1, Name = "Services", ApplicationNameAbbreviation = "LN", EnvironmentNameAbbreviation = "TSM" }
                };

                var mockSuiteService = new Mock<ISuiteService>();
                mockSuiteService.Setup(s => s.GetAll()).Returns(suiteDtos);

                var suiteResult = new SuiteResult { SuiteResultEndDateTime = null };
                var temporaryTestResults = new List<TemporaryTestResult>
                {
                    new TemporaryTestResult { SuiteId = 1, TestResultTypeId = 1, SuiteResult = suiteResult },
                    new TemporaryTestResult { SuiteId = 1, TestResultTypeId = 1, SuiteResult = suiteResult },
                    new TemporaryTestResult { SuiteId = 1, TestResultTypeId = 1, SuiteResult = suiteResult }
                };
                var mockTemporaryTestResults = temporaryTestResults.ToDbSetMock();
                mockTemporaryTestResults.Setup(m => m.AsNoTracking()).Returns(mockTemporaryTestResults.Object);

                var mockContext = new Mock<ZigNetEntities>();
                mockContext.Setup(m => m.TemporaryTestResults).Returns(mockTemporaryTestResults.Object);

                var zignetEntitiesWrapperMock = new Mock<IZigNetEntitiesWrapper>();
                zignetEntitiesWrapperMock.Setup(z => z.Get()).Returns(mockContext.Object);

                var latestSuiteResultsService = new LatestSuiteResultsService(zignetEntitiesWrapperMock.Object, mockSuiteService.Object);
                var suiteSummaries = latestSuiteResultsService.GetLatest().ToList();

                Assert.AreEqual(1, suiteSummaries.Count);
                Assert.AreEqual("LN Services (TSM)", suiteSummaries[0].SuiteName);
                Assert.AreEqual(1, suiteSummaries[0].SuiteIds.Count);
                Assert.AreEqual(1, suiteSummaries[0].SuiteIds[0]);
                Assert.IsNull(suiteSummaries[0].SuiteEndTime);
                Assert.AreEqual(3, suiteSummaries[0].TotalFailedTests);
                Assert.AreEqual(0, suiteSummaries[0].TotalInconclusiveTests);
                Assert.AreEqual(0, suiteSummaries[0].TotalPassedTests);
            }
        }

        [TestClass]
        public class GetLatestGrouped
        {
            [TestMethod]
            public void SingleSuite()
            {
                var suiteDto = new SuiteDto { ApplicationNameAbbreviation = "LN", EnvironmentNameAbbreviation = "TSM" };
                var now = DateTime.Now;

                var suites = new List<Suite>
                {
                    new Suite {
                        SuiteID = 1,
                        Application = new Application { ApplicationNameAbbreviation = suiteDto.ApplicationNameAbbreviation },
                        Environment = new Database.EntityFramework.Environment { EnvironmentNameAbbreviation = suiteDto.EnvironmentNameAbbreviation }
                    }
                };
                var mockSuites = suites.ToDbSetMock();
                mockSuites.Setup(m => m.AsNoTracking()).Returns(mockSuites.Object);
                mockSuites.Setup(m => m.Include(It.IsAny<string>())).Returns(mockSuites.Object);

                var temporaryTestResults = new List<TemporaryTestResult>
                {
                    new TemporaryTestResult { SuiteId = 1, TestResultTypeId = 1 },
                    new TemporaryTestResult { SuiteId = 1, TestResultTypeId = 2 },
                    new TemporaryTestResult { SuiteId = 1, TestResultTypeId = 3 }
                };
                var mockTemporaryTestResults = temporaryTestResults.ToDbSetMock();
                mockTemporaryTestResults.Setup(m => m.AsNoTracking()).Returns(mockTemporaryTestResults.Object);

                var mockContext = new Mock<ZigNetEntities>();
                mockContext.Setup(m => m.Suites).Returns(mockSuites.Object);
                mockContext.Setup(m => m.TemporaryTestResults).Returns(mockTemporaryTestResults.Object);

                var zignetEntitiesWrapperMock = new Mock<IZigNetEntitiesWrapper>();
                zignetEntitiesWrapperMock.Setup(z => z.Get()).Returns(mockContext.Object);

                var latestSuiteResultsService = new LatestSuiteResultsService(zignetEntitiesWrapperMock.Object, new Mock<ISuiteService>().Object);
                var suiteSummaries = latestSuiteResultsService.GetLatestGrouped().ToList();

                Assert.AreEqual(1, suiteSummaries.Count);
                Assert.AreEqual(suiteDto.GetNameGrouped(), suiteSummaries[0].SuiteName);
                Assert.AreEqual(1, suiteSummaries[0].SuiteIds.Count);
                Assert.AreEqual(1, suiteSummaries[0].SuiteIds[0]);
                Assert.IsNull(suiteSummaries[0].SuiteEndTime);
                Assert.AreEqual(1, suiteSummaries[0].TotalFailedTests);
                Assert.AreEqual(1, suiteSummaries[0].TotalInconclusiveTests);
                Assert.AreEqual(1, suiteSummaries[0].TotalPassedTests);
            }

            [TestMethod]
            public void MultipleSuitesInSameAppAndEnvironment()
            {
                var servicesSuiteDto = new SuiteDto { ApplicationNameAbbreviation = "LN", EnvironmentNameAbbreviation = "TSM" };
                var uiSuiteDto = new SuiteDto { ApplicationNameAbbreviation = "LN", EnvironmentNameAbbreviation = "TSM" };
                var now = DateTime.Now;

                var suites = new List<Suite>
                {
                    new Suite {
                        SuiteID = 1,
                        Application = new Application { ApplicationNameAbbreviation = servicesSuiteDto.ApplicationNameAbbreviation },
                        Environment = new Database.EntityFramework.Environment { EnvironmentNameAbbreviation = servicesSuiteDto.EnvironmentNameAbbreviation }
                    },
                    new Suite {
                        SuiteID = 2,
                        Application = new Application { ApplicationNameAbbreviation = uiSuiteDto.ApplicationNameAbbreviation },
                        Environment = new Database.EntityFramework.Environment { EnvironmentNameAbbreviation = uiSuiteDto.EnvironmentNameAbbreviation }
                    }
                };
                var mockSuites = suites.ToDbSetMock();
                mockSuites.Setup(m => m.AsNoTracking()).Returns(mockSuites.Object);
                mockSuites.Setup(m => m.Include(It.IsAny<string>())).Returns(mockSuites.Object);

                var temporaryTestResults = new List<TemporaryTestResult>
                {
                    new TemporaryTestResult { SuiteId = 1, TestResultTypeId = 1 },
                    new TemporaryTestResult { SuiteId = 1, TestResultTypeId = 2 },
                    new TemporaryTestResult { SuiteId = 1, TestResultTypeId = 3 },
                    new TemporaryTestResult { SuiteId = 1, TestResultTypeId = 3 },

                    new TemporaryTestResult { SuiteId = 2, TestResultTypeId = 2 },
                    new TemporaryTestResult { SuiteId = 2, TestResultTypeId = 3 }
                };
                var mockTemporaryTestResults = temporaryTestResults.ToDbSetMock();
                mockTemporaryTestResults.Setup(m => m.AsNoTracking()).Returns(mockTemporaryTestResults.Object);

                var mockContext = new Mock<ZigNetEntities>();
                mockContext.Setup(m => m.Suites).Returns(mockSuites.Object);
                mockContext.Setup(m => m.TemporaryTestResults).Returns(mockTemporaryTestResults.Object);

                var zignetEntitiesWrapperMock = new Mock<IZigNetEntitiesWrapper>();
                zignetEntitiesWrapperMock.Setup(z => z.Get()).Returns(mockContext.Object);

                var latestSuiteResultsService = new LatestSuiteResultsService(zignetEntitiesWrapperMock.Object, new Mock<ISuiteService>().Object);
                var suiteSummaries = latestSuiteResultsService.GetLatestGrouped().ToList();

                Assert.AreEqual(1, suiteSummaries.Count);
                Assert.AreEqual(servicesSuiteDto.GetNameGrouped(), suiteSummaries[0].SuiteName);
                Assert.AreEqual(2, suiteSummaries[0].SuiteIds.Count);
                var suiteIds = suiteSummaries[0].SuiteIds.OrderBy(s => s).ToList();
                Assert.AreEqual(1, suiteIds[0]);
                Assert.AreEqual(2, suiteIds[1]);
                Assert.IsNull(suiteSummaries[0].SuiteEndTime);
                Assert.AreEqual(1, suiteSummaries[0].TotalFailedTests);
                Assert.AreEqual(2, suiteSummaries[0].TotalInconclusiveTests);
                Assert.AreEqual(3, suiteSummaries[0].TotalPassedTests);
            }

            [TestMethod]
            public void MultipleSuitesInDifferentAppAndEnvironment()
            {
                var servicesSuiteDto = new SuiteDto { ApplicationNameAbbreviation = "LN", EnvironmentNameAbbreviation = "TSM" };
                var uiSuiteDto = new SuiteDto { ApplicationNameAbbreviation = "LN", EnvironmentNameAbbreviation = "TSR" };
                var now = DateTime.Now;

                var suites = new List<Suite>
                {
                    new Suite {
                        SuiteID = 1,
                        Application = new Application { ApplicationNameAbbreviation = servicesSuiteDto.ApplicationNameAbbreviation },
                        Environment = new Database.EntityFramework.Environment { EnvironmentNameAbbreviation = servicesSuiteDto.EnvironmentNameAbbreviation }
                    },
                    new Suite {
                        SuiteID = 2,
                        Application = new Application { ApplicationNameAbbreviation = uiSuiteDto.ApplicationNameAbbreviation },
                        Environment = new Database.EntityFramework.Environment { EnvironmentNameAbbreviation = uiSuiteDto.EnvironmentNameAbbreviation }
                    }
                };
                var mockSuites = suites.ToDbSetMock();
                mockSuites.Setup(m => m.AsNoTracking()).Returns(mockSuites.Object);
                mockSuites.Setup(m => m.Include(It.IsAny<string>())).Returns(mockSuites.Object);

                var temporaryTestResults = new List<TemporaryTestResult>
                {
                    new TemporaryTestResult { SuiteId = 1, TestResultTypeId = 3 },
                    new TemporaryTestResult { SuiteId = 1, TestResultTypeId = 3 },
                    new TemporaryTestResult { SuiteId = 1, TestResultTypeId = 3 },

                    new TemporaryTestResult { SuiteId = 2, TestResultTypeId = 2 },
                    new TemporaryTestResult { SuiteId = 2, TestResultTypeId = 3 },
                    new TemporaryTestResult { SuiteId = 2, TestResultTypeId = 3 }
                };
                var mockTemporaryTestResults = temporaryTestResults.ToDbSetMock();
                mockTemporaryTestResults.Setup(m => m.AsNoTracking()).Returns(mockTemporaryTestResults.Object);

                var mockContext = new Mock<ZigNetEntities>();
                mockContext.Setup(m => m.Suites).Returns(mockSuites.Object);
                mockContext.Setup(m => m.TemporaryTestResults).Returns(mockTemporaryTestResults.Object);

                var zignetEntitiesWrapperMock = new Mock<IZigNetEntitiesWrapper>();
                zignetEntitiesWrapperMock.Setup(z => z.Get()).Returns(mockContext.Object);

                var latestSuiteResultsService = new LatestSuiteResultsService(zignetEntitiesWrapperMock.Object, new Mock<ISuiteService>().Object);
                var suiteSummaries = latestSuiteResultsService.GetLatestGrouped().OrderBy(l => l.SuiteName).ToList();

                Assert.AreEqual(2, suiteSummaries.Count);

                Assert.AreEqual(servicesSuiteDto.GetNameGrouped(), suiteSummaries[0].SuiteName);
                Assert.AreEqual(1, suiteSummaries[0].SuiteIds.Count);
                Assert.AreEqual(1, suiteSummaries[0].SuiteIds[0]);
                Assert.IsNull(suiteSummaries[0].SuiteEndTime);
                Assert.AreEqual(0, suiteSummaries[0].TotalFailedTests);
                Assert.AreEqual(0, suiteSummaries[0].TotalInconclusiveTests);
                Assert.AreEqual(3, suiteSummaries[0].TotalPassedTests);

                Assert.AreEqual(uiSuiteDto.GetNameGrouped(), suiteSummaries[1].SuiteName);
                Assert.AreEqual(1, suiteSummaries[1].SuiteIds.Count);
                Assert.AreEqual(2, suiteSummaries[1].SuiteIds[0]);
                Assert.IsNull(suiteSummaries[1].SuiteEndTime);
                Assert.AreEqual(0, suiteSummaries[1].TotalFailedTests);
                Assert.AreEqual(1, suiteSummaries[1].TotalInconclusiveTests);
                Assert.AreEqual(2, suiteSummaries[1].TotalPassedTests);
            }

            [TestMethod]
            public void ZeroSuites()
            {
                var now = DateTime.Now;

                var suites = new List<Suite>();
                var mockSuites = suites.ToDbSetMock();
                mockSuites.Setup(m => m.AsNoTracking()).Returns(mockSuites.Object);
                mockSuites.Setup(m => m.Include(It.IsAny<string>())).Returns(mockSuites.Object);

                var temporaryTestResults = new List<TemporaryTestResult>();
                var mockTemporaryTestResults = temporaryTestResults.ToDbSetMock();
                mockTemporaryTestResults.Setup(m => m.AsNoTracking()).Returns(mockTemporaryTestResults.Object);

                var mockContext = new Mock<ZigNetEntities>();
                mockContext.Setup(m => m.Suites).Returns(mockSuites.Object);
                mockContext.Setup(m => m.TemporaryTestResults).Returns(mockTemporaryTestResults.Object);

                var zignetEntitiesWrapperMock = new Mock<IZigNetEntitiesWrapper>();
                zignetEntitiesWrapperMock.Setup(z => z.Get()).Returns(mockContext.Object);

                var latestSuiteResultsService = new LatestSuiteResultsService(zignetEntitiesWrapperMock.Object, new Mock<ISuiteService>().Object);
                var suiteSummaries = latestSuiteResultsService.GetLatestGrouped().ToList();

                Assert.AreEqual(0, suiteSummaries.Count);
            }

            [TestMethod]
            public void ZeroTestsInSuite()
            {
                var servicesSuiteDto = new SuiteDto { ApplicationNameAbbreviation = "LN", EnvironmentNameAbbreviation = "TSM" };
                var uiSuiteDto = new SuiteDto { ApplicationNameAbbreviation = "LN", EnvironmentNameAbbreviation = "TSM" };
                var now = DateTime.Now;

                var suites = new List<Suite>
                {
                    new Suite {
                        SuiteID = 1,
                        Application = new Application { ApplicationNameAbbreviation = servicesSuiteDto.ApplicationNameAbbreviation },
                        Environment = new Database.EntityFramework.Environment { EnvironmentNameAbbreviation = servicesSuiteDto.EnvironmentNameAbbreviation }
                    },
                    new Suite {
                        SuiteID = 2,
                        Application = new Application { ApplicationNameAbbreviation = uiSuiteDto.ApplicationNameAbbreviation },
                        Environment = new Database.EntityFramework.Environment { EnvironmentNameAbbreviation = uiSuiteDto.EnvironmentNameAbbreviation }
                    }
                };
                var mockSuites = suites.ToDbSetMock();
                mockSuites.Setup(m => m.AsNoTracking()).Returns(mockSuites.Object);
                mockSuites.Setup(m => m.Include(It.IsAny<string>())).Returns(mockSuites.Object);

                var temporaryTestResults = new List<TemporaryTestResult>
                {
                    new TemporaryTestResult { SuiteId = 1, TestResultTypeId = 1 },
                    new TemporaryTestResult { SuiteId = 1, TestResultTypeId = 1 },
                    new TemporaryTestResult { SuiteId = 1, TestResultTypeId = 1 }
                };
                var mockTemporaryTestResults = temporaryTestResults.ToDbSetMock();
                mockTemporaryTestResults.Setup(m => m.AsNoTracking()).Returns(mockTemporaryTestResults.Object);

                var mockContext = new Mock<ZigNetEntities>();
                mockContext.Setup(m => m.Suites).Returns(mockSuites.Object);
                mockContext.Setup(m => m.TemporaryTestResults).Returns(mockTemporaryTestResults.Object);

                var zignetEntitiesWrapperMock = new Mock<IZigNetEntitiesWrapper>();
                zignetEntitiesWrapperMock.Setup(z => z.Get()).Returns(mockContext.Object);

                var latestSuiteResultsService = new LatestSuiteResultsService(zignetEntitiesWrapperMock.Object, new Mock<ISuiteService>().Object);
                var suiteSummaries = latestSuiteResultsService.GetLatestGrouped().ToList();

                Assert.AreEqual(1, suiteSummaries.Count);
                Assert.AreEqual(servicesSuiteDto.GetNameGrouped(), suiteSummaries[0].SuiteName);
                Assert.AreEqual(2, suiteSummaries[0].SuiteIds.Count);
                var suiteIds = suiteSummaries[0].SuiteIds.OrderBy(s => s).ToList();
                Assert.AreEqual(1, suiteIds[0]);
                Assert.AreEqual(2, suiteIds[1]);
                Assert.IsNull(suiteSummaries[0].SuiteEndTime);
                Assert.AreEqual(3, suiteSummaries[0].TotalFailedTests);
                Assert.AreEqual(0, suiteSummaries[0].TotalInconclusiveTests);
                Assert.AreEqual(0, suiteSummaries[0].TotalPassedTests);
            }
        }
    }
}
