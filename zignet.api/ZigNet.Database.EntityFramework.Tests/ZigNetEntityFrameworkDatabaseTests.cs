using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using ZigNetSuite = ZigNet.Domain.Suite.Suite;
using ZigNetSuiteCategory = ZigNet.Domain.Suite.SuiteCategory;
using ZigNetSuiteResult = ZigNet.Domain.Suite.SuiteResult;
using ZigNetTest = ZigNet.Domain.Test.Test;
using ZigNetTestResult = ZigNet.Domain.Test.TestResult;
using ZigNetTestResultType = ZigNet.Domain.Test.TestResultType;
using ZigNetTestCategory = ZigNet.Domain.Test.TestCategory;
using ZigNetTestFailureDetails = ZigNet.Domain.Test.TestFailureDetails;
using ZigNetTestFailureType = ZigNet.Domain.Test.TestFailureType;

namespace ZigNet.Database.EntityFramework.Tests
{
    public class ZigNetEntityFrameworkDatabaseTests
    {
        [TestClass]
        public class SaveSuiteMethod
        {
            [TestMethod]
            public void SavesNewSuiteWithNoCategories()
            {
                var zigNetEntitiesWriterMock = new Mock<IZigNetEntitiesWriter>();
                zigNetEntitiesWriterMock.Setup(zewm => zewm.SaveSuite(It.IsAny<Suite>())).Returns(1);
                var zigNetEntitiesReadOnlyMock = new Mock<IZigNetEntitiesReadOnly>();

                var suite = new ZigNetSuite { Name = "new suite", Categories = new List<ZigNetSuiteCategory>() };

                var zigNetEntityFrameworkDatabase = new ZigNetEntityFrameworkDatabase(zigNetEntitiesWriterMock.Object, zigNetEntitiesReadOnlyMock.Object);
                var suiteId = zigNetEntityFrameworkDatabase.SaveSuite(suite);

                Assert.AreEqual(1, suiteId);
            }

            [TestMethod]
            public void SavesNewSuiteWithEmptyCategoryList()
            {
                var zigNetEntitiesWriterMock = new Mock<IZigNetEntitiesWriter>();
                zigNetEntitiesWriterMock.Setup(zewm => zewm.SaveSuite(It.IsAny<Suite>())).Returns(1);
                var zigNetEntitiesReadOnlyMock = new Mock<IZigNetEntitiesReadOnly>();

                var suite = new ZigNetSuite { Name = "new suite", Categories = new List<ZigNetSuiteCategory>() };

                var zigNetEntityFrameworkDatabase = new ZigNetEntityFrameworkDatabase(zigNetEntitiesWriterMock.Object, zigNetEntitiesReadOnlyMock.Object);
                var suiteId = zigNetEntityFrameworkDatabase.SaveSuite(suite);

                Assert.AreEqual(1, suiteId);
            }

            [TestMethod]
            public void SavesNewSuiteWithOnlyNewCategories()
            {
                var zigNetEntitiesWriterMock = new Mock<IZigNetEntitiesWriter>();
                zigNetEntitiesWriterMock.Setup(zewm => zewm.SaveSuite(It.IsAny<Suite>())).Returns(1);
                zigNetEntitiesWriterMock.Setup(zewm => zewm.GetSuiteCategories()).Returns(new List<SuiteCategory>().AsQueryable());
                var zigNetEntitiesReadOnlyMock = new Mock<IZigNetEntitiesReadOnly>();

                var suite = new ZigNetSuite
                {
                    Name = "new suite",
                    Categories = new List<ZigNetSuiteCategory> {
                        new ZigNetSuiteCategory { Name = "suite category 1" }
                    }
                };

                var zigNetEntityFrameworkDatabase = new ZigNetEntityFrameworkDatabase(zigNetEntitiesWriterMock.Object, zigNetEntitiesReadOnlyMock.Object);
                var suiteId = zigNetEntityFrameworkDatabase.SaveSuite(suite);

                Assert.AreEqual(1, suiteId);
            }

            [TestMethod]
            public void SavesNewSuiteWithOnlyExistingCategories()
            {
                var zigNetEntitiesWriterMock = new Mock<IZigNetEntitiesWriter>();
                zigNetEntitiesWriterMock.Setup(zewm => zewm.SaveSuite(It.IsAny<Suite>())).Returns(1);
                zigNetEntitiesWriterMock.Setup(zewm => zewm.GetSuiteCategories()).Returns(new List<SuiteCategory> {
                    new SuiteCategory { CategoryName = "suite category 1" }
                }.AsQueryable());
                var zigNetEntitiesReadOnlyMock = new Mock<IZigNetEntitiesReadOnly>();

                var suite = new ZigNetSuite
                {
                    Name = "new suite",
                    Categories = new List<ZigNetSuiteCategory> {
                        new ZigNetSuiteCategory { Name = "suite category 1" }
                    }
                };

                var zigNetEntityFrameworkDatabase = new ZigNetEntityFrameworkDatabase(zigNetEntitiesWriterMock.Object, zigNetEntitiesReadOnlyMock.Object);
                var suiteId = zigNetEntityFrameworkDatabase.SaveSuite(suite);

                Assert.AreEqual(1, suiteId);
            }

            [TestMethod]
            public void SavesNewSuiteWithNewAndExistingCategories()
            {
                var zigNetEntitiesWriterMock = new Mock<IZigNetEntitiesWriter>();
                zigNetEntitiesWriterMock.Setup(zewm => zewm.SaveSuite(It.IsAny<Suite>())).Returns(1);
                zigNetEntitiesWriterMock.Setup(zewm => zewm.GetSuiteCategories()).Returns(new List<SuiteCategory> {
                    new SuiteCategory { CategoryName = "suite category 1" },
                    new SuiteCategory { CategoryName = "suite category 2" },
                }.AsQueryable());
                var zigNetEntitiesReadOnlyMock = new Mock<IZigNetEntitiesReadOnly>();

                var suite = new ZigNetSuite
                {
                    Name = "new suite",
                    Categories = new List<ZigNetSuiteCategory> {
                        new ZigNetSuiteCategory { Name = "suite category 1" },
                        new ZigNetSuiteCategory { Name = "suite category 3" }
                    }
                };

                var zigNetEntityFrameworkDatabase = new ZigNetEntityFrameworkDatabase(zigNetEntitiesWriterMock.Object, zigNetEntitiesReadOnlyMock.Object);
                var suiteId = zigNetEntityFrameworkDatabase.SaveSuite(suite);

                Assert.AreEqual(1, suiteId);
            }

            [TestMethod]
            public void SavesExistingSuiteWithExistingCategory()
            {
                var zigNetEntitiesWriterMock = new Mock<IZigNetEntitiesWriter>();
                zigNetEntitiesWriterMock.Setup(zewm => zewm.GetSuite(1)).Returns(new Suite { SuiteID = 1 });
                zigNetEntitiesWriterMock.Setup(zewm => zewm.GetSuiteCategories()).Returns(new List<SuiteCategory> {
                    new SuiteCategory { SuiteCategoryID = 1, CategoryName = "suite category 1" }
                }.AsQueryable());
                zigNetEntitiesWriterMock.Setup(zewm => zewm.SaveSuite(It.IsAny<Suite>())).Returns(1);
                var zigNetEntitiesReadOnlyMock = new Mock<IZigNetEntitiesReadOnly>();

                var suite = new ZigNetSuite
                {
                    SuiteID = 1,
                    Name = "existing suite",
                    Categories = new List<ZigNetSuiteCategory> {
                        new ZigNetSuiteCategory { Name = "suite category 1" }
                    }
                };

                var zigNetEntityFrameworkDatabase = new ZigNetEntityFrameworkDatabase(zigNetEntitiesWriterMock.Object, zigNetEntitiesReadOnlyMock.Object);
                var suiteId = zigNetEntityFrameworkDatabase.SaveSuite(suite);

                Assert.AreEqual(1, suiteId);
            }

            [TestMethod]
            public void SavesExistingSuiteWithNewCategory()
            {
                var zigNetEntitiesWriterMock = new Mock<IZigNetEntitiesWriter>();
                zigNetEntitiesWriterMock.Setup(zewm => zewm.GetSuite(1)).Returns(new Suite { SuiteID = 1 });
                zigNetEntitiesWriterMock.Setup(zewm => zewm.GetSuiteCategories()).Returns(new List<SuiteCategory> {
                    new SuiteCategory { SuiteCategoryID = 1, CategoryName = "suite category 1" }
                }.AsQueryable());
                zigNetEntitiesWriterMock.Setup(zewm => zewm.SaveSuite(It.IsAny<Suite>())).Returns(1);
                var zigNetEntitiesReadOnlyMock = new Mock<IZigNetEntitiesReadOnly>();

                var suite = new ZigNetSuite
                {
                    SuiteID = 1,
                    Name = "existing suite",
                    Categories = new List<ZigNetSuiteCategory> {
                        new ZigNetSuiteCategory { Name = "suite category 2" }
                    }
                };

                var zigNetEntityFrameworkDatabase = new ZigNetEntityFrameworkDatabase(zigNetEntitiesWriterMock.Object, zigNetEntitiesReadOnlyMock.Object);
                var suiteId = zigNetEntityFrameworkDatabase.SaveSuite(suite);

                Assert.AreEqual(1, suiteId);
            }
        }

