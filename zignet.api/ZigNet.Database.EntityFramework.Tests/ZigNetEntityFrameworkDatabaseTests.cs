using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using ZigNetSuite = ZigNet.Domain.Suite.Suite;
using ZigNetSuiteCategory = ZigNet.Domain.Suite.SuiteCategory;
using ZigNetSuiteResult = ZigNet.Domain.Suite.SuiteResult;
using ZigNetSuiteResultType = ZigNet.Domain.Suite.SuiteResultType;
using ZigNetTest = ZigNet.Domain.Test.Test;
using ZigNetTestResult = ZigNet.Domain.Test.TestResult;
using ZigNetTestResultType = ZigNet.Domain.Test.TestResultType;
using ZigNetTestCategory = ZigNet.Domain.Test.TestCategory;
using ZigNetTestFailureDetails = ZigNet.Domain.Test.TestFailureDetails;
using ZigNetTestFailureType = ZigNet.Domain.Test.TestFailureType;

namespace ZigNet.Database.EntityFramework.Tests
{
    // todo: uncomment

    //public class ZigNetEntityFrameworkDatabaseTests
    //{
    //    [TestClass]
    //    public class SaveSuiteMethod
    //    {
    //        [TestMethod]
    //        public void SavesNewSuiteWithNoCategories()
    //        {
    //            var zigNetEntitiesWrapperMock = new Mock<IZigNetEntitiesWriter>();
    //            zigNetEntitiesWrapperMock.Setup(zewm => zewm.SaveSuite(It.IsAny<Suite>())).Returns(1);

    //            var suite = new ZigNetSuite { Name = "new suite", Categories = new List<ZigNetSuiteCategory>() };

    //            var zigNetEntityFrameworkDatabase = new ZigNetEntityFrameworkDatabase(zigNetEntitiesWrapperMock.Object);
    //            var suiteId = zigNetEntityFrameworkDatabase.SaveSuite(suite);

    //            Assert.AreEqual(1, suiteId);
    //        }

    //        [TestMethod]
    //        public void SavesNewSuiteWithEmptyCategoryList()
    //        {
    //            var zigNetEntitiesWrapperMock = new Mock<IZigNetEntitiesWriter>();
    //            zigNetEntitiesWrapperMock.Setup(zewm => zewm.SaveSuite(It.IsAny<Suite>())).Returns(1);

    //            var suite = new ZigNetSuite { Name = "new suite", Categories = new List<ZigNetSuiteCategory>() };

    //            var zigNetEntityFrameworkDatabase = new ZigNetEntityFrameworkDatabase(zigNetEntitiesWrapperMock.Object);
    //            var suiteId = zigNetEntityFrameworkDatabase.SaveSuite(suite);

    //            Assert.AreEqual(1, suiteId);
    //        }

    //        [TestMethod]
    //        public void SavesNewSuiteWithOnlyNewCategories()
    //        {
    //            var zigNetEntitiesWrapperMock = new Mock<IZigNetEntitiesWriter>();
    //            zigNetEntitiesWrapperMock.Setup(zewm => zewm.SaveSuite(It.IsAny<Suite>())).Returns(1);
    //            zigNetEntitiesWrapperMock.Setup(zewm => zewm.GetSuiteCategories()).Returns(new List<SuiteCategory>().AsQueryable());

    //            var suite = new ZigNetSuite
    //            {
    //                Name = "new suite",
    //                Categories = new List<ZigNetSuiteCategory> {
    //                    new ZigNetSuiteCategory { Name = "suite category 1" }
    //                }
    //            };

    //            var zigNetEntityFrameworkDatabase = new ZigNetEntityFrameworkDatabase(zigNetEntitiesWrapperMock.Object);
    //            var suiteId = zigNetEntityFrameworkDatabase.SaveSuite(suite);

    //            Assert.AreEqual(1, suiteId);
    //        }

    //        [TestMethod]
    //        public void SavesNewSuiteWithOnlyExistingCategories()
    //        {
    //            var zigNetEntitiesWrapperMock = new Mock<IZigNetEntitiesWriter>();
    //            zigNetEntitiesWrapperMock.Setup(zewm => zewm.SaveSuite(It.IsAny<Suite>())).Returns(1);
    //            zigNetEntitiesWrapperMock.Setup(zewm => zewm.GetSuiteCategories()).Returns(new List<SuiteCategory> {
    //                new SuiteCategory { CategoryName = "suite category 1" }
    //            }.AsQueryable());

    //            var suite = new ZigNetSuite
    //            {
    //                Name = "new suite",
    //                Categories = new List<ZigNetSuiteCategory> {
    //                    new ZigNetSuiteCategory { Name = "suite category 1" }
    //                }
    //            };

    //            var zigNetEntityFrameworkDatabase = new ZigNetEntityFrameworkDatabase(zigNetEntitiesWrapperMock.Object);
    //            var suiteId = zigNetEntityFrameworkDatabase.SaveSuite(suite);

    //            Assert.AreEqual(1, suiteId);
    //        }

    //        [TestMethod]
    //        public void SavesNewSuiteWithNewAndExistingCategories()
    //        {
    //            var zigNetEntitiesWrapperMock = new Mock<IZigNetEntitiesWriter>();
    //            zigNetEntitiesWrapperMock.Setup(zewm => zewm.SaveSuite(It.IsAny<Suite>())).Returns(1);
    //            zigNetEntitiesWrapperMock.Setup(zewm => zewm.GetSuiteCategories()).Returns(new List<SuiteCategory> {
    //                new SuiteCategory { CategoryName = "suite category 1" },
    //                new SuiteCategory { CategoryName = "suite category 2" },
    //            }.AsQueryable());

    //            var suite = new ZigNetSuite
    //            {
    //                Name = "new suite",
    //                Categories = new List<ZigNetSuiteCategory> {
    //                    new ZigNetSuiteCategory { Name = "suite category 1" },
    //                    new ZigNetSuiteCategory { Name = "suite category 3" }
    //                }
    //            };

    //            var zigNetEntityFrameworkDatabase = new ZigNetEntityFrameworkDatabase(zigNetEntitiesWrapperMock.Object);
    //            var suiteId = zigNetEntityFrameworkDatabase.SaveSuite(suite);

    //            Assert.AreEqual(1, suiteId);
    //        }

