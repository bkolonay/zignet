using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using ZigNet.Database.EntityFramework;
using ZigNet.Services.EntityFramework.Tests.Helpers;
using DomainTest = ZigNet.Domain.Test.Test;
using DomainTestCategory = ZigNet.Domain.Test.TestCategory;
using DomainTestResult = ZigNet.Domain.Test.TestResult;
using DomainSuiteResult = ZigNet.Domain.Suite.SuiteResult;
using DomainTestResultType = ZigNet.Domain.Test.TestResultType;
using DomainTestFailureType = ZigNet.Domain.Test.TestFailureType;
using DomainTestFailureDetails = ZigNet.Domain.Test.TestFailureDetails;
using TestFailureDuration = ZigNet.Database.EntityFramework.TestFailureDuration;
using ZigNet.Services.DTOs;

namespace ZigNet.Services.EntityFramework.Tests
{
    public class TestResultsServiceTests
    {
        [TestClass]
        public class GetLatestResults
        {
            [TestMethod]
            public void SingleTestResultZeroFailures()
            {
                var utcNow = DateTime.UtcNow;

                var latestTestResultDtos = new List<LatestTestResultDto> {
                    new LatestTestResultDto {
                        SuiteId = 1,
                        TestResultID = 2,
                        TestName = "test1",
                        SuiteName = "suite-name",
                        PassingFromDate = utcNow,
                    }
                };
                var mockLatestTestResultsService = new Mock<ILatestTestResultsService>();
                mockLatestTestResultsService.Setup(s => s.Get(1)).Returns(latestTestResultDtos);

                var mockTestFailureDurations = new List<TestFailureDuration>().ToDbSetMock();
                mockTestFailureDurations.Setup(m => m.AsNoTracking()).Returns(mockTestFailureDurations.Object);

                var mockContext = new Mock<ZigNetEntities>();
                mockContext.Setup(m => m.TestFailureDurations).Returns(mockTestFailureDurations.Object);

                var zignetEntitiesWrapperMock = new Mock<IZigNetEntitiesWrapper>();
                zignetEntitiesWrapperMock.Setup(z => z.Get()).Returns(mockContext.Object);

                var testResultService = new TestResultService(zignetEntitiesWrapperMock.Object,
                    new Mock<ISuiteService>().Object, mockLatestTestResultsService.Object);
                var actualLatestTestResults = testResultService.GetLatestResults(1, false).ToList();

                Assert.AreEqual(1, actualLatestTestResults.Count);
                Assert.AreEqual(2, actualLatestTestResults[0].TestResultID);
                Assert.AreEqual("test1", actualLatestTestResults[0].TestName);
                Assert.AreEqual("suite-name", actualLatestTestResults[0].SuiteName);
                Assert.AreEqual(utcNow, actualLatestTestResults[0].PassingFromDate);
                Assert.IsNull(actualLatestTestResults[0].FailingFromDate);
                Assert.AreEqual(0, actualLatestTestResults[0].TestFailureDurations.ToList().Count);
            }

            [TestMethod]
            public void ZeroTestResults()
            {
                var latestTestResultDtos = new List<LatestTestResultDto>();
                var mockLatestTestResultsService = new Mock<ILatestTestResultsService>();
                mockLatestTestResultsService.Setup(s => s.Get(1)).Returns(latestTestResultDtos);

                var mockTestFailureDurations = new List<TestFailureDuration>().ToDbSetMock();
                mockTestFailureDurations.Setup(m => m.AsNoTracking()).Returns(mockTestFailureDurations.Object);

                var mockContext = new Mock<ZigNetEntities>();
                mockContext.Setup(m => m.TestFailureDurations).Returns(mockTestFailureDurations.Object);

                var zignetEntitiesWrapperMock = new Mock<IZigNetEntitiesWrapper>();
                zignetEntitiesWrapperMock.Setup(z => z.Get()).Returns(mockContext.Object);

                var testResultService = new TestResultService(zignetEntitiesWrapperMock.Object,
                    new Mock<ISuiteService>().Object, mockLatestTestResultsService.Object);
                var actualLatestTestResults = testResultService.GetLatestResults(1, false).ToList();

                Assert.AreEqual(0, actualLatestTestResults.Count);
            }

            [TestMethod]
            public void SortsByFailingTheLongestThenPassingTheShortest()
            {
                var utcNow = DateTime.UtcNow;

                var latestTestResultDtos = new List<LatestTestResultDto> {
                    new LatestTestResultDto {
                        SuiteId = 1,
                        TestResultID = 2,
                        TestName = "test passing the longest",
                        PassingFromDate = new DateTime(2018, 3, 1, 1, 00, 00),
                    },
                    new LatestTestResultDto
                    {
                        SuiteId = 1,
                        TestResultID = 3,
                        TestName = "test failing the longest",
                        FailingFromDate = new DateTime(2018, 3, 1, 1, 00, 00),
                    },
                    new LatestTestResultDto
                    {
                        SuiteId = 1,
                        TestResultID = 4,
                        TestName = "test passing the shortest",
                        PassingFromDate = new DateTime(2018, 3, 1, 1, 01, 00),
                    },
                    new LatestTestResultDto
                    {
                        SuiteId = 1,
                        TestResultID = 5,
                        TestName = "test failing the shortest",
                        FailingFromDate = new DateTime(2018, 3, 1, 1, 01, 00),
                    }
                };
                var mockLatestTestResultsService = new Mock<ILatestTestResultsService>();
                mockLatestTestResultsService.Setup(s => s.Get(1)).Returns(latestTestResultDtos);

                var mockTestFailureDurations = new List<TestFailureDuration>().ToDbSetMock();
                mockTestFailureDurations.Setup(m => m.AsNoTracking()).Returns(mockTestFailureDurations.Object);

                var mockContext = new Mock<ZigNetEntities>();
                mockContext.Setup(m => m.TestFailureDurations).Returns(mockTestFailureDurations.Object);

                var zignetEntitiesWrapperMock = new Mock<IZigNetEntitiesWrapper>();
                zignetEntitiesWrapperMock.Setup(z => z.Get()).Returns(mockContext.Object);

                var testResultService = new TestResultService(zignetEntitiesWrapperMock.Object,
                    new Mock<ISuiteService>().Object, mockLatestTestResultsService.Object);
                var actualLatestTestResults = testResultService.GetLatestResults(1, false).ToList();

                Assert.AreEqual(4, actualLatestTestResults.Count);
                Assert.AreEqual("test failing the longest", actualLatestTestResults[0].TestName);
                Assert.AreEqual("test failing the shortest", actualLatestTestResults[1].TestName);
                Assert.AreEqual("test passing the shortest", actualLatestTestResults[2].TestName);
                Assert.AreEqual("test passing the longest", actualLatestTestResults[3].TestName);
            }

            [TestMethod]
            public void GroupsSingleSuite()
            {
                var utcNow = DateTime.UtcNow;

                var suiteDto = new SuiteDto { SuiteID = 1, ApplicationId = 4, EnvironmentId = 3 };
                var suiteDtos = new List<SuiteDto> { suiteDto };
                var mockSuiteService = new Mock<ISuiteService>();
                mockSuiteService.Setup(s => s.Get(1)).Returns(suiteDto);
                mockSuiteService.Setup(s => s.GetAll()).Returns(suiteDtos);

                var latestTestResultDtos = new List<LatestTestResultDto> {
                    new LatestTestResultDto {
                        SuiteId = 1,
                        TestResultID = 2,
                        TestName = "test1",
                        PassingFromDate = utcNow,
                    }
                };
                var mockLatestTestResultsService = new Mock<ILatestTestResultsService>();
                mockLatestTestResultsService.Setup(s => s.Get(new int[] { 1 })).Returns(latestTestResultDtos);

                var mockTestFailureDurations = new List<TestFailureDuration>().ToDbSetMock();
                mockTestFailureDurations.Setup(m => m.AsNoTracking()).Returns(mockTestFailureDurations.Object);

                var mockContext = new Mock<ZigNetEntities>();
                mockContext.Setup(m => m.TestFailureDurations).Returns(mockTestFailureDurations.Object);

                var zignetEntitiesWrapperMock = new Mock<IZigNetEntitiesWrapper>();
                zignetEntitiesWrapperMock.Setup(z => z.Get()).Returns(mockContext.Object);

                var testResultService = new TestResultService(zignetEntitiesWrapperMock.Object,
                    mockSuiteService.Object, mockLatestTestResultsService.Object);
                var actualLatestTestResults = testResultService.GetLatestResults(1, true).ToList();

                Assert.AreEqual(1, actualLatestTestResults.Count);
                Assert.AreEqual(2, actualLatestTestResults[0].TestResultID);
                Assert.AreEqual("test1", actualLatestTestResults[0].TestName);
                Assert.AreEqual(utcNow, actualLatestTestResults[0].PassingFromDate);
                Assert.IsNull(actualLatestTestResults[0].FailingFromDate);
                Assert.AreEqual(0, actualLatestTestResults[0].TestFailureDurations.ToList().Count);
            }

            [TestMethod]
            public void GroupsMultipleSuites()
            {
                var utcNow = DateTime.UtcNow;

                var suiteDto = new SuiteDto { SuiteID = 1, ApplicationId = 4, EnvironmentId = 3 };
                var suiteDtos = new List<SuiteDto> {
                    suiteDto,
                    new SuiteDto { SuiteID = 5, ApplicationId = 4, EnvironmentId = 3 }
                };
                var mockSuiteService = new Mock<ISuiteService>();
                mockSuiteService.Setup(s => s.Get(1)).Returns(suiteDto);
                mockSuiteService.Setup(s => s.GetAll()).Returns(suiteDtos);

                var latestTestResultDtos = new List<LatestTestResultDto> {
                    new LatestTestResultDto {
                        SuiteId = 1,
                        TestResultID = 2,
                        TestName = "test1",
                        PassingFromDate = utcNow,
                    },
                    new LatestTestResultDto {
                        SuiteId = 5,
                        TestResultID = 6,
                        TestName = "test2",
                        FailingFromDate = utcNow,
                    }
                };
                var mockLatestTestResultsService = new Mock<ILatestTestResultsService>();
                mockLatestTestResultsService.Setup(s => s.Get(new int[] { 1, 5 })).Returns(latestTestResultDtos);

                var mockTestFailureDurations = new List<TestFailureDuration>().ToDbSetMock();
                mockTestFailureDurations.Setup(m => m.AsNoTracking()).Returns(mockTestFailureDurations.Object);

                var mockContext = new Mock<ZigNetEntities>();
                mockContext.Setup(m => m.TestFailureDurations).Returns(mockTestFailureDurations.Object);

                var zignetEntitiesWrapperMock = new Mock<IZigNetEntitiesWrapper>();
                zignetEntitiesWrapperMock.Setup(z => z.Get()).Returns(mockContext.Object);

                var testResultService = new TestResultService(zignetEntitiesWrapperMock.Object,
                    mockSuiteService.Object, mockLatestTestResultsService.Object);
                var actualLatestTestResults = testResultService.GetLatestResults(1, true).OrderBy(l => l.TestResultID).ToList();

                Assert.AreEqual(2, actualLatestTestResults.Count);

                Assert.AreEqual(2, actualLatestTestResults[0].TestResultID);
                Assert.AreEqual("test1", actualLatestTestResults[0].TestName);
                Assert.AreEqual(utcNow, actualLatestTestResults[0].PassingFromDate);
                Assert.IsNull(actualLatestTestResults[0].FailingFromDate);
                Assert.AreEqual(0, actualLatestTestResults[0].TestFailureDurations.ToList().Count);

                Assert.AreEqual(6, actualLatestTestResults[1].TestResultID);
                Assert.AreEqual("test2", actualLatestTestResults[1].TestName);
                Assert.AreEqual(utcNow, actualLatestTestResults[1].FailingFromDate);
                Assert.IsNull(actualLatestTestResults[1].PassingFromDate);
                Assert.AreEqual(0, actualLatestTestResults[1].TestFailureDurations.ToList().Count);
            }

            [TestMethod]
            public void IgnoresSuitesWithoutEnvironmentIdWhenGrouped()
            {
                var utcNow = DateTime.UtcNow;

                var suiteDto = new SuiteDto { SuiteID = 1, ApplicationId = 4, EnvironmentId = 3 };
                var suiteDtos = new List<SuiteDto> {
                    suiteDto,
                    new SuiteDto { SuiteID = 5, EnvironmentId = 4, ApplicationId = 4 }
                };
                var mockSuiteService = new Mock<ISuiteService>();
                mockSuiteService.Setup(s => s.Get(1)).Returns(suiteDto);
                mockSuiteService.Setup(s => s.GetAll()).Returns(suiteDtos);

                var latestTestResultDtos = new List<LatestTestResultDto> {
                    new LatestTestResultDto {
                        SuiteId = 1,
                        TestResultID = 2,
                        TestName = "test1",
                        PassingFromDate = utcNow,
                    }
                };
                var mockLatestTestResultsService = new Mock<ILatestTestResultsService>();
                mockLatestTestResultsService.Setup(s => s.Get(new int[] { 1 })).Returns(latestTestResultDtos);

                var mockTestFailureDurations = new List<TestFailureDuration>().ToDbSetMock();
                mockTestFailureDurations.Setup(m => m.AsNoTracking()).Returns(mockTestFailureDurations.Object);

                var mockContext = new Mock<ZigNetEntities>();
                mockContext.Setup(m => m.TestFailureDurations).Returns(mockTestFailureDurations.Object);

                var zignetEntitiesWrapperMock = new Mock<IZigNetEntitiesWrapper>();
                zignetEntitiesWrapperMock.Setup(z => z.Get()).Returns(mockContext.Object);

                var testResultService = new TestResultService(zignetEntitiesWrapperMock.Object,
                    mockSuiteService.Object, mockLatestTestResultsService.Object);
                var actualLatestTestResults = testResultService.GetLatestResults(1, true).ToList();

                Assert.AreEqual(1, actualLatestTestResults.Count);
                Assert.AreEqual(2, actualLatestTestResults[0].TestResultID);
                Assert.AreEqual("test1", actualLatestTestResults[0].TestName);
                Assert.AreEqual(utcNow, actualLatestTestResults[0].PassingFromDate);
                Assert.IsNull(actualLatestTestResults[0].FailingFromDate);
            }

            [TestMethod]
            public void IgnoresSuitesWithoutApplicationIdWhenGrouped()
            {
                var utcNow = DateTime.UtcNow;

                var suiteDto = new SuiteDto { SuiteID = 1, ApplicationId = 3, EnvironmentId = 4 };
                var suiteDtos = new List<SuiteDto> {
                    suiteDto,
                    new SuiteDto { SuiteID = 5, EnvironmentId = 3, ApplicationId = 5 }
                };
                var mockSuiteService = new Mock<ISuiteService>();
                mockSuiteService.Setup(s => s.Get(1)).Returns(suiteDto);
                mockSuiteService.Setup(s => s.GetAll()).Returns(suiteDtos);

                var latestTestResultDtos = new List<LatestTestResultDto> {
                    new LatestTestResultDto {
                        SuiteId = 1,
                        TestResultID = 2,
                        TestName = "test1",
                        PassingFromDate = utcNow,
                    }
                };
                var mockLatestTestResultsService = new Mock<ILatestTestResultsService>();
                mockLatestTestResultsService.Setup(s => s.Get(new int[] { 1 })).Returns(latestTestResultDtos);

                var mockTestFailureDurations = new List<TestFailureDuration>().ToDbSetMock();
                mockTestFailureDurations.Setup(m => m.AsNoTracking()).Returns(mockTestFailureDurations.Object);

                var mockContext = new Mock<ZigNetEntities>();
                mockContext.Setup(m => m.TestFailureDurations).Returns(mockTestFailureDurations.Object);

                var zignetEntitiesWrapperMock = new Mock<IZigNetEntitiesWrapper>();
                zignetEntitiesWrapperMock.Setup(z => z.Get()).Returns(mockContext.Object);

                var testResultService = new TestResultService(zignetEntitiesWrapperMock.Object,
                    mockSuiteService.Object, mockLatestTestResultsService.Object);
                var actualLatestTestResults = testResultService.GetLatestResults(1, true).ToList();

                Assert.AreEqual(1, actualLatestTestResults.Count);
                Assert.AreEqual(2, actualLatestTestResults[0].TestResultID);
                Assert.AreEqual("test1", actualLatestTestResults[0].TestName);
                Assert.AreEqual(utcNow, actualLatestTestResults[0].PassingFromDate);
                Assert.IsNull(actualLatestTestResults[0].FailingFromDate);
            }