        [TestClass]
        public class SaveTestResultMethod
        {
            [TestMethod]
            public void AssignsTestIdWhenTestWithSameNameExists()
            {
                var zigNetEntitiesReadOnlyMock = new Mock<IZigNetEntitiesReadOnly>();
                zigNetEntitiesReadOnlyMock.Setup(zerom => zerom.GetMappedTestWithCategoriesOrDefault("test 1"))
                    .Returns(new ZigNetTest { Name = "test 1", TestID = 1, Categories = new List<ZigNetTestCategory>() });
                zigNetEntitiesReadOnlyMock.Setup(zewm => zewm.GetSuiteResult(2)).Returns(new SuiteResult { SuiteId = 2 });
                zigNetEntitiesReadOnlyMock.Setup(zewm => zewm.GetSuite(2)).Returns(new Suite { SuiteID = 2 });

                var zigNetEntitiesWriterMock = new Mock<IZigNetEntitiesWriter>();
                zigNetEntitiesWriterMock.Setup(zewm => zewm.GetTestWithSuites(1)).Returns(
                    new Test
                    {
                        TestID = 1,
                        TestCategories = new List<TestCategory>(),
                        Suites = new List<Suite> { new Suite { SuiteID = 2 } }
                    }
                );
                zigNetEntitiesWriterMock.Setup(zewm => zewm.GetLatestTestResults()).Returns(
                    new List<LatestTestResult> { 
                        new LatestTestResult { SuiteId = 2, TestId = 1, FailingFromDateTime = DateTime.UtcNow }
                    }.AsQueryable);
                zigNetEntitiesWriterMock.Setup(zewm => zewm.GetTestFailureDurations()).Returns(
                    new List<TestFailureDuration> {
                        new TestFailureDuration { SuiteId = 2, TestId = 1 }
                    }.AsQueryable);
                zigNetEntitiesWriterMock.Setup(zewm => zewm.SaveTestResult(It.IsAny<TestResult>())).Returns(
                    new TestResult { TestResultID = 3 });


                var testResult = new ZigNetTestResult
                {
                    Test = new ZigNetTest
                    {
                        Name = "test 1",
                        Categories = new List<ZigNetTestCategory>()
                    },
                    SuiteResult = new ZigNetSuiteResult { SuiteResultID = 2 }
                };

                var zigNetEntityFrameworkDatabase = new ZigNetEntityFrameworkDatabase(zigNetEntitiesWriterMock.Object, zigNetEntitiesReadOnlyMock.Object);
                zigNetEntityFrameworkDatabase.SaveTestResult(testResult);
            }

            [TestMethod]
            public void DoesNotAssignsTestIdWhenTestWithSameNameDoesNotExist()
            {
                var zigNetEntitiesReadOnlyMock = new Mock<IZigNetEntitiesReadOnly>();
                zigNetEntitiesReadOnlyMock.Setup(zewm => zewm.GetSuiteResult(2)).Returns(new SuiteResult { SuiteId = 2 });
                zigNetEntitiesReadOnlyMock.Setup(zewm => zewm.GetSuite(2)).Returns(new Suite { SuiteID = 2 });

                var zigNetEntitiesWriterMock = new Mock<IZigNetEntitiesWriter>();
                zigNetEntitiesWriterMock.Setup(zewm => zewm.SaveTestResult(It.IsAny<TestResult>()))
                    .Returns(new TestResult { Test = new Test { TestID = 3 } });

                var testResult = new ZigNetTestResult
                {
                    Test = new ZigNetTest
                    {
                        Name = "test 1",
                        Categories = new List<ZigNetTestCategory>()
                    },
                    SuiteResult = new ZigNetSuiteResult { SuiteResultID = 2 }
                };

                var zigNetEntityFrameworkDatabase = new ZigNetEntityFrameworkDatabase(zigNetEntitiesWriterMock.Object, zigNetEntitiesReadOnlyMock.Object);
                zigNetEntityFrameworkDatabase.SaveTestResult(testResult);
            }

            [TestMethod]
            [ExpectedException(typeof(NullReferenceException))]
            public void ThrowsIfSuiteResultDoesNotExist()
            {
                var zigNetEntitiesReadOnlyMock = new Mock<IZigNetEntitiesReadOnly>();
                var zigNetEntitiesWriterMock = new Mock<IZigNetEntitiesWriter>();

                var testResult = new ZigNetTestResult
                {
                    Test = new ZigNetTest
                    {
                        Name = "test 1",
                        Categories = new List<ZigNetTestCategory>()
                    },
                    SuiteResult = new ZigNetSuiteResult { SuiteResultID = 2 }
                };

                var zigNetEntityFrameworkDatabase = new ZigNetEntityFrameworkDatabase(zigNetEntitiesWriterMock.Object, zigNetEntitiesReadOnlyMock.Object);
                zigNetEntityFrameworkDatabase.SaveTestResult(testResult);
            }

            [TestMethod]
            public void CopiesExistingTestCategories()
            {
                var zigNetEntitiesReadOnlyMock = new Mock<IZigNetEntitiesReadOnly>();
                zigNetEntitiesReadOnlyMock.Setup(zerom => zerom.GetMappedTestWithCategoriesOrDefault("test 1")).Returns(
                    new ZigNetTest
                    {
                        Name = "test 1",
                        TestID = 1,
                        Categories = new List<ZigNetTestCategory> {
                            new ZigNetTestCategory { TestCategoryID = 1, Name = "test category 1" } }
                    });
                zigNetEntitiesReadOnlyMock.Setup(zewm => zewm.GetSuiteResult(2)).Returns(new SuiteResult { SuiteId = 2 });
                zigNetEntitiesReadOnlyMock.Setup(zewm => zewm.GetSuite(2)).Returns(new Suite { SuiteID = 2 });

                var zigNetEntitiesWriterMock = new Mock<IZigNetEntitiesWriter>();
                zigNetEntitiesWriterMock.Setup(zewm => zewm.GetTestWithSuites(1)).Returns(
                    new Test
                    {
                        TestID = 1,
                        TestCategories = new List<TestCategory>(),
                        Suites = new List<Suite> { new Suite { SuiteID = 2 } }
                    }
                );
                zigNetEntitiesWriterMock.Setup(zewm => zewm.GetLatestTestResults()).Returns(
                    new List<LatestTestResult> { 
                        new LatestTestResult { SuiteId = 2, TestId = 1, FailingFromDateTime = DateTime.UtcNow }
                    }.AsQueryable);
                zigNetEntitiesWriterMock.Setup(zewm => zewm.GetTestFailureDurations()).Returns(
                    new List<TestFailureDuration> {
                        new TestFailureDuration { SuiteId = 2, TestId = 1 }
                    }.AsQueryable);
                zigNetEntitiesWriterMock.Setup(zewm => zewm.SaveTestResult(It.IsAny<TestResult>())).Returns(
                    new TestResult { TestResultID = 3 });

                var testResult = new ZigNetTestResult
                {
                    Test = new ZigNetTest
                    {
                        Name = "test 1",
                        Categories = new List<ZigNetTestCategory>()
                    },
                    SuiteResult = new ZigNetSuiteResult { SuiteResultID = 2 }
                };

                var zigNetEntityFrameworkDatabase = new ZigNetEntityFrameworkDatabase(zigNetEntitiesWriterMock.Object, zigNetEntitiesReadOnlyMock.Object);
                zigNetEntityFrameworkDatabase.SaveTestResult(testResult);
            }

            [TestMethod]
            public void MergesNewAndExistingTestCategories()
            {
                var zigNetEntitiesReadOnlyMock = new Mock<IZigNetEntitiesReadOnly>();
                zigNetEntitiesReadOnlyMock.Setup(zerom => zerom.GetMappedTestWithCategoriesOrDefault("test 1")).Returns(
                    new ZigNetTest
                    {
                        Name = "test 1",
                        TestID = 1,
                        Categories = new List<ZigNetTestCategory> {
                            new ZigNetTestCategory { TestCategoryID = 1, Name = "test category 1" } }
                    });
                zigNetEntitiesReadOnlyMock.Setup(zewm => zewm.GetSuiteResult(2)).Returns(new SuiteResult { SuiteId = 2 });
                zigNetEntitiesReadOnlyMock.Setup(zewm => zewm.GetSuite(2)).Returns(new Suite { SuiteID = 2 });

                var zigNetEntitiesWriterMock = new Mock<IZigNetEntitiesWriter>();
                zigNetEntitiesWriterMock.Setup(zewm => zewm.GetTestWithSuites(1)).Returns(
                    new Test
                    {
                        TestID = 1,
                        TestCategories = new List<TestCategory>(),
                        Suites = new List<Suite> { new Suite { SuiteID = 2 } }
                    }
                );
                zigNetEntitiesWriterMock.Setup(zewm => zewm.GetLatestTestResults()).Returns(
                    new List<LatestTestResult> { 
                        new LatestTestResult { SuiteId = 2, TestId = 1, FailingFromDateTime = DateTime.UtcNow }
                    }.AsQueryable);
                zigNetEntitiesWriterMock.Setup(zewm => zewm.GetTestFailureDurations()).Returns(
                    new List<TestFailureDuration> {
                        new TestFailureDuration { SuiteId = 2, TestId = 1 }
                    }.AsQueryable);
                zigNetEntitiesWriterMock.Setup(zewm => zewm.SaveTestResult(It.IsAny<TestResult>())).Returns(
                    new TestResult { TestResultID = 3 });

                var testResult = new ZigNetTestResult
                {
                    Test = new ZigNetTest
                    {
                        Name = "test 1",
                        Categories = new List<ZigNetTestCategory> { new ZigNetTestCategory { Name = "test category 2" } }
                    },
                    SuiteResult = new ZigNetSuiteResult { SuiteResultID = 2 }
                };

                var zigNetEntityFrameworkDatabase = new ZigNetEntityFrameworkDatabase(zigNetEntitiesWriterMock.Object, zigNetEntitiesReadOnlyMock.Object);
                zigNetEntityFrameworkDatabase.SaveTestResult(testResult);
            }