    //        [TestMethod]
    //        public void SavesExistingSuiteWithExistingCategory()
    //        {
    //            var zigNetEntitiesWrapperMock = new Mock<IZigNetEntitiesWriter>();
    //            zigNetEntitiesWrapperMock.Setup(zewm => zewm.GetSuite(1)).Returns(new Suite { SuiteID = 1 });
    //            zigNetEntitiesWrapperMock.Setup(zewm => zewm.GetSuiteCategories()).Returns(new List<SuiteCategory> {
    //                new SuiteCategory { SuiteCategoryID = 1, CategoryName = "suite category 1" }
    //            }.AsQueryable());
    //            zigNetEntitiesWrapperMock.Setup(zewm => zewm.SaveSuite(It.IsAny<Suite>())).Returns(1);

    //            var suite = new ZigNetSuite
    //            {
    //                SuiteID = 1,
    //                Name = "existing suite",
    //                Categories = new List<ZigNetSuiteCategory> {
    //                    new ZigNetSuiteCategory { Name = "suite category 1" }
    //                }
    //            };

    //            var zigNetEntityFrameworkDatabase = new ZigNetEntityFrameworkDatabase(zigNetEntitiesWrapperMock.Object);
    //            var suiteId = zigNetEntityFrameworkDatabase.SaveSuite(suite);

    //            Assert.AreEqual(1, suiteId);
    //        }

    //        [TestMethod]
    //        public void SavesExistingSuiteWithNewCategory()
    //        {
    //            var zigNetEntitiesWrapperMock = new Mock<IZigNetEntitiesWriter>();
    //            zigNetEntitiesWrapperMock.Setup(zewm => zewm.GetSuite(1)).Returns(new Suite { SuiteID = 1 });
    //            zigNetEntitiesWrapperMock.Setup(zewm => zewm.GetSuiteCategories()).Returns(new List<SuiteCategory> {
    //                new SuiteCategory { SuiteCategoryID = 1, CategoryName = "suite category 1" }
    //            }.AsQueryable());
    //            zigNetEntitiesWrapperMock.Setup(zewm => zewm.SaveSuite(It.IsAny<Suite>())).Returns(1);

    //            var suite = new ZigNetSuite
    //            {
    //                SuiteID = 1,
    //                Name = "existing suite",
    //                Categories = new List<ZigNetSuiteCategory> {
    //                    new ZigNetSuiteCategory { Name = "suite category 2" }
    //                }
    //            };

    //            var zigNetEntityFrameworkDatabase = new ZigNetEntityFrameworkDatabase(zigNetEntitiesWrapperMock.Object);
    //            var suiteId = zigNetEntityFrameworkDatabase.SaveSuite(suite);

    //            Assert.AreEqual(1, suiteId);
    //        }
    //    }

    //    [TestClass]
    //    public class SaveSuiteResultMethod
    //    {
    //        [TestMethod]
    //        public void SavesNewSuiteResult()
    //        {
    //            var zigNetEntitiesWrapperMock = new Mock<IZigNetEntitiesWriter>();
    //            zigNetEntitiesWrapperMock.Setup(zewm => zewm.SaveSuiteResult(It.IsAny<SuiteResult>())).Returns(1);

    //            var suiteResult = new ZigNetSuiteResult
    //            {
    //                Suite = new ZigNetSuite { SuiteID = 2 },
    //                StartTime = DateTime.UtcNow,
    //                ResultType = ZigNetSuiteResultType.Inconclusive
    //            };

    //            var zigNetEntityFrameworkDatabase = new ZigNetEntityFrameworkDatabase(zigNetEntitiesWrapperMock.Object);
    //            var suiteResultId = zigNetEntityFrameworkDatabase.SaveSuiteResult(suiteResult);

    //            Assert.AreEqual(1, suiteResultId);
    //        }

    //        [TestMethod]
    //        public void SavesExistingSuiteResult()
    //        {
    //            var zigNetEntitiesWrapperMock = new Mock<IZigNetEntitiesWriter>();
    //            var suiteResult = new ZigNetSuiteResult
    //            {
    //                SuiteResultID = 2,
    //                Suite = new ZigNetSuite { SuiteID = 3 },
    //                StartTime = DateTime.UtcNow,
    //                EndTime = DateTime.UtcNow,
    //                ResultType = ZigNetSuiteResultType.Pass
    //            };
    //            zigNetEntitiesWrapperMock.Setup(zewm => zewm.GetSuiteResult(2)).Returns(new SuiteResult { SuiteResultID = 2 });
    //            zigNetEntitiesWrapperMock.Setup(zewm => zewm.SaveSuiteResult(It.IsAny<SuiteResult>())).Returns(2);

    //            var zigNetEntityFrameworkDatabase = new ZigNetEntityFrameworkDatabase(zigNetEntitiesWrapperMock.Object);
    //            var suiteResultId = zigNetEntityFrameworkDatabase.SaveSuiteResult(suiteResult);

    //            Assert.AreEqual(2, suiteResultId);
    //        }
    //    }

    //    [TestClass]
    //    public class GetSuiteResultMethod
    //    {
    //        [TestMethod]
    //        public void MapsDatabaseSuiteResult()
    //        {
    //            var startTime = DateTime.UtcNow;
    //            var endTime = DateTime.UtcNow;

    //            var zigNetEntitiesWrapperMock = new Mock<IZigNetEntitiesWriter>();
    //            zigNetEntitiesWrapperMock.Setup(zewm => zewm.GetSuiteResult(1)).Returns(
    //                new SuiteResult
    //                {
    //                    SuiteResultID = 1,
    //                    SuiteResultStartDateTime = startTime,
    //                    SuiteResultEndDateTime = endTime,
    //                    SuiteResultType = new SuiteResultType { SuiteResultTypeName = "Fail" },
    //                    Suite = new Suite
    //                    {
    //                        SuiteID = 2,
    //                        SuiteCategories = new List<SuiteCategory> { new SuiteCategory { CategoryName = "suite category 1" } }
    //                    }
    //                }
    //            );

    //            var zigNetEntityFrameworkDatabase = new ZigNetEntityFrameworkDatabase(zigNetEntitiesWrapperMock.Object);
    //            var suiteResult = zigNetEntityFrameworkDatabase.GetSuiteResult(1);

    //            Assert.AreEqual(1, suiteResult.SuiteResultID);
    //            Assert.AreEqual(startTime, suiteResult.StartTime);
    //            Assert.AreEqual(endTime, suiteResult.EndTime);
    //            Assert.AreEqual(ZigNetSuiteResultType.Fail, suiteResult.ResultType);
    //            Assert.AreEqual(2, suiteResult.Suite.SuiteID);
    //            Assert.AreEqual("suite category 1", suiteResult.Suite.Categories.ElementAt(0).Name);
    //        }

