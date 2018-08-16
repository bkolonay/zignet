using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using ZigNet.Database.EntityFramework;
using ZigNet.Services.DTOs;
using ZigNet.Services.EntityFramework.Mapping;

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

                var testResultService = new TestResultService(new Mock<IDbContext>().Object,
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

                var testResultService = new TestResultService(new Mock<IDbContext>().Object,
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

                var testResultService = new TestResultService(new Mock<IDbContext>().Object,
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

                var testResultService = new TestResultService(new Mock<IDbContext>().Object,
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

                var testResultService = new TestResultService(new Mock<IDbContext>().Object,
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

                var testResultService = new TestResultService(new Mock<IDbContext>().Object,
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

                var testResultService = new TestResultService(new Mock<IDbContext>().Object,
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

                var testResultService = new TestResultService(new Mock<IDbContext>().Object,
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

                var testResultService = new TestResultService(new Mock<IDbContext>().Object,
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

                var testResultService = new TestResultService(new Mock<IDbContext>().Object,
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

                var testResultService = new TestResultService(new Mock<IDbContext>().Object,
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

                var testResultService = new TestResultService(new Mock<IDbContext>().Object,
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

                var testResultService = new TestResultService(new Mock<IDbContext>().Object,
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

                var testResultService = new TestResultService(new Mock<IDbContext>().Object,
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

                var testResultService = new TestResultService(new Mock<IDbContext>().Object,
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

                var testResultService = new TestResultService(new Mock<IDbContext>().Object,
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

                var testResultService = new TestResultService(new Mock<IDbContext>().Object,
                    mockSuiteService.Object, mockLatestTestResultsService.Object,
                    mockTestFailureDurationService.Object, new Mock<ITestResultMapper>().Object, new Mock<ITemporaryTestResultsService>().Object);
                var actualLatestTestResults = testResultService.GetLatestResultsGrouped(1).ToList();

                Assert.AreEqual(0, actualLatestTestResults.Count);
            }
        }
    }
}