            [TestMethod]
            public void SavesFailedTestResultWithDetails()
            {
                var zigNetEntitiesReadOnlyMock = new Mock<IZigNetEntitiesReadOnly>();
                zigNetEntitiesReadOnlyMock.Setup(zerom => zerom.GetMappedTestWithCategoriesOrDefault("test 1"))
                    .Returns(new ZigNetTest { Name = "test 1", TestID = 3, Categories = new List<ZigNetTestCategory>() });
                zigNetEntitiesReadOnlyMock.Setup(zewm => zewm.GetSuiteResult(2)).Returns(new SuiteResult { SuiteId = 2 });
                zigNetEntitiesReadOnlyMock.Setup(zewm => zewm.GetSuite(2)).Returns(new Suite { SuiteID = 2 });

                var zigNetEntitiesWriterMock = new Mock<IZigNetEntitiesWriter>();
                zigNetEntitiesWriterMock.Setup(zewm => zewm.GetTestFailureType(2)).Returns(
                    new TestFailureType
                    {
                        TestFailureTypeID = 2,
                        TestFailureTypeName = "Exception"
                    }
                );
                zigNetEntitiesWriterMock.Setup(zewm => zewm.GetTestWithSuites(3)).Returns(
                    new Test
                    {
                        TestID = 3,
                        TestCategories = new List<TestCategory>(),
                        Suites = new List<Suite> { new Suite { SuiteID = 2 } }
                    }
                );
                zigNetEntitiesWriterMock.Setup(zewm => zewm.GetSuite(2)).Returns(new Suite { SuiteID = 2 });
                zigNetEntitiesWriterMock.Setup(zewm => zewm.GetLatestTestResults()).Returns(
                    new List<LatestTestResult> { 
                        new LatestTestResult { SuiteId = 2, TestId = 3, FailingFromDateTime = DateTime.UtcNow }
                    }.AsQueryable);
                zigNetEntitiesWriterMock.Setup(zewm => zewm.GetTestFailureDurations()).Returns(
                    new List<TestFailureDuration> {
                        new TestFailureDuration { SuiteId = 2, TestId = 3 }
                    }.AsQueryable);
                zigNetEntitiesWriterMock.Setup(zewm => zewm.SaveTestResult(It.IsAny<TestResult>())).Returns(
                    new TestResult { TestResultID = 3 });

                var testResult = new ZigNetTestResult
                {
                    Test = new ZigNetTest { Name = "test 1", Categories = new List<ZigNetTestCategory> { new ZigNetTestCategory { Name = "test category 1" } } },
                    SuiteResult = new ZigNetSuiteResult { SuiteResultID = 2 },
                    StartTime = DateTime.UtcNow,
                    EndTime = DateTime.UtcNow,
                    ResultType = ZigNetTestResultType.Fail,
                    TestFailureDetails = new ZigNetTestFailureDetails
                    {
                        FailureType = ZigNetTestFailureType.Exception,
                        FailureDetailMessage = "failed by exception at line 5"
                    }
                };

                var zigNetEntityFrameworkDatabase = new ZigNetEntityFrameworkDatabase(zigNetEntitiesWriterMock.Object, zigNetEntitiesReadOnlyMock.Object);
                zigNetEntityFrameworkDatabase.SaveTestResult(testResult);
            }

            [TestMethod]
            public void SavesExistingTestPassedTestResult()
            {
                var zigNetEntitiesReadOnlyMock = new Mock<IZigNetEntitiesReadOnly>();
                zigNetEntitiesReadOnlyMock.Setup(zerom => zerom.GetMappedTestWithCategoriesOrDefault("test 1"))
                    .Returns(new ZigNetTest { Name = "test 1", TestID = 1, Categories = new List<ZigNetTestCategory>() });
                zigNetEntitiesReadOnlyMock.Setup(zewm => zewm.GetSuiteResult(2)).Returns(new SuiteResult { SuiteId = 2 });
                zigNetEntitiesReadOnlyMock.Setup(zewm => zewm.GetSuite(2)).Returns(new Suite { SuiteID = 2 });

                var zigNetEntitiesWriterMock = new Mock<IZigNetEntitiesWriter>();
                zigNetEntitiesWriterMock.Setup(zewm => zewm.GetTestWithSuites(1)).Returns(
                    new Test
                    {
                        TestID = 1,
                        TestCategories = new List<TestCategory>(),
                        Suites = new List<Suite> { new Suite { SuiteID = 2 } }
                    }
                );
                zigNetEntitiesWriterMock.Setup(zewm => zewm.GetSuite(2)).Returns(new Suite { SuiteID = 2 });
                zigNetEntitiesWriterMock.Setup(zewm => zewm.GetLatestTestResults()).Returns(
                    new List<LatestTestResult> { 
                        new LatestTestResult { SuiteId = 2, TestId = 1, PassingFromDateTime = DateTime.UtcNow }
                    }.AsQueryable);
                zigNetEntitiesWriterMock.Setup(zewm => zewm.SaveTestResult(It.IsAny<TestResult>())).Returns(
                    new TestResult { TestResultID = 3 });

                var testResult = new ZigNetTestResult
                {
                    Test = new ZigNetTest
                    {
                        TestID = 1,
                        Name = "test 1",
                        Categories = new List<ZigNetTestCategory> { new ZigNetTestCategory { Name = "test category 1" } }
                    },
                    SuiteResult = new ZigNetSuiteResult { SuiteResultID = 2 },
                    StartTime = DateTime.UtcNow,
                    EndTime = DateTime.UtcNow,
                    ResultType = ZigNetTestResultType.Pass,
                };

                var zigNetEntityFrameworkDatabase = new ZigNetEntityFrameworkDatabase(zigNetEntitiesWriterMock.Object, zigNetEntitiesReadOnlyMock.Object);
                zigNetEntityFrameworkDatabase.SaveTestResult(testResult);
            }

            [TestMethod]
            public void SavesExistingTestFailedTestResult()
            {
                var zigNetEntitiesReadOnlyMock = new Mock<IZigNetEntitiesReadOnly>();
                zigNetEntitiesReadOnlyMock.Setup(zerom => zerom.GetMappedTestWithCategoriesOrDefault("test 1"))
                    .Returns(new ZigNetTest { Name = "test 1", TestID = 1, Categories = new List<ZigNetTestCategory>() });
                zigNetEntitiesReadOnlyMock.Setup(zewm => zewm.GetSuiteResult(2)).Returns(new SuiteResult { SuiteId = 2 });
                zigNetEntitiesReadOnlyMock.Setup(zewm => zewm.GetSuite(2)).Returns(new Suite { SuiteID = 2 });

                var zigNetEntitiesWriterMock = new Mock<IZigNetEntitiesWriter>();
                zigNetEntitiesWriterMock.Setup(zewm => zewm.GetTestWithSuites(1)).Returns(
                    new Test
                    {
                        TestID = 1,
                        TestCategories = new List<TestCategory> { new TestCategory { TestCategoryID = 1, CategoryName = "test category 1" } },
                        Suites = new List<Suite> { new Suite { SuiteID = 2 } }
                    }
                );
                zigNetEntitiesWriterMock.Setup(zewm => zewm.GetTestFailureType(1)).Returns(
                    new TestFailureType
                    {
                        TestFailureTypeID = 1,
                        TestFailureTypeName = "Assertion"
                    }
                );
                zigNetEntitiesWriterMock.Setup(zewm => zewm.GetSuite(2)).Returns(new Suite { SuiteID = 2 });
                zigNetEntitiesWriterMock.Setup(zewm => zewm.GetLatestTestResults()).Returns(
                    new List<LatestTestResult> { 
                        new LatestTestResult { SuiteId = 2, TestId = 1, FailingFromDateTime = DateTime.UtcNow }
                    }.AsQueryable);
                zigNetEntitiesWriterMock.Setup(zewm => zewm.GetTestFailureDurations()).Returns(
                    new List<TestFailureDuration> {
                        new TestFailureDuration { SuiteId = 2, TestId = 1 }
                    }.AsQueryable);
                zigNetEntitiesWriterMock.Setup(zewm => zewm.SaveTestResult(It.IsAny<TestResult>())).Returns(
                    new TestResult { TestResultID = 3 });

                var testResult = new ZigNetTestResult
                {
                    Test = new ZigNetTest
                    {
                        TestID = 1,
                        Name = "test 1",
                        Categories = new List<ZigNetTestCategory> {
                            new ZigNetTestCategory { Name = "test category 1" },
                            new ZigNetTestCategory { Name = "test category 2" }
                        }
                    },
                    SuiteResult = new ZigNetSuiteResult { SuiteResultID = 2 },
                    StartTime = DateTime.UtcNow,
                    EndTime = DateTime.UtcNow,
                    ResultType = ZigNetTestResultType.Fail,
                    TestFailureDetails = new ZigNetTestFailureDetails
                    {
                        FailureType = ZigNetTestFailureType.Assertion
                    }
                };

                var zigNetEntityFrameworkDatabase = new ZigNetEntityFrameworkDatabase(zigNetEntitiesWriterMock.Object, zigNetEntitiesReadOnlyMock.Object);
                zigNetEntityFrameworkDatabase.SaveTestResult(testResult);
            }

            [TestMethod]
            public void DoesNotThrowWhenClearingAllTestCategories()
            {
                var zigNetEntitiesReadOnlyMock = new Mock<IZigNetEntitiesReadOnly>();
                zigNetEntitiesReadOnlyMock.Setup(zerom => zerom.GetMappedTestWithCategoriesOrDefault("test 1"))
                    .Returns(new ZigNetTest { Name = "test 1", TestID = 1, Categories = new List<ZigNetTestCategory>() });
                zigNetEntitiesReadOnlyMock.Setup(zewm => zewm.GetSuiteResult(2)).Returns(new SuiteResult { SuiteId = 2 });
                zigNetEntitiesReadOnlyMock.Setup(zewm => zewm.GetSuite(2)).Returns(new Suite { SuiteID = 2 });

                var zigNetEntitiesWriterMock = new Mock<IZigNetEntitiesWriter>();
                zigNetEntitiesWriterMock.Setup(zewm => zewm.GetTestWithSuites(1)).Returns(
                    new Test
                    {
                        TestID = 1,
                        TestCategories = new List<TestCategory> { new TestCategory { TestCategoryID = 1, CategoryName = "test category 1" } },
                        Suites = new List<Suite> { new Suite { SuiteID = 2 } }
                    }
                );
                zigNetEntitiesWriterMock.Setup(zewm => zewm.GetSuite(2)).Returns(new Suite { SuiteID = 2 });
                zigNetEntitiesWriterMock.Setup(zewm => zewm.GetLatestTestResults()).Returns(
                    new List<LatestTestResult> { 
                        new LatestTestResult { SuiteId = 2, TestId = 1, PassingFromDateTime = DateTime.UtcNow }
                    }.AsQueryable);
                zigNetEntitiesWriterMock.Setup(zewm => zewm.SaveTestResult(It.IsAny<TestResult>())).Returns(
                    new TestResult { TestResultID = 3 });

                var testResult = new ZigNetTestResult
                {
                    Test = new ZigNetTest
                    {
                        TestID = 1,
                        Name = "test 1",
                        Categories = new List<ZigNetTestCategory>()
                    },
                    SuiteResult = new ZigNetSuiteResult { SuiteResultID = 2 },
                    StartTime = DateTime.UtcNow,
                    EndTime = DateTime.UtcNow,
                    ResultType = ZigNetTestResultType.Pass
                };

                var zigNetEntityFrameworkDatabase = new ZigNetEntityFrameworkDatabase(zigNetEntitiesWriterMock.Object, zigNetEntitiesReadOnlyMock.Object);
                zigNetEntityFrameworkDatabase.SaveTestResult(testResult);
            }