            [TestMethod]
            public void GroupedAndZeroTestResults()
            {
                var utcNow = DateTime.UtcNow;

                var suiteDto = new SuiteDto { SuiteID = 1, ApplicationId = 3, EnvironmentId = 4 };
                var suiteDtos = new List<SuiteDto> {
                    suiteDto,
                    new SuiteDto { SuiteID = 5, EnvironmentId = 3, ApplicationId = 4 }
                };
                var mockSuiteService = new Mock<ISuiteService>();
                mockSuiteService.Setup(s => s.Get(1)).Returns(suiteDto);
                mockSuiteService.Setup(s => s.GetAll()).Returns(suiteDtos);

                var latestTestResultDtos = new List<LatestTestResultDto>();
                var mockLatestTestResultsService = new Mock<ILatestTestResultsService>();
                mockLatestTestResultsService.Setup(s => s.Get(new int[] { 1 })).Returns(latestTestResultDtos);

                var mockTestFailureDurations = new List<TestFailureDuration>().ToDbSetMock();
                mockTestFailureDurations.Setup(m => m.AsNoTracking()).Returns(mockTestFailureDurations.Object);

                var mockContext = new Mock<ZigNetEntities>();
                mockContext.Setup(m => m.TestFailureDurations).Returns(mockTestFailureDurations.Object);

                var zignetEntitiesWrapperMock = new Mock<IZigNetEntitiesWrapper>();
                zignetEntitiesWrapperMock.Setup(z => z.Get()).Returns(mockContext.Object);

                var testResultService = new TestResultService(zignetEntitiesWrapperMock.Object,
                    mockSuiteService.Object, mockLatestTestResultsService.Object);
                var actualLatestTestResults = testResultService.GetLatestResults(1, true).ToList();

                Assert.AreEqual(0, actualLatestTestResults.Count);
            }

            [TestMethod]
            public void SingleTestFailureDurationWithEndDate()
            {
                var utcNow = DateTime.UtcNow;

                var latestTestResultDtos = new List<LatestTestResultDto> {
                    new LatestTestResultDto {
                        SuiteId = 1,
                        TestId = 2,
                        PassingFromDate = utcNow,
                    }
                };
                var mockLatestTestResultsService = new Mock<ILatestTestResultsService>();
                mockLatestTestResultsService.Setup(s => s.Get(1)).Returns(latestTestResultDtos);

                var mockTestFailureDurations = new List<TestFailureDuration>
                {
                    new TestFailureDuration { SuiteId = 1, TestId = 2, FailureEndDateTime = utcNow }
                }.ToDbSetMock();
                mockTestFailureDurations.Setup(m => m.AsNoTracking()).Returns(mockTestFailureDurations.Object);

                var mockContext = new Mock<ZigNetEntities>();
                mockContext.Setup(m => m.TestFailureDurations).Returns(mockTestFailureDurations.Object);

                var zignetEntitiesWrapperMock = new Mock<IZigNetEntitiesWrapper>();
                zignetEntitiesWrapperMock.Setup(z => z.Get()).Returns(mockContext.Object);

                var testResultService = new TestResultService(zignetEntitiesWrapperMock.Object,
                    new Mock<ISuiteService>().Object, mockLatestTestResultsService.Object);
                var actualLatestTestResults = testResultService.GetLatestResults(1, false).ToList();

                Assert.AreEqual(1, actualLatestTestResults.Count);
                Assert.AreEqual(1, actualLatestTestResults[0].TestFailureDurations.Count());
                Assert.AreEqual(utcNow, actualLatestTestResults[0].TestFailureDurations.ToList()[0].FailureEnd);
            }

            [TestMethod]
            public void SingleTestFailureDurationWithoutEndDate()
            {
                var utcNow = DateTime.UtcNow;

                var latestTestResultDtos = new List<LatestTestResultDto> {
                    new LatestTestResultDto {
                        SuiteId = 1,
                        TestId = 2,
                        PassingFromDate = utcNow,
                    }
                };
                var mockLatestTestResultsService = new Mock<ILatestTestResultsService>();
                mockLatestTestResultsService.Setup(s => s.Get(1)).Returns(latestTestResultDtos);

                var mockTestFailureDurations = new List<TestFailureDuration>
                {
                    new TestFailureDuration { SuiteId = 1, TestId = 2 }
                }.ToDbSetMock();
                mockTestFailureDurations.Setup(m => m.AsNoTracking()).Returns(mockTestFailureDurations.Object);

                var mockContext = new Mock<ZigNetEntities>();
                mockContext.Setup(m => m.TestFailureDurations).Returns(mockTestFailureDurations.Object);

                var zignetEntitiesWrapperMock = new Mock<IZigNetEntitiesWrapper>();
                zignetEntitiesWrapperMock.Setup(z => z.Get()).Returns(mockContext.Object);

                var testResultService = new TestResultService(zignetEntitiesWrapperMock.Object,
                    new Mock<ISuiteService>().Object, mockLatestTestResultsService.Object);
                var actualLatestTestResults = testResultService.GetLatestResults(1, false).ToList();

                Assert.AreEqual(1, actualLatestTestResults.Count);
                Assert.AreEqual(1, actualLatestTestResults[0].TestFailureDurations.Count());
                Assert.IsNull(actualLatestTestResults[0].TestFailureDurations.ToList()[0].FailureEnd);
            }

            [TestMethod]
            public void MultipleTestFailureDurationsWithAndWithoutEndDate()
            {
                var utcNow = DateTime.UtcNow;

                var latestTestResultDtos = new List<LatestTestResultDto> {
                    new LatestTestResultDto {
                        SuiteId = 1,
                        TestId = 2,
                        PassingFromDate = utcNow,
                    }
                };
                var mockLatestTestResultsService = new Mock<ILatestTestResultsService>();
                mockLatestTestResultsService.Setup(s => s.Get(1)).Returns(latestTestResultDtos);

                var mockTestFailureDurations = new List<TestFailureDuration>
                {
                    new TestFailureDuration { SuiteId = 1, TestId = 2, FailureStartDateTime = utcNow },
                    new TestFailureDuration { SuiteId = 1, TestId = 2, FailureStartDateTime = utcNow.AddMinutes(5), FailureEndDateTime = utcNow.AddMinutes(10) }
                }.ToDbSetMock();
                mockTestFailureDurations.Setup(m => m.AsNoTracking()).Returns(mockTestFailureDurations.Object);

                var mockContext = new Mock<ZigNetEntities>();
                mockContext.Setup(m => m.TestFailureDurations).Returns(mockTestFailureDurations.Object);

                var zignetEntitiesWrapperMock = new Mock<IZigNetEntitiesWrapper>();
                zignetEntitiesWrapperMock.Setup(z => z.Get()).Returns(mockContext.Object);

                var testResultService = new TestResultService(zignetEntitiesWrapperMock.Object,
                    new Mock<ISuiteService>().Object, mockLatestTestResultsService.Object);
                var actualLatestTestResults = testResultService.GetLatestResults(1, false).ToList();

                Assert.AreEqual(1, actualLatestTestResults.Count);
                Assert.AreEqual(2, actualLatestTestResults[0].TestFailureDurations.Count());
                Assert.AreEqual(utcNow, actualLatestTestResults[0].TestFailureDurations.ToList()[0].FailureStart);
                Assert.IsNull(actualLatestTestResults[0].TestFailureDurations.ToList()[0].FailureEnd);
                Assert.AreEqual(utcNow.AddMinutes(5), actualLatestTestResults[0].TestFailureDurations.ToList()[1].FailureStart);
                Assert.AreEqual(utcNow.AddMinutes(10), actualLatestTestResults[0].TestFailureDurations.ToList()[1].FailureEnd);
            }

            [TestMethod]
            public void IgnoresTestFailureDurationsWithEndDatePast24Hours()
            {
                var utcNow = DateTime.UtcNow;

                var latestTestResultDtos = new List<LatestTestResultDto> {
                    new LatestTestResultDto {
                        SuiteId = 1,
                        TestId = 2,
                        PassingFromDate = utcNow,
                    }
                };
                var mockLatestTestResultsService = new Mock<ILatestTestResultsService>();
                mockLatestTestResultsService.Setup(s => s.Get(1)).Returns(latestTestResultDtos);

                var mockTestFailureDurations = new List<TestFailureDuration>
                {
                    new TestFailureDuration { SuiteId = 1, TestId = 2, FailureEndDateTime = utcNow.AddHours(-25) },
                    new TestFailureDuration { SuiteId = 1, TestId = 2, FailureEndDateTime = utcNow.AddHours(-23) },
                    new TestFailureDuration { SuiteId = 1, TestId = 2, FailureEndDateTime = utcNow }
                }.ToDbSetMock();
                mockTestFailureDurations.Setup(m => m.AsNoTracking()).Returns(mockTestFailureDurations.Object);

                var mockContext = new Mock<ZigNetEntities>();
                mockContext.Setup(m => m.TestFailureDurations).Returns(mockTestFailureDurations.Object);

                var zignetEntitiesWrapperMock = new Mock<IZigNetEntitiesWrapper>();
                zignetEntitiesWrapperMock.Setup(z => z.Get()).Returns(mockContext.Object);

                var testResultService = new TestResultService(zignetEntitiesWrapperMock.Object,
                    new Mock<ISuiteService>().Object, mockLatestTestResultsService.Object);
                var latestTestResults = testResultService.GetLatestResults(1, false).ToList();

                Assert.AreEqual(1, latestTestResults.Count);
                Assert.AreEqual(2, latestTestResults[0].TestFailureDurations.Count());
                Assert.AreEqual(utcNow.AddHours(-23), latestTestResults[0].TestFailureDurations.ToList()[0].FailureEnd);
                Assert.AreEqual(utcNow, latestTestResults[0].TestFailureDurations.ToList()[1].FailureEnd);
            }

            [TestMethod]
            public void IgnoresTestFailureDurationsWithoutSuiteId()
            {
                var utcNow = DateTime.UtcNow;

                var latestTestResultDtos = new List<LatestTestResultDto> {
                    new LatestTestResultDto {
                        SuiteId = 1,
                        TestId = 2,
                        PassingFromDate = utcNow,
                    }
                };
                var mockLatestTestResultsService = new Mock<ILatestTestResultsService>();
                mockLatestTestResultsService.Setup(s => s.Get(1)).Returns(latestTestResultDtos);

                var mockTestFailureDurations = new List<TestFailureDuration>
                {
                    new TestFailureDuration { SuiteId = 99, TestId = 2, FailureEndDateTime = utcNow }
                }.ToDbSetMock();
                mockTestFailureDurations.Setup(m => m.AsNoTracking()).Returns(mockTestFailureDurations.Object);

                var mockContext = new Mock<ZigNetEntities>();
                mockContext.Setup(m => m.TestFailureDurations).Returns(mockTestFailureDurations.Object);

                var zignetEntitiesWrapperMock = new Mock<IZigNetEntitiesWrapper>();
                zignetEntitiesWrapperMock.Setup(z => z.Get()).Returns(mockContext.Object);

                var testResultService = new TestResultService(zignetEntitiesWrapperMock.Object,
                    new Mock<ISuiteService>().Object, mockLatestTestResultsService.Object);
                var latestTestResults = testResultService.GetLatestResults(1, false).ToList();

                Assert.AreEqual(1, latestTestResults.Count);
                Assert.AreEqual(0, latestTestResults[0].TestFailureDurations.Count());
            }

            [TestMethod]
            public void IgnoresTestFailureDurationsWithoutTestId()
            {
                var utcNow = DateTime.UtcNow;

                var latestTestResultDtos = new List<LatestTestResultDto> {
                    new LatestTestResultDto {
                        SuiteId = 1,
                        TestId = 2,
                        PassingFromDate = utcNow,
                    }
                };
                var mockLatestTestResultsService = new Mock<ILatestTestResultsService>();
                mockLatestTestResultsService.Setup(s => s.Get(1)).Returns(latestTestResultDtos);

                var mockTestFailureDurations = new List<TestFailureDuration>
                {
                    new TestFailureDuration { SuiteId = 1, TestId = 99, FailureEndDateTime = utcNow }
                }.ToDbSetMock();
                mockTestFailureDurations.Setup(m => m.AsNoTracking()).Returns(mockTestFailureDurations.Object);

                var mockContext = new Mock<ZigNetEntities>();
                mockContext.Setup(m => m.TestFailureDurations).Returns(mockTestFailureDurations.Object);

                var zignetEntitiesWrapperMock = new Mock<IZigNetEntitiesWrapper>();
                zignetEntitiesWrapperMock.Setup(z => z.Get()).Returns(mockContext.Object);

                var testResultService = new TestResultService(zignetEntitiesWrapperMock.Object,
                    new Mock<ISuiteService>().Object, mockLatestTestResultsService.Object);
                var latestTestResults = testResultService.GetLatestResults(1, false).ToList();

                Assert.AreEqual(1, latestTestResults.Count);
                Assert.AreEqual(0, latestTestResults[0].TestFailureDurations.Count());
            }

            [TestMethod]
            public void GetsTestFailureDurationsWithStartDateOutside24Hours()
            {
                var utcNow = DateTime.UtcNow;

                var latestTestResultDtos = new List<LatestTestResultDto> {
                    new LatestTestResultDto {
                        SuiteId = 1,
                        TestId = 2,
                        PassingFromDate = utcNow,
                    }
                };
                var mockLatestTestResultsService = new Mock<ILatestTestResultsService>();
                mockLatestTestResultsService.Setup(s => s.Get(1)).Returns(latestTestResultDtos);

                var mockTestFailureDurations = new List<TestFailureDuration>
                {
                    new TestFailureDuration { SuiteId = 1, TestId = 2, FailureStartDateTime = utcNow.AddHours(-48), FailureEndDateTime = utcNow.AddHours(-23) }
                }.ToDbSetMock();
                mockTestFailureDurations.Setup(m => m.AsNoTracking()).Returns(mockTestFailureDurations.Object);

                var mockContext = new Mock<ZigNetEntities>();
                mockContext.Setup(m => m.TestFailureDurations).Returns(mockTestFailureDurations.Object);

                var zignetEntitiesWrapperMock = new Mock<IZigNetEntitiesWrapper>();
                zignetEntitiesWrapperMock.Setup(z => z.Get()).Returns(mockContext.Object);

                var testResultService = new TestResultService(zignetEntitiesWrapperMock.Object,
                    new Mock<ISuiteService>().Object, mockLatestTestResultsService.Object);
                var latestTestResults = testResultService.GetLatestResults(1, false).ToList();

                Assert.AreEqual(1, latestTestResults.Count);
                Assert.AreEqual(1, latestTestResults[0].TestFailureDurations.Count());
                Assert.AreEqual(utcNow.AddHours(-48), latestTestResults[0].TestFailureDurations.ToList()[0].FailureStart);
                Assert.AreEqual(utcNow.AddHours(-23), latestTestResults[0].TestFailureDurations.ToList()[0].FailureEnd);
            }

            [TestMethod]
            public void IgnoresTestFailureDurationsFromOtherTests()
            {
                var utcNow = DateTime.UtcNow;

                var latestTestResultDtos = new List<LatestTestResultDto> {
                    new LatestTestResultDto {
                        SuiteId = 1,
                        TestId = 2,
                        PassingFromDate = utcNow,
                    }
                };
                var mockLatestTestResultsService = new Mock<ILatestTestResultsService>();
                mockLatestTestResultsService.Setup(s => s.Get(1)).Returns(latestTestResultDtos);

                var mockTestFailureDurations = new List<TestFailureDuration>
                {
                    new TestFailureDuration { SuiteId = 1, TestId = 2 },
                    new TestFailureDuration { SuiteId = 1, TestId = 3 }
                }.ToDbSetMock();
                mockTestFailureDurations.Setup(m => m.AsNoTracking()).Returns(mockTestFailureDurations.Object);

                var mockContext = new Mock<ZigNetEntities>();
                mockContext.Setup(m => m.TestFailureDurations).Returns(mockTestFailureDurations.Object);

                var zignetEntitiesWrapperMock = new Mock<IZigNetEntitiesWrapper>();
                zignetEntitiesWrapperMock.Setup(z => z.Get()).Returns(mockContext.Object);

                var testResultService = new TestResultService(zignetEntitiesWrapperMock.Object,
                    new Mock<ISuiteService>().Object, mockLatestTestResultsService.Object);
                var latestTestResults = testResultService.GetLatestResults(1, false).ToList();

                Assert.AreEqual(1, latestTestResults.Count);
                Assert.AreEqual(1, latestTestResults[0].TestFailureDurations.Count());
                Assert.IsNull(latestTestResults[0].TestFailureDurations.ToList()[0].FailureEnd);
            }

            [TestMethod]
            public void IgnoresTestFailureDurationsFromOtherSuites()
            {
                var utcNow = DateTime.UtcNow;

                var latestTestResultDtos = new List<LatestTestResultDto> {
                    new LatestTestResultDto {
                        SuiteId = 1,
                        TestId = 2,
                        PassingFromDate = utcNow,
                    }
                };
                var mockLatestTestResultsService = new Mock<ILatestTestResultsService>();
                mockLatestTestResultsService.Setup(s => s.Get(1)).Returns(latestTestResultDtos);

                var mockTestFailureDurations = new List<TestFailureDuration>
                {
                    new TestFailureDuration { SuiteId = 1, TestId = 2 },
                    new TestFailureDuration { SuiteId = 3, TestId = 2 }
                }.ToDbSetMock();
                mockTestFailureDurations.Setup(m => m.AsNoTracking()).Returns(mockTestFailureDurations.Object);

                var mockContext = new Mock<ZigNetEntities>();
                mockContext.Setup(m => m.TestFailureDurations).Returns(mockTestFailureDurations.Object);

                var zignetEntitiesWrapperMock = new Mock<IZigNetEntitiesWrapper>();
                zignetEntitiesWrapperMock.Setup(z => z.Get()).Returns(mockContext.Object);

                var testResultService = new TestResultService(zignetEntitiesWrapperMock.Object,
                    new Mock<ISuiteService>().Object, mockLatestTestResultsService.Object);
                var latestTestResults = testResultService.GetLatestResults(1, false).ToList();

                Assert.AreEqual(1, latestTestResults.Count);
                Assert.AreEqual(1, latestTestResults[0].TestFailureDurations.Count());
                Assert.IsNull(latestTestResults[0].TestFailureDurations.ToList()[0].FailureEnd);
            }
        }

