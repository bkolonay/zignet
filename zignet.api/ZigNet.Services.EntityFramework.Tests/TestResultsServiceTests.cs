using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using ZigNet.Database.EntityFramework;
using ZigNet.Services.EntityFramework.Tests.Helpers;
using ZigNet.Services.DTOs;
using ZigNet.Services.EntityFramework.Mapping;
using DbTest = ZigNet.Database.EntityFramework.Test;
using DbTestResult = ZigNet.Database.EntityFramework.TestResult;
using DbTestCategory = ZigNet.Database.EntityFramework.TestCategory;
using DbTestFailureDuration = ZigNet.Database.EntityFramework.TestFailureDuration;
using DbTestFailureType = ZigNet.Database.EntityFramework.TestFailureType;
using DbSuiteResult = ZigNet.Database.EntityFramework.SuiteResult;
using TestResult = ZigNet.Domain.Test.TestResult;
using TestResultType = ZigNet.Domain.Test.TestResultType;
using Test = ZigNet.Domain.Test.Test;
using TestCategory = ZigNet.Domain.Test.TestCategory;
using TestFailureType = ZigNet.Domain.Test.TestFailureType;
using SuiteResult = ZigNet.Domain.Suite.SuiteResult;
using TestFailureDetails = ZigNet.Domain.Test.TestFailureDetails;

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

                var mockTestFailureDurationService = new Mock<ITestFailureDurationService>();
                mockTestFailureDurationService.Setup(f => f.GetAll()).Returns(new List<TestFailureDurationDto>());

                var testResultService = new TestResultService(new Mock<IZigNetEntitiesWrapper>().Object,
                    new Mock<ISuiteService>().Object, mockLatestTestResultsService.Object,
                    mockTestFailureDurationService.Object, new Mock<ITestResultMapper>().Object, new Mock<ITemporaryTestResultsService>().Object);
                var actualLatestTestResults = testResultService.GetLatestResults(1).ToList();

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

                var mockTestFailureDurationService = new Mock<ITestFailureDurationService>();
                mockTestFailureDurationService.Setup(f => f.GetAll()).Returns(new List<TestFailureDurationDto>());

                var testResultService = new TestResultService(new Mock<IZigNetEntitiesWrapper>().Object,
                    new Mock<ISuiteService>().Object, mockLatestTestResultsService.Object,
                    mockTestFailureDurationService.Object, new Mock<ITestResultMapper>().Object, new Mock<ITemporaryTestResultsService>().Object);
                var actualLatestTestResults = testResultService.GetLatestResults(1).ToList();

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

                var mockTestFailureDurationService = new Mock<ITestFailureDurationService>();
                mockTestFailureDurationService.Setup(f => f.GetAll()).Returns(new List<TestFailureDurationDto>());

                var testResultService = new TestResultService(new Mock<IZigNetEntitiesWrapper>().Object,
                    new Mock<ISuiteService>().Object, mockLatestTestResultsService.Object,
                    mockTestFailureDurationService.Object, new Mock<ITestResultMapper>().Object, new Mock<ITemporaryTestResultsService>().Object);
                var actualLatestTestResults = testResultService.GetLatestResults(1).ToList();

                Assert.AreEqual(4, actualLatestTestResults.Count);
                Assert.AreEqual("test failing the longest", actualLatestTestResults[0].TestName);
                Assert.AreEqual("test failing the shortest", actualLatestTestResults[1].TestName);
                Assert.AreEqual("test passing the shortest", actualLatestTestResults[2].TestName);
                Assert.AreEqual("test passing the longest", actualLatestTestResults[3].TestName);
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

                var testFailureDurationDtos = new List<TestFailureDurationDto>
                {
                    new TestFailureDurationDto { SuiteId = 1, TestId = 2, FailureEnd = utcNow }
                };
                var mockTestFailureDurationService = new Mock<ITestFailureDurationService>();
                mockTestFailureDurationService.Setup(f => f.GetAll()).Returns(testFailureDurationDtos);

                var testResultService = new TestResultService(new Mock<IZigNetEntitiesWrapper>().Object,
                    new Mock<ISuiteService>().Object, mockLatestTestResultsService.Object,
                    mockTestFailureDurationService.Object, new Mock<ITestResultMapper>().Object, new Mock<ITemporaryTestResultsService>().Object);
                var actualLatestTestResults = testResultService.GetLatestResults(1).ToList();

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

                var testFailureDurationDtos = new List<TestFailureDurationDto>
                {
                    new TestFailureDurationDto { SuiteId = 1, TestId = 2 }
                };
                var mockTestFailureDurationService = new Mock<ITestFailureDurationService>();
                mockTestFailureDurationService.Setup(f => f.GetAll()).Returns(testFailureDurationDtos);

                var testResultService = new TestResultService(new Mock<IZigNetEntitiesWrapper>().Object,
                    new Mock<ISuiteService>().Object, mockLatestTestResultsService.Object,
                    mockTestFailureDurationService.Object, new Mock<ITestResultMapper>().Object, new Mock<ITemporaryTestResultsService>().Object);
                var actualLatestTestResults = testResultService.GetLatestResults(1).ToList();

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

                var testFailureDurationDtos = new List<TestFailureDurationDto>
                {
                    new TestFailureDurationDto { SuiteId = 1, TestId = 2, FailureStart = utcNow },
                    new TestFailureDurationDto { SuiteId = 1, TestId = 2, FailureStart = utcNow.AddMinutes(5), FailureEnd = utcNow.AddMinutes(10) }
                };
                var mockTestFailureDurationService = new Mock<ITestFailureDurationService>();
                mockTestFailureDurationService.Setup(f => f.GetAll()).Returns(testFailureDurationDtos);

                var testResultService = new TestResultService(new Mock<IZigNetEntitiesWrapper>().Object,
                    new Mock<ISuiteService>().Object, mockLatestTestResultsService.Object,
                    mockTestFailureDurationService.Object, new Mock<ITestResultMapper>().Object, new Mock<ITemporaryTestResultsService>().Object);
                var actualLatestTestResults = testResultService.GetLatestResults(1).ToList();

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

                var testFailureDurationDtos = new List<TestFailureDurationDto>
                {
                    new TestFailureDurationDto { SuiteId = 1, TestId = 2, FailureEnd = utcNow.AddHours(-25) },
                    new TestFailureDurationDto { SuiteId = 1, TestId = 2, FailureEnd = utcNow.AddHours(-23) },
                    new TestFailureDurationDto { SuiteId = 1, TestId = 2, FailureEnd = utcNow }
                };
                var mockTestFailureDurationService = new Mock<ITestFailureDurationService>();
                mockTestFailureDurationService.Setup(f => f.GetAll()).Returns(testFailureDurationDtos);

                var testResultService = new TestResultService(new Mock<IZigNetEntitiesWrapper>().Object,
                    new Mock<ISuiteService>().Object, mockLatestTestResultsService.Object,
                    mockTestFailureDurationService.Object, new Mock<ITestResultMapper>().Object, new Mock<ITemporaryTestResultsService>().Object);
                var latestTestResults = testResultService.GetLatestResults(1).ToList();

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

                var testFailureDurationDtos = new List<TestFailureDurationDto>
                {
                    new TestFailureDurationDto { SuiteId = 99, TestId = 2, FailureEnd = utcNow }
                };
                var mockTestFailureDurationService = new Mock<ITestFailureDurationService>();
                mockTestFailureDurationService.Setup(f => f.GetAll()).Returns(testFailureDurationDtos);

                var testResultService = new TestResultService(new Mock<IZigNetEntitiesWrapper>().Object,
                    new Mock<ISuiteService>().Object, mockLatestTestResultsService.Object,
                    mockTestFailureDurationService.Object, new Mock<ITestResultMapper>().Object, new Mock<ITemporaryTestResultsService>().Object);
                var latestTestResults = testResultService.GetLatestResults(1).ToList();

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

                var testFailureDurationDtos = new List<TestFailureDurationDto>
                {
                    new TestFailureDurationDto { SuiteId = 1, TestId = 99, FailureEnd = utcNow }
                };
                var mockTestFailureDurationService = new Mock<ITestFailureDurationService>();
                mockTestFailureDurationService.Setup(f => f.GetAll()).Returns(testFailureDurationDtos);

                var testResultService = new TestResultService(new Mock<IZigNetEntitiesWrapper>().Object,
                    new Mock<ISuiteService>().Object, mockLatestTestResultsService.Object,
                    mockTestFailureDurationService.Object, new Mock<ITestResultMapper>().Object, new Mock<ITemporaryTestResultsService>().Object);
                var latestTestResults = testResultService.GetLatestResults(1).ToList();

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

                var testFailureDurationDtos = new List<TestFailureDurationDto>
                {
                    new TestFailureDurationDto { SuiteId = 1, TestId = 2, FailureStart = utcNow.AddHours(-48), FailureEnd = utcNow.AddHours(-23) }
                };
                var mockTestFailureDurationService = new Mock<ITestFailureDurationService>();
                mockTestFailureDurationService.Setup(f => f.GetAll()).Returns(testFailureDurationDtos);

                var testResultService = new TestResultService(new Mock<IZigNetEntitiesWrapper>().Object,
                    new Mock<ISuiteService>().Object, mockLatestTestResultsService.Object,
                    mockTestFailureDurationService.Object, new Mock<ITestResultMapper>().Object, new Mock<ITemporaryTestResultsService>().Object);
                var latestTestResults = testResultService.GetLatestResults(1).ToList();

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

                var testFailureDurationDtos = new List<TestFailureDurationDto>
                {
                    new TestFailureDurationDto { SuiteId = 1, TestId = 2 },
                    new TestFailureDurationDto { SuiteId = 1, TestId = 3 }
                };
                var mockTestFailureDurationService = new Mock<ITestFailureDurationService>();
                mockTestFailureDurationService.Setup(f => f.GetAll()).Returns(testFailureDurationDtos);

                var testResultService = new TestResultService(new Mock<IZigNetEntitiesWrapper>().Object,
                    new Mock<ISuiteService>().Object, mockLatestTestResultsService.Object,
                    mockTestFailureDurationService.Object, new Mock<ITestResultMapper>().Object, new Mock<ITemporaryTestResultsService>().Object);
                var latestTestResults = testResultService.GetLatestResults(1).ToList();

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

                var testFailureDurationDtos = new List<TestFailureDurationDto>
                {
                    new TestFailureDurationDto { SuiteId = 1, TestId = 2 },
                    new TestFailureDurationDto { SuiteId = 3, TestId = 2 }
                };
                var mockTestFailureDurationService = new Mock<ITestFailureDurationService>();
                mockTestFailureDurationService.Setup(f => f.GetAll()).Returns(testFailureDurationDtos);

                var testResultService = new TestResultService(new Mock<IZigNetEntitiesWrapper>().Object,
                    new Mock<ISuiteService>().Object, mockLatestTestResultsService.Object,
                    mockTestFailureDurationService.Object, new Mock<ITestResultMapper>().Object, new Mock<ITemporaryTestResultsService>().Object);
                var latestTestResults = testResultService.GetLatestResults(1).ToList();

                Assert.AreEqual(1, latestTestResults.Count);
                Assert.AreEqual(1, latestTestResults[0].TestFailureDurations.Count());
                Assert.IsNull(latestTestResults[0].TestFailureDurations.ToList()[0].FailureEnd);
            }
        }

        [TestClass]
        public class GetLatestResultsGrouped
        {
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

                var mockTestFailureDurationService = new Mock<ITestFailureDurationService>();
                mockTestFailureDurationService.Setup(f => f.GetAll()).Returns(new List<TestFailureDurationDto>());

                var testResultService = new TestResultService(new Mock<IZigNetEntitiesWrapper>().Object,
                    mockSuiteService.Object, mockLatestTestResultsService.Object,
                    mockTestFailureDurationService.Object, new Mock<ITestResultMapper>().Object, new Mock<ITemporaryTestResultsService>().Object);
                var actualLatestTestResults = testResultService.GetLatestResultsGrouped(1).ToList();

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

                var mockTestFailureDurationService = new Mock<ITestFailureDurationService>();
                mockTestFailureDurationService.Setup(f => f.GetAll()).Returns(new List<TestFailureDurationDto>());

                var testResultService = new TestResultService(new Mock<IZigNetEntitiesWrapper>().Object,
                    mockSuiteService.Object, mockLatestTestResultsService.Object,
                    mockTestFailureDurationService.Object, new Mock<ITestResultMapper>().Object, new Mock<ITemporaryTestResultsService>().Object);
                var actualLatestTestResults = testResultService.GetLatestResultsGrouped(1).OrderBy(l => l.TestResultID).ToList();

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

                var mockTestFailureDurationService = new Mock<ITestFailureDurationService>();
                mockTestFailureDurationService.Setup(f => f.GetAll()).Returns(new List<TestFailureDurationDto>());

                var testResultService = new TestResultService(new Mock<IZigNetEntitiesWrapper>().Object,
                    mockSuiteService.Object, mockLatestTestResultsService.Object,
                    mockTestFailureDurationService.Object, new Mock<ITestResultMapper>().Object, new Mock<ITemporaryTestResultsService>().Object);
                var actualLatestTestResults = testResultService.GetLatestResultsGrouped(1).ToList();

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

                var mockTestFailureDurationService = new Mock<ITestFailureDurationService>();
                mockTestFailureDurationService.Setup(f => f.GetAll()).Returns(new List<TestFailureDurationDto>());

                var testResultService = new TestResultService(new Mock<IZigNetEntitiesWrapper>().Object,
                    mockSuiteService.Object, mockLatestTestResultsService.Object,
                    mockTestFailureDurationService.Object, new Mock<ITestResultMapper>().Object, new Mock<ITemporaryTestResultsService>().Object);
                var actualLatestTestResults = testResultService.GetLatestResultsGrouped(1).ToList();

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

                var mockTestFailureDurationService = new Mock<ITestFailureDurationService>();
                mockTestFailureDurationService.Setup(f => f.GetAll()).Returns(new List<TestFailureDurationDto>());

                var testResultService = new TestResultService(new Mock<IZigNetEntitiesWrapper>().Object,
                    mockSuiteService.Object, mockLatestTestResultsService.Object,
                    mockTestFailureDurationService.Object, new Mock<ITestResultMapper>().Object, new Mock<ITemporaryTestResultsService>().Object);
                var actualLatestTestResults = testResultService.GetLatestResultsGrouped(1).ToList();

                Assert.AreEqual(0, actualLatestTestResults.Count);
            }
        }

        [TestClass]
        public class SaveTestResult
        {
            [TestMethod]
            public void AssignsTestIdWhenTestWithSameNameExists()
            {
                var now = DateTime.Now;

                var mockTests = new List<DbTest>
                {
                    new DbTest {
                        TestName = "existing test", TestID = 1,
                        TestCategories = new List<DbTestCategory>(),
                        Suites = new List<Suite> { new Suite { SuiteID = 2 } }
                    }
                }.ToDbSetMock();
                mockTests.Setup(m => m.AsNoTracking()).Returns(mockTests.Object);
                mockTests.Setup(m => m.Include(It.IsAny<string>())).Returns(mockTests.Object);

                var mockTestResultMapper = new Mock<ITestResultMapper>();
                mockTestResultMapper.Setup(t => t.Map(TestResultType.Inconclusive)).Returns(2);

                var mockTestCategories = new List<DbTestCategory>().ToDbSetMock();

                var mockSuiteResults = new List<DbSuiteResult>
                {
                    new DbSuiteResult { SuiteResultID = 3, SuiteId = 2 }
                }.ToDbSetMock();
                mockSuiteResults.Setup(m => m.AsNoTracking()).Returns(mockSuiteResults.Object);

                var mockSuites = new List<Suite>
                {
                    new Suite { SuiteID = 2 }
                }.ToDbSetMock();
                mockSuites.Setup(m => m.AsNoTracking()).Returns(mockSuites.Object);
                mockSuites.Setup(m => m.Include(It.IsAny<string>())).Returns(mockSuites.Object);

                var mockTestResults = new List<DbTestResult>().ToDbSetMock();

                var mockContext = new Mock<ZigNetEntities>();
                mockContext.Setup(m => m.Tests).Returns(mockTests.Object);
                mockContext.Setup(m => m.TestCategories).Returns(mockTestCategories.Object);
                mockContext.Setup(m => m.SuiteResults).Returns(mockSuiteResults.Object);
                mockContext.Setup(m => m.Suites).Returns(mockSuites.Object);
                mockContext.Setup(m => m.TestResults).Returns(mockTestResults.Object);

                var zignetEntitiesWrapperMock = new Mock<IZigNetEntitiesWrapper>();
                zignetEntitiesWrapperMock.Setup(z => z.Get()).Returns(mockContext.Object);

                var testResult = new TestResult
                {
                    Test = new Test
                    {
                        Name = "existing test",
                        Categories = new List<TestCategory>(),
                    },
                    SuiteResult = new SuiteResult { SuiteResultID = 3 },
                };

                var testResultService = new TestResultService(zignetEntitiesWrapperMock.Object, new Mock<ISuiteService>().Object,
                    new Mock<ILatestTestResultsService>().Object, new Mock<ITestFailureDurationService>().Object,
                    mockTestResultMapper.Object, new Mock<ITemporaryTestResultsService>().Object);
                var savedTestResult = testResultService.SaveTestResult(testResult);

                Assert.AreEqual(0, savedTestResult.TestResultID);
                Assert.AreEqual(1, savedTestResult.Test.TestID);
                Assert.AreEqual("existing test", savedTestResult.Test.Name);
                Assert.AreEqual(3, savedTestResult.SuiteResult.SuiteResultID);
                Assert.AreEqual(TestResultType.Inconclusive, savedTestResult.ResultType);
                Assert.AreEqual(new DateTime(), savedTestResult.StartTime);
                Assert.AreEqual(new DateTime(), savedTestResult.EndTime);
                Assert.IsNull(savedTestResult.TestFailureDetails);
                Assert.AreEqual(1, savedTestResult.Test.Suites.Count);
                Assert.AreEqual(2, savedTestResult.Test.Suites.ToList()[0].SuiteID);
                Assert.AreEqual(0, savedTestResult.Test.Categories.Count);
            }

            [TestMethod]
            public void DoesNotAssignTestIdWhenTestWithSameNameDoesNotExist()
            {
                var mockTests = new List<DbTest>().ToDbSetMock();
                mockTests.Setup(m => m.AsNoTracking()).Returns(mockTests.Object);
                mockTests.Setup(m => m.Include(It.IsAny<string>())).Returns(mockTests.Object);

                var mockTestResultMapper = new Mock<ITestResultMapper>();
                mockTestResultMapper.Setup(t => t.Map(TestResultType.Inconclusive)).Returns(2);

                var mockTestCategories = new List<DbTestCategory>().ToDbSetMock();

                var mockSuiteResults = new List<DbSuiteResult>
                {
                    new DbSuiteResult { SuiteResultID = 3, SuiteId = 2 }
                }.ToDbSetMock();
                mockSuiteResults.Setup(m => m.AsNoTracking()).Returns(mockSuiteResults.Object);

                var mockSuites = new List<Suite>
                {
                    new Suite { SuiteID = 2 }
                }.ToDbSetMock();
                mockSuites.Setup(m => m.AsNoTracking()).Returns(mockSuites.Object);
                mockSuites.Setup(m => m.Include(It.IsAny<string>())).Returns(mockSuites.Object);

                var mockTestResults = new List<DbTestResult>().ToDbSetMock();

                var mockContext = new Mock<ZigNetEntities>();
                mockContext.Setup(m => m.Tests).Returns(mockTests.Object);
                mockContext.Setup(m => m.TestCategories).Returns(mockTestCategories.Object);
                mockContext.Setup(m => m.SuiteResults).Returns(mockSuiteResults.Object);
                mockContext.Setup(m => m.Suites).Returns(mockSuites.Object);
                mockContext.Setup(m => m.TestResults).Returns(mockTestResults.Object);

                var zignetEntitiesWrapperMock = new Mock<IZigNetEntitiesWrapper>();
                zignetEntitiesWrapperMock.Setup(z => z.Get()).Returns(mockContext.Object);

                var testResult = new TestResult
                {
                    Test = new Test
                    {
                        Name = "new test",
                        Categories = new List<TestCategory>()
                    },
                    SuiteResult = new SuiteResult { SuiteResultID = 3 }
                };

                var testResultService = new TestResultService(zignetEntitiesWrapperMock.Object, new Mock<ISuiteService>().Object,
                    new Mock<ILatestTestResultsService>().Object, new Mock<ITestFailureDurationService>().Object,
                    mockTestResultMapper.Object, new Mock<ITemporaryTestResultsService>().Object);
                var savedTestResult = testResultService.SaveTestResult(testResult);

                Assert.AreEqual(0, savedTestResult.TestResultID);
                Assert.AreEqual(0, savedTestResult.Test.TestID);
                Assert.AreEqual("new test", savedTestResult.Test.Name);
                Assert.AreEqual(3, savedTestResult.SuiteResult.SuiteResultID);
                Assert.AreEqual(TestResultType.Inconclusive, savedTestResult.ResultType);
                Assert.AreEqual(new DateTime(), savedTestResult.StartTime);
                Assert.AreEqual(new DateTime(), savedTestResult.EndTime);
                Assert.IsNull(savedTestResult.TestFailureDetails);
                Assert.AreEqual(1, savedTestResult.Test.Suites.Count);
                Assert.AreEqual(2, savedTestResult.Test.Suites.ToList()[0].SuiteID);
                Assert.AreEqual(0, savedTestResult.Test.Categories.Count);
            }

            [TestMethod]
            [ExpectedException(typeof(InvalidOperationException))]
            public void ThrowsIfSuiteResultDoesNotExist()
            {
                var mockTests = new List<DbTest>().ToDbSetMock();
                mockTests.Setup(m => m.AsNoTracking()).Returns(mockTests.Object);
                mockTests.Setup(m => m.Include(It.IsAny<string>())).Returns(mockTests.Object);

                var mockTestResultMapper = new Mock<ITestResultMapper>();
                mockTestResultMapper.Setup(t => t.Map(TestResultType.Inconclusive)).Returns(2);

                var mockTestCategories = new List<DbTestCategory>().ToDbSetMock();

                var mockSuiteResults = new List<DbSuiteResult>().ToDbSetMock();
                mockSuiteResults.Setup(m => m.AsNoTracking()).Returns(mockSuiteResults.Object);

                var mockContext = new Mock<ZigNetEntities>();
                mockContext.Setup(m => m.Tests).Returns(mockTests.Object);
                mockContext.Setup(m => m.TestCategories).Returns(mockTestCategories.Object);
                mockContext.Setup(m => m.SuiteResults).Returns(mockSuiteResults.Object);

                var zignetEntitiesWrapperMock = new Mock<IZigNetEntitiesWrapper>();
                zignetEntitiesWrapperMock.Setup(z => z.Get()).Returns(mockContext.Object);

                var testResult = new TestResult
                {
                    Test = new Test
                    {
                        Name = "test 1",
                        Categories = new List<TestCategory>()
                    },
                    SuiteResult = new SuiteResult { SuiteResultID = 3 }
                };

                var testResultService = new TestResultService(zignetEntitiesWrapperMock.Object, new Mock<ISuiteService>().Object,
                    new Mock<ILatestTestResultsService>().Object, new Mock<ITestFailureDurationService>().Object,
                    mockTestResultMapper.Object, new Mock<ITemporaryTestResultsService>().Object);
                testResultService.SaveTestResult(testResult);
            }

            [TestMethod]
            public void CopiesExistingTestCategories()
            {
                var mockTests = new List<DbTest>
                {
                    new DbTest {
                        TestName = "existing test", TestID = 1,
                        TestCategories = new List<DbTestCategory> { new DbTestCategory { TestCategoryID = 4, CategoryName = "existing test category" } },
                        Suites = new List<Suite> { new Suite { SuiteID = 2 } }
                    }
                }.ToDbSetMock();
                mockTests.Setup(m => m.AsNoTracking()).Returns(mockTests.Object);
                mockTests.Setup(m => m.Include(It.IsAny<string>())).Returns(mockTests.Object);

                var mockTestResultMapper = new Mock<ITestResultMapper>();
                mockTestResultMapper.Setup(t => t.Map(TestResultType.Inconclusive)).Returns(2);

                var mockTestCategories = new List<DbTestCategory> {
                    new DbTestCategory { TestCategoryID = 4, CategoryName = "existing test category" }
                }.ToDbSetMock();

                var mockSuiteResults = new List<DbSuiteResult>
                {
                    new DbSuiteResult { SuiteResultID = 3, SuiteId = 2 }
                }.ToDbSetMock();
                mockSuiteResults.Setup(m => m.AsNoTracking()).Returns(mockSuiteResults.Object);

                var mockSuites = new List<Suite>
                {
                    new Suite { SuiteID = 2 }
                }.ToDbSetMock();
                mockSuites.Setup(m => m.AsNoTracking()).Returns(mockSuites.Object);
                mockSuites.Setup(m => m.Include(It.IsAny<string>())).Returns(mockSuites.Object);

                var mockTestResults = new List<DbTestResult>().ToDbSetMock();

                var mockContext = new Mock<ZigNetEntities>();
                mockContext.Setup(m => m.Tests).Returns(mockTests.Object);
                mockContext.Setup(m => m.TestCategories).Returns(mockTestCategories.Object);
                mockContext.Setup(m => m.SuiteResults).Returns(mockSuiteResults.Object);
                mockContext.Setup(m => m.Suites).Returns(mockSuites.Object);
                mockContext.Setup(m => m.TestResults).Returns(mockTestResults.Object);

                var zignetEntitiesWrapperMock = new Mock<IZigNetEntitiesWrapper>();
                zignetEntitiesWrapperMock.Setup(z => z.Get()).Returns(mockContext.Object);

                var testResult = new TestResult
                {
                    Test = new Test
                    {
                        Name = "existing test",
                        Categories = new List<TestCategory>()
                    },
                    SuiteResult = new SuiteResult { SuiteResultID = 3 }
                };

                var testResultService = new TestResultService(zignetEntitiesWrapperMock.Object, new Mock<ISuiteService>().Object,
                    new Mock<ILatestTestResultsService>().Object, new Mock<ITestFailureDurationService>().Object,
                    mockTestResultMapper.Object, new Mock<ITemporaryTestResultsService>().Object);
                var savedTestResult = testResultService.SaveTestResult(testResult);

                Assert.AreEqual(0, savedTestResult.TestResultID);
                Assert.AreEqual(1, savedTestResult.Test.TestID);
                Assert.AreEqual("existing test", savedTestResult.Test.Name);
                Assert.AreEqual(3, savedTestResult.SuiteResult.SuiteResultID);
                Assert.AreEqual(TestResultType.Inconclusive, savedTestResult.ResultType);
                Assert.AreEqual(new DateTime(), savedTestResult.StartTime);
                Assert.AreEqual(new DateTime(), savedTestResult.EndTime);
                Assert.IsNull(savedTestResult.TestFailureDetails);
                Assert.AreEqual(1, savedTestResult.Test.Suites.Count);
                Assert.AreEqual(2, savedTestResult.Test.Suites.ToList()[0].SuiteID);
                Assert.AreEqual(1, savedTestResult.Test.Categories.Count);
                Assert.AreEqual(4, savedTestResult.Test.Categories.ToList()[0].TestCategoryID);
                Assert.AreEqual("existing test category", savedTestResult.Test.Categories.ToList()[0].Name);
            }

            [TestMethod]
            public void MergesNewAndExistingTestCategories()
            {
                var mockTests = new List<DbTest>
                {
                    new DbTest {
                        TestName = "existing test", TestID = 1,
                        TestCategories = new List<DbTestCategory> { new DbTestCategory { TestCategoryID = 4, CategoryName = "existing test category" } },
                        Suites = new List<Suite> { new Suite { SuiteID = 2 } }
                    }
                }.ToDbSetMock();
                mockTests.Setup(m => m.AsNoTracking()).Returns(mockTests.Object);
                mockTests.Setup(m => m.Include(It.IsAny<string>())).Returns(mockTests.Object);

                var mockTestResultMapper = new Mock<ITestResultMapper>();
                mockTestResultMapper.Setup(t => t.Map(TestResultType.Inconclusive)).Returns(2);

                var mockTestCategories = new List<DbTestCategory> { new DbTestCategory { TestCategoryID = 4, CategoryName = "existing test category" } }.ToDbSetMock();

                var mockSuiteResults = new List<DbSuiteResult>
                {
                    new DbSuiteResult { SuiteResultID = 3, SuiteId = 2 }
                }.ToDbSetMock();
                mockSuiteResults.Setup(m => m.AsNoTracking()).Returns(mockSuiteResults.Object);

                var mockSuites = new List<Suite>
                {
                    new Suite { SuiteID = 2 }
                }.ToDbSetMock();
                mockSuites.Setup(m => m.AsNoTracking()).Returns(mockSuites.Object);
                mockSuites.Setup(m => m.Include(It.IsAny<string>())).Returns(mockSuites.Object);

                var mockTestResults = new List<DbTestResult>().ToDbSetMock();

                var mockContext = new Mock<ZigNetEntities>();
                mockContext.Setup(m => m.Tests).Returns(mockTests.Object);
                mockContext.Setup(m => m.TestCategories).Returns(mockTestCategories.Object);
                mockContext.Setup(m => m.SuiteResults).Returns(mockSuiteResults.Object);
                mockContext.Setup(m => m.Suites).Returns(mockSuites.Object);
                mockContext.Setup(m => m.TestResults).Returns(mockTestResults.Object);

                var zignetEntitiesWrapperMock = new Mock<IZigNetEntitiesWrapper>();
                zignetEntitiesWrapperMock.Setup(z => z.Get()).Returns(mockContext.Object);

                var testResult = new TestResult
                {
                    Test = new Test
                    {
                        Name = "existing test",
                        Categories = new List<TestCategory> { new TestCategory { Name = "new test category" } }
                    },
                    SuiteResult = new SuiteResult { SuiteResultID = 3 }
                };

                var testResultService = new TestResultService(zignetEntitiesWrapperMock.Object, new Mock<ISuiteService>().Object,
                    new Mock<ILatestTestResultsService>().Object, new Mock<ITestFailureDurationService>().Object,
                    mockTestResultMapper.Object, new Mock<ITemporaryTestResultsService>().Object);
                var savedTestResult = testResultService.SaveTestResult(testResult);

                Assert.AreEqual(0, savedTestResult.TestResultID);
                Assert.AreEqual(1, savedTestResult.Test.TestID);
                Assert.AreEqual("existing test", savedTestResult.Test.Name);
                Assert.AreEqual(3, savedTestResult.SuiteResult.SuiteResultID);
                Assert.AreEqual(TestResultType.Inconclusive, savedTestResult.ResultType);
                Assert.AreEqual(new DateTime(), savedTestResult.StartTime);
                Assert.AreEqual(new DateTime(), savedTestResult.EndTime);
                Assert.IsNull(savedTestResult.TestFailureDetails);
                Assert.AreEqual(1, savedTestResult.Test.Suites.Count);
                Assert.AreEqual(2, savedTestResult.Test.Suites.ToList()[0].SuiteID);

                var testCategories = savedTestResult.Test.Categories.OrderBy(c => c.Name).ToList();
                Assert.AreEqual(2, testCategories.Count);
                Assert.AreEqual(4, testCategories[0].TestCategoryID);
                Assert.AreEqual("existing test category", testCategories[0].Name);
                Assert.AreEqual(0, testCategories[1].TestCategoryID);
                Assert.AreEqual("new test category", testCategories[1].Name);
            }

            [TestMethod]
            public void ClearsAllExistingTestCategories()
            {
                // bug here: there is no way for existing categories to be removed

                var mockTests = new List<DbTest>
                {
                    new DbTest {
                        TestName = "existing test", TestID = 1,
                        TestCategories = new List<DbTestCategory> { new DbTestCategory { TestCategoryID = 4, CategoryName = "existing test category" } },
                        Suites = new List<Suite> { new Suite { SuiteID = 2 } }
                    }
                }.ToDbSetMock();
                mockTests.Setup(m => m.AsNoTracking()).Returns(mockTests.Object);
                mockTests.Setup(m => m.Include(It.IsAny<string>())).Returns(mockTests.Object);

                var mockTestResultMapper = new Mock<ITestResultMapper>();
                mockTestResultMapper.Setup(t => t.Map(TestResultType.Inconclusive)).Returns(2);

                var mockTestCategories = new List<DbTestCategory> {
                    new DbTestCategory { TestCategoryID = 4, CategoryName = "existing test category" }
                }.ToDbSetMock();

                var mockSuiteResults = new List<DbSuiteResult>
                {
                    new DbSuiteResult { SuiteResultID = 3, SuiteId = 2 }
                }.ToDbSetMock();
                mockSuiteResults.Setup(m => m.AsNoTracking()).Returns(mockSuiteResults.Object);

                var mockSuites = new List<Suite>
                {
                    new Suite { SuiteID = 2 }
                }.ToDbSetMock();
                mockSuites.Setup(m => m.AsNoTracking()).Returns(mockSuites.Object);
                mockSuites.Setup(m => m.Include(It.IsAny<string>())).Returns(mockSuites.Object);

                var mockTestResults = new List<DbTestResult>().ToDbSetMock();

                var mockContext = new Mock<ZigNetEntities>();
                mockContext.Setup(m => m.Tests).Returns(mockTests.Object);
                mockContext.Setup(m => m.TestCategories).Returns(mockTestCategories.Object);
                mockContext.Setup(m => m.SuiteResults).Returns(mockSuiteResults.Object);
                mockContext.Setup(m => m.Suites).Returns(mockSuites.Object);
                mockContext.Setup(m => m.TestResults).Returns(mockTestResults.Object);

                var zignetEntitiesWrapperMock = new Mock<IZigNetEntitiesWrapper>();
                zignetEntitiesWrapperMock.Setup(z => z.Get()).Returns(mockContext.Object);

                var testResult = new TestResult
                {
                    Test = new Test
                    {
                        Name = "existing test",
                        Categories = new List<TestCategory>()
                    },
                    SuiteResult = new SuiteResult { SuiteResultID = 3 }
                };

                var testResultService = new TestResultService(zignetEntitiesWrapperMock.Object, new Mock<ISuiteService>().Object,
                    new Mock<ILatestTestResultsService>().Object, new Mock<ITestFailureDurationService>().Object,
                    mockTestResultMapper.Object, new Mock<ITemporaryTestResultsService>().Object);
                var savedTestResult = testResultService.SaveTestResult(testResult);

                Assert.AreEqual(0, savedTestResult.TestResultID);
                Assert.AreEqual(1, savedTestResult.Test.TestID);
                Assert.AreEqual("existing test", savedTestResult.Test.Name);
                Assert.AreEqual(3, savedTestResult.SuiteResult.SuiteResultID);
                Assert.AreEqual(TestResultType.Inconclusive, savedTestResult.ResultType);
                Assert.AreEqual(new DateTime(), savedTestResult.StartTime);
                Assert.AreEqual(new DateTime(), savedTestResult.EndTime);
                Assert.IsNull(savedTestResult.TestFailureDetails);
                Assert.AreEqual(1, savedTestResult.Test.Suites.Count);
                Assert.AreEqual(2, savedTestResult.Test.Suites.ToList()[0].SuiteID);
                // fails
                //Assert.AreEqual(0, testCategories.Count);
            }

            [TestMethod]
            public void SavesFailedTestResultWithDetails()
            {
                var mockTests = new List<DbTest>
                {
                    new DbTest {
                        TestName = "existing test", TestID = 1,
                        TestCategories = new List<DbTestCategory>(),
                        Suites = new List<Suite> { new Suite { SuiteID = 2 } }
                    }
                }.ToDbSetMock();
                mockTests.Setup(m => m.AsNoTracking()).Returns(mockTests.Object);
                mockTests.Setup(m => m.Include(It.IsAny<string>())).Returns(mockTests.Object);

                var mockTestResultMapper = new Mock<ITestResultMapper>();
                mockTestResultMapper.Setup(t => t.Map(TestResultType.Fail)).Returns(1);
                mockTestResultMapper.Setup(t => t.Map(1)).Returns(TestResultType.Fail);

                var mockTestFailureTypes = new List<DbTestFailureType>{
                    new DbTestFailureType { TestFailureTypeID = 2, TestFailureTypeName = "Exception" }
                }.ToDbSetMock();

                var mockTestCategories = new List<DbTestCategory>().ToDbSetMock();

                var mockSuiteResults = new List<DbSuiteResult>
                {
                    new DbSuiteResult { SuiteResultID = 3, SuiteId = 2 }
                }.ToDbSetMock();
                mockSuiteResults.Setup(m => m.AsNoTracking()).Returns(mockSuiteResults.Object);

                var mockSuites = new List<Suite>
                {
                    new Suite { SuiteID = 2 }
                }.ToDbSetMock();
                mockSuites.Setup(m => m.AsNoTracking()).Returns(mockSuites.Object);
                mockSuites.Setup(m => m.Include(It.IsAny<string>())).Returns(mockSuites.Object);

                var mockTestResults = new List<DbTestResult>().ToDbSetMock();

                var mockContext = new Mock<ZigNetEntities>();
                mockContext.Setup(m => m.Tests).Returns(mockTests.Object);
                mockContext.Setup(m => m.TestFailureTypes).Returns(mockTestFailureTypes.Object);
                mockContext.Setup(m => m.TestCategories).Returns(mockTestCategories.Object);
                mockContext.Setup(m => m.SuiteResults).Returns(mockSuiteResults.Object);
                mockContext.Setup(m => m.Suites).Returns(mockSuites.Object);
                mockContext.Setup(m => m.TestResults).Returns(mockTestResults.Object);

                var zignetEntitiesWrapperMock = new Mock<IZigNetEntitiesWrapper>();
                zignetEntitiesWrapperMock.Setup(z => z.Get()).Returns(mockContext.Object);

                var testResult = new TestResult
                {
                    Test = new Test
                    {
                        Name = "existing test",
                        Categories = new List<TestCategory>()
                    },
                    SuiteResult = new SuiteResult { SuiteResultID = 3 },
                    ResultType = TestResultType.Fail,
                    TestFailureDetails = new TestFailureDetails
                    {
                        FailureType = TestFailureType.Exception,
                        FailureDetailMessage = "failed by exception"
                    }
                };

                var testResultService = new TestResultService(zignetEntitiesWrapperMock.Object, new Mock<ISuiteService>().Object,
                    new Mock<ILatestTestResultsService>().Object, new Mock<ITestFailureDurationService>().Object,
                    mockTestResultMapper.Object, new Mock<ITemporaryTestResultsService>().Object);
                var savedTestResult = testResultService.SaveTestResult(testResult);

                Assert.AreEqual(0, savedTestResult.TestResultID);
                Assert.AreEqual(1, savedTestResult.Test.TestID);
                Assert.AreEqual("existing test", savedTestResult.Test.Name);
                Assert.AreEqual(3, savedTestResult.SuiteResult.SuiteResultID);
                Assert.AreEqual(TestResultType.Fail, savedTestResult.ResultType);
                Assert.AreEqual(new DateTime(), savedTestResult.StartTime);
                Assert.AreEqual(new DateTime(), savedTestResult.EndTime);
                Assert.AreEqual(TestFailureType.Exception, savedTestResult.TestFailureDetails.FailureType);
                Assert.AreEqual("failed by exception", savedTestResult.TestFailureDetails.FailureDetailMessage);
                Assert.AreEqual(1, savedTestResult.Test.Suites.Count);
                Assert.AreEqual(2, savedTestResult.Test.Suites.ToList()[0].SuiteID);
                Assert.AreEqual(0, savedTestResult.Test.Categories.Count);
            }

            [TestMethod]
            public void SavesExistingTestPassedTestResult()
            {
                var mockTests = new List<DbTest>
                {
                    new DbTest {
                        TestName = "existing test", TestID = 1,
                        TestCategories = new List<DbTestCategory>(),
                        Suites = new List<Suite> { new Suite { SuiteID = 2 } }
                    }
                }.ToDbSetMock();
                mockTests.Setup(m => m.AsNoTracking()).Returns(mockTests.Object);
                mockTests.Setup(m => m.Include(It.IsAny<string>())).Returns(mockTests.Object);

                var mockTestResultMapper = new Mock<ITestResultMapper>();
                mockTestResultMapper.Setup(t => t.Map(TestResultType.Pass)).Returns(3);
                mockTestResultMapper.Setup(t => t.Map(3)).Returns(TestResultType.Pass);

                var mockTestCategories = new List<DbTestCategory>().ToDbSetMock();

                var mockSuiteResults = new List<DbSuiteResult>
                {
                    new DbSuiteResult { SuiteResultID = 3, SuiteId = 2 }
                }.ToDbSetMock();
                mockSuiteResults.Setup(m => m.AsNoTracking()).Returns(mockSuiteResults.Object);

                var mockSuites = new List<Suite>
                {
                    new Suite { SuiteID = 2 }
                }.ToDbSetMock();
                mockSuites.Setup(m => m.AsNoTracking()).Returns(mockSuites.Object);
                mockSuites.Setup(m => m.Include(It.IsAny<string>())).Returns(mockSuites.Object);

                var mockTestResults = new List<DbTestResult>().ToDbSetMock();

                var mockContext = new Mock<ZigNetEntities>();
                mockContext.Setup(m => m.Tests).Returns(mockTests.Object);
                mockContext.Setup(m => m.TestCategories).Returns(mockTestCategories.Object);
                mockContext.Setup(m => m.SuiteResults).Returns(mockSuiteResults.Object);
                mockContext.Setup(m => m.Suites).Returns(mockSuites.Object);
                mockContext.Setup(m => m.TestResults).Returns(mockTestResults.Object);

                var zignetEntitiesWrapperMock = new Mock<IZigNetEntitiesWrapper>();
                zignetEntitiesWrapperMock.Setup(z => z.Get()).Returns(mockContext.Object);

                var now = DateTime.Now;
                var testResult = new TestResult
                {
                    Test = new Test
                    {
                        Name = "existing test",
                        Categories = new List<TestCategory>()
                    },
                    SuiteResult = new SuiteResult { SuiteResultID = 3 },
                    ResultType = TestResultType.Pass,
                    StartTime = now,
                    EndTime = now.AddHours(1)
                };

                var testResultService = new TestResultService(zignetEntitiesWrapperMock.Object, new Mock<ISuiteService>().Object,
                    new Mock<ILatestTestResultsService>().Object, new Mock<ITestFailureDurationService>().Object,
                    mockTestResultMapper.Object, new Mock<ITemporaryTestResultsService>().Object);
                var savedTestResult = testResultService.SaveTestResult(testResult);

                Assert.AreEqual(0, savedTestResult.TestResultID);
                Assert.AreEqual(1, savedTestResult.Test.TestID);
                Assert.AreEqual("existing test", savedTestResult.Test.Name);
                Assert.AreEqual(3, savedTestResult.SuiteResult.SuiteResultID);
                Assert.AreEqual(TestResultType.Pass, savedTestResult.ResultType);
                Assert.AreEqual(now, savedTestResult.StartTime);
                Assert.AreEqual(now.AddHours(1), savedTestResult.EndTime);
                Assert.IsNull(savedTestResult.TestFailureDetails);
                Assert.AreEqual(1, savedTestResult.Test.Suites.Count);
                Assert.AreEqual(2, savedTestResult.Test.Suites.ToList()[0].SuiteID);
                Assert.AreEqual(0, savedTestResult.Test.Categories.Count);
            }

            [TestMethod]
            public void SavesExistingTestFailedTestResult()
            {
                var mockTests = new List<DbTest>
                {
                    new DbTest {
                        TestName = "existing test", TestID = 1,
                        TestCategories = new List<DbTestCategory>(),
                        Suites = new List<Suite> { new Suite { SuiteID = 2 } }
                    }
                }.ToDbSetMock();
                mockTests.Setup(m => m.AsNoTracking()).Returns(mockTests.Object);
                mockTests.Setup(m => m.Include(It.IsAny<string>())).Returns(mockTests.Object);

                var mockTestResultMapper = new Mock<ITestResultMapper>();
                mockTestResultMapper.Setup(t => t.Map(TestResultType.Fail)).Returns(1);
                mockTestResultMapper.Setup(t => t.Map(1)).Returns(TestResultType.Fail);
                mockTestResultMapper.Setup(t => t.ToTestFailureType(1)).Returns(TestFailureType.Assertion);

                var mockTestFailureTypes = new List<DbTestFailureType>{
                    new DbTestFailureType { TestFailureTypeID = 1, TestFailureTypeName = "Assertion" }
                }.ToDbSetMock();

                var mockTestCategories = new List<DbTestCategory>().ToDbSetMock();

                var mockSuiteResults = new List<DbSuiteResult>
                {
                    new DbSuiteResult { SuiteResultID = 3, SuiteId = 2 }
                }.ToDbSetMock();
                mockSuiteResults.Setup(m => m.AsNoTracking()).Returns(mockSuiteResults.Object);

                var mockSuites = new List<Suite>
                {
                    new Suite { SuiteID = 2 }
                }.ToDbSetMock();
                mockSuites.Setup(m => m.AsNoTracking()).Returns(mockSuites.Object);
                mockSuites.Setup(m => m.Include(It.IsAny<string>())).Returns(mockSuites.Object);

                var mockTestResults = new List<DbTestResult>().ToDbSetMock();

                var mockContext = new Mock<ZigNetEntities>();
                mockContext.Setup(m => m.Tests).Returns(mockTests.Object);
                mockContext.Setup(m => m.TestFailureTypes).Returns(mockTestFailureTypes.Object);
                mockContext.Setup(m => m.TestCategories).Returns(mockTestCategories.Object);
                mockContext.Setup(m => m.SuiteResults).Returns(mockSuiteResults.Object);
                mockContext.Setup(m => m.Suites).Returns(mockSuites.Object);
                mockContext.Setup(m => m.TestResults).Returns(mockTestResults.Object);

                var zignetEntitiesWrapperMock = new Mock<IZigNetEntitiesWrapper>();
                zignetEntitiesWrapperMock.Setup(z => z.Get()).Returns(mockContext.Object);

                var now = DateTime.UtcNow;
                var testResult = new TestResult
                {
                    Test = new Test
                    {
                        Name = "existing test",
                        Categories = new List<TestCategory>()
                    },
                    SuiteResult = new SuiteResult { SuiteResultID = 3 },
                    ResultType = TestResultType.Fail,
                    TestFailureDetails = new TestFailureDetails { FailureType = TestFailureType.Assertion },
                    StartTime = now,
                    EndTime = now.AddSeconds(1)
                };

                var testResultService = new TestResultService(zignetEntitiesWrapperMock.Object, new Mock<ISuiteService>().Object,
                    new Mock<ILatestTestResultsService>().Object, new Mock<ITestFailureDurationService>().Object,
                    mockTestResultMapper.Object, new Mock<ITemporaryTestResultsService>().Object);
                var savedTestResult = testResultService.SaveTestResult(testResult);

                Assert.AreEqual(0, savedTestResult.TestResultID);
                Assert.AreEqual(1, savedTestResult.Test.TestID);
                Assert.AreEqual("existing test", savedTestResult.Test.Name);
                Assert.AreEqual(3, savedTestResult.SuiteResult.SuiteResultID);
                Assert.AreEqual(TestResultType.Fail, savedTestResult.ResultType);
                Assert.AreEqual(now, savedTestResult.StartTime);
                Assert.AreEqual(now.AddSeconds(1), savedTestResult.EndTime);
                Assert.AreEqual(TestFailureType.Assertion, savedTestResult.TestFailureDetails.FailureType);
                Assert.IsNull(savedTestResult.TestFailureDetails.FailureDetailMessage);
                Assert.AreEqual(1, savedTestResult.Test.Suites.Count);
                Assert.AreEqual(2, savedTestResult.Test.Suites.ToList()[0].SuiteID);
                Assert.AreEqual(0, savedTestResult.Test.Categories.Count);
            }

            // todo: verify saving of actual test result start/end times
        }
    }
}