            [TestMethod]
            public void CreatesNewLatestTestResult()
            {
                var zigNetEntitiesReadOnlyMock = new Mock<IZigNetEntitiesReadOnly>();
                zigNetEntitiesReadOnlyMock.Setup(zewm => zewm.GetSuiteResult(2)).Returns(new SuiteResult { SuiteId = 2 });
                zigNetEntitiesReadOnlyMock.Setup(zewm => zewm.GetSuite(2)).Returns(new Suite { SuiteID = 2 });

                var zigNetEntitiesWriterMock = new Mock<IZigNetEntitiesWriter>();
                zigNetEntitiesWriterMock.Setup(zewm => zewm.SaveTestResult(It.IsAny<TestResult>()))
                    .Returns(new TestResult
                    {
                        TestResultID = 3,
                        Test = new Test { TestID = 4 }
                    });
                zigNetEntitiesWriterMock.Setup(zewm => zewm.GetSuite(2)).Returns(new Suite { SuiteID = 2 });
                zigNetEntitiesWriterMock.Setup(zewm => zewm.GetLatestTestResults()).Returns(new List<LatestTestResult>().AsQueryable);

                var testResult = new ZigNetTestResult
                {
                    Test = new ZigNetTest
                    {
                        TestID = 0,
                        Name = "test 1",
                        Categories = new List<ZigNetTestCategory>()
                    },
                    SuiteResult = new ZigNetSuiteResult { SuiteResultID = 2 },
                    ResultType = ZigNetTestResultType.Fail,
                    TestFailureDetails = new ZigNetTestFailureDetails()
                };

                var zigNetEntityFrameworkDatabase = new ZigNetEntityFrameworkDatabase(zigNetEntitiesWriterMock.Object, zigNetEntitiesReadOnlyMock.Object);
                zigNetEntityFrameworkDatabase.SaveTestResult(testResult);
            }

            [TestMethod]
            public void CreatesNewLatestTestResultWhenSuiteIdDoesNotMatch()
            {
                var zigNetEntitiesReadOnlyMock = new Mock<IZigNetEntitiesReadOnly>();
                zigNetEntitiesReadOnlyMock.Setup(zewm => zewm.GetSuiteResult(2)).Returns(new SuiteResult { SuiteId = 2 });
                zigNetEntitiesReadOnlyMock.Setup(zewm => zewm.GetSuite(2)).Returns(new Suite { SuiteID = 2 });

                var zigNetEntitiesWriterMock = new Mock<IZigNetEntitiesWriter>();
                zigNetEntitiesWriterMock.Setup(zewm => zewm.SaveTestResult(It.IsAny<TestResult>()))
                    .Returns(new TestResult
                    {
                        TestResultID = 3,
                        Test = new Test { TestID = 4 }
                    });
                zigNetEntitiesWriterMock.Setup(zewm => zewm.GetLatestTestResults())
                    .Returns(new List<LatestTestResult>{
                        new LatestTestResult { SuiteId = 1, TestId = 4 }
                    }.AsQueryable);
                zigNetEntitiesWriterMock.Setup(zewm => zewm.GetTestWithSuites(4)).Returns(
                    new Test
                    {
                        TestID = 4,
                        TestCategories = new List<TestCategory>()
                    }
                );

                var testResult = new ZigNetTestResult
                {
                    Test = new ZigNetTest
                    {
                        TestID = 4,
                        Name = "test 1",
                        Categories = new List<ZigNetTestCategory>()
                    },
                    SuiteResult = new ZigNetSuiteResult { SuiteResultID = 2 },
                    ResultType = ZigNetTestResultType.Fail,
                    TestFailureDetails = new ZigNetTestFailureDetails()
                };

                var zigNetEntityFrameworkDatabase = new ZigNetEntityFrameworkDatabase(zigNetEntitiesWriterMock.Object, zigNetEntitiesReadOnlyMock.Object);
                zigNetEntityFrameworkDatabase.SaveTestResult(testResult);
            }

            [TestMethod]
            public void CreatesNewLatestTestResultWhenTestIdDoesNotMatch()
            {
                var zigNetEntitiesReadOnlyMock = new Mock<IZigNetEntitiesReadOnly>();
                zigNetEntitiesReadOnlyMock.Setup(zewm => zewm.GetSuiteResult(2)).Returns(new SuiteResult { SuiteId = 2 });
                zigNetEntitiesReadOnlyMock.Setup(zewm => zewm.GetSuite(2)).Returns(new Suite { SuiteID = 2 });

                var zigNetEntitiesWriterMock = new Mock<IZigNetEntitiesWriter>();
                zigNetEntitiesWriterMock.Setup(zewm => zewm.SaveTestResult(It.IsAny<TestResult>()))
                    .Returns(new TestResult
                    {
                        TestResultID = 3,
                        Test = new Test { TestID = 4 }
                    });
                zigNetEntitiesWriterMock.Setup(zewm => zewm.GetLatestTestResults())
                    .Returns(new List<LatestTestResult>{
                        new LatestTestResult { SuiteId = 2, TestId = 3 }
                    }.AsQueryable);
                zigNetEntitiesWriterMock.Setup(zewm => zewm.GetTestWithSuites(4)).Returns(
                    new Test
                    {
                        TestID = 4,
                        TestCategories = new List<TestCategory>()
                    }
                );

                var testResult = new ZigNetTestResult
                {
                    Test = new ZigNetTest
                    {
                        TestID = 4,
                        Name = "test 1",
                        Categories = new List<ZigNetTestCategory>()
                    },
                    SuiteResult = new ZigNetSuiteResult { SuiteResultID = 2 },
                    ResultType = ZigNetTestResultType.Fail,
                    TestFailureDetails = new ZigNetTestFailureDetails()
                };

                var zigNetEntityFrameworkDatabase = new ZigNetEntityFrameworkDatabase(zigNetEntitiesWriterMock.Object, zigNetEntitiesReadOnlyMock.Object);
                zigNetEntityFrameworkDatabase.SaveTestResult(testResult);
            }

            [TestMethod]
            public void UpdatesExistingLatestTestResult()
            {
                var zigNetEntitiesReadOnlyMock = new Mock<IZigNetEntitiesReadOnly>();
                zigNetEntitiesReadOnlyMock.Setup(zerom => zerom.GetMappedTestWithCategoriesOrDefault("test 1"))
                    .Returns(new ZigNetTest { Name = "test 1", TestID = 1, Categories = new List<ZigNetTestCategory>() });
                zigNetEntitiesReadOnlyMock.Setup(zewm => zewm.GetSuiteResult(2)).Returns(new SuiteResult { SuiteId = 2 });
                zigNetEntitiesReadOnlyMock.Setup(zewm => zewm.GetSuite(2)).Returns(new Suite { SuiteID = 2 });

                var zigNetEntitiesWriterMock = new Mock<IZigNetEntitiesWriter>();
                zigNetEntitiesWriterMock.Setup(zewm => zewm.GetTestWithSuites(1)).Returns(
                    new Test
                    {
                        TestID = 1,
                        TestCategories = new List<TestCategory>(),
                        Suites = new List<Suite> { new Suite { SuiteID = 2 } }
                    }
                );
                zigNetEntitiesWriterMock.Setup(zewm => zewm.SaveTestResult(It.IsAny<TestResult>()))
                    .Returns(new TestResult { Test = new Test { TestID = 1 } });
                zigNetEntitiesWriterMock.Setup(zewm => zewm.GetLatestTestResults())
                    .Returns(new List<LatestTestResult>{
                        new LatestTestResult { SuiteId = 2, TestId = 1 }
                    }.AsQueryable);

                var testResult = new ZigNetTestResult
                {
                    Test = new ZigNetTest
                    {
                        TestID = 1,
                        Name = "test 1",
                        Categories = new List<ZigNetTestCategory>()
                    },
                    SuiteResult = new ZigNetSuiteResult { SuiteResultID = 2 },
                    ResultType = ZigNetTestResultType.Fail,
                    TestFailureDetails = new ZigNetTestFailureDetails()
                };

                var zigNetEntityFrameworkDatabase = new ZigNetEntityFrameworkDatabase(zigNetEntitiesWriterMock.Object, zigNetEntitiesReadOnlyMock.Object);
                zigNetEntityFrameworkDatabase.SaveTestResult(testResult);
            }

            [TestMethod]
            public void AssignsSuiteNameToNewLatestTestResult()
            {
                var zigNetEntitiesReadOnlyMock = new Mock<IZigNetEntitiesReadOnly>();
                zigNetEntitiesReadOnlyMock.Setup(zewm => zewm.GetSuiteResult(2)).Returns(new SuiteResult { SuiteId = 2 });
                zigNetEntitiesReadOnlyMock.Setup(zewm => zewm.GetSuite(2)).Returns(new Suite { SuiteID = 2, SuiteName = "services" });

                var zigNetEntitiesWriterMock = new Mock<IZigNetEntitiesWriter>();
                zigNetEntitiesWriterMock.Setup(zewm => zewm.SaveTestResult(It.IsAny<TestResult>()))
                    .Returns(new TestResult
                    {
                        TestResultID = 3,
                        Test = new Test { TestID = 4 }
                    });
                zigNetEntitiesWriterMock.Setup(zewm => zewm.GetSuite(2)).Returns(new Suite { SuiteID = 2 });
                zigNetEntitiesWriterMock.Setup(zewm => zewm.GetLatestTestResults()).Returns(new List<LatestTestResult>().AsQueryable);

                var testResult = new ZigNetTestResult
                {
                    Test = new ZigNetTest
                    {
                        TestID = 0,
                        Name = "test 1",
                        Categories = new List<ZigNetTestCategory>()
                    },
                    SuiteResult = new ZigNetSuiteResult { SuiteResultID = 2 },
                    ResultType = ZigNetTestResultType.Fail,
                    TestFailureDetails = new ZigNetTestFailureDetails()
                };

                var zigNetEntityFrameworkDatabase = new ZigNetEntityFrameworkDatabase(zigNetEntitiesWriterMock.Object, zigNetEntitiesReadOnlyMock.Object);
                zigNetEntityFrameworkDatabase.SaveTestResult(testResult);
            }

