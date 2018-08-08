using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using ZigNet.Database.EntityFramework;
using ZigNet.Services.EntityFramework.Tests.Helpers;

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

                var latestTestResults = new List<LatestTestResult>
                {
                    new LatestTestResult {
                        SuiteId = 1,
                        TestResultId = 2,
                        TestName = "test1",
                        SuiteName = "suite-name",
                        PassingFromDateTime = utcNow,
                    }
                };
                var mockLatestTestResults = latestTestResults.ToDbSetMock();
                mockLatestTestResults.Setup(m => m.AsNoTracking()).Returns(mockLatestTestResults.Object);

                var mockTestFailureDurations = new List<TestFailureDuration>().ToDbSetMock();
                mockTestFailureDurations.Setup(m => m.AsNoTracking()).Returns(mockTestFailureDurations.Object);

                var mockContext = new Mock<ZigNetEntities>();
                mockContext.Setup(m => m.LatestTestResults).Returns(mockLatestTestResults.Object);
                mockContext.Setup(m => m.TestFailureDurations).Returns(mockTestFailureDurations.Object);

                var zignetEntitiesWrapperMock = new Mock<IZigNetEntitiesWrapper>();
                zignetEntitiesWrapperMock.Setup(z => z.Get()).Returns(mockContext.Object);

                var testResultService = new TestResultService(zignetEntitiesWrapperMock.Object);
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
                var latestTestResults = new List<LatestTestResult>();
                var mockLatestTestResults = latestTestResults.ToDbSetMock();
                mockLatestTestResults.Setup(m => m.AsNoTracking()).Returns(mockLatestTestResults.Object);

                var mockTestFailureDurations = new List<TestFailureDuration>().ToDbSetMock();
                mockTestFailureDurations.Setup(m => m.AsNoTracking()).Returns(mockTestFailureDurations.Object);

                var mockContext = new Mock<ZigNetEntities>();
                mockContext.Setup(m => m.LatestTestResults).Returns(mockLatestTestResults.Object);
                mockContext.Setup(m => m.TestFailureDurations).Returns(mockTestFailureDurations.Object);

                var zignetEntitiesWrapperMock = new Mock<IZigNetEntitiesWrapper>();
                zignetEntitiesWrapperMock.Setup(z => z.Get()).Returns(mockContext.Object);

                var testResultService = new TestResultService(zignetEntitiesWrapperMock.Object);
                var actualLatestTestResults = testResultService.GetLatestResults(1, false).ToList();

                Assert.AreEqual(0, actualLatestTestResults.Count);
            }

            [TestMethod]
            public void SortsByFailingTheLongestThenPassingTheShortest()
            {
                var utcNow = DateTime.UtcNow;

                var latestTestResults = new List<LatestTestResult>
                {
                    new LatestTestResult
                    {
                        SuiteId = 1,
                        TestResultId = 2,
                        TestName = "test passing the longest",
                        PassingFromDateTime = new DateTime(2018, 3, 1, 1, 00, 00),
                    },
                    new LatestTestResult
                    {
                        SuiteId = 1,
                        TestResultId = 3,
                        TestName = "test failing the longest",
                        FailingFromDateTime = new DateTime(2018, 3, 1, 1, 00, 00),
                    },
                    new LatestTestResult
                    {
                        SuiteId = 1,
                        TestResultId = 4,
                        TestName = "test passing the shortest",
                        PassingFromDateTime = new DateTime(2018, 3, 1, 1, 01, 00),
                    },
                    new LatestTestResult
                    {
                        SuiteId = 1,
                        TestResultId = 5,
                        TestName = "test failing the shortest",
                        FailingFromDateTime = new DateTime(2018, 3, 1, 1, 01, 00),
                    }
                };
                var mockLatestTestResults = latestTestResults.ToDbSetMock();
                mockLatestTestResults.Setup(m => m.AsNoTracking()).Returns(mockLatestTestResults.Object);

                var mockTestFailureDurations = new List<TestFailureDuration>().ToDbSetMock();
                mockTestFailureDurations.Setup(m => m.AsNoTracking()).Returns(mockTestFailureDurations.Object);

                var mockContext = new Mock<ZigNetEntities>();
                mockContext.Setup(m => m.LatestTestResults).Returns(mockLatestTestResults.Object);
                mockContext.Setup(m => m.TestFailureDurations).Returns(mockTestFailureDurations.Object);

                var zignetEntitiesWrapperMock = new Mock<IZigNetEntitiesWrapper>();
                zignetEntitiesWrapperMock.Setup(z => z.Get()).Returns(mockContext.Object);

                var testResultService = new TestResultService(zignetEntitiesWrapperMock.Object);
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

                var suites = new List<Suite>
                {
                    new Suite { SuiteID = 1, ApplicationId = 4, EnvironmentId = 3 }
                };
                var mockSuites = suites.ToDbSetMock();
                mockSuites.Setup(m => m.AsNoTracking()).Returns(mockSuites.Object);
                mockSuites.Setup(m => m.Include(It.IsAny<string>())).Returns(mockSuites.Object);

                var latestTestResults = new List<LatestTestResult>
                {
                    new LatestTestResult {
                        SuiteId = 1,
                        TestResultId = 2,
                        TestName = "test1",
                        PassingFromDateTime = utcNow,
                    }
                };
                var mockLatestTestResults = latestTestResults.ToDbSetMock();
                mockLatestTestResults.Setup(m => m.AsNoTracking()).Returns(mockLatestTestResults.Object);

                var mockTestFailureDurations = new List<TestFailureDuration>().ToDbSetMock();
                mockTestFailureDurations.Setup(m => m.AsNoTracking()).Returns(mockTestFailureDurations.Object);

                var mockContext = new Mock<ZigNetEntities>();
                mockContext.Setup(m => m.Suites).Returns(mockSuites.Object);
                mockContext.Setup(m => m.LatestTestResults).Returns(mockLatestTestResults.Object);
                mockContext.Setup(m => m.TestFailureDurations).Returns(mockTestFailureDurations.Object);

                var zignetEntitiesWrapperMock = new Mock<IZigNetEntitiesWrapper>();
                zignetEntitiesWrapperMock.Setup(z => z.Get()).Returns(mockContext.Object);

                var testResultService = new TestResultService(zignetEntitiesWrapperMock.Object);
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

                var suites = new List<Suite> {
                    new Suite { SuiteID = 1, ApplicationId = 4, EnvironmentId = 3 },
                    new Suite { SuiteID = 5, ApplicationId = 4, EnvironmentId = 3 }
                };
                var mockSuites = suites.ToDbSetMock();
                mockSuites.Setup(m => m.AsNoTracking()).Returns(mockSuites.Object);
                mockSuites.Setup(m => m.Include(It.IsAny<string>())).Returns(mockSuites.Object);

                var latestTestResults = new List<LatestTestResult>
                {
                    new LatestTestResult {
                        SuiteId = 1,
                        TestResultId = 2,
                        TestName = "test1",
                        PassingFromDateTime = utcNow,
                    },
                    new LatestTestResult {
                        SuiteId = 5,
                        TestResultId = 6,
                        TestName = "test2",
                        FailingFromDateTime = utcNow,
                    }
                };
                var mockLatestTestResults = latestTestResults.ToDbSetMock();
                mockLatestTestResults.Setup(m => m.AsNoTracking()).Returns(mockLatestTestResults.Object);

                var mockTestFailureDurations = new List<TestFailureDuration>().ToDbSetMock();
                mockTestFailureDurations.Setup(m => m.AsNoTracking()).Returns(mockTestFailureDurations.Object);

                var mockContext = new Mock<ZigNetEntities>();
                mockContext.Setup(m => m.Suites).Returns(mockSuites.Object);
                mockContext.Setup(m => m.LatestTestResults).Returns(mockLatestTestResults.Object);
                mockContext.Setup(m => m.TestFailureDurations).Returns(mockTestFailureDurations.Object);

                var zignetEntitiesWrapperMock = new Mock<IZigNetEntitiesWrapper>();
                zignetEntitiesWrapperMock.Setup(z => z.Get()).Returns(mockContext.Object);

                var testResultService = new TestResultService(zignetEntitiesWrapperMock.Object);
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

                var suites = new List<Suite> {
                    new Suite { SuiteID = 1, ApplicationId = 4, EnvironmentId = 3 },
                    new Suite { SuiteID = 5, EnvironmentId = 4, ApplicationId = 4 }
                };
                var mockSuites = suites.ToDbSetMock();
                mockSuites.Setup(m => m.AsNoTracking()).Returns(mockSuites.Object);
                mockSuites.Setup(m => m.Include(It.IsAny<string>())).Returns(mockSuites.Object);

                var latestTestResults = new List<LatestTestResult>
                {
                    new LatestTestResult {
                        SuiteId = 1,
                        TestResultId = 2,
                        TestName = "test1",
                        PassingFromDateTime = utcNow,
                    },
                    new LatestTestResult {
                        SuiteId = 5,
                        TestResultId = 6,
                        TestName = "test2",
                        FailingFromDateTime = utcNow,
                    }
                };
                var mockLatestTestResults = latestTestResults.ToDbSetMock();
                mockLatestTestResults.Setup(m => m.AsNoTracking()).Returns(mockLatestTestResults.Object);

                var mockTestFailureDurations = new List<TestFailureDuration>().ToDbSetMock();
                mockTestFailureDurations.Setup(m => m.AsNoTracking()).Returns(mockTestFailureDurations.Object);

                var mockContext = new Mock<ZigNetEntities>();
                mockContext.Setup(m => m.Suites).Returns(mockSuites.Object);
                mockContext.Setup(m => m.LatestTestResults).Returns(mockLatestTestResults.Object);
                mockContext.Setup(m => m.TestFailureDurations).Returns(mockTestFailureDurations.Object);

                var zignetEntitiesWrapperMock = new Mock<IZigNetEntitiesWrapper>();
                zignetEntitiesWrapperMock.Setup(z => z.Get()).Returns(mockContext.Object);

                var testResultService = new TestResultService(zignetEntitiesWrapperMock.Object);
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

                var suites = new List<Suite> {
                    new Suite { SuiteID = 1, ApplicationId = 3, EnvironmentId = 4 },
                    new Suite { SuiteID = 5, EnvironmentId = 3, ApplicationId = 5 }
                };
                var mockSuites = suites.ToDbSetMock();
                mockSuites.Setup(m => m.AsNoTracking()).Returns(mockSuites.Object);
                mockSuites.Setup(m => m.Include(It.IsAny<string>())).Returns(mockSuites.Object);

                var latestTestResults = new List<LatestTestResult>
                {
                    new LatestTestResult {
                        SuiteId = 1,
                        TestResultId = 2,
                        TestName = "test1",
                        PassingFromDateTime = utcNow,
                    },
                    new LatestTestResult {
                        SuiteId = 5,
                        TestResultId = 6,
                        TestName = "test2",
                        FailingFromDateTime = utcNow,
                    }
                };
                var mockLatestTestResults = latestTestResults.ToDbSetMock();
                mockLatestTestResults.Setup(m => m.AsNoTracking()).Returns(mockLatestTestResults.Object);

                var mockTestFailureDurations = new List<TestFailureDuration>().ToDbSetMock();
                mockTestFailureDurations.Setup(m => m.AsNoTracking()).Returns(mockTestFailureDurations.Object);

                var mockContext = new Mock<ZigNetEntities>();
                mockContext.Setup(m => m.Suites).Returns(mockSuites.Object);
                mockContext.Setup(m => m.LatestTestResults).Returns(mockLatestTestResults.Object);
                mockContext.Setup(m => m.TestFailureDurations).Returns(mockTestFailureDurations.Object);

                var zignetEntitiesWrapperMock = new Mock<IZigNetEntitiesWrapper>();
                zignetEntitiesWrapperMock.Setup(z => z.Get()).Returns(mockContext.Object);

                var testResultService = new TestResultService(zignetEntitiesWrapperMock.Object);
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

                var suites = new List<Suite> {
                    new Suite { SuiteID = 1, ApplicationId = 3, EnvironmentId = 4 },
                    new Suite { SuiteID = 5, ApplicationId = 3, EnvironmentId = 4 }
                };
                var mockSuites = suites.ToDbSetMock();
                mockSuites.Setup(m => m.AsNoTracking()).Returns(mockSuites.Object);
                mockSuites.Setup(m => m.Include(It.IsAny<string>())).Returns(mockSuites.Object);

                var latestTestResults = new List<LatestTestResult>();
                var mockLatestTestResults = latestTestResults.ToDbSetMock();
                mockLatestTestResults.Setup(m => m.AsNoTracking()).Returns(mockLatestTestResults.Object);

                var mockTestFailureDurations = new List<TestFailureDuration>().ToDbSetMock();
                mockTestFailureDurations.Setup(m => m.AsNoTracking()).Returns(mockTestFailureDurations.Object);

                var mockContext = new Mock<ZigNetEntities>();
                mockContext.Setup(m => m.Suites).Returns(mockSuites.Object);
                mockContext.Setup(m => m.LatestTestResults).Returns(mockLatestTestResults.Object);
                mockContext.Setup(m => m.TestFailureDurations).Returns(mockTestFailureDurations.Object);

                var zignetEntitiesWrapperMock = new Mock<IZigNetEntitiesWrapper>();
                zignetEntitiesWrapperMock.Setup(z => z.Get()).Returns(mockContext.Object);

                var testResultService = new TestResultService(zignetEntitiesWrapperMock.Object);
                var actualLatestTestResults = testResultService.GetLatestResults(1, true).ToList();

                Assert.AreEqual(0, actualLatestTestResults.Count);
            }

            [TestMethod]
            public void SingleTestFailureDurationWithEndDate()
            {
                var utcNow = DateTime.UtcNow;

                var mockLatestTestResults = new List<LatestTestResult>
                {
                    new LatestTestResult {
                        SuiteId = 1,
                        TestId = 2,
                        PassingFromDateTime = utcNow,
                    }
                }.ToDbSetMock();
                mockLatestTestResults.Setup(m => m.AsNoTracking()).Returns(mockLatestTestResults.Object);

                var mockTestFailureDurations = new List<TestFailureDuration>
                {
                    new TestFailureDuration { SuiteId = 1, TestId = 2, FailureEndDateTime = utcNow }
                }.ToDbSetMock();
                mockTestFailureDurations.Setup(m => m.AsNoTracking()).Returns(mockTestFailureDurations.Object);

                var mockContext = new Mock<ZigNetEntities>();
                mockContext.Setup(m => m.LatestTestResults).Returns(mockLatestTestResults.Object);
                mockContext.Setup(m => m.TestFailureDurations).Returns(mockTestFailureDurations.Object);

                var zignetEntitiesWrapperMock = new Mock<IZigNetEntitiesWrapper>();
                zignetEntitiesWrapperMock.Setup(z => z.Get()).Returns(mockContext.Object);

                var testResultService = new TestResultService(zignetEntitiesWrapperMock.Object);
                var actualLatestTestResults = testResultService.GetLatestResults(1, false).ToList();

                Assert.AreEqual(1, actualLatestTestResults.Count);
                Assert.AreEqual(1, actualLatestTestResults[0].TestFailureDurations.Count());
                Assert.AreEqual(utcNow, actualLatestTestResults[0].TestFailureDurations.ToList()[0].FailureEnd);
            }

            [TestMethod]
            public void SingleTestFailureDurationWithoutEndDate()
            {
                var utcNow = DateTime.UtcNow;

                var mockLatestTestResults = new List<LatestTestResult>
                {
                    new LatestTestResult {
                        SuiteId = 1,
                        TestId = 2,
                        PassingFromDateTime = utcNow,
                    }
                }.ToDbSetMock();
                mockLatestTestResults.Setup(m => m.AsNoTracking()).Returns(mockLatestTestResults.Object);

                var mockTestFailureDurations = new List<TestFailureDuration>
                {
                    new TestFailureDuration { SuiteId = 1, TestId = 2 }
                }.ToDbSetMock();
                mockTestFailureDurations.Setup(m => m.AsNoTracking()).Returns(mockTestFailureDurations.Object);

                var mockContext = new Mock<ZigNetEntities>();
                mockContext.Setup(m => m.LatestTestResults).Returns(mockLatestTestResults.Object);
                mockContext.Setup(m => m.TestFailureDurations).Returns(mockTestFailureDurations.Object);

                var zignetEntitiesWrapperMock = new Mock<IZigNetEntitiesWrapper>();
                zignetEntitiesWrapperMock.Setup(z => z.Get()).Returns(mockContext.Object);

                var testResultService = new TestResultService(zignetEntitiesWrapperMock.Object);
                var actualLatestTestResults = testResultService.GetLatestResults(1, false).ToList();

                Assert.AreEqual(1, actualLatestTestResults.Count);
                Assert.AreEqual(1, actualLatestTestResults[0].TestFailureDurations.Count());
                Assert.IsNull(actualLatestTestResults[0].TestFailureDurations.ToList()[0].FailureEnd);
            }

            [TestMethod]
            public void MultipleTestFailureDurationsWithAndWithoutEndDate()
            {
                var utcNow = DateTime.UtcNow;

                var mockLatestTestResults = new List<LatestTestResult>
                {
                    new LatestTestResult {
                        SuiteId = 1,
                        TestId = 2,
                        PassingFromDateTime = utcNow
                    }
                }.ToDbSetMock();
                mockLatestTestResults.Setup(m => m.AsNoTracking()).Returns(mockLatestTestResults.Object);

                var mockTestFailureDurations = new List<TestFailureDuration>
                {
                    new TestFailureDuration { SuiteId = 1, TestId = 2, FailureStartDateTime = utcNow },
                    new TestFailureDuration { SuiteId = 1, TestId = 2, FailureStartDateTime = utcNow.AddMinutes(5), FailureEndDateTime = utcNow.AddMinutes(10) }
                }.ToDbSetMock();
                mockTestFailureDurations.Setup(m => m.AsNoTracking()).Returns(mockTestFailureDurations.Object);

                var mockContext = new Mock<ZigNetEntities>();
                mockContext.Setup(m => m.LatestTestResults).Returns(mockLatestTestResults.Object);
                mockContext.Setup(m => m.TestFailureDurations).Returns(mockTestFailureDurations.Object);

                var zignetEntitiesWrapperMock = new Mock<IZigNetEntitiesWrapper>();
                zignetEntitiesWrapperMock.Setup(z => z.Get()).Returns(mockContext.Object);

                var testResultService = new TestResultService(zignetEntitiesWrapperMock.Object);
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

                var mockLatestTestResults = new List<LatestTestResult>
                {
                    new LatestTestResult {
                        SuiteId = 1,
                        TestId = 2,
                        PassingFromDateTime = utcNow
                    }
                }.ToDbSetMock();
                mockLatestTestResults.Setup(m => m.AsNoTracking()).Returns(mockLatestTestResults.Object);

                var mockTestFailureDurations = new List<TestFailureDuration>
                {
                    new TestFailureDuration { SuiteId = 1, TestId = 2, FailureEndDateTime = utcNow.AddHours(-25) },
                    new TestFailureDuration { SuiteId = 1, TestId = 2, FailureEndDateTime = utcNow.AddHours(-23) },
                    new TestFailureDuration { SuiteId = 1, TestId = 2, FailureEndDateTime = utcNow }
                }.ToDbSetMock();
                mockTestFailureDurations.Setup(m => m.AsNoTracking()).Returns(mockTestFailureDurations.Object);

                var mockContext = new Mock<ZigNetEntities>();
                mockContext.Setup(m => m.LatestTestResults).Returns(mockLatestTestResults.Object);
                mockContext.Setup(m => m.TestFailureDurations).Returns(mockTestFailureDurations.Object);

                var zignetEntitiesWrapperMock = new Mock<IZigNetEntitiesWrapper>();
                zignetEntitiesWrapperMock.Setup(z => z.Get()).Returns(mockContext.Object);

                var testResultService = new TestResultService(zignetEntitiesWrapperMock.Object);
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

                var mockLatestTestResults = new List<LatestTestResult>
                {
                    new LatestTestResult {
                        SuiteId = 1,
                        TestId = 2,
                        PassingFromDateTime = utcNow
                    }
                }.ToDbSetMock();
                mockLatestTestResults.Setup(m => m.AsNoTracking()).Returns(mockLatestTestResults.Object);

                var mockTestFailureDurations = new List<TestFailureDuration>
                {
                    new TestFailureDuration { SuiteId = 99, TestId = 2, FailureEndDateTime = utcNow }
                }.ToDbSetMock();
                mockTestFailureDurations.Setup(m => m.AsNoTracking()).Returns(mockTestFailureDurations.Object);

                var mockContext = new Mock<ZigNetEntities>();
                mockContext.Setup(m => m.LatestTestResults).Returns(mockLatestTestResults.Object);
                mockContext.Setup(m => m.TestFailureDurations).Returns(mockTestFailureDurations.Object);

                var zignetEntitiesWrapperMock = new Mock<IZigNetEntitiesWrapper>();
                zignetEntitiesWrapperMock.Setup(z => z.Get()).Returns(mockContext.Object);

                var testResultService = new TestResultService(zignetEntitiesWrapperMock.Object);
                var latestTestResults = testResultService.GetLatestResults(1, false).ToList();

                Assert.AreEqual(1, latestTestResults.Count);
                Assert.AreEqual(0, latestTestResults[0].TestFailureDurations.Count());
            }

            [TestMethod]
            public void IgnoresTestFailureDurationsWithoutTestId()
            {
                var utcNow = DateTime.UtcNow;

                var mockLatestTestResults = new List<LatestTestResult>
                {
                    new LatestTestResult {
                        SuiteId = 1,
                        TestId = 2,
                        PassingFromDateTime = utcNow
                    }
                }.ToDbSetMock();
                mockLatestTestResults.Setup(m => m.AsNoTracking()).Returns(mockLatestTestResults.Object);

                var mockTestFailureDurations = new List<TestFailureDuration>
                {
                    new TestFailureDuration { SuiteId = 1, TestId = 99, FailureEndDateTime = utcNow }
                }.ToDbSetMock();
                mockTestFailureDurations.Setup(m => m.AsNoTracking()).Returns(mockTestFailureDurations.Object);

                var mockContext = new Mock<ZigNetEntities>();
                mockContext.Setup(m => m.LatestTestResults).Returns(mockLatestTestResults.Object);
                mockContext.Setup(m => m.TestFailureDurations).Returns(mockTestFailureDurations.Object);

                var zignetEntitiesWrapperMock = new Mock<IZigNetEntitiesWrapper>();
                zignetEntitiesWrapperMock.Setup(z => z.Get()).Returns(mockContext.Object);

                var testResultService = new TestResultService(zignetEntitiesWrapperMock.Object);
                var latestTestResults = testResultService.GetLatestResults(1, false).ToList();

                Assert.AreEqual(1, latestTestResults.Count);
                Assert.AreEqual(0, latestTestResults[0].TestFailureDurations.Count());
            }

            [TestMethod]
            public void GetsTestFailureDurationsWithStartDateOutside24Hours()
            {
                var utcNow = DateTime.UtcNow;

                var mockLatestTestResults = new List<LatestTestResult>
                {
                    new LatestTestResult {
                        SuiteId = 1,
                        TestId = 2,
                        PassingFromDateTime = utcNow
                    }
                }.ToDbSetMock();
                mockLatestTestResults.Setup(m => m.AsNoTracking()).Returns(mockLatestTestResults.Object);

                var mockTestFailureDurations = new List<TestFailureDuration>
                {
                    new TestFailureDuration { SuiteId = 1, TestId = 2, FailureStartDateTime = utcNow.AddHours(-48), FailureEndDateTime = utcNow.AddHours(-23) }
                }.ToDbSetMock();
                mockTestFailureDurations.Setup(m => m.AsNoTracking()).Returns(mockTestFailureDurations.Object);

                var mockContext = new Mock<ZigNetEntities>();
                mockContext.Setup(m => m.LatestTestResults).Returns(mockLatestTestResults.Object);
                mockContext.Setup(m => m.TestFailureDurations).Returns(mockTestFailureDurations.Object);

                var zignetEntitiesWrapperMock = new Mock<IZigNetEntitiesWrapper>();
                zignetEntitiesWrapperMock.Setup(z => z.Get()).Returns(mockContext.Object);

                var testResultService = new TestResultService(zignetEntitiesWrapperMock.Object);
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

                var mockLatestTestResults = new List<LatestTestResult>
                {
                    new LatestTestResult {
                        SuiteId = 1,
                        TestId = 2,
                        PassingFromDateTime = utcNow
                    }
                }.ToDbSetMock();
                mockLatestTestResults.Setup(m => m.AsNoTracking()).Returns(mockLatestTestResults.Object);

                var mockTestFailureDurations = new List<TestFailureDuration>
                {
                    new TestFailureDuration { SuiteId = 1, TestId = 2 },
                    new TestFailureDuration { SuiteId = 1, TestId = 3 }
                }.ToDbSetMock();
                mockTestFailureDurations.Setup(m => m.AsNoTracking()).Returns(mockTestFailureDurations.Object);

                var mockContext = new Mock<ZigNetEntities>();
                mockContext.Setup(m => m.LatestTestResults).Returns(mockLatestTestResults.Object);
                mockContext.Setup(m => m.TestFailureDurations).Returns(mockTestFailureDurations.Object);

                var zignetEntitiesWrapperMock = new Mock<IZigNetEntitiesWrapper>();
                zignetEntitiesWrapperMock.Setup(z => z.Get()).Returns(mockContext.Object);

                var testResultService = new TestResultService(zignetEntitiesWrapperMock.Object);
                var latestTestResults = testResultService.GetLatestResults(1, false).ToList();

                Assert.AreEqual(1, latestTestResults.Count);
                Assert.AreEqual(1, latestTestResults[0].TestFailureDurations.Count());
                Assert.IsNull(latestTestResults[0].TestFailureDurations.ToList()[0].FailureEnd);
            }

            [TestMethod]
            public void IgnoresTestFailureDurationsFromOtherSuites()
            {
                var utcNow = DateTime.UtcNow;

                var mockLatestTestResults = new List<LatestTestResult>
                {
                    new LatestTestResult {
                        SuiteId = 1,
                        TestId = 2,
                        PassingFromDateTime = utcNow
                    }
                }.ToDbSetMock();
                mockLatestTestResults.Setup(m => m.AsNoTracking()).Returns(mockLatestTestResults.Object);

                var mockTestFailureDurations = new List<TestFailureDuration>
                {
                    new TestFailureDuration { SuiteId = 1, TestId = 2 },
                    new TestFailureDuration { SuiteId = 3, TestId = 2 }
                }.ToDbSetMock();
                mockTestFailureDurations.Setup(m => m.AsNoTracking()).Returns(mockTestFailureDurations.Object);

                var mockContext = new Mock<ZigNetEntities>();
                mockContext.Setup(m => m.LatestTestResults).Returns(mockLatestTestResults.Object);
                mockContext.Setup(m => m.TestFailureDurations).Returns(mockTestFailureDurations.Object);

                var zignetEntitiesWrapperMock = new Mock<IZigNetEntitiesWrapper>();
                zignetEntitiesWrapperMock.Setup(z => z.Get()).Returns(mockContext.Object);

                var testResultService = new TestResultService(zignetEntitiesWrapperMock.Object);
                var latestTestResults = testResultService.GetLatestResults(1, false).ToList();

                Assert.AreEqual(1, latestTestResults.Count);
                Assert.AreEqual(1, latestTestResults[0].TestFailureDurations.Count());
                Assert.IsNull(latestTestResults[0].TestFailureDurations.ToList()[0].FailureEnd);
            }
        }
    }
}