        [TestClass]
        public class SaveTestResult
        {
            [TestMethod]
            public void AssignsTestIdWhenTestWithSameNameExists()
            {
                var mockTests = new List<Test>
                {
                    new Test {
                        TestName = "test 1", TestID = 1,
                        TestCategories = new List<TestCategory>(),
                        Suites = new List<Suite> { new Suite { SuiteID = 2 } }
                    }
                }.ToDbSetMock();
                mockTests.Setup(m => m.AsNoTracking()).Returns(mockTests.Object);
                mockTests.Setup(m => m.Include(It.IsAny<string>())).Returns(mockTests.Object);

                var mockTestCategories = new List<TestCategory>().ToDbSetMock();

                var mockSuiteResults = new List<SuiteResult>
                {
                    new SuiteResult { SuiteResultID = 3, SuiteId = 2 }
                }.ToDbSetMock();
                mockSuiteResults.Setup(m => m.AsNoTracking()).Returns(mockSuiteResults.Object);

                var mockSuites = new List<Suite>
                {
                    new Suite { SuiteID = 2 }
                }.ToDbSetMock();
                mockSuites.Setup(m => m.AsNoTracking()).Returns(mockSuites.Object);
                mockSuites.Setup(m => m.Include(It.IsAny<string>())).Returns(mockSuites.Object);

                var mockTestResults = new List<TestResult>().ToDbSetMock();

                var mockTemporaryTestResults = new List<TemporaryTestResult>().ToDbSetMock();

                var mockLatestTestResults = new List<LatestTestResult>
                {
                    new LatestTestResult { SuiteId = 2, TestId = 1, FailingFromDateTime = DateTime.UtcNow }
                }.ToDbSetMock();

                var mockTestFailureDurations = new List<TestFailureDuration> {
                    new TestFailureDuration { SuiteId = 2, TestId = 1 }
                }.ToDbSetMock();

                var mockContext = new Mock<ZigNetEntities>();
                mockContext.Setup(m => m.Tests).Returns(mockTests.Object);
                mockContext.Setup(m => m.TestCategories).Returns(mockTestCategories.Object);
                mockContext.Setup(m => m.SuiteResults).Returns(mockSuiteResults.Object);
                mockContext.Setup(m => m.Suites).Returns(mockSuites.Object);
                mockContext.Setup(m => m.TestResults).Returns(mockTestResults.Object);
                mockContext.Setup(m => m.TemporaryTestResults).Returns(mockTemporaryTestResults.Object);
                mockContext.Setup(m => m.LatestTestResults).Returns(mockLatestTestResults.Object);
                mockContext.Setup(m => m.TestFailureDurations).Returns(mockTestFailureDurations.Object);

                var zignetEntitiesWrapperMock = new Mock<IZigNetEntitiesWrapper>();
                zignetEntitiesWrapperMock.Setup(z => z.Get()).Returns(mockContext.Object);

                var testResult = new DomainTestResult
                {
                    Test = new DomainTest
                    {
                        Name = "test 1",
                        Categories = new List<DomainTestCategory>()
                    },
                    SuiteResult = new DomainSuiteResult { SuiteResultID = 3 }
                };

                var testResultService = new TestResultService(zignetEntitiesWrapperMock.Object, new Mock<ISuiteService>().Object, new Mock<ILatestTestResultsService>().Object);
                testResultService.SaveTestResult(testResult);
            }

            [TestMethod]
            public void DoesNotAssignTestIdWhenTestWithSameNameDoesNotExist()
            {
                var mockTests = new List<Test>().ToDbSetMock();
                mockTests.Setup(m => m.AsNoTracking()).Returns(mockTests.Object);
                mockTests.Setup(m => m.Include(It.IsAny<string>())).Returns(mockTests.Object);

                var mockTestCategories = new List<TestCategory>().ToDbSetMock();

                var mockSuiteResults = new List<SuiteResult>
                {
                    new SuiteResult { SuiteResultID = 3, SuiteId = 2 }
                }.ToDbSetMock();
                mockSuiteResults.Setup(m => m.AsNoTracking()).Returns(mockSuiteResults.Object);

                var mockSuites = new List<Suite>
                {
                    new Suite { SuiteID = 2 }
                }.ToDbSetMock();
                mockSuites.Setup(m => m.AsNoTracking()).Returns(mockSuites.Object);
                mockSuites.Setup(m => m.Include(It.IsAny<string>())).Returns(mockSuites.Object);

                var mockTestResults = new List<TestResult>().ToDbSetMock();

                var mockTemporaryTestResults = new List<TemporaryTestResult>().ToDbSetMock();

                var mockLatestTestResults = new List<LatestTestResult>
                {
                    new LatestTestResult { SuiteId = 2, TestId = 1, FailingFromDateTime = DateTime.UtcNow }
                }.ToDbSetMock();

                var mockTestFailureDurations = new List<TestFailureDuration> {
                    new TestFailureDuration { SuiteId = 2, TestId = 1 }
                }.ToDbSetMock();

                var mockContext = new Mock<ZigNetEntities>();
                mockContext.Setup(m => m.Tests).Returns(mockTests.Object);
                mockContext.Setup(m => m.TestCategories).Returns(mockTestCategories.Object);
                mockContext.Setup(m => m.SuiteResults).Returns(mockSuiteResults.Object);
                mockContext.Setup(m => m.Suites).Returns(mockSuites.Object);
                mockContext.Setup(m => m.TestResults).Returns(mockTestResults.Object);
                mockContext.Setup(m => m.TemporaryTestResults).Returns(mockTemporaryTestResults.Object);
                mockContext.Setup(m => m.LatestTestResults).Returns(mockLatestTestResults.Object);
                mockContext.Setup(m => m.TestFailureDurations).Returns(mockTestFailureDurations.Object);

                var zignetEntitiesWrapperMock = new Mock<IZigNetEntitiesWrapper>();
                zignetEntitiesWrapperMock.Setup(z => z.Get()).Returns(mockContext.Object);

                var testResult = new DomainTestResult
                {
                    Test = new DomainTest
                    {
                        Name = "test 1",
                        Categories = new List<DomainTestCategory>()
                    },
                    SuiteResult = new DomainSuiteResult { SuiteResultID = 3 }
                };

                var testResultService = new TestResultService(zignetEntitiesWrapperMock.Object, new Mock<ISuiteService>().Object, new Mock<ILatestTestResultsService>().Object);
                testResultService.SaveTestResult(testResult);
            }

            [TestMethod]
            [ExpectedException(typeof(InvalidOperationException))]
            public void ThrowsIfSuiteResultDoesNotExist()
            {
                var mockTests = new List<Test>().ToDbSetMock();
                mockTests.Setup(m => m.AsNoTracking()).Returns(mockTests.Object);
                mockTests.Setup(m => m.Include(It.IsAny<string>())).Returns(mockTests.Object);

                var mockTestCategories = new List<TestCategory>().ToDbSetMock();

                var mockSuiteResults = new List<SuiteResult>().ToDbSetMock();
                mockSuiteResults.Setup(m => m.AsNoTracking()).Returns(mockSuiteResults.Object);

                var mockContext = new Mock<ZigNetEntities>();
                mockContext.Setup(m => m.Tests).Returns(mockTests.Object);
                mockContext.Setup(m => m.TestCategories).Returns(mockTestCategories.Object);
                mockContext.Setup(m => m.SuiteResults).Returns(mockSuiteResults.Object);

                var zignetEntitiesWrapperMock = new Mock<IZigNetEntitiesWrapper>();
                zignetEntitiesWrapperMock.Setup(z => z.Get()).Returns(mockContext.Object);

                var testResult = new DomainTestResult
                {
                    Test = new DomainTest
                    {
                        Name = "test 1",
                        Categories = new List<DomainTestCategory>()
                    },
                    SuiteResult = new DomainSuiteResult { SuiteResultID = 3 }
                };

                var testResultService = new TestResultService(zignetEntitiesWrapperMock.Object, new Mock<ISuiteService>().Object, new Mock<ILatestTestResultsService>().Object);
                testResultService.SaveTestResult(testResult);
            }

            [TestMethod]
            public void CopiesExistingTestCategories()
            {
                var mockTests = new List<Test>
                {
                    new Test {
                        TestName = "test 1", TestID = 1,
                        TestCategories = new List<TestCategory> {new TestCategory { TestCategoryID = 1, CategoryName = "test category 1" } },
                        Suites = new List<Suite> { new Suite { SuiteID = 2 } }
                    }
                }.ToDbSetMock();
                mockTests.Setup(m => m.AsNoTracking()).Returns(mockTests.Object);
                mockTests.Setup(m => m.Include(It.IsAny<string>())).Returns(mockTests.Object);

                var mockTestCategories = new List<TestCategory>().ToDbSetMock();

                var mockSuiteResults = new List<SuiteResult>
                {
                    new SuiteResult { SuiteResultID = 3, SuiteId = 2 }
                }.ToDbSetMock();
                mockSuiteResults.Setup(m => m.AsNoTracking()).Returns(mockSuiteResults.Object);

                var mockSuites = new List<Suite>
                {
                    new Suite { SuiteID = 2 }
                }.ToDbSetMock();
                mockSuites.Setup(m => m.AsNoTracking()).Returns(mockSuites.Object);
                mockSuites.Setup(m => m.Include(It.IsAny<string>())).Returns(mockSuites.Object);

                var mockTestResults = new List<TestResult>().ToDbSetMock();

                var mockTemporaryTestResults = new List<TemporaryTestResult>().ToDbSetMock();

                var mockLatestTestResults = new List<LatestTestResult>
                {
                    new LatestTestResult { SuiteId = 2, TestId = 1, FailingFromDateTime = DateTime.UtcNow }
                }.ToDbSetMock();

                var mockTestFailureDurations = new List<TestFailureDuration> {
                    new TestFailureDuration { SuiteId = 2, TestId = 1 }
                }.ToDbSetMock();

                var mockContext = new Mock<ZigNetEntities>();
                mockContext.Setup(m => m.Tests).Returns(mockTests.Object);
                mockContext.Setup(m => m.TestCategories).Returns(mockTestCategories.Object);
                mockContext.Setup(m => m.SuiteResults).Returns(mockSuiteResults.Object);
                mockContext.Setup(m => m.Suites).Returns(mockSuites.Object);
                mockContext.Setup(m => m.TestResults).Returns(mockTestResults.Object);
                mockContext.Setup(m => m.TemporaryTestResults).Returns(mockTemporaryTestResults.Object);
                mockContext.Setup(m => m.LatestTestResults).Returns(mockLatestTestResults.Object);
                mockContext.Setup(m => m.TestFailureDurations).Returns(mockTestFailureDurations.Object);

                var zignetEntitiesWrapperMock = new Mock<IZigNetEntitiesWrapper>();
                zignetEntitiesWrapperMock.Setup(z => z.Get()).Returns(mockContext.Object);

                var testResult = new DomainTestResult
                {
                    Test = new DomainTest
                    {
                        Name = "test 1",
                        Categories = new List<DomainTestCategory>()
                    },
                    SuiteResult = new DomainSuiteResult { SuiteResultID = 3 }
                };

                var testResultService = new TestResultService(zignetEntitiesWrapperMock.Object, new Mock<ISuiteService>().Object, new Mock<ILatestTestResultsService>().Object);
                testResultService.SaveTestResult(testResult);
            }

            [TestMethod]
            public void MergesNewAndExistingTestCategories()
            {
                var mockTests = new List<Test>
                {
                    new Test {
                        TestName = "test 1", TestID = 1,
                        TestCategories = new List<TestCategory> {new TestCategory { TestCategoryID = 1, CategoryName = "test category 1" } },
                        Suites = new List<Suite> { new Suite { SuiteID = 2 } }
                    }
                }.ToDbSetMock();
                mockTests.Setup(m => m.AsNoTracking()).Returns(mockTests.Object);
                mockTests.Setup(m => m.Include(It.IsAny<string>())).Returns(mockTests.Object);

                var mockTestCategories = new List<TestCategory> { new TestCategory { TestCategoryID = 1, CategoryName = "test category 1" } }.ToDbSetMock();

                var mockSuiteResults = new List<SuiteResult>
                {
                    new SuiteResult { SuiteResultID = 3, SuiteId = 2 }
                }.ToDbSetMock();
                mockSuiteResults.Setup(m => m.AsNoTracking()).Returns(mockSuiteResults.Object);

                var mockSuites = new List<Suite>
                {
                    new Suite { SuiteID = 2 }
                }.ToDbSetMock();
                mockSuites.Setup(m => m.AsNoTracking()).Returns(mockSuites.Object);
                mockSuites.Setup(m => m.Include(It.IsAny<string>())).Returns(mockSuites.Object);

                var mockTestResults = new List<TestResult>().ToDbSetMock();

                var mockTemporaryTestResults = new List<TemporaryTestResult>().ToDbSetMock();

                var mockLatestTestResults = new List<LatestTestResult>
                {
                    new LatestTestResult { SuiteId = 2, TestId = 1, FailingFromDateTime = DateTime.UtcNow }
                }.ToDbSetMock();

                var mockTestFailureDurations = new List<TestFailureDuration> {
                    new TestFailureDuration { SuiteId = 2, TestId = 1 }
                }.ToDbSetMock();

                var mockContext = new Mock<ZigNetEntities>();
                mockContext.Setup(m => m.Tests).Returns(mockTests.Object);
                mockContext.Setup(m => m.TestCategories).Returns(mockTestCategories.Object);
                mockContext.Setup(m => m.SuiteResults).Returns(mockSuiteResults.Object);
                mockContext.Setup(m => m.Suites).Returns(mockSuites.Object);
                mockContext.Setup(m => m.TestResults).Returns(mockTestResults.Object);
                mockContext.Setup(m => m.TemporaryTestResults).Returns(mockTemporaryTestResults.Object);
                mockContext.Setup(m => m.LatestTestResults).Returns(mockLatestTestResults.Object);
                mockContext.Setup(m => m.TestFailureDurations).Returns(mockTestFailureDurations.Object);

                var zignetEntitiesWrapperMock = new Mock<IZigNetEntitiesWrapper>();
                zignetEntitiesWrapperMock.Setup(z => z.Get()).Returns(mockContext.Object);

                var testResult = new DomainTestResult
                {
                    Test = new DomainTest
                    {
                        Name = "test 1",
                        Categories = new List<DomainTestCategory> { new DomainTestCategory { Name = "test category 2" } }
                    },
                    SuiteResult = new DomainSuiteResult { SuiteResultID = 3 }
                };

                var testResultService = new TestResultService(zignetEntitiesWrapperMock.Object, new Mock<ISuiteService>().Object, new Mock<ILatestTestResultsService>().Object);
                testResultService.SaveTestResult(testResult);
            }

            [TestMethod]
            public void ClearsAllExistingTestCategories()
            {
                // bug here: there is no way for existing categories to be removed

                var mockTests = new List<Test>
                {
                    new Test {
                        TestName = "test 1", TestID = 1,
                        TestCategories = new List<TestCategory> { new TestCategory { TestCategoryID = 1, CategoryName = "test category 1" } },
                        Suites = new List<Suite> { new Suite { SuiteID = 2 } }
                    }
                }.ToDbSetMock();
                mockTests.Setup(m => m.AsNoTracking()).Returns(mockTests.Object);
                mockTests.Setup(m => m.Include(It.IsAny<string>())).Returns(mockTests.Object);

                var mockTestCategories = new List<TestCategory>().ToDbSetMock();

                var mockSuiteResults = new List<SuiteResult>
                {
                    new SuiteResult { SuiteResultID = 3, SuiteId = 2 }
                }.ToDbSetMock();
                mockSuiteResults.Setup(m => m.AsNoTracking()).Returns(mockSuiteResults.Object);

                var mockSuites = new List<Suite>
                {
                    new Suite { SuiteID = 2 }
                }.ToDbSetMock();
                mockSuites.Setup(m => m.AsNoTracking()).Returns(mockSuites.Object);
                mockSuites.Setup(m => m.Include(It.IsAny<string>())).Returns(mockSuites.Object);

                var mockTestResults = new List<TestResult>().ToDbSetMock();

                var mockTemporaryTestResults = new List<TemporaryTestResult>().ToDbSetMock();

                var mockLatestTestResults = new List<LatestTestResult>
                {
                    new LatestTestResult { SuiteId = 2, TestId = 1, FailingFromDateTime = DateTime.UtcNow }
                }.ToDbSetMock();

                var mockTestFailureDurations = new List<TestFailureDuration> {
                    new TestFailureDuration { SuiteId = 2, TestId = 1 }
                }.ToDbSetMock();

                var mockContext = new Mock<ZigNetEntities>();
                mockContext.Setup(m => m.Tests).Returns(mockTests.Object);
                mockContext.Setup(m => m.TestCategories).Returns(mockTestCategories.Object);
                mockContext.Setup(m => m.SuiteResults).Returns(mockSuiteResults.Object);
                mockContext.Setup(m => m.Suites).Returns(mockSuites.Object);
                mockContext.Setup(m => m.TestResults).Returns(mockTestResults.Object);
                mockContext.Setup(m => m.TemporaryTestResults).Returns(mockTemporaryTestResults.Object);
                mockContext.Setup(m => m.LatestTestResults).Returns(mockLatestTestResults.Object);
                mockContext.Setup(m => m.TestFailureDurations).Returns(mockTestFailureDurations.Object);

                var zignetEntitiesWrapperMock = new Mock<IZigNetEntitiesWrapper>();
                zignetEntitiesWrapperMock.Setup(z => z.Get()).Returns(mockContext.Object);

                var testResult = new DomainTestResult
                {
                    Test = new DomainTest
                    {
                        Name = "test 1",
                        Categories = new List<DomainTestCategory>()
                    },
                    SuiteResult = new DomainSuiteResult { SuiteResultID = 3 }
                };

                var testResultService = new TestResultService(zignetEntitiesWrapperMock.Object, new Mock<ISuiteService>().Object, new Mock<ILatestTestResultsService>().Object);
                testResultService.SaveTestResult(testResult);
            }