            [TestMethod]
            public void UpdatesLatestTestResultSuiteNameIfDoesNotMatch()
            {
                var zigNetEntitiesReadOnlyMock = new Mock<IZigNetEntitiesReadOnly>();
                zigNetEntitiesReadOnlyMock.Setup(zerom => zerom.GetMappedTestWithCategoriesOrDefault("test 1"))
                    .Returns(new ZigNetTest { Name = "test 1", TestID = 1, Categories = new List<ZigNetTestCategory>() });
                zigNetEntitiesReadOnlyMock.Setup(zewm => zewm.GetSuiteResult(2)).Returns(new SuiteResult { SuiteId = 2 });
                zigNetEntitiesReadOnlyMock.Setup(zewm => zewm.GetSuite(2)).Returns(new Suite { SuiteID = 2, SuiteName = "new-name" });

                var zigNetEntitiesWriterMock = new Mock<IZigNetEntitiesWriter>();
                zigNetEntitiesWriterMock.Setup(zewm => zewm.GetTestWithSuites(1)).Returns(
                    new Test
                    {
                        TestID = 1,
                        TestCategories = new List<TestCategory>(),
                        Suites = new List<Suite> { new Suite { SuiteID = 2 } }
                    }
                );
                zigNetEntitiesWriterMock.Setup(zewm => zewm.SaveTestResult(It.IsAny<TestResult>()))
                    .Returns(new TestResult { Test = new Test { TestID = 1 } });
                zigNetEntitiesWriterMock.Setup(zewm => zewm.GetLatestTestResults())
                    .Returns(new List<LatestTestResult>{
                        new LatestTestResult { SuiteId = 2, TestId = 1, SuiteName = "old-name", PassingFromDateTime = DateTime.Now }
                    }.AsQueryable);

                var testResult = new ZigNetTestResult
                {
                    Test = new ZigNetTest
                    {
                        TestID = 1,
                        Name = "test 1",
                        Categories = new List<ZigNetTestCategory>()
                    },
                    SuiteResult = new ZigNetSuiteResult { SuiteResultID = 2 },
                    ResultType = ZigNetTestResultType.Pass,
                    TestFailureDetails = new ZigNetTestFailureDetails()
                };

                var zigNetEntityFrameworkDatabase = new ZigNetEntityFrameworkDatabase(zigNetEntitiesWriterMock.Object, zigNetEntitiesReadOnlyMock.Object);
                zigNetEntityFrameworkDatabase.SaveTestResult(testResult);
            }

            [TestMethod]
            public void UpdatesPassingFromDate()
            {
                var zigNetEntitiesReadOnlyMock = new Mock<IZigNetEntitiesReadOnly>();
                zigNetEntitiesReadOnlyMock.Setup(zerom => zerom.GetMappedTestWithCategoriesOrDefault("test 1"))
                    .Returns(new ZigNetTest { Name = "test 1", TestID = 1, Categories = new List<ZigNetTestCategory>() });
                zigNetEntitiesReadOnlyMock.Setup(zewm => zewm.GetSuiteResult(2)).Returns(new SuiteResult { SuiteId = 2 });
                zigNetEntitiesReadOnlyMock.Setup(zewm => zewm.GetSuite(2)).Returns(new Suite { SuiteID = 2 });

                var zigNetEntitiesWriterMock = new Mock<IZigNetEntitiesWriter>();
                zigNetEntitiesWriterMock.Setup(zewm => zewm.GetTestWithSuites(1)).Returns(
                    new Test
                    {
                        TestID = 1,
                        TestCategories = new List<TestCategory>(),
                        Suites = new List<Suite> { new Suite { SuiteID = 2 } }
                    }
                );
                zigNetEntitiesWriterMock.Setup(zewm => zewm.SaveTestResult(It.IsAny<TestResult>()))
                    .Returns(new TestResult { Test = new Test { TestID = 1 } });
                zigNetEntitiesWriterMock.Setup(zewm => zewm.GetLatestTestResults())
                    .Returns(new List<LatestTestResult>{
                        new LatestTestResult { SuiteId = 2, TestId = 1, FailingFromDateTime = DateTime.UtcNow }
                    }.AsQueryable);

                var testResult = new ZigNetTestResult
                {
                    Test = new ZigNetTest
                    {
                        TestID = 1,
                        Name = "test 1",
                        Categories = new List<ZigNetTestCategory>()
                    },
                    SuiteResult = new ZigNetSuiteResult { SuiteResultID = 2 },
                    ResultType = ZigNetTestResultType.Pass
                };

                var zigNetEntityFrameworkDatabase = new ZigNetEntityFrameworkDatabase(zigNetEntitiesWriterMock.Object, zigNetEntitiesReadOnlyMock.Object);
                zigNetEntityFrameworkDatabase.SaveTestResult(testResult);
            }

            [TestMethod]
            public void UpdatesFailingFromDate()
            {
                var zigNetEntitiesReadOnlyMock = new Mock<IZigNetEntitiesReadOnly>();
                zigNetEntitiesReadOnlyMock.Setup(zerom => zerom.GetMappedTestWithCategoriesOrDefault("test 1"))
                    .Returns(new ZigNetTest { Name = "test 1", TestID = 1, Categories = new List<ZigNetTestCategory>() });
                zigNetEntitiesReadOnlyMock.Setup(zewm => zewm.GetSuiteResult(2)).Returns(new SuiteResult { SuiteId = 2 });
                zigNetEntitiesReadOnlyMock.Setup(zewm => zewm.GetSuite(2)).Returns(new Suite { SuiteID = 2 });

                var zigNetEntitiesWriterMock = new Mock<IZigNetEntitiesWriter>();
                zigNetEntitiesWriterMock.Setup(zewm => zewm.GetTestWithSuites(1)).Returns(
                    new Test
                    {
                        TestID = 1,
                        TestCategories = new List<TestCategory>(),
                        Suites = new List<Suite> { new Suite { SuiteID = 2 } }
                    }
                );
                zigNetEntitiesWriterMock.Setup(zewm => zewm.SaveTestResult(It.IsAny<TestResult>()))
                    .Returns(new TestResult { Test = new Test { TestID = 1 } });
                zigNetEntitiesWriterMock.Setup(zewm => zewm.GetLatestTestResults())
                    .Returns(new List<LatestTestResult>{
                        new LatestTestResult { SuiteId = 2, TestId = 1, PassingFromDateTime = DateTime.UtcNow }
                    }.AsQueryable);

                var testResult = new ZigNetTestResult
                {
                    Test = new ZigNetTest
                    {
                        TestID = 1,
                        Name = "test 1",
                        Categories = new List<ZigNetTestCategory>()
                    },
                    SuiteResult = new ZigNetSuiteResult { SuiteResultID = 2 },
                    ResultType = ZigNetTestResultType.Fail,
                    TestFailureDetails = new ZigNetTestFailureDetails()
                };

                var zigNetEntityFrameworkDatabase = new ZigNetEntityFrameworkDatabase(zigNetEntitiesWriterMock.Object, zigNetEntitiesReadOnlyMock.Object);
                zigNetEntityFrameworkDatabase.SaveTestResult(testResult);
            }

            [TestMethod]
            public void IgnoresPassingFromDate()
            {
                var zigNetEntitiesReadOnlyMock = new Mock<IZigNetEntitiesReadOnly>();
                zigNetEntitiesReadOnlyMock.Setup(zerom => zerom.GetMappedTestWithCategoriesOrDefault("test 1"))
                    .Returns(new ZigNetTest { Name = "test 1", TestID = 1, Categories = new List<ZigNetTestCategory>() });
                zigNetEntitiesReadOnlyMock.Setup(zewm => zewm.GetSuiteResult(2)).Returns(new SuiteResult { SuiteId = 2 });
                zigNetEntitiesReadOnlyMock.Setup(zewm => zewm.GetSuite(2)).Returns(new Suite { SuiteID = 2 });

                var zigNetEntitiesWriterMock = new Mock<IZigNetEntitiesWriter>();
                zigNetEntitiesWriterMock.Setup(zewm => zewm.GetTestWithSuites(1)).Returns(
                    new Test
                    {
                        TestID = 1,
                        TestCategories = new List<TestCategory>(),
                        Suites = new List<Suite> { new Suite { SuiteID = 2 } }
                    }
                );
                zigNetEntitiesWriterMock.Setup(zewm => zewm.SaveTestResult(It.IsAny<TestResult>()))
                    .Returns(new TestResult { Test = new Test { TestID = 1 } });
                zigNetEntitiesWriterMock.Setup(zewm => zewm.GetLatestTestResults())
                    .Returns(new List<LatestTestResult>{
                        new LatestTestResult { SuiteId = 2, TestId = 1, PassingFromDateTime = DateTime.UtcNow }
                    }.AsQueryable);

                var testResult = new ZigNetTestResult
                {
                    Test = new ZigNetTest
                    {
                        TestID = 1,
                        Name = "test 1",
                        Categories = new List<ZigNetTestCategory>()
                    },
                    SuiteResult = new ZigNetSuiteResult { SuiteResultID = 2 },
                    ResultType = ZigNetTestResultType.Pass
                };

                var zigNetEntityFrameworkDatabase = new ZigNetEntityFrameworkDatabase(zigNetEntitiesWriterMock.Object, zigNetEntitiesReadOnlyMock.Object);
                zigNetEntityFrameworkDatabase.SaveTestResult(testResult);
            }

            [TestMethod]
            public void IgnoresFailingFromDate()
            {
                var zigNetEntitiesReadOnlyMock = new Mock<IZigNetEntitiesReadOnly>();
                zigNetEntitiesReadOnlyMock.Setup(zerom => zerom.GetMappedTestWithCategoriesOrDefault("test 1"))
                    .Returns(new ZigNetTest { Name = "test 1", TestID = 1, Categories = new List<ZigNetTestCategory>() });
                zigNetEntitiesReadOnlyMock.Setup(zewm => zewm.GetSuiteResult(2)).Returns(new SuiteResult { SuiteId = 2 });
                zigNetEntitiesReadOnlyMock.Setup(zewm => zewm.GetSuite(2)).Returns(new Suite { SuiteID = 2 });

                var zigNetEntitiesWriterMock = new Mock<IZigNetEntitiesWriter>();
                zigNetEntitiesWriterMock.Setup(zewm => zewm.GetTestWithSuites(1)).Returns(
                    new Test
                    {
                        TestID = 1,
                        TestCategories = new List<TestCategory>(),
                        Suites = new List<Suite> { new Suite { SuiteID = 2 } }
                    }
                );
                zigNetEntitiesWriterMock.Setup(zewm => zewm.SaveTestResult(It.IsAny<TestResult>()))
                    .Returns(new TestResult { Test = new Test { TestID = 1 } });
                zigNetEntitiesWriterMock.Setup(zewm => zewm.GetLatestTestResults())
                    .Returns(new List<LatestTestResult>{
                        new LatestTestResult { SuiteId = 2, TestId = 1, FailingFromDateTime = DateTime.UtcNow }
                    }.AsQueryable);

                var testResult = new ZigNetTestResult
                {
                    Test = new ZigNetTest
                    {
                        TestID = 1,
                        Name = "test 1",
                        Categories = new List<ZigNetTestCategory>()
                    },
                    SuiteResult = new ZigNetSuiteResult { SuiteResultID = 2 },
                    ResultType = ZigNetTestResultType.Fail,
                    TestFailureDetails = new ZigNetTestFailureDetails()
                };

                var zigNetEntityFrameworkDatabase = new ZigNetEntityFrameworkDatabase(zigNetEntitiesWriterMock.Object, zigNetEntitiesReadOnlyMock.Object);
                zigNetEntityFrameworkDatabase.SaveTestResult(testResult);
            }