    //        [TestMethod]
    //        public void DoesNotThrowWhenEndDateTimeNull()
    //        {
    //            var zigNetEntitiesWrapperMock = new Mock<IZigNetEntitiesWriter>();
    //            zigNetEntitiesWrapperMock.Setup(zewm => zewm.GetSuiteResult(1)).Returns(
    //                new SuiteResult
    //                {
    //                    SuiteResultID = 1,
    //                    SuiteResultEndDateTime = null,
    //                    SuiteResultType = new SuiteResultType { SuiteResultTypeName = "Fail" },
    //                    Suite = new Suite()
    //                }
    //            );

    //            var zigNetEntityFrameworkDatabase = new ZigNetEntityFrameworkDatabase(zigNetEntitiesWrapperMock.Object);
    //            var suiteResult = zigNetEntityFrameworkDatabase.GetSuiteResult(1);

    //            Assert.AreEqual(1, suiteResult.SuiteResultID);
    //            Assert.IsNull(suiteResult.EndTime);
    //        }
    //    }

    //    [TestClass]
    //    public class GetTestMethod
    //    {
    //        [TestMethod]
    //        public void ReturnsNullWhenTestDoesNotExist()
    //        {
    //            var zigNetEntitiesWrapperMock = new Mock<IZigNetEntitiesWriter>();
    //            zigNetEntitiesWrapperMock.Setup(zewm => zewm.GetTestOrDefault("test 1")).Returns((ZigNetTest)null);

    //            var zigNetEntityFrameworkDatabase = new ZigNetEntityFrameworkDatabase(zigNetEntitiesWrapperMock.Object);
    //            var test = zigNetEntityFrameworkDatabase.GetTestOrDefault("test 1");

    //            Assert.IsNull(test);
    //        }
    //    }

    //    [TestClass]
    //    public class SaveTestResultMethod
    //    {
    //        [TestMethod]
    //        public void SavesFailedTestResultWithDetails()
    //        {
    //            var zigNetEntitiesWrapperMock = new Mock<IZigNetEntitiesWriter>();
    //            zigNetEntitiesWrapperMock.Setup(zewm => zewm.GetTestFailureType(2)).Returns(
    //                new TestFailureType
    //                {
    //                    TestFailureTypeID = 2,
    //                    TestFailureTypeName = "Exception"
    //                }
    //            );
    //            zigNetEntitiesWrapperMock.Setup(zewm => zewm.GetTestCategories()).Returns(
    //                new List<TestCategory> { new TestCategory { TestCategoryID = 1, CategoryName = "test category 1" } }.AsQueryable);
    //            zigNetEntitiesWrapperMock.Setup(zewm => zewm.GetSuiteResultWithoutTracking(1)).Returns(new SuiteResult { SuiteId = 2 });
    //            zigNetEntitiesWrapperMock.Setup(zewm => zewm.GetSuite(2)).Returns(new Suite { SuiteID = 2 });
    //            zigNetEntitiesWrapperMock.Setup(zewm => zewm.SaveTestResult(It.IsAny<TestResult>()))
    //                .Returns(new TestResult
    //                {
    //                    TestResultID = 3,
    //                    Test = new Test { TestID = 4 }
    //                });

    //            var testResult = new ZigNetTestResult
    //            {
    //                Test = new ZigNetTest { Name = "test 1", Categories = new List<ZigNetTestCategory> { new ZigNetTestCategory { Name = "test category 1" } } },
    //                SuiteResult = new ZigNetSuiteResult { SuiteResultID = 1 },
    //                StartTime = DateTime.UtcNow,
    //                EndTime = DateTime.UtcNow,
    //                ResultType = ZigNetTestResultType.Fail,
    //                TestFailureDetails = new ZigNetTestFailureDetails
    //                {
    //                    FailureType = ZigNetTestFailureType.Exception,
    //                    FailureDetailMessage = "failed by exception at line 5"
    //                }
    //            };

    //            var zigNetEntityFrameworkDatabase = new ZigNetEntityFrameworkDatabase(zigNetEntitiesWrapperMock.Object);
    //            zigNetEntityFrameworkDatabase.SaveTestResult(testResult);
    //        }

    //        [TestMethod]
    //        public void SavesExistingTestPassedTestResult()
    //        {
    //            var zigNetEntitiesWrapperMock = new Mock<IZigNetEntitiesWriter>();
    //            zigNetEntitiesWrapperMock.Setup(zewm => zewm.GetTestCategories()).Returns(
    //                new List<TestCategory> {
    //                    new TestCategory { TestCategoryID = 1, CategoryName = "test category 1" }
    //                }.AsQueryable
    //            );
    //            zigNetEntitiesWrapperMock.Setup(zewm => zewm.GetTest(1)).Returns(
    //                new Test
    //                {
    //                    TestID = 1,
    //                    TestCategories = new List<TestCategory> { new TestCategory { TestCategoryID = 1, CategoryName = "test category 1" } },
    //                    Suites = new List<Suite> { new Suite { SuiteID = 2 } }
    //                }
    //            );
    //            zigNetEntitiesWrapperMock.Setup(zewm => zewm.GetSuiteResultWithoutTracking(1)).Returns(new SuiteResult { SuiteId = 2 });
    //            zigNetEntitiesWrapperMock.Setup(zewm => zewm.SaveTestResult(It.IsAny<TestResult>()))
    //                .Returns(new TestResult
    //                {
    //                    TestResultID = 3,
    //                    Test = new Test { TestID = 4 }
    //                });

    //            var testResult = new ZigNetTestResult
    //            {
    //                Test = new ZigNetTest
    //                {
    //                    TestID = 1,
    //                    Name = "test 1",
    //                    Categories = new List<ZigNetTestCategory> { new ZigNetTestCategory { Name = "test category 1" } }
    //                },
    //                SuiteResult = new ZigNetSuiteResult { SuiteResultID = 1 },
    //                StartTime = DateTime.UtcNow,
    //                EndTime = DateTime.UtcNow,
    //                ResultType = ZigNetTestResultType.Pass,
    //            };

    //            var zigNetEntityFrameworkDatabase = new ZigNetEntityFrameworkDatabase(zigNetEntitiesWrapperMock.Object);
    //            zigNetEntityFrameworkDatabase.SaveTestResult(testResult);
    //        }