            [TestMethod]
            public void SavesFailedTestResultWithDetails()
            {
                var mockTests = new List<Test>
                {
                    new Test {
                        TestName = "test 1", TestID = 1,
                        TestCategories = new List<TestCategory>(),
                        Suites = new List<Suite> { new Suite { SuiteID = 2 } }
                    }
                }.ToDbSetMock();
                mockTests.Setup(m => m.AsNoTracking()).Returns(mockTests.Object);
                mockTests.Setup(m => m.Include(It.IsAny<string>())).Returns(mockTests.Object);

                var mockTestFailureTypes = new List<TestFailureType>{
                    new TestFailureType { TestFailureTypeID = 2, TestFailureTypeName = "Exception" }
                }.ToDbSetMock();

                var mockTestCategories = new List<TestCategory>().ToDbSetMock();

                var mockSuiteResults = new List<SuiteResult>
                {
                    new SuiteResult { SuiteResultID = 3, SuiteId = 2 }
                }.ToDbSetMock();
                mockSuiteResults.Setup(m => m.AsNoTracking()).Returns(mockSuiteResults.Object);

                var mockSuites = new List<Suite>
                {
                    new Suite { SuiteID = 2 }
                }.ToDbSetMock();
                mockSuites.Setup(m => m.AsNoTracking()).Returns(mockSuites.Object);
                mockSuites.Setup(m => m.Include(It.IsAny<string>())).Returns(mockSuites.Object);

                var mockTestResults = new List<TestResult>().ToDbSetMock();

                var mockTemporaryTestResults = new List<TemporaryTestResult>().ToDbSetMock();

                var mockLatestTestResults = new List<LatestTestResult>
                {
                    new LatestTestResult { SuiteId = 2, TestId = 1, FailingFromDateTime = DateTime.UtcNow }
                }.ToDbSetMock();

                var mockTestFailureDurations = new List<TestFailureDuration> {
                    new TestFailureDuration { SuiteId = 2, TestId = 1 }
                }.ToDbSetMock();

                var mockContext = new Mock<ZigNetEntities>();
                mockContext.Setup(m => m.Tests).Returns(mockTests.Object);
                mockContext.Setup(m => m.TestFailureTypes).Returns(mockTestFailureTypes.Object);
                mockContext.Setup(m => m.TestCategories).Returns(mockTestCategories.Object);
                mockContext.Setup(m => m.SuiteResults).Returns(mockSuiteResults.Object);
                mockContext.Setup(m => m.Suites).Returns(mockSuites.Object);
                mockContext.Setup(m => m.TestResults).Returns(mockTestResults.Object);
                mockContext.Setup(m => m.TemporaryTestResults).Returns(mockTemporaryTestResults.Object);
                mockContext.Setup(m => m.LatestTestResults).Returns(mockLatestTestResults.Object);
                mockContext.Setup(m => m.TestFailureDurations).Returns(mockTestFailureDurations.Object);

                var zignetEntitiesWrapperMock = new Mock<IZigNetEntitiesWrapper>();
                zignetEntitiesWrapperMock.Setup(z => z.Get()).Returns(mockContext.Object);

                var testResult = new DomainTestResult
                {
                    Test = new DomainTest
                    {
                        Name = "test 1",
                        Categories = new List<DomainTestCategory>()
                    },
                    SuiteResult = new DomainSuiteResult { SuiteResultID = 3 },
                    ResultType = DomainTestResultType.Fail,
                    TestFailureDetails = new DomainTestFailureDetails
                    {
                        FailureType = DomainTestFailureType.Exception,
                        FailureDetailMessage = "failed by exception at line 5"
                    }
                };

                var testResultService = new TestResultService(zignetEntitiesWrapperMock.Object, new Mock<ISuiteService>().Object, new Mock<ILatestTestResultsService>().Object);
                testResultService.SaveTestResult(testResult);
            }

            [TestMethod]
            public void SavesExistingTestPassedTestResult()
            {
                var mockTests = new List<Test>
                {
                    new Test {
                        TestName = "test 1", TestID = 1,
                        TestCategories = new List<TestCategory>(),
                        Suites = new List<Suite> { new Suite { SuiteID = 2 } }
                    }
                }.ToDbSetMock();
                mockTests.Setup(m => m.AsNoTracking()).Returns(mockTests.Object);
                mockTests.Setup(m => m.Include(It.IsAny<string>())).Returns(mockTests.Object);

                var mockTestCategories = new List<TestCategory>().ToDbSetMock();

                var mockSuiteResults = new List<SuiteResult>
                {
                    new SuiteResult { SuiteResultID = 3, SuiteId = 2 }
                }.ToDbSetMock();
                mockSuiteResults.Setup(m => m.AsNoTracking()).Returns(mockSuiteResults.Object);

                var mockSuites = new List<Suite>
                {
                    new Suite { SuiteID = 2 }
                }.ToDbSetMock();
                mockSuites.Setup(m => m.AsNoTracking()).Returns(mockSuites.Object);
                mockSuites.Setup(m => m.Include(It.IsAny<string>())).Returns(mockSuites.Object);

                var mockTestResults = new List<TestResult>().ToDbSetMock();

                var mockTemporaryTestResults = new List<TemporaryTestResult>().ToDbSetMock();

                var mockLatestTestResults = new List<LatestTestResult>
                {
                    new LatestTestResult { SuiteId = 2, TestId = 1, PassingFromDateTime = DateTime.UtcNow }
                }.ToDbSetMock();

                var mockTestFailureDurations = new List<TestFailureDuration> {
                    new TestFailureDuration { SuiteId = 2, TestId = 1 }
                }.ToDbSetMock();

                var mockContext = new Mock<ZigNetEntities>();
                mockContext.Setup(m => m.Tests).Returns(mockTests.Object);
                mockContext.Setup(m => m.TestCategories).Returns(mockTestCategories.Object);
                mockContext.Setup(m => m.SuiteResults).Returns(mockSuiteResults.Object);
                mockContext.Setup(m => m.Suites).Returns(mockSuites.Object);
                mockContext.Setup(m => m.TestResults).Returns(mockTestResults.Object);
                mockContext.Setup(m => m.TemporaryTestResults).Returns(mockTemporaryTestResults.Object);
                mockContext.Setup(m => m.LatestTestResults).Returns(mockLatestTestResults.Object);
                mockContext.Setup(m => m.TestFailureDurations).Returns(mockTestFailureDurations.Object);

                var zignetEntitiesWrapperMock = new Mock<IZigNetEntitiesWrapper>();
                zignetEntitiesWrapperMock.Setup(z => z.Get()).Returns(mockContext.Object);

                var testResult = new DomainTestResult
                {
                    Test = new DomainTest
                    {
                        Name = "test 1",
                        Categories = new List<DomainTestCategory>()
                    },
                    SuiteResult = new DomainSuiteResult { SuiteResultID = 3 },
                    ResultType = DomainTestResultType.Pass,
                };

                var testResultService = new TestResultService(zignetEntitiesWrapperMock.Object, new Mock<ISuiteService>().Object, new Mock<ILatestTestResultsService>().Object);
                testResultService.SaveTestResult(testResult);
            }

            [TestMethod]
            public void SavesExistingTestFailedTestResult()
            {
                var mockTests = new List<Test>
                {
                    new Test {
                        TestName = "test 1", TestID = 1,
                        TestCategories = new List<TestCategory>(),
                        Suites = new List<Suite> { new Suite { SuiteID = 2 } }
                    }
                }.ToDbSetMock();
                mockTests.Setup(m => m.AsNoTracking()).Returns(mockTests.Object);
                mockTests.Setup(m => m.Include(It.IsAny<string>())).Returns(mockTests.Object);

                var mockTestFailureTypes = new List<TestFailureType>{
                    new TestFailureType { TestFailureTypeID = 1, TestFailureTypeName = "Assertion" }
                }.ToDbSetMock();

                var mockTestCategories = new List<TestCategory>().ToDbSetMock();

                var mockSuiteResults = new List<SuiteResult>
                {
                    new SuiteResult { SuiteResultID = 3, SuiteId = 2 }
                }.ToDbSetMock();
                mockSuiteResults.Setup(m => m.AsNoTracking()).Returns(mockSuiteResults.Object);

                var mockSuites = new List<Suite>
                {
                    new Suite { SuiteID = 2 }
                }.ToDbSetMock();
                mockSuites.Setup(m => m.AsNoTracking()).Returns(mockSuites.Object);
                mockSuites.Setup(m => m.Include(It.IsAny<string>())).Returns(mockSuites.Object);

                var mockTestResults = new List<TestResult>().ToDbSetMock();

                var mockTemporaryTestResults = new List<TemporaryTestResult>().ToDbSetMock();

                var mockLatestTestResults = new List<LatestTestResult>
                {
                    new LatestTestResult { SuiteId = 2, TestId = 1, FailingFromDateTime = DateTime.UtcNow }
                }.ToDbSetMock();

                var mockTestFailureDurations = new List<TestFailureDuration> {
                    new TestFailureDuration { SuiteId = 2, TestId = 1 }
                }.ToDbSetMock();

                var mockContext = new Mock<ZigNetEntities>();
                mockContext.Setup(m => m.Tests).Returns(mockTests.Object);
                mockContext.Setup(m => m.TestFailureTypes).Returns(mockTestFailureTypes.Object);
                mockContext.Setup(m => m.TestCategories).Returns(mockTestCategories.Object);
                mockContext.Setup(m => m.SuiteResults).Returns(mockSuiteResults.Object);
                mockContext.Setup(m => m.Suites).Returns(mockSuites.Object);
                mockContext.Setup(m => m.TestResults).Returns(mockTestResults.Object);
                mockContext.Setup(m => m.TemporaryTestResults).Returns(mockTemporaryTestResults.Object);
                mockContext.Setup(m => m.LatestTestResults).Returns(mockLatestTestResults.Object);
                mockContext.Setup(m => m.TestFailureDurations).Returns(mockTestFailureDurations.Object);

                var zignetEntitiesWrapperMock = new Mock<IZigNetEntitiesWrapper>();
                zignetEntitiesWrapperMock.Setup(z => z.Get()).Returns(mockContext.Object);

                var testResult = new DomainTestResult
                {
                    Test = new DomainTest
                    {
                        Name = "test 1",
                        Categories = new List<DomainTestCategory>()
                    },
                    SuiteResult = new DomainSuiteResult { SuiteResultID = 3 },
                    ResultType = DomainTestResultType.Fail,
                    TestFailureDetails = new DomainTestFailureDetails { FailureType = DomainTestFailureType.Assertion }
                };

                var testResultService = new TestResultService(zignetEntitiesWrapperMock.Object, new Mock<ISuiteService>().Object, new Mock<ILatestTestResultsService>().Object);
                testResultService.SaveTestResult(testResult);
            }

            [TestMethod]
            public void CreatesNewLatestTestResult()
            {
                var mockTests = new List<Test>().ToDbSetMock();
                mockTests.Setup(m => m.AsNoTracking()).Returns(mockTests.Object);
                mockTests.Setup(m => m.Include(It.IsAny<string>())).Returns(mockTests.Object);

                var mockTestFailureTypes = new List<TestFailureType>{
                    new TestFailureType { TestFailureTypeID = 1, TestFailureTypeName = "Assertion" }
                }.ToDbSetMock();

                var mockTestCategories = new List<TestCategory>().ToDbSetMock();

                var mockSuiteResults = new List<SuiteResult>
                {
                    new SuiteResult { SuiteResultID = 3, SuiteId = 2 }
                }.ToDbSetMock();
                mockSuiteResults.Setup(m => m.AsNoTracking()).Returns(mockSuiteResults.Object);

                var mockSuites = new List<Suite>
                {
                    new Suite { SuiteID = 2 }
                }.ToDbSetMock();
                mockSuites.Setup(m => m.AsNoTracking()).Returns(mockSuites.Object);
                mockSuites.Setup(m => m.Include(It.IsAny<string>())).Returns(mockSuites.Object);

                var mockTestResults = new List<TestResult>().ToDbSetMock();

                var mockTemporaryTestResults = new List<TemporaryTestResult>().ToDbSetMock();

                var mockLatestTestResults = new List<LatestTestResult>().ToDbSetMock();

                var mockTestFailureDurations = new List<TestFailureDuration>().ToDbSetMock();

                var mockContext = new Mock<ZigNetEntities>();
                mockContext.Setup(m => m.Tests).Returns(mockTests.Object);
                mockContext.Setup(m => m.TestFailureTypes).Returns(mockTestFailureTypes.Object);
                mockContext.Setup(m => m.TestCategories).Returns(mockTestCategories.Object);
                mockContext.Setup(m => m.SuiteResults).Returns(mockSuiteResults.Object);
                mockContext.Setup(m => m.Suites).Returns(mockSuites.Object);
                mockContext.Setup(m => m.TestResults).Returns(mockTestResults.Object);
                mockContext.Setup(m => m.TemporaryTestResults).Returns(mockTemporaryTestResults.Object);
                mockContext.Setup(m => m.LatestTestResults).Returns(mockLatestTestResults.Object);
                mockContext.Setup(m => m.TestFailureDurations).Returns(mockTestFailureDurations.Object);

                var zignetEntitiesWrapperMock = new Mock<IZigNetEntitiesWrapper>();
                zignetEntitiesWrapperMock.Setup(z => z.Get()).Returns(mockContext.Object);

                var testResult = new DomainTestResult
                {
                    Test = new DomainTest
                    {
                        TestID = 0,
                        Name = "test 1",
                        Categories = new List<DomainTestCategory>()
                    },
                    SuiteResult = new DomainSuiteResult { SuiteResultID = 3 },
                    ResultType = DomainTestResultType.Fail,
                    TestFailureDetails = new DomainTestFailureDetails { FailureType = DomainTestFailureType.Assertion }
                };

                var testResultService = new TestResultService(zignetEntitiesWrapperMock.Object, new Mock<ISuiteService>().Object, new Mock<ILatestTestResultsService>().Object);
                testResultService.SaveTestResult(testResult);
            }

            [TestMethod]
            public void CreatesNewLatestTestResultWhenSuiteIdDoesNotMatch()
            {
                var mockTests = new List<Test>
                {
                    new Test {
                        TestName = "test 1", TestID = 1,
                        TestCategories = new List<TestCategory>(),
                        Suites = new List<Suite> { new Suite { SuiteID = 2 } }
                    }
                }.ToDbSetMock();
                mockTests.Setup(m => m.AsNoTracking()).Returns(mockTests.Object);
                mockTests.Setup(m => m.Include(It.IsAny<string>())).Returns(mockTests.Object);

                var mockTestFailureTypes = new List<TestFailureType>{
                    new TestFailureType { TestFailureTypeID = 1, TestFailureTypeName = "Assertion" }
                }.ToDbSetMock();

                var mockTestCategories = new List<TestCategory>().ToDbSetMock();

                var mockSuiteResults = new List<SuiteResult>
                {
                    new SuiteResult { SuiteResultID = 3, SuiteId = 2 }
                }.ToDbSetMock();
                mockSuiteResults.Setup(m => m.AsNoTracking()).Returns(mockSuiteResults.Object);

                var mockSuites = new List<Suite>
                {
                    new Suite { SuiteID = 2 }
                }.ToDbSetMock();
                mockSuites.Setup(m => m.AsNoTracking()).Returns(mockSuites.Object);
                mockSuites.Setup(m => m.Include(It.IsAny<string>())).Returns(mockSuites.Object);

                var mockTestResults = new List<TestResult>().ToDbSetMock();

                var mockTemporaryTestResults = new List<TemporaryTestResult>().ToDbSetMock();

                var mockLatestTestResults = new List<LatestTestResult> {
                    new LatestTestResult{ SuiteId = 7, TestId = 1 }
                }.ToDbSetMock();

                var mockTestFailureDurations = new List<TestFailureDuration>().ToDbSetMock();

                var mockContext = new Mock<ZigNetEntities>();
                mockContext.Setup(m => m.Tests).Returns(mockTests.Object);
                mockContext.Setup(m => m.TestFailureTypes).Returns(mockTestFailureTypes.Object);
                mockContext.Setup(m => m.TestCategories).Returns(mockTestCategories.Object);
                mockContext.Setup(m => m.SuiteResults).Returns(mockSuiteResults.Object);
                mockContext.Setup(m => m.Suites).Returns(mockSuites.Object);
                mockContext.Setup(m => m.TestResults).Returns(mockTestResults.Object);
                mockContext.Setup(m => m.TemporaryTestResults).Returns(mockTemporaryTestResults.Object);
                mockContext.Setup(m => m.LatestTestResults).Returns(mockLatestTestResults.Object);
                mockContext.Setup(m => m.TestFailureDurations).Returns(mockTestFailureDurations.Object);

                var zignetEntitiesWrapperMock = new Mock<IZigNetEntitiesWrapper>();
                zignetEntitiesWrapperMock.Setup(z => z.Get()).Returns(mockContext.Object);

                var testResult = new DomainTestResult
                {
                    Test = new DomainTest
                    {
                        TestID = 0,
                        Name = "test 1",
                        Categories = new List<DomainTestCategory>()
                    },
                    SuiteResult = new DomainSuiteResult { SuiteResultID = 3 },
                    ResultType = DomainTestResultType.Fail,
                    TestFailureDetails = new DomainTestFailureDetails { FailureType = DomainTestFailureType.Assertion }
                };

                var testResultService = new TestResultService(zignetEntitiesWrapperMock.Object, new Mock<ISuiteService>().Object, new Mock<ILatestTestResultsService>().Object);
                testResultService.SaveTestResult(testResult);
            }