            [TestMethod]
            public void UpdatesFailingFromDateIfInconclusive()
            {
                var zigNetEntitiesReadOnlyMock = new Mock<IZigNetEntitiesReadOnly>();
                zigNetEntitiesReadOnlyMock.Setup(zerom => zerom.GetMappedTestWithCategoriesOrDefault("test 1"))
                    .Returns(new ZigNetTest { Name = "test 1", TestID = 1, Categories = new List<ZigNetTestCategory>() });
                zigNetEntitiesReadOnlyMock.Setup(zewm => zewm.GetSuiteResult(2)).Returns(new SuiteResult { SuiteId = 2 });
                zigNetEntitiesReadOnlyMock.Setup(zewm => zewm.GetSuite(2)).Returns(new Suite { SuiteID = 2 });

                var zigNetEntitiesWriterMock = new Mock<IZigNetEntitiesWriter>();
                zigNetEntitiesWriterMock.Setup(zewm => zewm.GetTestWithSuites(1)).Returns(
                    new Test
                    {
                        TestID = 1,
                        TestCategories = new List<TestCategory>(),
                        Suites = new List<Suite> { new Suite { SuiteID = 2 } }
                    }
                );
                zigNetEntitiesWriterMock.Setup(zewm => zewm.SaveTestResult(It.IsAny<TestResult>()))
                    .Returns(new TestResult { Test = new Test { TestID = 1 } });
                zigNetEntitiesWriterMock.Setup(zewm => zewm.GetLatestTestResults())
                    .Returns(new List<LatestTestResult>{
                        new LatestTestResult { SuiteId = 2, TestId = 1, PassingFromDateTime = DateTime.UtcNow }
                    }.AsQueryable);

                var testResult = new ZigNetTestResult
                {
                    Test = new ZigNetTest
                    {
                        TestID = 1,
                        Name = "test 1",
                        Categories = new List<ZigNetTestCategory>()
                    },
                    SuiteResult = new ZigNetSuiteResult { SuiteResultID = 2 },
                    ResultType = ZigNetTestResultType.Inconclusive
                };

                var zigNetEntityFrameworkDatabase = new ZigNetEntityFrameworkDatabase(zigNetEntitiesWriterMock.Object, zigNetEntitiesReadOnlyMock.Object);
                zigNetEntityFrameworkDatabase.SaveTestResult(testResult);
            }

            [TestMethod]
            public void IgnoresFailingFromDateIfInconclusive()
            {
                var zigNetEntitiesReadOnlyMock = new Mock<IZigNetEntitiesReadOnly>();
                zigNetEntitiesReadOnlyMock.Setup(zerom => zerom.GetMappedTestWithCategoriesOrDefault("test 1"))
                    .Returns(new ZigNetTest { Name = "test 1", TestID = 1, Categories = new List<ZigNetTestCategory>() });
                zigNetEntitiesReadOnlyMock.Setup(zewm => zewm.GetSuiteResult(2)).Returns(new SuiteResult { SuiteId = 2 });
                zigNetEntitiesReadOnlyMock.Setup(zewm => zewm.GetSuite(2)).Returns(new Suite { SuiteID = 2 });

                var zigNetEntitiesWriterMock = new Mock<IZigNetEntitiesWriter>();
                zigNetEntitiesWriterMock.Setup(zewm => zewm.GetTestWithSuites(1)).Returns(
                    new Test
                    {
                        TestID = 1,
                        TestCategories = new List<TestCategory>(),
                        Suites = new List<Suite> { new Suite { SuiteID = 2 } }
                    }
                );
                zigNetEntitiesWriterMock.Setup(zewm => zewm.SaveTestResult(It.IsAny<TestResult>()))
                    .Returns(new TestResult { Test = new Test { TestID = 1 } });
                zigNetEntitiesWriterMock.Setup(zewm => zewm.GetLatestTestResults())
                    .Returns(new List<LatestTestResult>{
                        new LatestTestResult { SuiteId = 2, TestId = 1, FailingFromDateTime = DateTime.UtcNow }
                    }.AsQueryable);

                var testResult = new ZigNetTestResult
                {
                    Test = new ZigNetTest
                    {
                        TestID = 1,
                        Name = "test 1",
                        Categories = new List<ZigNetTestCategory>()
                    },
                    SuiteResult = new ZigNetSuiteResult { SuiteResultID = 2 },
                    ResultType = ZigNetTestResultType.Inconclusive
                };

                var zigNetEntityFrameworkDatabase = new ZigNetEntityFrameworkDatabase(zigNetEntitiesWriterMock.Object, zigNetEntitiesReadOnlyMock.Object);
                zigNetEntityFrameworkDatabase.SaveTestResult(testResult);
            }

            [TestMethod]
            public void IgnoresTestFailedDurationIfAlwaysPassed()
            {
                var zigNetEntitiesReadOnlyMock = new Mock<IZigNetEntitiesReadOnly>();
                zigNetEntitiesReadOnlyMock.Setup(zerom => zerom.GetMappedTestWithCategoriesOrDefault("test 1"))
                    .Returns(new ZigNetTest { Name = "test 1", TestID = 1, Categories = new List<ZigNetTestCategory>() });
                zigNetEntitiesReadOnlyMock.Setup(zewm => zewm.GetSuiteResult(2)).Returns(new SuiteResult { SuiteId = 2 });
                zigNetEntitiesReadOnlyMock.Setup(zewm => zewm.GetSuite(2)).Returns(new Suite { SuiteID = 2 });

                var zigNetEntitiesWriterMock = new Mock<IZigNetEntitiesWriter>();
                zigNetEntitiesWriterMock.Setup(zewm => zewm.GetTestWithSuites(1)).Returns(
                    new Test
                    {
                        TestID = 1,
                        TestCategories = new List<TestCategory>(),
                        Suites = new List<Suite> { new Suite { SuiteID = 2 } }
                    }
                );
                zigNetEntitiesWriterMock.Setup(zewm => zewm.SaveTestResult(It.IsAny<TestResult>()))
                    .Returns(new TestResult
                    {
                        TestResultID = 3,
                        Test = new Test { TestID = 1 }
                    });
                zigNetEntitiesWriterMock.Setup(zewm => zewm.GetSuite(2)).Returns(new Suite { SuiteID = 2 });
                zigNetEntitiesWriterMock.Setup(zewm => zewm.GetLatestTestResults()).Returns(
                    new List<LatestTestResult> {
                        new LatestTestResult { SuiteId = 2, TestId = 1, FailingFromDateTime = DateTime.UtcNow }
                    }.AsQueryable);

                var testResult = new ZigNetTestResult
                {
                    Test = new ZigNetTest
                    {
                        TestID = 1,
                        Name = "test 1",
                        Categories = new List<ZigNetTestCategory>()
                    },
                    SuiteResult = new ZigNetSuiteResult { SuiteResultID = 2 },
                    ResultType = ZigNetTestResultType.Pass
                };

                var zigNetEntityFrameworkDatabase = new ZigNetEntityFrameworkDatabase(zigNetEntitiesWriterMock.Object, zigNetEntitiesReadOnlyMock.Object);
                zigNetEntityFrameworkDatabase.SaveTestResult(testResult);
            }

            [TestMethod]
            public void IgnoresTestFailedDurationIfPassedBefore()
            {
                var zigNetEntitiesReadOnlyMock = new Mock<IZigNetEntitiesReadOnly>();
                zigNetEntitiesReadOnlyMock.Setup(zerom => zerom.GetMappedTestWithCategoriesOrDefault("test 1"))
                    .Returns(new ZigNetTest { Name = "test 1", TestID = 1, Categories = new List<ZigNetTestCategory>() });
                zigNetEntitiesReadOnlyMock.Setup(zewm => zewm.GetSuiteResult(2)).Returns(new SuiteResult { SuiteId = 2 });
                zigNetEntitiesReadOnlyMock.Setup(zewm => zewm.GetSuite(2)).Returns(new Suite { SuiteID = 2 });

                var zigNetEntitiesWriterMock = new Mock<IZigNetEntitiesWriter>();
                zigNetEntitiesWriterMock.Setup(zewm => zewm.GetTestWithSuites(1)).Returns(
                    new Test
                    {
                        TestID = 1,
                        TestCategories = new List<TestCategory>(),
                        Suites = new List<Suite> { new Suite { SuiteID = 2 } }
                    }
                );
                zigNetEntitiesWriterMock.Setup(zewm => zewm.SaveTestResult(It.IsAny<TestResult>()))
                    .Returns(new TestResult
                    {
                        TestResultID = 3,
                        Test = new Test { TestID = 1 }
                    });
                zigNetEntitiesWriterMock.Setup(zewm => zewm.GetSuite(2)).Returns(new Suite { SuiteID = 2 });
                zigNetEntitiesWriterMock.Setup(zewm => zewm.GetLatestTestResults()).Returns(
                    new List<LatestTestResult> {
                        new LatestTestResult { SuiteId = 2, TestId = 1, FailingFromDateTime = DateTime.UtcNow }
                    }.AsQueryable);
                zigNetEntitiesWriterMock.Setup(zewm => zewm.GetTestFailureDurations()).Returns(
                    new List<TestFailureDuration> {
                        new TestFailureDuration { TestFailureDurationID = 4, SuiteId = 2, TestId = 1, TestResultId = 3, FailureStartDateTime = DateTime.Now, FailureEndDateTime = DateTime.UtcNow }
                    }.AsQueryable);

                var testResult = new ZigNetTestResult
                {
                    Test = new ZigNetTest
                    {
                        TestID = 1,
                        Name = "test 1",
                        Categories = new List<ZigNetTestCategory>()
                    },
                    SuiteResult = new ZigNetSuiteResult { SuiteResultID = 2 },
                    ResultType = ZigNetTestResultType.Pass
                };

                var zigNetEntityFrameworkDatabase = new ZigNetEntityFrameworkDatabase(zigNetEntitiesWriterMock.Object, zigNetEntitiesReadOnlyMock.Object);
                zigNetEntityFrameworkDatabase.SaveTestResult(testResult);
            }