    //        [TestMethod]
    //        public void SavesExistingTestFailedTestResult()
    //        {
    //            var zigNetEntitiesWrapperMock = new Mock<IZigNetEntitiesWriter>();
    //            zigNetEntitiesWrapperMock.Setup(zewm => zewm.GetTestFailureType(1)).Returns(
    //                new TestFailureType
    //                {
    //                    TestFailureTypeID = 1,
    //                    TestFailureTypeName = "Assertion"
    //                }
    //            );
    //            zigNetEntitiesWrapperMock.Setup(zewm => zewm.GetTestCategories()).Returns(
    //                new List<TestCategory> {
    //                    new TestCategory { TestCategoryID = 1, CategoryName = "test category 1" }
    //                }.AsQueryable
    //            );
    //            zigNetEntitiesWrapperMock.Setup(zewm => zewm.GetTest(1)).Returns(
    //                new Test
    //                {
    //                    TestID = 1,
    //                    TestCategories = new List<TestCategory> { new TestCategory { TestCategoryID = 1, CategoryName = "test category 1" } },
    //                    Suites = new List<Suite> { new Suite { SuiteID = 2 } }
    //                }
    //            );
    //            zigNetEntitiesWrapperMock.Setup(zewm => zewm.GetSuiteResultWithoutTracking(1)).Returns(new SuiteResult { SuiteId = 2 });
    //            zigNetEntitiesWrapperMock.Setup(zewm => zewm.SaveTestResult(It.IsAny<TestResult>()))
    //                .Returns(new TestResult
    //                {
    //                    TestResultID = 3,
    //                    Test = new Test { TestID = 4 }
    //                });

    //            var testResult = new ZigNetTestResult
    //            {
    //                Test = new ZigNetTest
    //                {
    //                    TestID = 1,
    //                    Name = "test 1",
    //                    Categories = new List<ZigNetTestCategory> {
    //                        new ZigNetTestCategory { Name = "test category 1" },
    //                        new ZigNetTestCategory { Name = "test category 2" }
    //                    }
    //                },
    //                SuiteResult = new ZigNetSuiteResult { SuiteResultID = 1 },
    //                StartTime = DateTime.UtcNow,
    //                EndTime = DateTime.UtcNow,
    //                ResultType = ZigNetTestResultType.Fail,
    //                TestFailureDetails = new ZigNetTestFailureDetails
    //                {
    //                    FailureType = ZigNetTestFailureType.Assertion
    //                }
    //            };

    //            var zigNetEntityFrameworkDatabase = new ZigNetEntityFrameworkDatabase(zigNetEntitiesWrapperMock.Object);
    //            zigNetEntityFrameworkDatabase.SaveTestResult(testResult);
    //        }

    //        [TestMethod]
    //        public void DoesNotThrowWhenClearingAllTestCategories()
    //        {
    //            var zigNetEntitiesWrapperMock = new Mock<IZigNetEntitiesWriter>();
    //            zigNetEntitiesWrapperMock.Setup(zewm => zewm.GetTest(1)).Returns(
    //                new Test
    //                {
    //                    TestID = 1,
    //                    TestCategories = new List<TestCategory> { new TestCategory { TestCategoryID = 1, CategoryName = "test category 1" } },
    //                    Suites = new List<Suite> { new Suite { SuiteID = 2 } }
    //                }
    //            );
    //            zigNetEntitiesWrapperMock.Setup(zewm => zewm.GetSuiteResultWithoutTracking(1)).Returns(new SuiteResult { SuiteId = 2 });
    //            zigNetEntitiesWrapperMock.Setup(zewm => zewm.SaveTestResult(It.IsAny<TestResult>()))
    //                .Returns(new TestResult
    //                {
    //                    TestResultID = 3,
    //                    Test = new Test { TestID = 4 }
    //                });

    //            var testResult = new ZigNetTestResult
    //            {
    //                Test = new ZigNetTest
    //                {
    //                    TestID = 1,
    //                    Name = "test 1",
    //                    Categories = new List<ZigNetTestCategory>()
    //                },
    //                SuiteResult = new ZigNetSuiteResult { SuiteResultID = 1 },
    //                StartTime = DateTime.UtcNow,
    //                EndTime = DateTime.UtcNow,
    //                ResultType = ZigNetTestResultType.Pass
    //            };

    //            var zigNetEntityFrameworkDatabase = new ZigNetEntityFrameworkDatabase(zigNetEntitiesWrapperMock.Object);
    //            zigNetEntityFrameworkDatabase.SaveTestResult(testResult);
    //        }

    //        [TestMethod]
    //        public void CreatesNewLatestTestResult()
    //        {
    //            var zigNetEntitiesWrapperMock = new Mock<IZigNetEntitiesWriter>();
    //            zigNetEntitiesWrapperMock.Setup(zewm => zewm.GetSuiteResultWithoutTracking(1)).Returns(new SuiteResult { SuiteId = 2 });
    //            zigNetEntitiesWrapperMock.Setup(zewm => zewm.SaveTestResult(It.IsAny<TestResult>()))
    //                .Returns(new TestResult
    //                {
    //                    TestResultID = 3,
    //                    Test = new Test { TestID = 4 }
    //                });
    //            zigNetEntitiesWrapperMock.Setup(zewm => zewm.GetLatestTestResults()).Returns(new List<LatestTestResult>().AsQueryable);

    //            var testResult = new ZigNetTestResult
    //            {
    //                Test = new ZigNetTest
    //                {
    //                    TestID = 0,
    //                    Name = "test 1",
    //                    Categories = new List<ZigNetTestCategory>()
    //                },
    //                SuiteResult = new ZigNetSuiteResult { SuiteResultID = 1 },
    //                ResultType = ZigNetTestResultType.Fail,
    //                TestFailureDetails = new ZigNetTestFailureDetails()
    //            };

    //            var zigNetEntityFrameworkDatabase = new ZigNetEntityFrameworkDatabase(zigNetEntitiesWrapperMock.Object);
    //            zigNetEntityFrameworkDatabase.SaveTestResult(testResult);
    //        }