            [TestMethod]
            public void CreatesNewLatestTestResultWhenTestIdDoesNotMatch()
            {
                var mockTests = new List<Test>
                {
                    new Test {
                        TestName = "test 1", TestID = 1,
                        TestCategories = new List<TestCategory>(),
                        Suites = new List<Suite> { new Suite { SuiteID = 2 } }
                    }
                }.ToDbSetMock();
                mockTests.Setup(m => m.AsNoTracking()).Returns(mockTests.Object);
                mockTests.Setup(m => m.Include(It.IsAny<string>())).Returns(mockTests.Object);

                var mockTestFailureTypes = new List<TestFailureType>{
                    new TestFailureType { TestFailureTypeID = 1, TestFailureTypeName = "Assertion" }
                }.ToDbSetMock();

                var mockTestCategories = new List<TestCategory>().ToDbSetMock();

                var mockSuiteResults = new List<SuiteResult>
                {
                    new SuiteResult { SuiteResultID = 3, SuiteId = 2 }
                }.ToDbSetMock();
                mockSuiteResults.Setup(m => m.AsNoTracking()).Returns(mockSuiteResults.Object);

                var mockSuites = new List<Suite>
                {
                    new Suite { SuiteID = 2 }
                }.ToDbSetMock();
                mockSuites.Setup(m => m.AsNoTracking()).Returns(mockSuites.Object);
                mockSuites.Setup(m => m.Include(It.IsAny<string>())).Returns(mockSuites.Object);

                var mockTestResults = new List<TestResult>().ToDbSetMock();

                var mockTemporaryTestResults = new List<TemporaryTestResult>().ToDbSetMock();

                var mockLatestTestResults = new List<LatestTestResult> {
                    new LatestTestResult{ SuiteId = 2, TestId = 7 }
                }.ToDbSetMock();

                var mockTestFailureDurations = new List<TestFailureDuration>().ToDbSetMock();

                var mockContext = new Mock<ZigNetEntities>();
                mockContext.Setup(m => m.Tests).Returns(mockTests.Object);
                mockContext.Setup(m => m.TestFailureTypes).Returns(mockTestFailureTypes.Object);
                mockContext.Setup(m => m.TestCategories).Returns(mockTestCategories.Object);
                mockContext.Setup(m => m.SuiteResults).Returns(mockSuiteResults.Object);
                mockContext.Setup(m => m.Suites).Returns(mockSuites.Object);
                mockContext.Setup(m => m.TestResults).Returns(mockTestResults.Object);
                mockContext.Setup(m => m.TemporaryTestResults).Returns(mockTemporaryTestResults.Object);
                mockContext.Setup(m => m.LatestTestResults).Returns(mockLatestTestResults.Object);
                mockContext.Setup(m => m.TestFailureDurations).Returns(mockTestFailureDurations.Object);

                var zignetEntitiesWrapperMock = new Mock<IZigNetEntitiesWrapper>();
                zignetEntitiesWrapperMock.Setup(z => z.Get()).Returns(mockContext.Object);

                var testResult = new DomainTestResult
                {
                    Test = new DomainTest
                    {
                        TestID = 0,
                        Name = "test 1",
                        Categories = new List<DomainTestCategory>()
                    },
                    SuiteResult = new DomainSuiteResult { SuiteResultID = 3 },
                    ResultType = DomainTestResultType.Fail,
                    TestFailureDetails = new DomainTestFailureDetails { FailureType = DomainTestFailureType.Assertion }
                };

                var testResultService = new TestResultService(zignetEntitiesWrapperMock.Object, new Mock<ISuiteService>().Object, new Mock<ILatestTestResultsService>().Object);
                testResultService.SaveTestResult(testResult);
            }

            [TestMethod]
            public void UpdatesExistingLatestTestResult()
            {
                var mockTests = new List<Test>
                {
                    new Test {
                        TestName = "test 1", TestID = 1,
                        TestCategories = new List<TestCategory>(),
                        Suites = new List<Suite> { new Suite { SuiteID = 2 } }
                    }
                }.ToDbSetMock();
                mockTests.Setup(m => m.AsNoTracking()).Returns(mockTests.Object);
                mockTests.Setup(m => m.Include(It.IsAny<string>())).Returns(mockTests.Object);

                var mockTestFailureTypes = new List<TestFailureType>{
                    new TestFailureType { TestFailureTypeID = 1, TestFailureTypeName = "Assertion" }
                }.ToDbSetMock();

                var mockTestCategories = new List<TestCategory>().ToDbSetMock();

                var mockSuiteResults = new List<SuiteResult>
                {
                    new SuiteResult { SuiteResultID = 3, SuiteId = 2 }
                }.ToDbSetMock();
                mockSuiteResults.Setup(m => m.AsNoTracking()).Returns(mockSuiteResults.Object);

                var mockSuites = new List<Suite>
                {
                    new Suite { SuiteID = 2 }
                }.ToDbSetMock();
                mockSuites.Setup(m => m.AsNoTracking()).Returns(mockSuites.Object);
                mockSuites.Setup(m => m.Include(It.IsAny<string>())).Returns(mockSuites.Object);

                var mockTestResults = new List<TestResult>().ToDbSetMock();

                var mockTemporaryTestResults = new List<TemporaryTestResult>().ToDbSetMock();

                var mockLatestTestResults = new List<LatestTestResult> {
                    new LatestTestResult{ SuiteId = 2, TestId = 1 }
                }.ToDbSetMock();

                var mockTestFailureDurations = new List<TestFailureDuration>().ToDbSetMock();

                var mockContext = new Mock<ZigNetEntities>();
                mockContext.Setup(m => m.Tests).Returns(mockTests.Object);
                mockContext.Setup(m => m.TestFailureTypes).Returns(mockTestFailureTypes.Object);
                mockContext.Setup(m => m.TestCategories).Returns(mockTestCategories.Object);
                mockContext.Setup(m => m.SuiteResults).Returns(mockSuiteResults.Object);
                mockContext.Setup(m => m.Suites).Returns(mockSuites.Object);
                mockContext.Setup(m => m.TestResults).Returns(mockTestResults.Object);
                mockContext.Setup(m => m.TemporaryTestResults).Returns(mockTemporaryTestResults.Object);
                mockContext.Setup(m => m.LatestTestResults).Returns(mockLatestTestResults.Object);
                mockContext.Setup(m => m.TestFailureDurations).Returns(mockTestFailureDurations.Object);

                var zignetEntitiesWrapperMock = new Mock<IZigNetEntitiesWrapper>();
                zignetEntitiesWrapperMock.Setup(z => z.Get()).Returns(mockContext.Object);

                var testResult = new DomainTestResult
                {
                    Test = new DomainTest
                    {
                        TestID = 0,
                        Name = "test 1",
                        Categories = new List<DomainTestCategory>()
                    },
                    SuiteResult = new DomainSuiteResult { SuiteResultID = 3 },
                    ResultType = DomainTestResultType.Fail,
                    TestFailureDetails = new DomainTestFailureDetails { FailureType = DomainTestFailureType.Assertion }
                };

                var testResultService = new TestResultService(zignetEntitiesWrapperMock.Object, new Mock<ISuiteService>().Object, new Mock<ILatestTestResultsService>().Object);
                testResultService.SaveTestResult(testResult);
            }

            [TestMethod]
            public void AssignsSuiteNameToNewLatestTestResult()
            {
                var mockTests = new List<Test>
                {
                    new Test {
                        TestName = "test 1", TestID = 1,
                        TestCategories = new List<TestCategory>(),
                        Suites = new List<Suite> { new Suite { SuiteID = 2 } }
                    }
                }.ToDbSetMock();
                mockTests.Setup(m => m.AsNoTracking()).Returns(mockTests.Object);
                mockTests.Setup(m => m.Include(It.IsAny<string>())).Returns(mockTests.Object);

                var mockTestFailureTypes = new List<TestFailureType>{
                    new TestFailureType { TestFailureTypeID = 1, TestFailureTypeName = "Assertion" }
                }.ToDbSetMock();

                var mockTestCategories = new List<TestCategory>().ToDbSetMock();

                var mockSuiteResults = new List<SuiteResult>
                {
                    new SuiteResult { SuiteResultID = 3, SuiteId = 2 }
                }.ToDbSetMock();
                mockSuiteResults.Setup(m => m.AsNoTracking()).Returns(mockSuiteResults.Object);

                var mockSuites = new List<Suite>
                {
                    new Suite { SuiteID = 2, SuiteName = "services" }
                }.ToDbSetMock();
                mockSuites.Setup(m => m.AsNoTracking()).Returns(mockSuites.Object);
                mockSuites.Setup(m => m.Include(It.IsAny<string>())).Returns(mockSuites.Object);

                var mockTestResults = new List<TestResult>().ToDbSetMock();

                var mockTemporaryTestResults = new List<TemporaryTestResult>().ToDbSetMock();

                var mockLatestTestResults = new List<LatestTestResult>().ToDbSetMock();

                var mockTestFailureDurations = new List<TestFailureDuration>().ToDbSetMock();

                var mockContext = new Mock<ZigNetEntities>();
                mockContext.Setup(m => m.Tests).Returns(mockTests.Object);
                mockContext.Setup(m => m.TestFailureTypes).Returns(mockTestFailureTypes.Object);
                mockContext.Setup(m => m.TestCategories).Returns(mockTestCategories.Object);
                mockContext.Setup(m => m.SuiteResults).Returns(mockSuiteResults.Object);
                mockContext.Setup(m => m.Suites).Returns(mockSuites.Object);
                mockContext.Setup(m => m.TestResults).Returns(mockTestResults.Object);
                mockContext.Setup(m => m.TemporaryTestResults).Returns(mockTemporaryTestResults.Object);
                mockContext.Setup(m => m.LatestTestResults).Returns(mockLatestTestResults.Object);
                mockContext.Setup(m => m.TestFailureDurations).Returns(mockTestFailureDurations.Object);

                var zignetEntitiesWrapperMock = new Mock<IZigNetEntitiesWrapper>();
                zignetEntitiesWrapperMock.Setup(z => z.Get()).Returns(mockContext.Object);

                var testResult = new DomainTestResult
                {
                    Test = new DomainTest
                    {
                        TestID = 0,
                        Name = "test 1",
                        Categories = new List<DomainTestCategory>()
                    },
                    SuiteResult = new DomainSuiteResult { SuiteResultID = 3 },
                    ResultType = DomainTestResultType.Fail,
                    TestFailureDetails = new DomainTestFailureDetails { FailureType = DomainTestFailureType.Assertion }
                };

                var testResultService = new TestResultService(zignetEntitiesWrapperMock.Object, new Mock<ISuiteService>().Object, new Mock<ILatestTestResultsService>().Object);
                testResultService.SaveTestResult(testResult);
            }

            [TestMethod]
            public void UpdatesLatestTestResultSuiteNameIfDoesNotMatch()
            {
                var mockTests = new List<Test>
                {
                    new Test {
                        TestName = "test 1", TestID = 1,
                        TestCategories = new List<TestCategory>(),
                        Suites = new List<Suite> { new Suite { SuiteID = 2 } }
                    }
                }.ToDbSetMock();
                mockTests.Setup(m => m.AsNoTracking()).Returns(mockTests.Object);
                mockTests.Setup(m => m.Include(It.IsAny<string>())).Returns(mockTests.Object);

                var mockTestFailureTypes = new List<TestFailureType>{
                    new TestFailureType { TestFailureTypeID = 1, TestFailureTypeName = "Assertion" }
                }.ToDbSetMock();

                var mockTestCategories = new List<TestCategory>().ToDbSetMock();

                var mockSuiteResults = new List<SuiteResult>
                {
                    new SuiteResult { SuiteResultID = 3, SuiteId = 2 }
                }.ToDbSetMock();
                mockSuiteResults.Setup(m => m.AsNoTracking()).Returns(mockSuiteResults.Object);

                var mockSuites = new List<Suite>
                {
                    new Suite { SuiteID = 2, SuiteName = "new-name" }
                }.ToDbSetMock();
                mockSuites.Setup(m => m.AsNoTracking()).Returns(mockSuites.Object);
                mockSuites.Setup(m => m.Include(It.IsAny<string>())).Returns(mockSuites.Object);

                var mockTestResults = new List<TestResult>().ToDbSetMock();

                var mockTemporaryTestResults = new List<TemporaryTestResult>().ToDbSetMock();

                var mockLatestTestResults = new List<LatestTestResult> {
                    new LatestTestResult{ SuiteId = 2, TestId = 1, SuiteName = "old-name" }
                }.ToDbSetMock();

                var mockTestFailureDurations = new List<TestFailureDuration>().ToDbSetMock();

                var mockContext = new Mock<ZigNetEntities>();
                mockContext.Setup(m => m.Tests).Returns(mockTests.Object);
                mockContext.Setup(m => m.TestFailureTypes).Returns(mockTestFailureTypes.Object);
                mockContext.Setup(m => m.TestCategories).Returns(mockTestCategories.Object);
                mockContext.Setup(m => m.SuiteResults).Returns(mockSuiteResults.Object);
                mockContext.Setup(m => m.Suites).Returns(mockSuites.Object);
                mockContext.Setup(m => m.TestResults).Returns(mockTestResults.Object);
                mockContext.Setup(m => m.TemporaryTestResults).Returns(mockTemporaryTestResults.Object);
                mockContext.Setup(m => m.LatestTestResults).Returns(mockLatestTestResults.Object);
                mockContext.Setup(m => m.TestFailureDurations).Returns(mockTestFailureDurations.Object);

                var zignetEntitiesWrapperMock = new Mock<IZigNetEntitiesWrapper>();
                zignetEntitiesWrapperMock.Setup(z => z.Get()).Returns(mockContext.Object);

                var testResult = new DomainTestResult
                {
                    Test = new DomainTest
                    {
                        TestID = 0,
                        Name = "test 1",
                        Categories = new List<DomainTestCategory>()
                    },
                    SuiteResult = new DomainSuiteResult { SuiteResultID = 3 },
                    ResultType = DomainTestResultType.Fail,
                    TestFailureDetails = new DomainTestFailureDetails { FailureType = DomainTestFailureType.Assertion }
                };

                var testResultService = new TestResultService(zignetEntitiesWrapperMock.Object, new Mock<ISuiteService>().Object, new Mock<ILatestTestResultsService>().Object);
                testResultService.SaveTestResult(testResult);
            }