            [TestMethod]
            public void UpdatesTestFailedDurationEndTimeWhenNewlyPassing()
            {
                var zigNetEntitiesReadOnlyMock = new Mock<IZigNetEntitiesReadOnly>();
                zigNetEntitiesReadOnlyMock.Setup(zerom => zerom.GetMappedTestWithCategoriesOrDefault("test 1"))
                    .Returns(new ZigNetTest { Name = "test 1", TestID = 1, Categories = new List<ZigNetTestCategory>() });
                zigNetEntitiesReadOnlyMock.Setup(zewm => zewm.GetSuiteResult(2)).Returns(new SuiteResult { SuiteId = 2 });
                zigNetEntitiesReadOnlyMock.Setup(zewm => zewm.GetSuite(2)).Returns(new Suite { SuiteID = 2 });

                var zigNetEntitiesWriterMock = new Mock<IZigNetEntitiesWriter>();
                zigNetEntitiesWriterMock.Setup(zewm => zewm.GetTestWithSuites(1)).Returns(
                    new Test
                    {
                        TestID = 1,
                        TestCategories = new List<TestCategory>(),
                        Suites = new List<Suite> { new Suite { SuiteID = 2 } }
                    }
                );
                zigNetEntitiesWriterMock.Setup(zewm => zewm.SaveTestResult(It.IsAny<TestResult>()))
                    .Returns(new TestResult
                    {
                        TestResultID = 3,
                        Test = new Test { TestID = 1 }
                    });
                zigNetEntitiesWriterMock.Setup(zewm => zewm.GetSuite(2)).Returns(new Suite { SuiteID = 2 });
                zigNetEntitiesWriterMock.Setup(zewm => zewm.GetLatestTestResults()).Returns(
                    new List<LatestTestResult> {
                        new LatestTestResult { SuiteId = 2, TestId = 1, FailingFromDateTime = DateTime.UtcNow }
                    }.AsQueryable);
                zigNetEntitiesWriterMock.Setup(zewm => zewm.GetTestFailureDurations()).Returns(
                    new List<TestFailureDuration> {
                        new TestFailureDuration { SuiteId = 2, TestId = 1, FailureStartDateTime = DateTime.Now }
                    }.AsQueryable);

                var testResult = new ZigNetTestResult
                {
                    Test = new ZigNetTest
                    {
                        TestID = 1,
                        Name = "test 1",
                        Categories = new List<ZigNetTestCategory>()
                    },
                    SuiteResult = new ZigNetSuiteResult { SuiteResultID = 2 },
                    ResultType = ZigNetTestResultType.Pass
                };

                var zigNetEntityFrameworkDatabase = new ZigNetEntityFrameworkDatabase(zigNetEntitiesWriterMock.Object, zigNetEntitiesReadOnlyMock.Object);
                zigNetEntityFrameworkDatabase.SaveTestResult(testResult);
            }

            [TestMethod]
            public void CreatesTestFailedDurationIfFirstFailure()
            {
                var zigNetEntitiesReadOnlyMock = new Mock<IZigNetEntitiesReadOnly>();
                zigNetEntitiesReadOnlyMock.Setup(zerom => zerom.GetMappedTestWithCategoriesOrDefault("test 1"))
                    .Returns(new ZigNetTest { Name = "test 1", TestID = 1, Categories = new List<ZigNetTestCategory>() });
                zigNetEntitiesReadOnlyMock.Setup(zewm => zewm.GetSuiteResult(2)).Returns(new SuiteResult { SuiteId = 2 });
                zigNetEntitiesReadOnlyMock.Setup(zewm => zewm.GetSuite(2)).Returns(new Suite { SuiteID = 2 });

                var zigNetEntitiesWriterMock = new Mock<IZigNetEntitiesWriter>();
                zigNetEntitiesWriterMock.Setup(zewm => zewm.GetTestWithSuites(1)).Returns(
                    new Test
                    {
                        TestID = 1,
                        TestCategories = new List<TestCategory>(),
                        Suites = new List<Suite> { new Suite { SuiteID = 2 } }
                    }
                );
                zigNetEntitiesWriterMock.Setup(zewm => zewm.SaveTestResult(It.IsAny<TestResult>()))
                    .Returns(new TestResult
                    {
                        TestResultID = 3,
                        Test = new Test { TestID = 1 }
                    });
                zigNetEntitiesWriterMock.Setup(zewm => zewm.GetSuite(2)).Returns(new Suite { SuiteID = 2 });
                zigNetEntitiesWriterMock.Setup(zewm => zewm.GetLatestTestResults()).Returns(
                    new List<LatestTestResult> {
                        new LatestTestResult { SuiteId = 2, TestId = 1, FailingFromDateTime = DateTime.UtcNow }
                    }.AsQueryable);

                var testResult = new ZigNetTestResult
                {
                    Test = new ZigNetTest
                    {
                        TestID = 1,
                        Name = "test 1",
                        Categories = new List<ZigNetTestCategory>()
                    },
                    SuiteResult = new ZigNetSuiteResult { SuiteResultID = 2 },
                    ResultType = ZigNetTestResultType.Fail,
                    TestFailureDetails = new ZigNetTestFailureDetails()
                };

                var zigNetEntityFrameworkDatabase = new ZigNetEntityFrameworkDatabase(zigNetEntitiesWriterMock.Object, zigNetEntitiesReadOnlyMock.Object);
                zigNetEntityFrameworkDatabase.SaveTestResult(testResult);
            }

            [TestMethod]
            public void CreatesTestFailedDurationIfNewFailure()
            {
                var zigNetEntitiesReadOnlyMock = new Mock<IZigNetEntitiesReadOnly>();
                zigNetEntitiesReadOnlyMock.Setup(zerom => zerom.GetMappedTestWithCategoriesOrDefault("test 1"))
                    .Returns(new ZigNetTest { Name = "test 1", TestID = 1, Categories = new List<ZigNetTestCategory>() });
                zigNetEntitiesReadOnlyMock.Setup(zewm => zewm.GetSuiteResult(2)).Returns(new SuiteResult { SuiteId = 2 });
                zigNetEntitiesReadOnlyMock.Setup(zewm => zewm.GetSuite(2)).Returns(new Suite { SuiteID = 2 });

                var zigNetEntitiesWriterMock = new Mock<IZigNetEntitiesWriter>();
                zigNetEntitiesWriterMock.Setup(zewm => zewm.GetTestWithSuites(1)).Returns(
                    new Test
                    {
                        TestID = 1,
                        TestCategories = new List<TestCategory>(),
                        Suites = new List<Suite> { new Suite { SuiteID = 2 } }
                    }
                );
                zigNetEntitiesWriterMock.Setup(zewm => zewm.SaveTestResult(It.IsAny<TestResult>()))
                    .Returns(new TestResult
                    {
                        TestResultID = 3,
                        Test = new Test { TestID = 1 }
                    });
                zigNetEntitiesWriterMock.Setup(zewm => zewm.GetSuite(2)).Returns(new Suite { SuiteID = 2 });
                zigNetEntitiesWriterMock.Setup(zewm => zewm.GetLatestTestResults()).Returns(
                    new List<LatestTestResult> {
                        new LatestTestResult { SuiteId = 2, TestId = 1, FailingFromDateTime = DateTime.UtcNow }
                    }.AsQueryable);
                zigNetEntitiesWriterMock.Setup(zewm => zewm.GetTestFailureDurations()).Returns(
                    new List<TestFailureDuration> {
                        new TestFailureDuration { SuiteId = 2, TestId = 1, FailureStartDateTime = DateTime.Now, FailureEndDateTime = DateTime.Now }
                    }.AsQueryable);

                var testResult = new ZigNetTestResult
                {
                    Test = new ZigNetTest
                    {
                        TestID = 1,
                        Name = "test 1",
                        Categories = new List<ZigNetTestCategory>()
                    },
                    SuiteResult = new ZigNetSuiteResult { SuiteResultID = 2 },
                    ResultType = ZigNetTestResultType.Fail,
                    TestFailureDetails = new ZigNetTestFailureDetails()
                };

                var zigNetEntityFrameworkDatabase = new ZigNetEntityFrameworkDatabase(zigNetEntitiesWriterMock.Object, zigNetEntitiesReadOnlyMock.Object);
                zigNetEntityFrameworkDatabase.SaveTestResult(testResult);
            }

            [TestMethod]
            public void IgnoresTestFailedDurationIfSecondFailure()
            {
                var zigNetEntitiesReadOnlyMock = new Mock<IZigNetEntitiesReadOnly>();
                zigNetEntitiesReadOnlyMock.Setup(zerom => zerom.GetMappedTestWithCategoriesOrDefault("test 1"))
                    .Returns(new ZigNetTest { Name = "test 1", TestID = 1, Categories = new List<ZigNetTestCategory>() });
                zigNetEntitiesReadOnlyMock.Setup(zewm => zewm.GetSuiteResult(2)).Returns(new SuiteResult { SuiteId = 2 });
                zigNetEntitiesReadOnlyMock.Setup(zewm => zewm.GetSuite(2)).Returns(new Suite { SuiteID = 2 });

                var zigNetEntitiesWriterMock = new Mock<IZigNetEntitiesWriter>();
                zigNetEntitiesWriterMock.Setup(zewm => zewm.GetTestWithSuites(1)).Returns(
                    new Test
                    {
                        TestID = 1,
                        TestCategories = new List<TestCategory>(),
                        Suites = new List<Suite> { new Suite { SuiteID = 2 } }
                    }
                );
                zigNetEntitiesWriterMock.Setup(zewm => zewm.SaveTestResult(It.IsAny<TestResult>()))
                    .Returns(new TestResult
                    {
                        TestResultID = 3,
                        Test = new Test { TestID = 1 }
                    });
                zigNetEntitiesWriterMock.Setup(zewm => zewm.GetSuite(2)).Returns(new Suite { SuiteID = 2 });
                zigNetEntitiesWriterMock.Setup(zewm => zewm.GetLatestTestResults()).Returns(
                    new List<LatestTestResult> {
                        new LatestTestResult { SuiteId = 2, TestId = 1, FailingFromDateTime = DateTime.UtcNow }
                    }.AsQueryable);
                zigNetEntitiesWriterMock.Setup(zewm => zewm.GetTestFailureDurations()).Returns(
                    new List<TestFailureDuration> {
                        new TestFailureDuration { SuiteId = 2, TestId = 1, FailureStartDateTime = DateTime.Now }
                    }.AsQueryable);

                var testResult = new ZigNetTestResult
                {
                    Test = new ZigNetTest
                    {
                        TestID = 1,
                        Name = "test 1",
                        Categories = new List<ZigNetTestCategory>()
                    },
                    SuiteResult = new ZigNetSuiteResult { SuiteResultID = 2 },
                    ResultType = ZigNetTestResultType.Fail,
                    TestFailureDetails = new ZigNetTestFailureDetails()
                };

                var zigNetEntityFrameworkDatabase = new ZigNetEntityFrameworkDatabase(zigNetEntitiesWriterMock.Object, zigNetEntitiesReadOnlyMock.Object);
                zigNetEntityFrameworkDatabase.SaveTestResult(testResult);
            }

