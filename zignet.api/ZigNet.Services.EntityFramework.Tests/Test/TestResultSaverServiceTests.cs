using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using ZigNet.Database.EntityFramework;
using ZigNet.Services.EntityFramework.Tests.Helpers;
using ZigNet.Services.EntityFramework.Mapping;
using DbTest = ZigNet.Database.EntityFramework.Test;
using DbTestResult = ZigNet.Database.EntityFramework.TestResult;
using DbTestCategory = ZigNet.Database.EntityFramework.TestCategory;
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
    public class TestResultSaverServiceTests
    {
        [TestClass]
        public class Save
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
                mockTestResultMapper.Setup(t => t.ToDbTestResultTypeId(TestResultType.Inconclusive)).Returns(2);

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

                var zignetEntitiesWrapperMock = new Mock<IDbContext>();
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

                var testResultSaverService = new TestResultSaverService(zignetEntitiesWrapperMock.Object,
                    new Mock<ILatestTestResultService>().Object, new Mock<ITemporaryTestResultService>().Object,
                    new Mock<ITestFailureDurationService>().Object, mockTestResultMapper.Object);
                var savedTestResult = testResultSaverService.Save(testResult);

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
                mockTestResultMapper.Setup(t => t.ToDbTestResultTypeId(TestResultType.Inconclusive)).Returns(2);

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

                var zignetEntitiesWrapperMock = new Mock<IDbContext>();
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

                var testResultSaverService = new TestResultSaverService(zignetEntitiesWrapperMock.Object,
                    new Mock<ILatestTestResultService>().Object, new Mock<ITemporaryTestResultService>().Object,
                    new Mock<ITestFailureDurationService>().Object, mockTestResultMapper.Object);
                var savedTestResult = testResultSaverService.Save(testResult);

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
                mockTestResultMapper.Setup(t => t.ToDbTestResultTypeId(TestResultType.Inconclusive)).Returns(2);

                var mockTestCategories = new List<DbTestCategory>().ToDbSetMock();

                var mockSuiteResults = new List<DbSuiteResult>().ToDbSetMock();
                mockSuiteResults.Setup(m => m.AsNoTracking()).Returns(mockSuiteResults.Object);

                var mockContext = new Mock<ZigNetEntities>();
                mockContext.Setup(m => m.Tests).Returns(mockTests.Object);
                mockContext.Setup(m => m.TestCategories).Returns(mockTestCategories.Object);
                mockContext.Setup(m => m.SuiteResults).Returns(mockSuiteResults.Object);

                var zignetEntitiesWrapperMock = new Mock<IDbContext>();
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

                var testResultSaverService = new TestResultSaverService(zignetEntitiesWrapperMock.Object,
                    new Mock<ILatestTestResultService>().Object, new Mock<ITemporaryTestResultService>().Object,
                    new Mock<ITestFailureDurationService>().Object, mockTestResultMapper.Object);
                testResultSaverService.Save(testResult);
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
                mockTestResultMapper.Setup(t => t.ToDbTestResultTypeId(TestResultType.Inconclusive)).Returns(2);

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

                var zignetEntitiesWrapperMock = new Mock<IDbContext>();
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

                var testResultSaverService = new TestResultSaverService(zignetEntitiesWrapperMock.Object,
                    new Mock<ILatestTestResultService>().Object, new Mock<ITemporaryTestResultService>().Object,
                    new Mock<ITestFailureDurationService>().Object, mockTestResultMapper.Object);
                var savedTestResult = testResultSaverService.Save(testResult);

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
                mockTestResultMapper.Setup(t => t.ToDbTestResultTypeId(TestResultType.Inconclusive)).Returns(2);

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

                var zignetEntitiesWrapperMock = new Mock<IDbContext>();
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

                var testResultSaverService = new TestResultSaverService(zignetEntitiesWrapperMock.Object,
                    new Mock<ILatestTestResultService>().Object, new Mock<ITemporaryTestResultService>().Object,
                    new Mock<ITestFailureDurationService>().Object, mockTestResultMapper.Object);
                var savedTestResult = testResultSaverService.Save(testResult);

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
                mockTestResultMapper.Setup(t => t.ToDbTestResultTypeId(TestResultType.Inconclusive)).Returns(2);

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

                var zignetEntitiesWrapperMock = new Mock<IDbContext>();
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

                var testResultSaverService = new TestResultSaverService(zignetEntitiesWrapperMock.Object,
                    new Mock<ILatestTestResultService>().Object, new Mock<ITemporaryTestResultService>().Object,
                    new Mock<ITestFailureDurationService>().Object, mockTestResultMapper.Object);
                var savedTestResult = testResultSaverService.Save(testResult);

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
                mockTestResultMapper.Setup(t => t.ToDbTestResultTypeId(TestResultType.Fail)).Returns(1);
                mockTestResultMapper.Setup(t => t.ToTestResultType(1)).Returns(TestResultType.Fail);
                mockTestResultMapper.Setup(t => t.ToDbTestFailureTypeId(TestFailureType.Exception)).Returns(2);

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

                var zignetEntitiesWrapperMock = new Mock<IDbContext>();
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

                var testResultSaverService = new TestResultSaverService(zignetEntitiesWrapperMock.Object,
                    new Mock<ILatestTestResultService>().Object, new Mock<ITemporaryTestResultService>().Object,
                    new Mock<ITestFailureDurationService>().Object, mockTestResultMapper.Object);
                var savedTestResult = testResultSaverService.Save(testResult);

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
                mockTestResultMapper.Setup(t => t.ToDbTestResultTypeId(TestResultType.Pass)).Returns(3);
                mockTestResultMapper.Setup(t => t.ToTestResultType(3)).Returns(TestResultType.Pass);

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

                var zignetEntitiesWrapperMock = new Mock<IDbContext>();
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

                var testResultSaverService = new TestResultSaverService(zignetEntitiesWrapperMock.Object,
                    new Mock<ILatestTestResultService>().Object, new Mock<ITemporaryTestResultService>().Object,
                    new Mock<ITestFailureDurationService>().Object, mockTestResultMapper.Object);
                var savedTestResult = testResultSaverService.Save(testResult);

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
                mockTestResultMapper.Setup(t => t.ToDbTestResultTypeId(TestResultType.Fail)).Returns(1);
                mockTestResultMapper.Setup(t => t.ToTestResultType(1)).Returns(TestResultType.Fail);
                mockTestResultMapper.Setup(t => t.ToDbTestFailureTypeId(TestFailureType.Assertion)).Returns(1);
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

                var zignetEntitiesWrapperMock = new Mock<IDbContext>();
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

                var testResultSaverService = new TestResultSaverService(zignetEntitiesWrapperMock.Object,
                    new Mock<ILatestTestResultService>().Object, new Mock<ITemporaryTestResultService>().Object,
                    new Mock<ITestFailureDurationService>().Object, mockTestResultMapper.Object);
                var savedTestResult = testResultSaverService.Save(testResult);

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