            [TestMethod]
            public void UpdatesPassingFromDate()
            {
                var mockTests = new List<Test>
                {
                    new Test {
                        TestName = "test 1", TestID = 1,
                        TestCategories = new List<TestCategory>(),
                        Suites = new List<Suite> { new Suite { SuiteID = 2 } }
                    }
                }.ToDbSetMock();
                mockTests.Setup(m => m.AsNoTracking()).Returns(mockTests.Object);
                mockTests.Setup(m => m.Include(It.IsAny<string>())).Returns(mockTests.Object);

                var mockTestFailureTypes = new List<TestFailureType>{
                    new TestFailureType { TestFailureTypeID = 1, TestFailureTypeName = "Assertion" }
                }.ToDbSetMock();

                var mockTestCategories = new List<TestCategory>().ToDbSetMock();

                var mockSuiteResults = new List<SuiteResult>
                {
                    new SuiteResult { SuiteResultID = 3, SuiteId = 2 }
                }.ToDbSetMock();
                mockSuiteResults.Setup(m => m.AsNoTracking()).Returns(mockSuiteResults.Object);

                var mockSuites = new List<Suite>
                {
                    new Suite { SuiteID = 2 }
                }.ToDbSetMock();
                mockSuites.Setup(m => m.AsNoTracking()).Returns(mockSuites.Object);
                mockSuites.Setup(m => m.Include(It.IsAny<string>())).Returns(mockSuites.Object);

                var mockTestResults = new List<TestResult>().ToDbSetMock();

                var mockTemporaryTestResults = new List<TemporaryTestResult>().ToDbSetMock();

                var mockLatestTestResults = new List<LatestTestResult> {
                    new LatestTestResult{ SuiteId = 2, TestId = 1, FailingFromDateTime = DateTime.UtcNow }
                }.ToDbSetMock();

                var mockTestFailureDurations = new List<TestFailureDuration>().ToDbSetMock();

                var mockContext = new Mock<ZigNetEntities>();
                mockContext.Setup(m => m.Tests).Returns(mockTests.Object);
                mockContext.Setup(m => m.TestFailureTypes).Returns(mockTestFailureTypes.Object);
                mockContext.Setup(m => m.TestCategories).Returns(mockTestCategories.Object);
                mockContext.Setup(m => m.SuiteResults).Returns(mockSuiteResults.Object);
                mockContext.Setup(m => m.Suites).Returns(mockSuites.Object);
                mockContext.Setup(m => m.TestResults).Returns(mockTestResults.Object);
                mockContext.Setup(m => m.TemporaryTestResults).Returns(mockTemporaryTestResults.Object);
                mockContext.Setup(m => m.LatestTestResults).Returns(mockLatestTestResults.Object);
                mockContext.Setup(m => m.TestFailureDurations).Returns(mockTestFailureDurations.Object);

                var zignetEntitiesWrapperMock = new Mock<IZigNetEntitiesWrapper>();
                zignetEntitiesWrapperMock.Setup(z => z.Get()).Returns(mockContext.Object);

                var testResult = new DomainTestResult
                {
                    Test = new DomainTest
                    {
                        TestID = 0,
                        Name = "test 1",
                        Categories = new List<DomainTestCategory>()
                    },
                    SuiteResult = new DomainSuiteResult { SuiteResultID = 3 },
                    ResultType = DomainTestResultType.Pass,
                };

                var testResultService = new TestResultService(zignetEntitiesWrapperMock.Object, new Mock<ISuiteService>().Object, new Mock<ILatestTestResultsService>().Object);
                testResultService.SaveTestResult(testResult);
            }

            [TestMethod]
            public void UpdatesFailingFromDate()
            {
                var mockTests = new List<Test>
                {
                    new Test {
                        TestName = "test 1", TestID = 1,
                        TestCategories = new List<TestCategory>(),
                        Suites = new List<Suite> { new Suite { SuiteID = 2 } }
                    }
                }.ToDbSetMock();
                mockTests.Setup(m => m.AsNoTracking()).Returns(mockTests.Object);
                mockTests.Setup(m => m.Include(It.IsAny<string>())).Returns(mockTests.Object);

                var mockTestFailureTypes = new List<TestFailureType>{
                    new TestFailureType { TestFailureTypeID = 1, TestFailureTypeName = "Assertion" }
                }.ToDbSetMock();

                var mockTestCategories = new List<TestCategory>().ToDbSetMock();

                var mockSuiteResults = new List<SuiteResult>
                {
                    new SuiteResult { SuiteResultID = 3, SuiteId = 2 }
                }.ToDbSetMock();
                mockSuiteResults.Setup(m => m.AsNoTracking()).Returns(mockSuiteResults.Object);

                var mockSuites = new List<Suite>
                {
                    new Suite { SuiteID = 2 }
                }.ToDbSetMock();
                mockSuites.Setup(m => m.AsNoTracking()).Returns(mockSuites.Object);
                mockSuites.Setup(m => m.Include(It.IsAny<string>())).Returns(mockSuites.Object);

                var mockTestResults = new List<TestResult>().ToDbSetMock();

                var mockTemporaryTestResults = new List<TemporaryTestResult>().ToDbSetMock();

                var mockLatestTestResults = new List<LatestTestResult> {
                    new LatestTestResult{ SuiteId = 2, TestId = 1, PassingFromDateTime = DateTime.UtcNow }
                }.ToDbSetMock();

                var mockTestFailureDurations = new List<TestFailureDuration>().ToDbSetMock();

                var mockContext = new Mock<ZigNetEntities>();
                mockContext.Setup(m => m.Tests).Returns(mockTests.Object);
                mockContext.Setup(m => m.TestFailureTypes).Returns(mockTestFailureTypes.Object);
                mockContext.Setup(m => m.TestCategories).Returns(mockTestCategories.Object);
                mockContext.Setup(m => m.SuiteResults).Returns(mockSuiteResults.Object);
                mockContext.Setup(m => m.Suites).Returns(mockSuites.Object);
                mockContext.Setup(m => m.TestResults).Returns(mockTestResults.Object);
                mockContext.Setup(m => m.TemporaryTestResults).Returns(mockTemporaryTestResults.Object);
                mockContext.Setup(m => m.LatestTestResults).Returns(mockLatestTestResults.Object);
                mockContext.Setup(m => m.TestFailureDurations).Returns(mockTestFailureDurations.Object);

                var zignetEntitiesWrapperMock = new Mock<IZigNetEntitiesWrapper>();
                zignetEntitiesWrapperMock.Setup(z => z.Get()).Returns(mockContext.Object);

                var testResult = new DomainTestResult
                {
                    Test = new DomainTest
                    {
                        TestID = 0,
                        Name = "test 1",
                        Categories = new List<DomainTestCategory>()
                    },
                    SuiteResult = new DomainSuiteResult { SuiteResultID = 3 },
                    ResultType = DomainTestResultType.Fail,
                    TestFailureDetails = new DomainTestFailureDetails { FailureType = DomainTestFailureType.Assertion }
                };

                var testResultService = new TestResultService(zignetEntitiesWrapperMock.Object, new Mock<ISuiteService>().Object, new Mock<ILatestTestResultsService>().Object);
                testResultService.SaveTestResult(testResult);
            }

            [TestMethod]
            public void UpdatesFailingFromDateIfInconclusive()
            {
                var mockTests = new List<Test>
                {
                    new Test {
                        TestName = "test 1", TestID = 1,
                        TestCategories = new List<TestCategory>(),
                        Suites = new List<Suite> { new Suite { SuiteID = 2 } }
                    }
                }.ToDbSetMock();
                mockTests.Setup(m => m.AsNoTracking()).Returns(mockTests.Object);
                mockTests.Setup(m => m.Include(It.IsAny<string>())).Returns(mockTests.Object);

                var mockTestCategories = new List<TestCategory>().ToDbSetMock();

                var mockSuiteResults = new List<SuiteResult>
                {
                    new SuiteResult { SuiteResultID = 3, SuiteId = 2 }
                }.ToDbSetMock();
                mockSuiteResults.Setup(m => m.AsNoTracking()).Returns(mockSuiteResults.Object);

                var mockSuites = new List<Suite>
                {
                    new Suite { SuiteID = 2 }
                }.ToDbSetMock();
                mockSuites.Setup(m => m.AsNoTracking()).Returns(mockSuites.Object);
                mockSuites.Setup(m => m.Include(It.IsAny<string>())).Returns(mockSuites.Object);

                var mockTestResults = new List<TestResult>().ToDbSetMock();

                var mockTemporaryTestResults = new List<TemporaryTestResult>().ToDbSetMock();

                var mockLatestTestResults = new List<LatestTestResult> {
                    new LatestTestResult{ SuiteId = 2, TestId = 1, PassingFromDateTime = DateTime.UtcNow }
                }.ToDbSetMock();

                var mockTestFailureDurations = new List<TestFailureDuration>().ToDbSetMock();

                var mockContext = new Mock<ZigNetEntities>();
                mockContext.Setup(m => m.Tests).Returns(mockTests.Object);
                mockContext.Setup(m => m.TestCategories).Returns(mockTestCategories.Object);
                mockContext.Setup(m => m.SuiteResults).Returns(mockSuiteResults.Object);
                mockContext.Setup(m => m.Suites).Returns(mockSuites.Object);
                mockContext.Setup(m => m.TestResults).Returns(mockTestResults.Object);
                mockContext.Setup(m => m.TemporaryTestResults).Returns(mockTemporaryTestResults.Object);
                mockContext.Setup(m => m.LatestTestResults).Returns(mockLatestTestResults.Object);
                mockContext.Setup(m => m.TestFailureDurations).Returns(mockTestFailureDurations.Object);

                var zignetEntitiesWrapperMock = new Mock<IZigNetEntitiesWrapper>();
                zignetEntitiesWrapperMock.Setup(z => z.Get()).Returns(mockContext.Object);

                var testResult = new DomainTestResult
                {
                    Test = new DomainTest
                    {
                        TestID = 0,
                        Name = "test 1",
                        Categories = new List<DomainTestCategory>()
                    },
                    SuiteResult = new DomainSuiteResult { SuiteResultID = 3 },
                    ResultType = DomainTestResultType.Inconclusive,
                };

                var testResultService = new TestResultService(zignetEntitiesWrapperMock.Object, new Mock<ISuiteService>().Object, new Mock<ILatestTestResultsService>().Object);
                testResultService.SaveTestResult(testResult);
            }

            [TestMethod]
            public void IgnoresPassingFromDate()
            {
                var mockTests = new List<Test>
                {
                    new Test {
                        TestName = "test 1", TestID = 1,
                        TestCategories = new List<TestCategory>(),
                        Suites = new List<Suite> { new Suite { SuiteID = 2 } }
                    }
                }.ToDbSetMock();
                mockTests.Setup(m => m.AsNoTracking()).Returns(mockTests.Object);
                mockTests.Setup(m => m.Include(It.IsAny<string>())).Returns(mockTests.Object);

                var mockTestCategories = new List<TestCategory>().ToDbSetMock();

                var mockSuiteResults = new List<SuiteResult>
                {
                    new SuiteResult { SuiteResultID = 3, SuiteId = 2 }
                }.ToDbSetMock();
                mockSuiteResults.Setup(m => m.AsNoTracking()).Returns(mockSuiteResults.Object);

                var mockSuites = new List<Suite>
                {
                    new Suite { SuiteID = 2 }
                }.ToDbSetMock();
                mockSuites.Setup(m => m.AsNoTracking()).Returns(mockSuites.Object);
                mockSuites.Setup(m => m.Include(It.IsAny<string>())).Returns(mockSuites.Object);

                var mockTestResults = new List<TestResult>().ToDbSetMock();

                var mockTemporaryTestResults = new List<TemporaryTestResult>().ToDbSetMock();

                var mockLatestTestResults = new List<LatestTestResult> {
                    new LatestTestResult{ SuiteId = 2, TestId = 1, PassingFromDateTime = DateTime.UtcNow }
                }.ToDbSetMock();

                var mockTestFailureDurations = new List<TestFailureDuration>().ToDbSetMock();

                var mockContext = new Mock<ZigNetEntities>();
                mockContext.Setup(m => m.Tests).Returns(mockTests.Object);
                mockContext.Setup(m => m.TestCategories).Returns(mockTestCategories.Object);
                mockContext.Setup(m => m.SuiteResults).Returns(mockSuiteResults.Object);
                mockContext.Setup(m => m.Suites).Returns(mockSuites.Object);
                mockContext.Setup(m => m.TestResults).Returns(mockTestResults.Object);
                mockContext.Setup(m => m.TemporaryTestResults).Returns(mockTemporaryTestResults.Object);
                mockContext.Setup(m => m.LatestTestResults).Returns(mockLatestTestResults.Object);
                mockContext.Setup(m => m.TestFailureDurations).Returns(mockTestFailureDurations.Object);

                var zignetEntitiesWrapperMock = new Mock<IZigNetEntitiesWrapper>();
                zignetEntitiesWrapperMock.Setup(z => z.Get()).Returns(mockContext.Object);

                var testResult = new DomainTestResult
                {
                    Test = new DomainTest
                    {
                        TestID = 0,
                        Name = "test 1",
                        Categories = new List<DomainTestCategory>()
                    },
                    SuiteResult = new DomainSuiteResult { SuiteResultID = 3 },
                    ResultType = DomainTestResultType.Pass,
                };

                var testResultService = new TestResultService(zignetEntitiesWrapperMock.Object, new Mock<ISuiteService>().Object, new Mock<ILatestTestResultsService>().Object);
                testResultService.SaveTestResult(testResult);
            }

            [TestMethod]
            public void IgnoresFailingFromDate()
            {
                var mockTests = new List<Test>
                {
                    new Test {
                        TestName = "test 1", TestID = 1,
                        TestCategories = new List<TestCategory>(),
                        Suites = new List<Suite> { new Suite { SuiteID = 2 } }
                    }
                }.ToDbSetMock();
                mockTests.Setup(m => m.AsNoTracking()).Returns(mockTests.Object);
                mockTests.Setup(m => m.Include(It.IsAny<string>())).Returns(mockTests.Object);

                var mockTestFailureTypes = new List<TestFailureType>{
                    new TestFailureType { TestFailureTypeID = 1, TestFailureTypeName = "Assertion" }
                }.ToDbSetMock();

                var mockTestCategories = new List<TestCategory>().ToDbSetMock();

                var mockSuiteResults = new List<SuiteResult>
                {
                    new SuiteResult { SuiteResultID = 3, SuiteId = 2 }
                }.ToDbSetMock();
                mockSuiteResults.Setup(m => m.AsNoTracking()).Returns(mockSuiteResults.Object);

                var mockSuites = new List<Suite>
                {
                    new Suite { SuiteID = 2 }
                }.ToDbSetMock();
                mockSuites.Setup(m => m.AsNoTracking()).Returns(mockSuites.Object);
                mockSuites.Setup(m => m.Include(It.IsAny<string>())).Returns(mockSuites.Object);

                var mockTestResults = new List<TestResult>().ToDbSetMock();

                var mockTemporaryTestResults = new List<TemporaryTestResult>().ToDbSetMock();

                var mockLatestTestResults = new List<LatestTestResult> {
                    new LatestTestResult{ SuiteId = 2, TestId = 1, FailingFromDateTime = DateTime.UtcNow }
                }.ToDbSetMock();

                var mockTestFailureDurations = new List<TestFailureDuration>().ToDbSetMock();

                var mockContext = new Mock<ZigNetEntities>();
                mockContext.Setup(m => m.Tests).Returns(mockTests.Object);
                mockContext.Setup(m => m.TestFailureTypes).Returns(mockTestFailureTypes.Object);
                mockContext.Setup(m => m.TestCategories).Returns(mockTestCategories.Object);
                mockContext.Setup(m => m.SuiteResults).Returns(mockSuiteResults.Object);
                mockContext.Setup(m => m.Suites).Returns(mockSuites.Object);
                mockContext.Setup(m => m.TestResults).Returns(mockTestResults.Object);
                mockContext.Setup(m => m.TemporaryTestResults).Returns(mockTemporaryTestResults.Object);
                mockContext.Setup(m => m.LatestTestResults).Returns(mockLatestTestResults.Object);
                mockContext.Setup(m => m.TestFailureDurations).Returns(mockTestFailureDurations.Object);

                var zignetEntitiesWrapperMock = new Mock<IZigNetEntitiesWrapper>();
                zignetEntitiesWrapperMock.Setup(z => z.Get()).Returns(mockContext.Object);

                var testResult = new DomainTestResult
                {
                    Test = new DomainTest
                    {
                        TestID = 0,
                        Name = "test 1",
                        Categories = new List<DomainTestCategory>()
                    },
                    SuiteResult = new DomainSuiteResult { SuiteResultID = 3 },
                    ResultType = DomainTestResultType.Fail,
                    TestFailureDetails = new DomainTestFailureDetails { FailureType = DomainTestFailureType.Assertion }
                };

                var testResultService = new TestResultService(zignetEntitiesWrapperMock.Object, new Mock<ISuiteService>().Object, new Mock<ILatestTestResultsService>().Object);
                testResultService.SaveTestResult(testResult);
            }

            [TestMethod]
            public void IgnoresFailingFromDateIfInconclusive()
            {
                var mockTests = new List<Test>
                {
                    new Test {
                        TestName = "test 1", TestID = 1,
                        TestCategories = new List<TestCategory>(),
                        Suites = new List<Suite> { new Suite { SuiteID = 2 } }
                    }
                }.ToDbSetMock();
                mockTests.Setup(m => m.AsNoTracking()).Returns(mockTests.Object);
                mockTests.Setup(m => m.Include(It.IsAny<string>())).Returns(mockTests.Object);

                var mockTestCategories = new List<TestCategory>().ToDbSetMock();

                var mockSuiteResults = new List<SuiteResult>
                {
                    new SuiteResult { SuiteResultID = 3, SuiteId = 2 }
                }.ToDbSetMock();
                mockSuiteResults.Setup(m => m.AsNoTracking()).Returns(mockSuiteResults.Object);

                var mockSuites = new List<Suite>
                {
                    new Suite { SuiteID = 2 }
                }.ToDbSetMock();
                mockSuites.Setup(m => m.AsNoTracking()).Returns(mockSuites.Object);
                mockSuites.Setup(m => m.Include(It.IsAny<string>())).Returns(mockSuites.Object);

                var mockTestResults = new List<TestResult>().ToDbSetMock();

                var mockTemporaryTestResults = new List<TemporaryTestResult>().ToDbSetMock();

                var mockLatestTestResults = new List<LatestTestResult> {
                    new LatestTestResult{ SuiteId = 2, TestId = 1, FailingFromDateTime = DateTime.UtcNow }
                }.ToDbSetMock();

                var mockTestFailureDurations = new List<TestFailureDuration>().ToDbSetMock();

                var mockContext = new Mock<ZigNetEntities>();
                mockContext.Setup(m => m.Tests).Returns(mockTests.Object);
                mockContext.Setup(m => m.TestCategories).Returns(mockTestCategories.Object);
                mockContext.Setup(m => m.SuiteResults).Returns(mockSuiteResults.Object);
                mockContext.Setup(m => m.Suites).Returns(mockSuites.Object);
                mockContext.Setup(m => m.TestResults).Returns(mockTestResults.Object);
                mockContext.Setup(m => m.TemporaryTestResults).Returns(mockTemporaryTestResults.Object);
                mockContext.Setup(m => m.LatestTestResults).Returns(mockLatestTestResults.Object);
                mockContext.Setup(m => m.TestFailureDurations).Returns(mockTestFailureDurations.Object);

                var zignetEntitiesWrapperMock = new Mock<IZigNetEntitiesWrapper>();
                zignetEntitiesWrapperMock.Setup(z => z.Get()).Returns(mockContext.Object);

                var testResult = new DomainTestResult
                {
                    Test = new DomainTest
                    {
                        TestID = 0,
                        Name = "test 1",
                        Categories = new List<DomainTestCategory>()
                    },
                    SuiteResult = new DomainSuiteResult { SuiteResultID = 3 },
                    ResultType = DomainTestResultType.Inconclusive,
                };

                var testResultService = new TestResultService(zignetEntitiesWrapperMock.Object, new Mock<ISuiteService>().Object, new Mock<ILatestTestResultsService>().Object);
                testResultService.SaveTestResult(testResult);
            }