    //        [TestMethod]
    //        public void CreatesNewLatestTestResultWhenSuiteIdDoesNotMatch()
    //        {
    //            var zigNetEntitiesWrapperMock = new Mock<IZigNetEntitiesWriter>();
    //            zigNetEntitiesWrapperMock.Setup(zewm => zewm.GetSuiteResultWithoutTracking(5)).Returns(new SuiteResult { SuiteId = 2 });
    //            zigNetEntitiesWrapperMock.Setup(zewm => zewm.SaveTestResult(It.IsAny<TestResult>()))
    //                .Returns(new TestResult
    //                {
    //                    TestResultID = 3,
    //                    Test = new Test { TestID = 4 }
    //                });
    //            zigNetEntitiesWrapperMock.Setup(zewm => zewm.GetLatestTestResults())
    //                .Returns(new List<LatestTestResult>{
    //                    new LatestTestResult { SuiteId = 1, TestId = 4 }
    //                }.AsQueryable);
    //            zigNetEntitiesWrapperMock.Setup(zewm => zewm.GetTest(4)).Returns(
    //                new Test
    //                {
    //                    TestID = 4,
    //                    TestCategories = new List<TestCategory>()
    //                }
    //            );

    //            var testResult = new ZigNetTestResult
    //            {
    //                Test = new ZigNetTest
    //                {
    //                    TestID = 4,
    //                    Name = "test 1",
    //                    Categories = new List<ZigNetTestCategory>()
    //                },
    //                SuiteResult = new ZigNetSuiteResult { SuiteResultID = 5 },
    //                ResultType = ZigNetTestResultType.Fail,
    //                TestFailureDetails = new ZigNetTestFailureDetails()
    //            };

    //            var zigNetEntityFrameworkDatabase = new ZigNetEntityFrameworkDatabase(zigNetEntitiesWrapperMock.Object);
    //            zigNetEntityFrameworkDatabase.SaveTestResult(testResult);
    //        }

    //        [TestMethod]
    //        public void CreatesNewLatestTestResultWhenTestIdDoesNotMatch()
    //        {
    //            var zigNetEntitiesWrapperMock = new Mock<IZigNetEntitiesWriter>();
    //            zigNetEntitiesWrapperMock.Setup(zewm => zewm.GetSuiteResultWithoutTracking(5)).Returns(new SuiteResult { SuiteId = 2 });
    //            zigNetEntitiesWrapperMock.Setup(zewm => zewm.SaveTestResult(It.IsAny<TestResult>()))
    //                .Returns(new TestResult
    //                {
    //                    TestResultID = 3,
    //                    Test = new Test { TestID = 4 }
    //                });
    //            zigNetEntitiesWrapperMock.Setup(zewm => zewm.GetLatestTestResults())
    //                .Returns(new List<LatestTestResult>{
    //                    new LatestTestResult { SuiteId = 2, TestId = 3 }
    //                }.AsQueryable);
    //            zigNetEntitiesWrapperMock.Setup(zewm => zewm.GetTest(4)).Returns(
    //                new Test
    //                {
    //                    TestID = 4,
    //                    TestCategories = new List<TestCategory>()
    //                }
    //            );

    //            var testResult = new ZigNetTestResult
    //            {
    //                Test = new ZigNetTest
    //                {
    //                    TestID = 4,
    //                    Name = "test 1",
    //                    Categories = new List<ZigNetTestCategory>()
    //                },
    //                SuiteResult = new ZigNetSuiteResult { SuiteResultID = 5 },
    //                ResultType = ZigNetTestResultType.Fail,
    //                TestFailureDetails = new ZigNetTestFailureDetails()
    //            };

    //            var zigNetEntityFrameworkDatabase = new ZigNetEntityFrameworkDatabase(zigNetEntitiesWrapperMock.Object);
    //            zigNetEntityFrameworkDatabase.SaveTestResult(testResult);
    //        }

    //        [TestMethod]
    //        public void UpdatesExistingLatestTestResult()
    //        {
    //            var zigNetEntitiesWrapperMock = new Mock<IZigNetEntitiesWriter>();
    //            zigNetEntitiesWrapperMock.Setup(zewm => zewm.GetSuiteResultWithoutTracking(5)).Returns(new SuiteResult { SuiteId = 2 });
    //            zigNetEntitiesWrapperMock.Setup(zewm => zewm.SaveTestResult(It.IsAny<TestResult>()))
    //                .Returns(new TestResult
    //                {
    //                    TestResultID = 3,
    //                    Test = new Test { TestID = 4 }
    //                });
    //            zigNetEntitiesWrapperMock.Setup(zewm => zewm.GetLatestTestResults())
    //                .Returns(new List<LatestTestResult>{
    //                    new LatestTestResult { SuiteId = 2, TestId = 4 }
    //                }.AsQueryable);
    //            zigNetEntitiesWrapperMock.Setup(zewm => zewm.GetTest(4)).Returns(
    //                new Test
    //                {
    //                    TestID = 4,
    //                    TestCategories = new List<TestCategory>()
    //                }
    //            );

    //            var testResult = new ZigNetTestResult
    //            {
    //                Test = new ZigNetTest
    //                {
    //                    TestID = 4,
    //                    Name = "test 1",
    //                    Categories = new List<ZigNetTestCategory>()
    //                },
    //                SuiteResult = new ZigNetSuiteResult { SuiteResultID = 5 },
    //                ResultType = ZigNetTestResultType.Fail,
    //                TestFailureDetails = new ZigNetTestFailureDetails()
    //            };

    //            var zigNetEntityFrameworkDatabase = new ZigNetEntityFrameworkDatabase(zigNetEntitiesWrapperMock.Object);
    //            zigNetEntityFrameworkDatabase.SaveTestResult(testResult);
    //        }

    //        [TestMethod]
    //        public void UpdatesPassingFromDate()
    //        {
    //            var zigNetEntitiesWrapperMock = new Mock<IZigNetEntitiesWriter>();
    //            zigNetEntitiesWrapperMock.Setup(zewm => zewm.GetSuiteResultWithoutTracking(5)).Returns(new SuiteResult { SuiteId = 2 });
    //            zigNetEntitiesWrapperMock.Setup(zewm => zewm.SaveTestResult(It.IsAny<TestResult>()))
    //                .Returns(new TestResult
    //                {
    //                    TestResultID = 3,
    //                    Test = new Test { TestID = 4 }
    //                });
    //            zigNetEntitiesWrapperMock.Setup(zewm => zewm.GetLatestTestResults())
    //                .Returns(new List<LatestTestResult>{
    //                    new LatestTestResult { SuiteId = 2, TestId = 4, FailingFromDateTime = DateTime.UtcNow }
    //                }.AsQueryable);
    //            zigNetEntitiesWrapperMock.Setup(zewm => zewm.GetTest(4)).Returns(
    //                new Test
    //                {
    //                    TestID = 4,
    //                    TestCategories = new List<TestCategory>()
    //                }
    //            );