            [TestMethod]
            public void GetLatestFailureDurationRecord()
            {
                var zigNetEntitiesReadOnlyMock = new Mock<IZigNetEntitiesReadOnly>();
                zigNetEntitiesReadOnlyMock.Setup(zerom => zerom.GetMappedTestWithCategoriesOrDefault("test 1"))
                    .Returns(new ZigNetTest { Name = "test 1", TestID = 1, Categories = new List<ZigNetTestCategory>() });
                zigNetEntitiesReadOnlyMock.Setup(zewm => zewm.GetSuiteResult(2)).Returns(new SuiteResult { SuiteId = 2 });
                zigNetEntitiesReadOnlyMock.Setup(zewm => zewm.GetSuite(2)).Returns(new Suite { SuiteID = 2 });

                var zigNetEntitiesWriterMock = new Mock<IZigNetEntitiesWriter>();
                zigNetEntitiesWriterMock.Setup(zewm => zewm.GetTestWithSuites(1)).Returns(
                    new Test
                    {
                        TestID = 1,
                        TestCategories = new List<TestCategory>(),
                        Suites = new List<Suite> { new Suite { SuiteID = 2 } }
                    }
                );
                zigNetEntitiesWriterMock.Setup(zewm => zewm.SaveTestResult(It.IsAny<TestResult>()))
                    .Returns(new TestResult
                    {
                        TestResultID = 3,
                        Test = new Test { TestID = 1 }
                    });
                zigNetEntitiesWriterMock.Setup(zewm => zewm.GetSuite(2)).Returns(new Suite { SuiteID = 2 });
                zigNetEntitiesWriterMock.Setup(zewm => zewm.GetLatestTestResults()).Returns(
                    new List<LatestTestResult> {
                        new LatestTestResult { SuiteId = 2, TestId = 1, FailingFromDateTime = DateTime.UtcNow }
                    }.AsQueryable);
                zigNetEntitiesWriterMock.Setup(zewm => zewm.GetTestFailureDurations()).Returns(
                    new List<TestFailureDuration> {
                        new TestFailureDuration { SuiteId = 2, TestId = 1, FailureStartDateTime = DateTime.UtcNow.AddDays(5) },
                        new TestFailureDuration { SuiteId = 2, TestId = 1, FailureStartDateTime = DateTime.Now },
                        new TestFailureDuration { SuiteId = 2, TestId = 1, FailureStartDateTime = new DateTime(3000, 1, 1) }
                    }.AsQueryable);

                var testResult = new ZigNetTestResult
                {
                    Test = new ZigNetTest
                    {
                        TestID = 1,
                        Name = "test 1",
                        Categories = new List<ZigNetTestCategory>()
                    },
                    SuiteResult = new ZigNetSuiteResult { SuiteResultID = 2 },
                    ResultType = ZigNetTestResultType.Fail,
                    TestFailureDetails = new ZigNetTestFailureDetails()
                };

                var zigNetEntityFrameworkDatabase = new ZigNetEntityFrameworkDatabase(zigNetEntitiesWriterMock.Object, zigNetEntitiesReadOnlyMock.Object);
                zigNetEntityFrameworkDatabase.SaveTestResult(testResult);
            }

            [TestMethod]
            public void IgnoresFailureDurationRecordWithoutSuiteId()
            {
                var zigNetEntitiesReadOnlyMock = new Mock<IZigNetEntitiesReadOnly>();
                zigNetEntitiesReadOnlyMock.Setup(zerom => zerom.GetMappedTestWithCategoriesOrDefault("test 1"))
                    .Returns(new ZigNetTest { Name = "test 1", TestID = 1, Categories = new List<ZigNetTestCategory>() });
                zigNetEntitiesReadOnlyMock.Setup(zewm => zewm.GetSuiteResult(2)).Returns(new SuiteResult { SuiteId = 2 });
                zigNetEntitiesReadOnlyMock.Setup(zewm => zewm.GetSuite(2)).Returns(new Suite { SuiteID = 2 });

                var zigNetEntitiesWriterMock = new Mock<IZigNetEntitiesWriter>();
                zigNetEntitiesWriterMock.Setup(zewm => zewm.GetTestWithSuites(1)).Returns(
                    new Test
                    {
                        TestID = 1,
                        TestCategories = new List<TestCategory>(),
                        Suites = new List<Suite> { new Suite { SuiteID = 2 } }
                    }
                );
                zigNetEntitiesWriterMock.Setup(zewm => zewm.SaveTestResult(It.IsAny<TestResult>()))
                    .Returns(new TestResult
                    {
                        TestResultID = 3,
                        Test = new Test { TestID = 1 }
                    });
                zigNetEntitiesWriterMock.Setup(zewm => zewm.GetSuite(2)).Returns(new Suite { SuiteID = 2 });
                zigNetEntitiesWriterMock.Setup(zewm => zewm.GetLatestTestResults()).Returns(
                    new List<LatestTestResult> {
                        new LatestTestResult { SuiteId = 2, TestId = 1, FailingFromDateTime = DateTime.UtcNow }
                    }.AsQueryable);
                zigNetEntitiesWriterMock.Setup(zewm => zewm.GetTestFailureDurations()).Returns(
                    new List<TestFailureDuration> {
                        new TestFailureDuration { SuiteId = 99, TestId = 1, FailureStartDateTime = new DateTime(3000, 1, 1) },
                        new TestFailureDuration { SuiteId = 2, TestId = 1, FailureStartDateTime = DateTime.Now }

                    }.AsQueryable);

                var testResult = new ZigNetTestResult
                {
                    Test = new ZigNetTest
                    {
                        TestID = 1,
                        Name = "test 1",
                        Categories = new List<ZigNetTestCategory>()
                    },
                    SuiteResult = new ZigNetSuiteResult { SuiteResultID = 2 },
                    ResultType = ZigNetTestResultType.Fail,
                    TestFailureDetails = new ZigNetTestFailureDetails()
                };

                var zigNetEntityFrameworkDatabase = new ZigNetEntityFrameworkDatabase(zigNetEntitiesWriterMock.Object, zigNetEntitiesReadOnlyMock.Object);
                zigNetEntityFrameworkDatabase.SaveTestResult(testResult);
            }

            [TestMethod]
            public void IgnoresFailureDurationRecordWithoutTestId()
            {
                var zigNetEntitiesReadOnlyMock = new Mock<IZigNetEntitiesReadOnly>();
                zigNetEntitiesReadOnlyMock.Setup(zerom => zerom.GetMappedTestWithCategoriesOrDefault("test 1"))
                    .Returns(new ZigNetTest { Name = "test 1", TestID = 1, Categories = new List<ZigNetTestCategory>() });
                zigNetEntitiesReadOnlyMock.Setup(zewm => zewm.GetSuiteResult(2)).Returns(new SuiteResult { SuiteId = 2 });
                zigNetEntitiesReadOnlyMock.Setup(zewm => zewm.GetSuite(2)).Returns(new Suite { SuiteID = 2 });

                var zigNetEntitiesWriterMock = new Mock<IZigNetEntitiesWriter>();
                zigNetEntitiesWriterMock.Setup(zewm => zewm.GetTestWithSuites(1)).Returns(
                    new Test
                    {
                        TestID = 1,
                        TestCategories = new List<TestCategory>(),
                        Suites = new List<Suite> { new Suite { SuiteID = 2 } }
                    }
                );
                zigNetEntitiesWriterMock.Setup(zewm => zewm.SaveTestResult(It.IsAny<TestResult>()))
                    .Returns(new TestResult
                    {
                        TestResultID = 3,
                        Test = new Test { TestID = 1 }
                    });
                zigNetEntitiesWriterMock.Setup(zewm => zewm.GetSuite(2)).Returns(new Suite { SuiteID = 2 });
                zigNetEntitiesWriterMock.Setup(zewm => zewm.GetLatestTestResults()).Returns(
                    new List<LatestTestResult> {
                        new LatestTestResult { SuiteId = 2, TestId = 1, FailingFromDateTime = DateTime.UtcNow }
                    }.AsQueryable);
                zigNetEntitiesWriterMock.Setup(zewm => zewm.GetTestFailureDurations()).Returns(
                    new List<TestFailureDuration> {
                        new TestFailureDuration { SuiteId = 2, TestId = 99, FailureStartDateTime = new DateTime(3000, 1, 1) },
                        new TestFailureDuration { SuiteId = 2, TestId = 1, FailureStartDateTime = DateTime.Now }

                    }.AsQueryable);

                var testResult = new ZigNetTestResult
                {
                    Test = new ZigNetTest
                    {
                        TestID = 1,
                        Name = "test 1",
                        Categories = new List<ZigNetTestCategory>()
                    },
                    SuiteResult = new ZigNetSuiteResult { SuiteResultID = 2 },
                    ResultType = ZigNetTestResultType.Fail,
                    TestFailureDetails = new ZigNetTestFailureDetails()
                };

                var zigNetEntityFrameworkDatabase = new ZigNetEntityFrameworkDatabase(zigNetEntitiesWriterMock.Object, zigNetEntitiesReadOnlyMock.Object);
                zigNetEntityFrameworkDatabase.SaveTestResult(testResult);
            }
        }
    }
}