            [TestMethod]
            public void IgnoresTestFailedDurationIfAlwaysPassed()
            {
                var mockTests = new List<Test>
                {
                    new Test {
                        TestName = "test 1", TestID = 1,
                        TestCategories = new List<TestCategory>(),
                        Suites = new List<Suite> { new Suite { SuiteID = 2 } }
                    }
                }.ToDbSetMock();
                mockTests.Setup(m => m.AsNoTracking()).Returns(mockTests.Object);
                mockTests.Setup(m => m.Include(It.IsAny<string>())).Returns(mockTests.Object);

                var mockTestCategories = new List<TestCategory>().ToDbSetMock();

                var mockSuiteResults = new List<SuiteResult>
                {
                    new SuiteResult { SuiteResultID = 3, SuiteId = 2 }
                }.ToDbSetMock();
                mockSuiteResults.Setup(m => m.AsNoTracking()).Returns(mockSuiteResults.Object);

                var mockSuites = new List<Suite>
                {
                    new Suite { SuiteID = 2 }
                }.ToDbSetMock();
                mockSuites.Setup(m => m.AsNoTracking()).Returns(mockSuites.Object);
                mockSuites.Setup(m => m.Include(It.IsAny<string>())).Returns(mockSuites.Object);

                var mockTestResults = new List<TestResult>().ToDbSetMock();

                var mockTemporaryTestResults = new List<TemporaryTestResult>().ToDbSetMock();

                var mockLatestTestResults = new List<LatestTestResult> {
                    new LatestTestResult{ SuiteId = 2, TestId = 1, PassingFromDateTime = DateTime.UtcNow }
                }.ToDbSetMock();

                var mockTestFailureDurations = new List<TestFailureDuration>().ToDbSetMock();

                var mockContext = new Mock<ZigNetEntities>();
                mockContext.Setup(m => m.Tests).Returns(mockTests.Object);
                mockContext.Setup(m => m.TestCategories).Returns(mockTestCategories.Object);
                mockContext.Setup(m => m.SuiteResults).Returns(mockSuiteResults.Object);
                mockContext.Setup(m => m.Suites).Returns(mockSuites.Object);
                mockContext.Setup(m => m.TestResults).Returns(mockTestResults.Object);
                mockContext.Setup(m => m.TemporaryTestResults).Returns(mockTemporaryTestResults.Object);
                mockContext.Setup(m => m.LatestTestResults).Returns(mockLatestTestResults.Object);
                mockContext.Setup(m => m.TestFailureDurations).Returns(mockTestFailureDurations.Object);

                var zignetEntitiesWrapperMock = new Mock<IZigNetEntitiesWrapper>();
                zignetEntitiesWrapperMock.Setup(z => z.Get()).Returns(mockContext.Object);

                var testResult = new DomainTestResult
                {
                    Test = new DomainTest
                    {
                        TestID = 0,
                        Name = "test 1",
                        Categories = new List<DomainTestCategory>()
                    },
                    SuiteResult = new DomainSuiteResult { SuiteResultID = 3 },
                    ResultType = DomainTestResultType.Pass,
                };

                var testResultService = new TestResultService(zignetEntitiesWrapperMock.Object, new Mock<ISuiteService>().Object, new Mock<ILatestTestResultsService>().Object);
                testResultService.SaveTestResult(testResult);
            }

            [TestMethod]
            public void IgnoresTestFailedDurationIfPassedBefore()
            {
                var mockTests = new List<Test>
                {
                    new Test {
                        TestName = "test 1", TestID = 1,
                        TestCategories = new List<TestCategory>(),
                        Suites = new List<Suite> { new Suite { SuiteID = 2 } }
                    }
                }.ToDbSetMock();
                mockTests.Setup(m => m.AsNoTracking()).Returns(mockTests.Object);
                mockTests.Setup(m => m.Include(It.IsAny<string>())).Returns(mockTests.Object);

                var mockTestCategories = new List<TestCategory>().ToDbSetMock();

                var mockSuiteResults = new List<SuiteResult>
                {
                    new SuiteResult { SuiteResultID = 3, SuiteId = 2 }
                }.ToDbSetMock();
                mockSuiteResults.Setup(m => m.AsNoTracking()).Returns(mockSuiteResults.Object);

                var mockSuites = new List<Suite>
                {
                    new Suite { SuiteID = 2 }
                }.ToDbSetMock();
                mockSuites.Setup(m => m.AsNoTracking()).Returns(mockSuites.Object);
                mockSuites.Setup(m => m.Include(It.IsAny<string>())).Returns(mockSuites.Object);

                var mockTestResults = new List<TestResult>().ToDbSetMock();

                var mockTemporaryTestResults = new List<TemporaryTestResult>().ToDbSetMock();

                var mockLatestTestResults = new List<LatestTestResult> {
                    new LatestTestResult{ SuiteId = 2, TestId = 1, FailingFromDateTime = DateTime.UtcNow }
                }.ToDbSetMock();

                var mockTestFailureDurations = new List<TestFailureDuration>
                {
                    new TestFailureDuration { SuiteId = 2, TestId = 1, FailureStartDateTime = DateTime.UtcNow, FailureEndDateTime = DateTime.UtcNow }
                }.ToDbSetMock();

                var mockContext = new Mock<ZigNetEntities>();
                mockContext.Setup(m => m.Tests).Returns(mockTests.Object);
                mockContext.Setup(m => m.TestCategories).Returns(mockTestCategories.Object);
                mockContext.Setup(m => m.SuiteResults).Returns(mockSuiteResults.Object);
                mockContext.Setup(m => m.Suites).Returns(mockSuites.Object);
                mockContext.Setup(m => m.TestResults).Returns(mockTestResults.Object);
                mockContext.Setup(m => m.TemporaryTestResults).Returns(mockTemporaryTestResults.Object);
                mockContext.Setup(m => m.LatestTestResults).Returns(mockLatestTestResults.Object);
                mockContext.Setup(m => m.TestFailureDurations).Returns(mockTestFailureDurations.Object);

                var zignetEntitiesWrapperMock = new Mock<IZigNetEntitiesWrapper>();
                zignetEntitiesWrapperMock.Setup(z => z.Get()).Returns(mockContext.Object);

                var testResult = new DomainTestResult
                {
                    Test = new DomainTest
                    {
                        TestID = 0,
                        Name = "test 1",
                        Categories = new List<DomainTestCategory>()
                    },
                    SuiteResult = new DomainSuiteResult { SuiteResultID = 3 },
                    ResultType = DomainTestResultType.Pass,
                };

                var testResultService = new TestResultService(zignetEntitiesWrapperMock.Object, new Mock<ISuiteService>().Object, new Mock<ILatestTestResultsService>().Object);
                testResultService.SaveTestResult(testResult);
            }

            [TestMethod]
            public void UpdatesTestFailedDurationEndTimeWhenNewlyPassing()
            {
                var mockTests = new List<Test>
                {
                    new Test {
                        TestName = "test 1", TestID = 1,
                        TestCategories = new List<TestCategory>(),
                        Suites = new List<Suite> { new Suite { SuiteID = 2 } }
                    }
                }.ToDbSetMock();
                mockTests.Setup(m => m.AsNoTracking()).Returns(mockTests.Object);
                mockTests.Setup(m => m.Include(It.IsAny<string>())).Returns(mockTests.Object);

                var mockTestCategories = new List<TestCategory>().ToDbSetMock();

                var mockSuiteResults = new List<SuiteResult>
                {
                    new SuiteResult { SuiteResultID = 3, SuiteId = 2 }
                }.ToDbSetMock();
                mockSuiteResults.Setup(m => m.AsNoTracking()).Returns(mockSuiteResults.Object);

                var mockSuites = new List<Suite>
                {
                    new Suite { SuiteID = 2 }
                }.ToDbSetMock();
                mockSuites.Setup(m => m.AsNoTracking()).Returns(mockSuites.Object);
                mockSuites.Setup(m => m.Include(It.IsAny<string>())).Returns(mockSuites.Object);

                var mockTestResults = new List<TestResult>().ToDbSetMock();

                var mockTemporaryTestResults = new List<TemporaryTestResult>().ToDbSetMock();

                var mockLatestTestResults = new List<LatestTestResult> {
                    new LatestTestResult{ SuiteId = 2, TestId = 1, FailingFromDateTime = DateTime.UtcNow }
                }.ToDbSetMock();

                var mockTestFailureDurations = new List<TestFailureDuration>
                {
                    new TestFailureDuration { SuiteId = 2, TestId = 1, FailureStartDateTime = DateTime.Now }
                }.ToDbSetMock();

                var mockContext = new Mock<ZigNetEntities>();
                mockContext.Setup(m => m.Tests).Returns(mockTests.Object);
                mockContext.Setup(m => m.TestCategories).Returns(mockTestCategories.Object);
                mockContext.Setup(m => m.SuiteResults).Returns(mockSuiteResults.Object);
                mockContext.Setup(m => m.Suites).Returns(mockSuites.Object);
                mockContext.Setup(m => m.TestResults).Returns(mockTestResults.Object);
                mockContext.Setup(m => m.TemporaryTestResults).Returns(mockTemporaryTestResults.Object);
                mockContext.Setup(m => m.LatestTestResults).Returns(mockLatestTestResults.Object);
                mockContext.Setup(m => m.TestFailureDurations).Returns(mockTestFailureDurations.Object);

                var zignetEntitiesWrapperMock = new Mock<IZigNetEntitiesWrapper>();
                zignetEntitiesWrapperMock.Setup(z => z.Get()).Returns(mockContext.Object);

                var testResult = new DomainTestResult
                {
                    Test = new DomainTest
                    {
                        TestID = 0,
                        Name = "test 1",
                        Categories = new List<DomainTestCategory>()
                    },
                    SuiteResult = new DomainSuiteResult { SuiteResultID = 3 },
                    ResultType = DomainTestResultType.Pass,
                };

                var testResultService = new TestResultService(zignetEntitiesWrapperMock.Object, new Mock<ISuiteService>().Object, new Mock<ILatestTestResultsService>().Object);
                testResultService.SaveTestResult(testResult);
            }

            [TestMethod]
            public void CreatesTestFailedDurationIfFirstFailure()
            {
                var mockTests = new List<Test>
                {
                    new Test {
                        TestName = "test 1", TestID = 1,
                        TestCategories = new List<TestCategory>(),
                        Suites = new List<Suite> { new Suite { SuiteID = 2 } }
                    }
                }.ToDbSetMock();
                mockTests.Setup(m => m.AsNoTracking()).Returns(mockTests.Object);
                mockTests.Setup(m => m.Include(It.IsAny<string>())).Returns(mockTests.Object);

                var mockTestFailureTypes = new List<TestFailureType>{
                    new TestFailureType { TestFailureTypeID = 1, TestFailureTypeName = "Assertion" }
                }.ToDbSetMock();

                var mockTestCategories = new List<TestCategory>().ToDbSetMock();

                var mockSuiteResults = new List<SuiteResult>
                {
                    new SuiteResult { SuiteResultID = 3, SuiteId = 2 }
                }.ToDbSetMock();
                mockSuiteResults.Setup(m => m.AsNoTracking()).Returns(mockSuiteResults.Object);

                var mockSuites = new List<Suite>
                {
                    new Suite { SuiteID = 2 }
                }.ToDbSetMock();
                mockSuites.Setup(m => m.AsNoTracking()).Returns(mockSuites.Object);
                mockSuites.Setup(m => m.Include(It.IsAny<string>())).Returns(mockSuites.Object);

                var mockTestResults = new List<TestResult>().ToDbSetMock();

                var mockTemporaryTestResults = new List<TemporaryTestResult>().ToDbSetMock();

                var mockLatestTestResults = new List<LatestTestResult> {
                    new LatestTestResult{ SuiteId = 2, TestId = 1, FailingFromDateTime = DateTime.UtcNow }
                }.ToDbSetMock();

                var mockTestFailureDurations = new List<TestFailureDuration>
                {
                    new TestFailureDuration { SuiteId = 2, TestId = 1, FailureStartDateTime = DateTime.Now, FailureEndDateTime = DateTime.Now }
                }.ToDbSetMock();

                var mockContext = new Mock<ZigNetEntities>();
                mockContext.Setup(m => m.Tests).Returns(mockTests.Object);
                mockContext.Setup(m => m.TestFailureTypes).Returns(mockTestFailureTypes.Object);
                mockContext.Setup(m => m.TestCategories).Returns(mockTestCategories.Object);
                mockContext.Setup(m => m.SuiteResults).Returns(mockSuiteResults.Object);
                mockContext.Setup(m => m.Suites).Returns(mockSuites.Object);
                mockContext.Setup(m => m.TestResults).Returns(mockTestResults.Object);
                mockContext.Setup(m => m.TemporaryTestResults).Returns(mockTemporaryTestResults.Object);
                mockContext.Setup(m => m.LatestTestResults).Returns(mockLatestTestResults.Object);
                mockContext.Setup(m => m.TestFailureDurations).Returns(mockTestFailureDurations.Object);

                var zignetEntitiesWrapperMock = new Mock<IZigNetEntitiesWrapper>();
                zignetEntitiesWrapperMock.Setup(z => z.Get()).Returns(mockContext.Object);

                var testResult = new DomainTestResult
                {
                    Test = new DomainTest
                    {
                        TestID = 0,
                        Name = "test 1",
                        Categories = new List<DomainTestCategory>()
                    },
                    SuiteResult = new DomainSuiteResult { SuiteResultID = 3 },
                    ResultType = DomainTestResultType.Fail,
                    TestFailureDetails = new DomainTestFailureDetails { FailureType = DomainTestFailureType.Assertion }
                };

                var testResultService = new TestResultService(zignetEntitiesWrapperMock.Object, new Mock<ISuiteService>().Object, new Mock<ILatestTestResultsService>().Object);
                testResultService.SaveTestResult(testResult);
            }

            [TestMethod]
            public void CreatesTestFailedDurationIfNewFailure()
            {
                var mockTests = new List<Test>
                {
                    new Test {
                        TestName = "test 1", TestID = 1,
                        TestCategories = new List<TestCategory>(),
                        Suites = new List<Suite> { new Suite { SuiteID = 2 } }
                    }
                }.ToDbSetMock();
                mockTests.Setup(m => m.AsNoTracking()).Returns(mockTests.Object);
                mockTests.Setup(m => m.Include(It.IsAny<string>())).Returns(mockTests.Object);

                var mockTestFailureTypes = new List<TestFailureType>{
                    new TestFailureType { TestFailureTypeID = 1, TestFailureTypeName = "Assertion" }
                }.ToDbSetMock();

                var mockTestCategories = new List<TestCategory>().ToDbSetMock();

                var mockSuiteResults = new List<SuiteResult>
                {
                    new SuiteResult { SuiteResultID = 3, SuiteId = 2 }
                }.ToDbSetMock();
                mockSuiteResults.Setup(m => m.AsNoTracking()).Returns(mockSuiteResults.Object);

                var mockSuites = new List<Suite>
                {
                    new Suite { SuiteID = 2 }
                }.ToDbSetMock();
                mockSuites.Setup(m => m.AsNoTracking()).Returns(mockSuites.Object);
                mockSuites.Setup(m => m.Include(It.IsAny<string>())).Returns(mockSuites.Object);

                var mockTestResults = new List<TestResult>().ToDbSetMock();

                var mockTemporaryTestResults = new List<TemporaryTestResult>().ToDbSetMock();

                var mockLatestTestResults = new List<LatestTestResult> {
                    new LatestTestResult{ SuiteId = 2, TestId = 1, FailingFromDateTime = DateTime.UtcNow }
                }.ToDbSetMock();

                var mockTestFailureDurations = new List<TestFailureDuration>
                {
                    new TestFailureDuration { SuiteId = 2, TestId = 1, FailureStartDateTime = DateTime.Now, FailureEndDateTime = DateTime.Now }
                }.ToDbSetMock();

                var mockContext = new Mock<ZigNetEntities>();
                mockContext.Setup(m => m.Tests).Returns(mockTests.Object);
                mockContext.Setup(m => m.TestFailureTypes).Returns(mockTestFailureTypes.Object);
                mockContext.Setup(m => m.TestCategories).Returns(mockTestCategories.Object);
                mockContext.Setup(m => m.SuiteResults).Returns(mockSuiteResults.Object);
                mockContext.Setup(m => m.Suites).Returns(mockSuites.Object);
                mockContext.Setup(m => m.TestResults).Returns(mockTestResults.Object);
                mockContext.Setup(m => m.TemporaryTestResults).Returns(mockTemporaryTestResults.Object);
                mockContext.Setup(m => m.LatestTestResults).Returns(mockLatestTestResults.Object);
                mockContext.Setup(m => m.TestFailureDurations).Returns(mockTestFailureDurations.Object);

                var zignetEntitiesWrapperMock = new Mock<IZigNetEntitiesWrapper>();
                zignetEntitiesWrapperMock.Setup(z => z.Get()).Returns(mockContext.Object);

                var testResult = new DomainTestResult
                {
                    Test = new DomainTest
                    {
                        TestID = 0,
                        Name = "test 1",
                        Categories = new List<DomainTestCategory>()
                    },
                    SuiteResult = new DomainSuiteResult { SuiteResultID = 3 },
                    ResultType = DomainTestResultType.Fail,
                    TestFailureDetails = new DomainTestFailureDetails { FailureType = DomainTestFailureType.Assertion }
                };

                var testResultService = new TestResultService(zignetEntitiesWrapperMock.Object, new Mock<ISuiteService>().Object, new Mock<ILatestTestResultsService>().Object);
                testResultService.SaveTestResult(testResult);
            }