    //            var testResult = new ZigNetTestResult
    //            {
    //                Test = new ZigNetTest
    //                {
    //                    TestID = 4,
    //                    Name = "test 1",
    //                    Categories = new List<ZigNetTestCategory>()
    //                },
    //                SuiteResult = new ZigNetSuiteResult { SuiteResultID = 5 },
    //                ResultType = ZigNetTestResultType.Pass
    //            };

    //            var zigNetEntityFrameworkDatabase = new ZigNetEntityFrameworkDatabase(zigNetEntitiesWrapperMock.Object);
    //            zigNetEntityFrameworkDatabase.SaveTestResult(testResult);
    //        }

    //        [TestMethod]
    //        public void UpdatesFailingFromDate()
    //        {
    //            var zigNetEntitiesWrapperMock = new Mock<IZigNetEntitiesWriter>();
    //            zigNetEntitiesWrapperMock.Setup(zewm => zewm.GetSuiteResultWithoutTracking(5)).Returns(new SuiteResult { SuiteId = 2 });
    //            zigNetEntitiesWrapperMock.Setup(zewm => zewm.SaveTestResult(It.IsAny<TestResult>()))
    //                .Returns(new TestResult
    //                {
    //                    TestResultID = 3,
    //                    Test = new Test { TestID = 4 }
    //                });
    //            zigNetEntitiesWrapperMock.Setup(zewm => zewm.GetLatestTestResults())
    //                .Returns(new List<LatestTestResult>{
    //                    new LatestTestResult { SuiteId = 2, TestId = 4, PassingFromDateTime = DateTime.UtcNow }
    //                }.AsQueryable);
    //            zigNetEntitiesWrapperMock.Setup(zewm => zewm.GetTest(4)).Returns(
    //                new Test
    //                {
    //                    TestID = 4,
    //                    TestCategories = new List<TestCategory>()
    //                }
    //            );

    //            var testResult = new ZigNetTestResult
    //            {
    //                Test = new ZigNetTest
    //                {
    //                    TestID = 4,
    //                    Name = "test 1",
    //                    Categories = new List<ZigNetTestCategory>()
    //                },
    //                SuiteResult = new ZigNetSuiteResult { SuiteResultID = 5 },
    //                ResultType = ZigNetTestResultType.Fail,
    //                TestFailureDetails = new ZigNetTestFailureDetails()
    //            };

    //            var zigNetEntityFrameworkDatabase = new ZigNetEntityFrameworkDatabase(zigNetEntitiesWrapperMock.Object);
    //            zigNetEntityFrameworkDatabase.SaveTestResult(testResult);
    //        }

    //        [TestMethod]
    //        public void IgnoresPassingFromDate()
    //        {
    //            var zigNetEntitiesWrapperMock = new Mock<IZigNetEntitiesWriter>();
    //            zigNetEntitiesWrapperMock.Setup(zewm => zewm.GetSuiteResultWithoutTracking(5)).Returns(new SuiteResult { SuiteId = 2 });
    //            zigNetEntitiesWrapperMock.Setup(zewm => zewm.SaveTestResult(It.IsAny<TestResult>()))
    //                .Returns(new TestResult
    //                {
    //                    TestResultID = 3,
    //                    Test = new Test { TestID = 4 }
    //                });
    //            zigNetEntitiesWrapperMock.Setup(zewm => zewm.GetLatestTestResults())
    //                .Returns(new List<LatestTestResult>{
    //                    new LatestTestResult { SuiteId = 2, TestId = 4, PassingFromDateTime = DateTime.UtcNow }
    //                }.AsQueryable);
    //            zigNetEntitiesWrapperMock.Setup(zewm => zewm.GetTest(4)).Returns(
    //                new Test
    //                {
    //                    TestID = 4,
    //                    TestCategories = new List<TestCategory>()
    //                }
    //            );

    //            var testResult = new ZigNetTestResult
    //            {
    //                Test = new ZigNetTest
    //                {
    //                    TestID = 4,
    //                    Name = "test 1",
    //                    Categories = new List<ZigNetTestCategory>()
    //                },
    //                SuiteResult = new ZigNetSuiteResult { SuiteResultID = 5 },
    //                ResultType = ZigNetTestResultType.Pass
    //            };

    //            var zigNetEntityFrameworkDatabase = new ZigNetEntityFrameworkDatabase(zigNetEntitiesWrapperMock.Object);
    //            zigNetEntityFrameworkDatabase.SaveTestResult(testResult);
    //        }

    //        [TestMethod]
    //        public void IgnoresFailingFromDate()
    //        {
    //            var zigNetEntitiesWrapperMock = new Mock<IZigNetEntitiesWriter>();
    //            zigNetEntitiesWrapperMock.Setup(zewm => zewm.GetSuiteResultWithoutTracking(5)).Returns(new SuiteResult { SuiteId = 2 });
    //            zigNetEntitiesWrapperMock.Setup(zewm => zewm.SaveTestResult(It.IsAny<TestResult>()))
    //                .Returns(new TestResult
    //                {
    //                    TestResultID = 3,
    //                    Test = new Test { TestID = 4 }
    //                });
    //            zigNetEntitiesWrapperMock.Setup(zewm => zewm.GetLatestTestResults())
    //                .Returns(new List<LatestTestResult>{
    //                    new LatestTestResult { SuiteId = 2, TestId = 4, FailingFromDateTime = DateTime.UtcNow }
    //                }.AsQueryable);
    //            zigNetEntitiesWrapperMock.Setup(zewm => zewm.GetTest(4)).Returns(
    //                new Test
    //                {
    //                    TestID = 4,
    //                    TestCategories = new List<TestCategory>()
    //                }
    //            );

    //            var testResult = new ZigNetTestResult
    //            {
    //                Test = new ZigNetTest
    //                {
    //                    TestID = 4,
    //                    Name = "test 1",
    //                    Categories = new List<ZigNetTestCategory>()
    //                },
    //                SuiteResult = new ZigNetSuiteResult { SuiteResultID = 5 },
    //                ResultType = ZigNetTestResultType.Fail,
    //                TestFailureDetails = new ZigNetTestFailureDetails()
    //            };

    //            var zigNetEntityFrameworkDatabase = new ZigNetEntityFrameworkDatabase(zigNetEntitiesWrapperMock.Object);
    //            zigNetEntityFrameworkDatabase.SaveTestResult(testResult);
    //        }

    //        [TestMethod]
    //        public void UpdatesFailingFromDateIfInconclusive()
    //        {
    //            var zigNetEntitiesWrapperMock = new Mock<IZigNetEntitiesWriter>();
    //            zigNetEntitiesWrapperMock.Setup(zewm => zewm.GetSuiteResultWithoutTracking(5)).Returns(new SuiteResult { SuiteId = 2 });
    //            zigNetEntitiesWrapperMock.Setup(zewm => zewm.SaveTestResult(It.IsAny<TestResult>()))
    //                .Returns(new TestResult
    //                {
    //                    TestResultID = 3,
    //                    Test = new Test { TestID = 4 }
    //                });
    //            zigNetEntitiesWrapperMock.Setup(zewm => zewm.GetLatestTestResults())
    //                .Returns(new List<LatestTestResult>{
    //                    new LatestTestResult { SuiteId = 2, TestId = 4, PassingFromDateTime = DateTime.UtcNow }
    //                }.AsQueryable);
    //            zigNetEntitiesWrapperMock.Setup(zewm => zewm.GetTest(4)).Returns(
    //                new Test
    //                {
    //                    TestID = 4,
    //                    TestCategories = new List<TestCategory>()
    //                }
    //            );

    //            var testResult = new ZigNetTestResult
    //            {
    //                Test = new ZigNetTest
    //                {
    //                    TestID = 4,
    //                    Name = "test 1",
    //                    Categories = new List<ZigNetTestCategory>()
    //                },
    //                SuiteResult = new ZigNetSuiteResult { SuiteResultID = 5 },
    //                ResultType = ZigNetTestResultType.Inconclusive
    //            };

    //            var zigNetEntityFrameworkDatabase = new ZigNetEntityFrameworkDatabase(zigNetEntitiesWrapperMock.Object);
    //            zigNetEntityFrameworkDatabase.SaveTestResult(testResult);
    //        }

    //        [TestMethod]
    //        public void IgnoresFailingFromDateIfInconclusive()
    //        {
    //            var zigNetEntitiesWrapperMock = new Mock<IZigNetEntitiesWriter>();
    //            zigNetEntitiesWrapperMock.Setup(zewm => zewm.GetSuiteResultWithoutTracking(5)).Returns(new SuiteResult { SuiteId = 2 });
    //            zigNetEntitiesWrapperMock.Setup(zewm => zewm.SaveTestResult(It.IsAny<TestResult>()))
    //                .Returns(new TestResult
    //                {
    //                    TestResultID = 3,
    //                    Test = new Test { TestID = 4 }
    //                });
    //            zigNetEntitiesWrapperMock.Setup(zewm => zewm.GetLatestTestResults())
    //                .Returns(new List<LatestTestResult>{
    //                    new LatestTestResult { SuiteId = 2, TestId = 4, FailingFromDateTime = DateTime.UtcNow }
    //                }.AsQueryable);
    //            zigNetEntitiesWrapperMock.Setup(zewm => zewm.GetTest(4)).Returns(
    //                new Test
    //                {
    //                    TestID = 4,
    //                    TestCategories = new List<TestCategory>()
    //                }
    //            );

    //            var testResult = new ZigNetTestResult
    //            {
    //                Test = new ZigNetTest
    //                {
    //                    TestID = 4,
    //                    Name = "test 1",
    //                    Categories = new List<ZigNetTestCategory>()
    //                },
    //                SuiteResult = new ZigNetSuiteResult { SuiteResultID = 5 },
    //                ResultType = ZigNetTestResultType.Inconclusive
    //            };

    //            var zigNetEntityFrameworkDatabase = new ZigNetEntityFrameworkDatabase(zigNetEntitiesWrapperMock.Object);
    //            zigNetEntityFrameworkDatabase.SaveTestResult(testResult);
    //        }
    //    }

    //    [TestClass]
    //    public class GetTestResultsForSuiteMethod
    //    {
    //        [TestMethod]
    //        public void MapsTestResults()
    //        {
    //            var zigNetEntitiesWrapperMock = new Mock<IZigNetEntitiesWriter>();
    //            zigNetEntitiesWrapperMock.Setup(zewm => zewm.GetTestResults()).Returns(
    //                new List<TestResult> {
    //                    new TestResult
    //                    {
    //                        SuiteResult = new SuiteResult { SuiteId = 1 },
    //                        TestResultEndDateTime = new DateTime(2018, 3, 13, 15, 30, 27),
    //                        TestResultType = new TestResultType { TestResultTypeName = "Pass" },
    //                        Test = new Test { TestID = 2 }
    //                    },
    //                    new TestResult
    //                    {
    //                        SuiteResult = new SuiteResult { SuiteId = 1 },
    //                        TestResultEndDateTime = new DateTime(2018, 3, 14, 15, 30, 27),
    //                        TestResultType = new TestResultType { TestResultTypeName = "Fail" },
    //                        Test = new Test { TestID = 2 }
    //                    },
    //                    new TestResult
    //                    {
    //                        SuiteResult = new SuiteResult { SuiteId = 1 },
    //                        TestResultEndDateTime = new DateTime(2018, 3, 15, 15, 30, 27),
    //                        TestResultType = new TestResultType { TestResultTypeName = "Inconclusive" },
    //                        Test = new Test { TestID = 2 }
    //                    },
    //                    new TestResult
    //                    {
    //                        SuiteResult = new SuiteResult { SuiteId = 1 },
    //                        TestResultEndDateTime = new DateTime(2018, 3, 16, 15, 30, 27),
    //                        TestResultType = new TestResultType { TestResultTypeName = "Pass" },
    //                        Test = new Test { TestID = 2 }
    //                    }
    //                }.AsQueryable
    //            );

    //            var zigNetEntityFrameworkDatabase = new ZigNetEntityFrameworkDatabase(zigNetEntitiesWrapperMock.Object);

    //            var testResultsForTestInSuite = zigNetEntityFrameworkDatabase.GetTestResultsForSuite(1);

    //            Assert.AreEqual(4, testResultsForTestInSuite.Count());
    //            Assert.IsTrue(testResultsForTestInSuite.All(tr => tr.Test.TestID == 2));
    //            Assert.IsTrue(testResultsForTestInSuite.All(tr => tr.Test.Categories == null));
    //        }