            [TestMethod]
            public void IgnoresTestFailedDurationIfSecondFailure()
            {
                var mockTests = new List<Test>
                {
                    new Test {
                        TestName = "test 1", TestID = 1,
                        TestCategories = new List<TestCategory>(),
                        Suites = new List<Suite> { new Suite { SuiteID = 2 } }
                    }
                }.ToDbSetMock();
                mockTests.Setup(m => m.AsNoTracking()).Returns(mockTests.Object);
                mockTests.Setup(m => m.Include(It.IsAny<string>())).Returns(mockTests.Object);

                var mockTestFailureTypes = new List<TestFailureType>{
                    new TestFailureType { TestFailureTypeID = 1, TestFailureTypeName = "Assertion" }
                }.ToDbSetMock();

                var mockTestCategories = new List<TestCategory>().ToDbSetMock();

                var mockSuiteResults = new List<SuiteResult>
                {
                    new SuiteResult { SuiteResultID = 3, SuiteId = 2 }
                }.ToDbSetMock();
                mockSuiteResults.Setup(m => m.AsNoTracking()).Returns(mockSuiteResults.Object);

                var mockSuites = new List<Suite>
                {
                    new Suite { SuiteID = 2 }
                }.ToDbSetMock();
                mockSuites.Setup(m => m.AsNoTracking()).Returns(mockSuites.Object);
                mockSuites.Setup(m => m.Include(It.IsAny<string>())).Returns(mockSuites.Object);

                var mockTestResults = new List<TestResult>().ToDbSetMock();

                var mockTemporaryTestResults = new List<TemporaryTestResult>().ToDbSetMock();

                var mockLatestTestResults = new List<LatestTestResult> {
                    new LatestTestResult{ SuiteId = 2, TestId = 1, FailingFromDateTime = DateTime.UtcNow }
                }.ToDbSetMock();

                var mockTestFailureDurations = new List<TestFailureDuration>
                {
                    new TestFailureDuration { SuiteId = 2, TestId = 1, FailureStartDateTime = DateTime.Now }
                }.ToDbSetMock();

                var mockContext = new Mock<ZigNetEntities>();
                mockContext.Setup(m => m.Tests).Returns(mockTests.Object);
                mockContext.Setup(m => m.TestFailureTypes).Returns(mockTestFailureTypes.Object);
                mockContext.Setup(m => m.TestCategories).Returns(mockTestCategories.Object);
                mockContext.Setup(m => m.SuiteResults).Returns(mockSuiteResults.Object);
                mockContext.Setup(m => m.Suites).Returns(mockSuites.Object);
                mockContext.Setup(m => m.TestResults).Returns(mockTestResults.Object);
                mockContext.Setup(m => m.TemporaryTestResults).Returns(mockTemporaryTestResults.Object);
                mockContext.Setup(m => m.LatestTestResults).Returns(mockLatestTestResults.Object);
                mockContext.Setup(m => m.TestFailureDurations).Returns(mockTestFailureDurations.Object);

                var zignetEntitiesWrapperMock = new Mock<IZigNetEntitiesWrapper>();
                zignetEntitiesWrapperMock.Setup(z => z.Get()).Returns(mockContext.Object);

                var testResult = new DomainTestResult
                {
                    Test = new DomainTest
                    {
                        TestID = 0,
                        Name = "test 1",
                        Categories = new List<DomainTestCategory>()
                    },
                    SuiteResult = new DomainSuiteResult { SuiteResultID = 3 },
                    ResultType = DomainTestResultType.Fail,
                    TestFailureDetails = new DomainTestFailureDetails { FailureType = DomainTestFailureType.Assertion }
                };

                var testResultService = new TestResultService(zignetEntitiesWrapperMock.Object, new Mock<ISuiteService>().Object, new Mock<ILatestTestResultsService>().Object);
                testResultService.SaveTestResult(testResult);
            }

            [TestMethod]
            public void GetsLatestTestFailureDurationRecord()
            {
                var mockTests = new List<Test>
                {
                    new Test {
                        TestName = "test 1", TestID = 1,
                        TestCategories = new List<TestCategory>(),
                        Suites = new List<Suite> { new Suite { SuiteID = 2 } }
                    }
                }.ToDbSetMock();
                mockTests.Setup(m => m.AsNoTracking()).Returns(mockTests.Object);
                mockTests.Setup(m => m.Include(It.IsAny<string>())).Returns(mockTests.Object);

                var mockTestFailureTypes = new List<TestFailureType>{
                    new TestFailureType { TestFailureTypeID = 1, TestFailureTypeName = "Assertion" }
                }.ToDbSetMock();

                var mockTestCategories = new List<TestCategory>().ToDbSetMock();

                var mockSuiteResults = new List<SuiteResult>
                {
                    new SuiteResult { SuiteResultID = 3, SuiteId = 2 }
                }.ToDbSetMock();
                mockSuiteResults.Setup(m => m.AsNoTracking()).Returns(mockSuiteResults.Object);

                var mockSuites = new List<Suite>
                {
                    new Suite { SuiteID = 2 }
                }.ToDbSetMock();
                mockSuites.Setup(m => m.AsNoTracking()).Returns(mockSuites.Object);
                mockSuites.Setup(m => m.Include(It.IsAny<string>())).Returns(mockSuites.Object);

                var mockTestResults = new List<TestResult>().ToDbSetMock();

                var mockTemporaryTestResults = new List<TemporaryTestResult>().ToDbSetMock();

                var mockLatestTestResults = new List<LatestTestResult> {
                    new LatestTestResult{ SuiteId = 2, TestId = 1, FailingFromDateTime = DateTime.UtcNow }
                }.ToDbSetMock();

                var mockTestFailureDurations = new List<TestFailureDuration>
                {
                    new TestFailureDuration { TestFailureDurationID = 2, SuiteId = 2, TestId = 1, FailureStartDateTime = DateTime.UtcNow.AddDays(5) },
                    new TestFailureDuration { TestFailureDurationID = 3, SuiteId = 2, TestId = 1, FailureStartDateTime = DateTime.Now },
                    new TestFailureDuration { TestFailureDurationID = 1, SuiteId = 2, TestId = 1, FailureStartDateTime = new DateTime(3000, 1, 1) }
                }.ToDbSetMock();

                var mockContext = new Mock<ZigNetEntities>();
                mockContext.Setup(m => m.Tests).Returns(mockTests.Object);
                mockContext.Setup(m => m.TestFailureTypes).Returns(mockTestFailureTypes.Object);
                mockContext.Setup(m => m.TestCategories).Returns(mockTestCategories.Object);
                mockContext.Setup(m => m.SuiteResults).Returns(mockSuiteResults.Object);
                mockContext.Setup(m => m.Suites).Returns(mockSuites.Object);
                mockContext.Setup(m => m.TestResults).Returns(mockTestResults.Object);
                mockContext.Setup(m => m.TemporaryTestResults).Returns(mockTemporaryTestResults.Object);
                mockContext.Setup(m => m.LatestTestResults).Returns(mockLatestTestResults.Object);
                mockContext.Setup(m => m.TestFailureDurations).Returns(mockTestFailureDurations.Object);

                var zignetEntitiesWrapperMock = new Mock<IZigNetEntitiesWrapper>();
                zignetEntitiesWrapperMock.Setup(z => z.Get()).Returns(mockContext.Object);

                var testResult = new DomainTestResult
                {
                    Test = new DomainTest
                    {
                        TestID = 0,
                        Name = "test 1",
                        Categories = new List<DomainTestCategory>()
                    },
                    SuiteResult = new DomainSuiteResult { SuiteResultID = 3 },
                    ResultType = DomainTestResultType.Fail,
                    TestFailureDetails = new DomainTestFailureDetails { FailureType = DomainTestFailureType.Assertion }
                };

                var testResultService = new TestResultService(zignetEntitiesWrapperMock.Object, new Mock<ISuiteService>().Object, new Mock<ILatestTestResultsService>().Object);
                testResultService.SaveTestResult(testResult);
            }

            [TestMethod]
            public void IgnoresTestFailureDurationWithoutSuiteId()
            {
                var mockTests = new List<Test>
                {
                    new Test {
                        TestName = "test 1", TestID = 1,
                        TestCategories = new List<TestCategory>(),
                        Suites = new List<Suite> { new Suite { SuiteID = 2 } }
                    }
                }.ToDbSetMock();
                mockTests.Setup(m => m.AsNoTracking()).Returns(mockTests.Object);
                mockTests.Setup(m => m.Include(It.IsAny<string>())).Returns(mockTests.Object);

                var mockTestFailureTypes = new List<TestFailureType>{
                    new TestFailureType { TestFailureTypeID = 1, TestFailureTypeName = "Assertion" }
                }.ToDbSetMock();

                var mockTestCategories = new List<TestCategory>().ToDbSetMock();

                var mockSuiteResults = new List<SuiteResult>
                {
                    new SuiteResult { SuiteResultID = 3, SuiteId = 2 }
                }.ToDbSetMock();
                mockSuiteResults.Setup(m => m.AsNoTracking()).Returns(mockSuiteResults.Object);

                var mockSuites = new List<Suite>
                {
                    new Suite { SuiteID = 2 }
                }.ToDbSetMock();
                mockSuites.Setup(m => m.AsNoTracking()).Returns(mockSuites.Object);
                mockSuites.Setup(m => m.Include(It.IsAny<string>())).Returns(mockSuites.Object);

                var mockTestResults = new List<TestResult>().ToDbSetMock();

                var mockTemporaryTestResults = new List<TemporaryTestResult>().ToDbSetMock();

                var mockLatestTestResults = new List<LatestTestResult> {
                    new LatestTestResult{ SuiteId = 2, TestId = 1, FailingFromDateTime = DateTime.UtcNow }
                }.ToDbSetMock();

                var mockTestFailureDurations = new List<TestFailureDuration>
                {
                    new TestFailureDuration { SuiteId = 99, TestId = 1, FailureStartDateTime = new DateTime(3000, 1, 1) },
                    new TestFailureDuration { SuiteId = 2, TestId = 1, FailureStartDateTime = DateTime.Now }
                }.ToDbSetMock();

                var mockContext = new Mock<ZigNetEntities>();
                mockContext.Setup(m => m.Tests).Returns(mockTests.Object);
                mockContext.Setup(m => m.TestFailureTypes).Returns(mockTestFailureTypes.Object);
                mockContext.Setup(m => m.TestCategories).Returns(mockTestCategories.Object);
                mockContext.Setup(m => m.SuiteResults).Returns(mockSuiteResults.Object);
                mockContext.Setup(m => m.Suites).Returns(mockSuites.Object);
                mockContext.Setup(m => m.TestResults).Returns(mockTestResults.Object);
                mockContext.Setup(m => m.TemporaryTestResults).Returns(mockTemporaryTestResults.Object);
                mockContext.Setup(m => m.LatestTestResults).Returns(mockLatestTestResults.Object);
                mockContext.Setup(m => m.TestFailureDurations).Returns(mockTestFailureDurations.Object);

                var zignetEntitiesWrapperMock = new Mock<IZigNetEntitiesWrapper>();
                zignetEntitiesWrapperMock.Setup(z => z.Get()).Returns(mockContext.Object);

                var testResult = new DomainTestResult
                {
                    Test = new DomainTest
                    {
                        TestID = 0,
                        Name = "test 1",
                        Categories = new List<DomainTestCategory>()
                    },
                    SuiteResult = new DomainSuiteResult { SuiteResultID = 3 },
                    ResultType = DomainTestResultType.Fail,
                    TestFailureDetails = new DomainTestFailureDetails { FailureType = DomainTestFailureType.Assertion }
                };

                var testResultService = new TestResultService(zignetEntitiesWrapperMock.Object, new Mock<ISuiteService>().Object, new Mock<ILatestTestResultsService>().Object);
                testResultService.SaveTestResult(testResult);
            }

            [TestMethod]
            public void IgnoresTestFailureDurationWithoutTestId()
            {
                var mockTests = new List<Test>
                {
                    new Test {
                        TestName = "test 1", TestID = 1,
                        TestCategories = new List<TestCategory>(),
                        Suites = new List<Suite> { new Suite { SuiteID = 2 } }
                    }
                }.ToDbSetMock();
                mockTests.Setup(m => m.AsNoTracking()).Returns(mockTests.Object);
                mockTests.Setup(m => m.Include(It.IsAny<string>())).Returns(mockTests.Object);

                var mockTestFailureTypes = new List<TestFailureType>{
                    new TestFailureType { TestFailureTypeID = 1, TestFailureTypeName = "Assertion" }
                }.ToDbSetMock();

                var mockTestCategories = new List<TestCategory>().ToDbSetMock();

                var mockSuiteResults = new List<SuiteResult>
                {
                    new SuiteResult { SuiteResultID = 3, SuiteId = 2 }
                }.ToDbSetMock();
                mockSuiteResults.Setup(m => m.AsNoTracking()).Returns(mockSuiteResults.Object);

                var mockSuites = new List<Suite>
                {
                    new Suite { SuiteID = 2 }
                }.ToDbSetMock();
                mockSuites.Setup(m => m.AsNoTracking()).Returns(mockSuites.Object);
                mockSuites.Setup(m => m.Include(It.IsAny<string>())).Returns(mockSuites.Object);

                var mockTestResults = new List<TestResult>().ToDbSetMock();

                var mockTemporaryTestResults = new List<TemporaryTestResult>().ToDbSetMock();

                var mockLatestTestResults = new List<LatestTestResult> {
                    new LatestTestResult{ SuiteId = 2, TestId = 1, FailingFromDateTime = DateTime.UtcNow }
                }.ToDbSetMock();

                var mockTestFailureDurations = new List<TestFailureDuration>
                {
                    new TestFailureDuration { SuiteId = 2, TestId = 99, FailureStartDateTime = new DateTime(3000, 1, 1) },
                    new TestFailureDuration { SuiteId = 2, TestId = 1, FailureStartDateTime = DateTime.Now }
                }.ToDbSetMock();

                var mockContext = new Mock<ZigNetEntities>();
                mockContext.Setup(m => m.Tests).Returns(mockTests.Object);
                mockContext.Setup(m => m.TestFailureTypes).Returns(mockTestFailureTypes.Object);
                mockContext.Setup(m => m.TestCategories).Returns(mockTestCategories.Object);
                mockContext.Setup(m => m.SuiteResults).Returns(mockSuiteResults.Object);
                mockContext.Setup(m => m.Suites).Returns(mockSuites.Object);
                mockContext.Setup(m => m.TestResults).Returns(mockTestResults.Object);
                mockContext.Setup(m => m.TemporaryTestResults).Returns(mockTemporaryTestResults.Object);
                mockContext.Setup(m => m.LatestTestResults).Returns(mockLatestTestResults.Object);
                mockContext.Setup(m => m.TestFailureDurations).Returns(mockTestFailureDurations.Object);

                var zignetEntitiesWrapperMock = new Mock<IZigNetEntitiesWrapper>();
                zignetEntitiesWrapperMock.Setup(z => z.Get()).Returns(mockContext.Object);

                var testResult = new DomainTestResult
                {
                    Test = new DomainTest
                    {
                        TestID = 0,
                        Name = "test 1",
                        Categories = new List<DomainTestCategory>()
                    },
                    SuiteResult = new DomainSuiteResult { SuiteResultID = 3 },
                    ResultType = DomainTestResultType.Fail,
                    TestFailureDetails = new DomainTestFailureDetails { FailureType = DomainTestFailureType.Assertion }
                };

                var testResultService = new TestResultService(zignetEntitiesWrapperMock.Object, new Mock<ISuiteService>().Object, new Mock<ILatestTestResultsService>().Object);
                testResultService.SaveTestResult(testResult);
            }
        }
    }
}