    //        [TestMethod]
    //        public void IgnoresTestResultsWithoutSuiteId()
    //        {
    //            var zigNetEntitiesWrapperMock = new Mock<IZigNetEntitiesWriter>();
    //            zigNetEntitiesWrapperMock.Setup(zewm => zewm.GetTestResults()).Returns(
    //                new List<TestResult> {
    //                    new TestResult
    //                    {
    //                        SuiteResult = new SuiteResult { SuiteId = 1 },
    //                        TestResultEndDateTime = new DateTime(2018, 3, 13, 15, 30, 27),
    //                        TestResultType = new TestResultType { TestResultTypeName = "Pass" },
    //                        Test = new Test { TestID = 2 }
    //                    },
    //                    new TestResult
    //                    {
    //                        SuiteResult = new SuiteResult { SuiteId = 2 },
    //                        TestResultEndDateTime = new DateTime(2018, 3, 14, 15, 30, 27),
    //                        TestResultType = new TestResultType { TestResultTypeName = "Pass" },
    //                        Test = new Test { TestID = 2 }
    //                    }
    //                }.AsQueryable
    //            );

    //            var zigNetEntityFrameworkDatabase = new ZigNetEntityFrameworkDatabase(zigNetEntitiesWrapperMock.Object);

    //            var testResultsForTestInSuite = zigNetEntityFrameworkDatabase.GetTestResultsForSuite(1);

    //            Assert.AreEqual(1, testResultsForTestInSuite.Count());
    //            Assert.IsTrue(testResultsForTestInSuite.All(tr => tr.Test.TestID == 2));
    //        }

    //        [TestMethod]
    //        public void DoesNotThrowWhenZeroTestResultsReturned()
    //        {
    //            var zigNetEntitiesWrapperMock = new Mock<IZigNetEntitiesWriter>();
    //            zigNetEntitiesWrapperMock.Setup(zewm => zewm.GetTestResults()).Returns(
    //                new List<TestResult>()
    //                .AsQueryable
    //            );

    //            var zigNetEntityFrameworkDatabase = new ZigNetEntityFrameworkDatabase(zigNetEntitiesWrapperMock.Object);

    //            var testResultsForTestInSuite = zigNetEntityFrameworkDatabase.GetTestResultsForSuite(1);

    //            Assert.AreEqual(0, testResultsForTestInSuite.Count());
    //        }
    //    }

    //    [TestClass]
    //    public class GetLatestTestResultsMethod
    //    {
    //        [TestMethod]
    //        public void MapsCorrectly()
    //        {
    //            var utcNow = DateTime.UtcNow;
    //            var zigNetEntitiesWrapperMock = new Mock<IZigNetEntitiesWriter>();
    //            zigNetEntitiesWrapperMock.Setup(zewm => zewm.GetLatestTestResults()).Returns(
    //                new List<LatestTestResult> { 
    //                    new LatestTestResult 
    //                    {
    //                        SuiteId = 1,
    //                        TestResultId = 2,
    //                        TestName = "test1",
    //                        PassingFromDateTime = utcNow,
    //                    }
    //                }.AsQueryable);

    //            var zigNetEntityFrameworkDatabase = new ZigNetEntityFrameworkDatabase(zigNetEntitiesWrapperMock.Object);
    //            var latestTestResults = zigNetEntityFrameworkDatabase.GetLatestTestResults(1).ToList();

    //            Assert.AreEqual(1, latestTestResults.Count);
    //            Assert.AreEqual(2, latestTestResults[0].TestResultID);
    //            Assert.AreEqual("test1", latestTestResults[0].TestName);
    //            Assert.AreEqual(utcNow, latestTestResults[0].PassingFromDate);
    //            Assert.IsNull(latestTestResults[0].FailingFromDate);
    //        }

    //        [TestMethod]
    //        public void DoesNotThrowWhenNoLatestTestResultsForSuite()
    //        {
    //            var utcNow = DateTime.UtcNow;
    //            var zigNetEntitiesWrapperMock = new Mock<IZigNetEntitiesWriter>();
    //            zigNetEntitiesWrapperMock.Setup(zewm => zewm.GetLatestTestResults()).Returns(
    //                new List<LatestTestResult>().AsQueryable);

    //            var zigNetEntityFrameworkDatabase = new ZigNetEntityFrameworkDatabase(zigNetEntitiesWrapperMock.Object);
    //            var latestTestResults = zigNetEntityFrameworkDatabase.GetLatestTestResults(1).ToList();

    //            Assert.AreEqual(0, latestTestResults.Count);
    //        }

    //        [TestMethod]
    //        public void SortsByFailingTheLongestThenPassingTheShortest()
    //        {
    //            var utcNow = DateTime.UtcNow;
    //            var zigNetEntitiesWrapperMock = new Mock<IZigNetEntitiesWriter>();
    //            zigNetEntitiesWrapperMock.Setup(zewm => zewm.GetLatestTestResults()).Returns(
    //                new List<LatestTestResult> { 
    //                    new LatestTestResult 
    //                    {
    //                        SuiteId = 1,
    //                        TestResultId = 2,
    //                        TestName = "test passing the longest",
    //                        PassingFromDateTime = new DateTime(2018, 3, 1, 1, 00, 00),
    //                    },
    //                    new LatestTestResult 
    //                    {
    //                        SuiteId = 1,
    //                        TestResultId = 3,
    //                        TestName = "test failing the longest",
    //                        FailingFromDateTime = new DateTime(2018, 3, 1, 1, 00, 00),
    //                    },
    //                    new LatestTestResult 
    //                    {
    //                        SuiteId = 1,
    //                        TestResultId = 4,
    //                        TestName = "test passing the shortest",
    //                        PassingFromDateTime = new DateTime(2018, 3, 1, 1, 01, 00),
    //                    },
    //                    new LatestTestResult 
    //                    {
    //                        SuiteId = 1,
    //                        TestResultId = 5,
    //                        TestName = "test failing the shortest",
    //                        FailingFromDateTime = new DateTime(2018, 3, 1, 1, 01, 00),
    //                    }
    //                }.AsQueryable);

    //            var zigNetEntityFrameworkDatabase = new ZigNetEntityFrameworkDatabase(zigNetEntitiesWrapperMock.Object);
    //            var latestTestResults = zigNetEntityFrameworkDatabase.GetLatestTestResults(1).ToList();

    //            Assert.AreEqual(4, latestTestResults.Count);
    //            Assert.AreEqual("test failing the longest", latestTestResults[0].TestName);
    //            Assert.AreEqual("test failing the shortest", latestTestResults[1].TestName);
    //            Assert.AreEqual("test passing the shortest", latestTestResults[2].TestName);
    //            Assert.AreEqual("test passing the longest", latestTestResults[3].TestName);
    //        }
    //    }
    //}
}
