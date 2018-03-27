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
                    SuiteResult = new ZigNetSuiteResult { SuiteResultID = 2 } };

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

        //    [TestMethod]
        //    public void SavesFailedTestResultWithDetails()
        //    {
        //        var zigNetEntitiesWriterMock = new Mock<IZigNetEntitiesWriter>();
        //        zigNetEntitiesWriterMock.Setup(zewm => zewm.GetTestFailureType(2)).Returns(
        //            new TestFailureType
        //            {
        //                TestFailureTypeID = 2,
        //                TestFailureTypeName = "Exception"
        //            }
        //        );
        //        zigNetEntitiesWriterMock.Setup(zewm => zewm.GetTestCategories()).Returns(
        //            new List<TestCategory> { new TestCategory { TestCategoryID = 1, CategoryName = "test category 1" } }.AsQueryable);
        //        zigNetEntitiesWriterMock.Setup(zewm => zewm.GetSuiteResultWithoutTracking(1)).Returns(new SuiteResult { SuiteId = 2 });
        //        zigNetEntitiesWriterMock.Setup(zewm => zewm.GetSuite(2)).Returns(new Suite { SuiteID = 2 });
        //        zigNetEntitiesWriterMock.Setup(zewm => zewm.SaveTestResult(It.IsAny<TestResult>()))
        //            .Returns(new TestResult
        //            {
        //                TestResultID = 3,
        //                Test = new Test { TestID = 4 }
        //            });
        //        var zigNetEntitiesReadOnlyMock = new Mock<IZigNetEntitiesReadOnly>();
        //        zigNetEntitiesReadOnlyMock.

        //        var testResult = new ZigNetTestResult
        //        {
        //            Test = new ZigNetTest { Name = "test 1", Categories = new List<ZigNetTestCategory> { new ZigNetTestCategory { Name = "test category 1" } } },
        //            SuiteResult = new ZigNetSuiteResult { SuiteResultID = 1 },
        //            StartTime = DateTime.UtcNow,
        //            EndTime = DateTime.UtcNow,
        //            ResultType = ZigNetTestResultType.Fail,
        //            TestFailureDetails = new ZigNetTestFailureDetails
        //            {
        //                FailureType = ZigNetTestFailureType.Exception,
        //                FailureDetailMessage = "failed by exception at line 5"
        //            }
        //        };

        //        var zigNetEntityFrameworkDatabase = new ZigNetEntityFrameworkDatabase(zigNetEntitiesWriterMock.Object);
        //        zigNetEntityFrameworkDatabase.SaveTestResult(testResult);
        //    }

        //    [TestMethod]
        //    public void SavesExistingTestPassedTestResult()
        //    {
        //        var zigNetEntitiesWriterMock = new Mock<IZigNetEntitiesWriter>();
        //        zigNetEntitiesWriterMock.Setup(zewm => zewm.GetTestCategories()).Returns(
        //            new List<TestCategory> {
        //                new TestCategory { TestCategoryID = 1, CategoryName = "test category 1" }
        //            }.AsQueryable
        //        );
        //        zigNetEntitiesWriterMock.Setup(zewm => zewm.GetTest(1)).Returns(
        //            new Test
        //            {
        //                TestID = 1,
        //                TestCategories = new List<TestCategory> { new TestCategory { TestCategoryID = 1, CategoryName = "test category 1" } },
        //                Suites = new List<Suite> { new Suite { SuiteID = 2 } }
        //            }
        //        );
        //        zigNetEntitiesWriterMock.Setup(zewm => zewm.GetSuiteResultWithoutTracking(1)).Returns(new SuiteResult { SuiteId = 2 });
        //        zigNetEntitiesWriterMock.Setup(zewm => zewm.SaveTestResult(It.IsAny<TestResult>()))
        //            .Returns(new TestResult
        //            {
        //                TestResultID = 3,
        //                Test = new Test { TestID = 4 }
        //            });

        //        var testResult = new ZigNetTestResult
        //        {
        //            Test = new ZigNetTest
        //            {
        //                TestID = 1,
        //                Name = "test 1",
        //                Categories = new List<ZigNetTestCategory> { new ZigNetTestCategory { Name = "test category 1" } }
        //            },
        //            SuiteResult = new ZigNetSuiteResult { SuiteResultID = 1 },
        //            StartTime = DateTime.UtcNow,
        //            EndTime = DateTime.UtcNow,
        //            ResultType = ZigNetTestResultType.Pass,
        //        };

        //        var zigNetEntityFrameworkDatabase = new ZigNetEntityFrameworkDatabase(zigNetEntitiesWriterMock.Object);
        //        zigNetEntityFrameworkDatabase.SaveTestResult(testResult);
        //    }

        //    [TestMethod]
        //    public void SavesExistingTestFailedTestResult()
        //    {
        //        var zigNetEntitiesWriterMock = new Mock<IZigNetEntitiesWriter>();
        //        zigNetEntitiesWriterMock.Setup(zewm => zewm.GetTestFailureType(1)).Returns(
        //            new TestFailureType
        //            {
        //                TestFailureTypeID = 1,
        //                TestFailureTypeName = "Assertion"
        //            }
        //        );
        //        zigNetEntitiesWriterMock.Setup(zewm => zewm.GetTestCategories()).Returns(
        //            new List<TestCategory> {
        //                new TestCategory { TestCategoryID = 1, CategoryName = "test category 1" }
        //            }.AsQueryable
        //        );
        //        zigNetEntitiesWriterMock.Setup(zewm => zewm.GetTest(1)).Returns(
        //            new Test
        //            {
        //                TestID = 1,
        //                TestCategories = new List<TestCategory> { new TestCategory { TestCategoryID = 1, CategoryName = "test category 1" } },
        //                Suites = new List<Suite> { new Suite { SuiteID = 2 } }
        //            }
        //        );
        //        zigNetEntitiesWriterMock.Setup(zewm => zewm.GetSuiteResultWithoutTracking(1)).Returns(new SuiteResult { SuiteId = 2 });
        //        zigNetEntitiesWriterMock.Setup(zewm => zewm.SaveTestResult(It.IsAny<TestResult>()))
        //            .Returns(new TestResult
        //            {
        //                TestResultID = 3,
        //                Test = new Test { TestID = 4 }
        //            });

        //        var testResult = new ZigNetTestResult
        //        {
        //            Test = new ZigNetTest
        //            {
        //                TestID = 1,
        //                Name = "test 1",
        //                Categories = new List<ZigNetTestCategory> {
        //                    new ZigNetTestCategory { Name = "test category 1" },
        //                    new ZigNetTestCategory { Name = "test category 2" }
        //                }
        //            },
        //            SuiteResult = new ZigNetSuiteResult { SuiteResultID = 1 },
        //            StartTime = DateTime.UtcNow,
        //            EndTime = DateTime.UtcNow,
        //            ResultType = ZigNetTestResultType.Fail,
        //            TestFailureDetails = new ZigNetTestFailureDetails
        //            {
        //                FailureType = ZigNetTestFailureType.Assertion
        //            }
        //        };

        //        var zigNetEntityFrameworkDatabase = new ZigNetEntityFrameworkDatabase(zigNetEntitiesWriterMock.Object);
        //        zigNetEntityFrameworkDatabase.SaveTestResult(testResult);
        //    }

        //    [TestMethod]
        //    public void DoesNotThrowWhenClearingAllTestCategories()
        //    {
        //        var zigNetEntitiesWriterMock = new Mock<IZigNetEntitiesWriter>();
        //        zigNetEntitiesWriterMock.Setup(zewm => zewm.GetTest(1)).Returns(
        //            new Test
        //            {
        //                TestID = 1,
        //                TestCategories = new List<TestCategory> { new TestCategory { TestCategoryID = 1, CategoryName = "test category 1" } },
        //                Suites = new List<Suite> { new Suite { SuiteID = 2 } }
        //            }
        //        );
        //        zigNetEntitiesWriterMock.Setup(zewm => zewm.GetSuiteResultWithoutTracking(1)).Returns(new SuiteResult { SuiteId = 2 });
        //        zigNetEntitiesWriterMock.Setup(zewm => zewm.SaveTestResult(It.IsAny<TestResult>()))
        //            .Returns(new TestResult
        //            {
        //                TestResultID = 3,
        //                Test = new Test { TestID = 4 }
        //            });

        //        var testResult = new ZigNetTestResult
        //        {
        //            Test = new ZigNetTest
        //            {
        //                TestID = 1,
        //                Name = "test 1",
        //                Categories = new List<ZigNetTestCategory>()
        //            },
        //            SuiteResult = new ZigNetSuiteResult { SuiteResultID = 1 },
        //            StartTime = DateTime.UtcNow,
        //            EndTime = DateTime.UtcNow,
        //            ResultType = ZigNetTestResultType.Pass
        //        };

        //        var zigNetEntityFrameworkDatabase = new ZigNetEntityFrameworkDatabase(zigNetEntitiesWriterMock.Object);
        //        zigNetEntityFrameworkDatabase.SaveTestResult(testResult);
        //    }

        //    [TestMethod]
        //    public void CreatesNewLatestTestResult()
        //    {
        //        var zigNetEntitiesWriterMock = new Mock<IZigNetEntitiesWriter>();
        //        zigNetEntitiesWriterMock.Setup(zewm => zewm.GetSuiteResultWithoutTracking(1)).Returns(new SuiteResult { SuiteId = 2 });
        //        zigNetEntitiesWriterMock.Setup(zewm => zewm.SaveTestResult(It.IsAny<TestResult>()))
        //            .Returns(new TestResult
        //            {
        //                TestResultID = 3,
        //                Test = new Test { TestID = 4 }
        //            });
        //        zigNetEntitiesWriterMock.Setup(zewm => zewm.GetLatestTestResults()).Returns(new List<LatestTestResult>().AsQueryable);

        //        var testResult = new ZigNetTestResult
        //        {
        //            Test = new ZigNetTest
        //            {
        //                TestID = 0,
        //                Name = "test 1",
        //                Categories = new List<ZigNetTestCategory>()
        //            },
        //            SuiteResult = new ZigNetSuiteResult { SuiteResultID = 1 },
        //            ResultType = ZigNetTestResultType.Fail,
        //            TestFailureDetails = new ZigNetTestFailureDetails()
        //        };

        //        var zigNetEntityFrameworkDatabase = new ZigNetEntityFrameworkDatabase(zigNetEntitiesWriterMock.Object);
        //        zigNetEntityFrameworkDatabase.SaveTestResult(testResult);
        //    }

        //    [TestMethod]
        //    public void CreatesNewLatestTestResultWhenSuiteIdDoesNotMatch()
        //    {
        //        var zigNetEntitiesWriterMock = new Mock<IZigNetEntitiesWriter>();
        //        zigNetEntitiesWriterMock.Setup(zewm => zewm.GetSuiteResultWithoutTracking(5)).Returns(new SuiteResult { SuiteId = 2 });
        //        zigNetEntitiesWriterMock.Setup(zewm => zewm.SaveTestResult(It.IsAny<TestResult>()))
        //            .Returns(new TestResult
        //            {
        //                TestResultID = 3,
        //                Test = new Test { TestID = 4 }
        //            });
        //        zigNetEntitiesWriterMock.Setup(zewm => zewm.GetLatestTestResults())
        //            .Returns(new List<LatestTestResult>{
        //                new LatestTestResult { SuiteId = 1, TestId = 4 }
        //            }.AsQueryable);
        //        zigNetEntitiesWriterMock.Setup(zewm => zewm.GetTest(4)).Returns(
        //            new Test
        //            {
        //                TestID = 4,
        //                TestCategories = new List<TestCategory>()
        //            }
        //        );

        //        var testResult = new ZigNetTestResult
        //        {
        //            Test = new ZigNetTest
        //            {
        //                TestID = 4,
        //                Name = "test 1",
        //                Categories = new List<ZigNetTestCategory>()
        //            },
        //            SuiteResult = new ZigNetSuiteResult { SuiteResultID = 5 },
        //            ResultType = ZigNetTestResultType.Fail,
        //            TestFailureDetails = new ZigNetTestFailureDetails()
        //        };

        //        var zigNetEntityFrameworkDatabase = new ZigNetEntityFrameworkDatabase(zigNetEntitiesWriterMock.Object);
        //        zigNetEntityFrameworkDatabase.SaveTestResult(testResult);
        //    }

        //    [TestMethod]
        //    public void CreatesNewLatestTestResultWhenTestIdDoesNotMatch()
        //    {
        //        var zigNetEntitiesWriterMock = new Mock<IZigNetEntitiesWriter>();
        //        zigNetEntitiesWriterMock.Setup(zewm => zewm.GetSuiteResultWithoutTracking(5)).Returns(new SuiteResult { SuiteId = 2 });
        //        zigNetEntitiesWriterMock.Setup(zewm => zewm.SaveTestResult(It.IsAny<TestResult>()))
        //            .Returns(new TestResult
        //            {
        //                TestResultID = 3,
        //                Test = new Test { TestID = 4 }
        //            });
        //        zigNetEntitiesWriterMock.Setup(zewm => zewm.GetLatestTestResults())
        //            .Returns(new List<LatestTestResult>{
        //                new LatestTestResult { SuiteId = 2, TestId = 3 }
        //            }.AsQueryable);
        //        zigNetEntitiesWriterMock.Setup(zewm => zewm.GetTest(4)).Returns(
        //            new Test
        //            {
        //                TestID = 4,
        //                TestCategories = new List<TestCategory>()
        //            }
        //        );

        //        var testResult = new ZigNetTestResult
        //        {
        //            Test = new ZigNetTest
        //            {
        //                TestID = 4,
        //                Name = "test 1",
        //                Categories = new List<ZigNetTestCategory>()
        //            },
        //            SuiteResult = new ZigNetSuiteResult { SuiteResultID = 5 },
        //            ResultType = ZigNetTestResultType.Fail,
        //            TestFailureDetails = new ZigNetTestFailureDetails()
        //        };

        //        var zigNetEntityFrameworkDatabase = new ZigNetEntityFrameworkDatabase(zigNetEntitiesWriterMock.Object);
        //        zigNetEntityFrameworkDatabase.SaveTestResult(testResult);
        //    }

        //    [TestMethod]
        //    public void UpdatesExistingLatestTestResult()
        //    {
        //        var zigNetEntitiesWriterMock = new Mock<IZigNetEntitiesWriter>();
        //        zigNetEntitiesWriterMock.Setup(zewm => zewm.GetSuiteResultWithoutTracking(5)).Returns(new SuiteResult { SuiteId = 2 });
        //        zigNetEntitiesWriterMock.Setup(zewm => zewm.SaveTestResult(It.IsAny<TestResult>()))
        //            .Returns(new TestResult
        //            {
        //                TestResultID = 3,
        //                Test = new Test { TestID = 4 }
        //            });
        //        zigNetEntitiesWriterMock.Setup(zewm => zewm.GetLatestTestResults())
        //            .Returns(new List<LatestTestResult>{
        //                new LatestTestResult { SuiteId = 2, TestId = 4 }
        //            }.AsQueryable);
        //        zigNetEntitiesWriterMock.Setup(zewm => zewm.GetTest(4)).Returns(
        //            new Test
        //            {
        //                TestID = 4,
        //                TestCategories = new List<TestCategory>()
        //            }
        //        );

        //        var testResult = new ZigNetTestResult
        //        {
        //            Test = new ZigNetTest
        //            {
        //                TestID = 4,
        //                Name = "test 1",
        //                Categories = new List<ZigNetTestCategory>()
        //            },
        //            SuiteResult = new ZigNetSuiteResult { SuiteResultID = 5 },
        //            ResultType = ZigNetTestResultType.Fail,
        //            TestFailureDetails = new ZigNetTestFailureDetails()
        //        };

        //        var zigNetEntityFrameworkDatabase = new ZigNetEntityFrameworkDatabase(zigNetEntitiesWriterMock.Object);
        //        zigNetEntityFrameworkDatabase.SaveTestResult(testResult);
        //    }

        //    [TestMethod]
        //    public void UpdatesPassingFromDate()
        //    {
        //        var zigNetEntitiesWriterMock = new Mock<IZigNetEntitiesWriter>();
        //        zigNetEntitiesWriterMock.Setup(zewm => zewm.GetSuiteResultWithoutTracking(5)).Returns(new SuiteResult { SuiteId = 2 });
        //        zigNetEntitiesWriterMock.Setup(zewm => zewm.SaveTestResult(It.IsAny<TestResult>()))
        //            .Returns(new TestResult
        //            {
        //                TestResultID = 3,
        //                Test = new Test { TestID = 4 }
        //            });
        //        zigNetEntitiesWriterMock.Setup(zewm => zewm.GetLatestTestResults())
        //            .Returns(new List<LatestTestResult>{
        //                new LatestTestResult { SuiteId = 2, TestId = 4, FailingFromDateTime = DateTime.UtcNow }
        //            }.AsQueryable);
        //        zigNetEntitiesWriterMock.Setup(zewm => zewm.GetTest(4)).Returns(
        //            new Test
        //            {
        //                TestID = 4,
        //                TestCategories = new List<TestCategory>()
        //            }
        //        );

        //        var testResult = new ZigNetTestResult
        //        {
        //            Test = new ZigNetTest
        //            {
        //                TestID = 4,
        //                Name = "test 1",
        //                Categories = new List<ZigNetTestCategory>()
        //            },
        //            SuiteResult = new ZigNetSuiteResult { SuiteResultID = 5 },
        //            ResultType = ZigNetTestResultType.Pass
        //        };

        //        var zigNetEntityFrameworkDatabase = new ZigNetEntityFrameworkDatabase(zigNetEntitiesWriterMock.Object);
        //        zigNetEntityFrameworkDatabase.SaveTestResult(testResult);
        //    }

        //    [TestMethod]
        //    public void UpdatesFailingFromDate()
        //    {
        //        var zigNetEntitiesWriterMock = new Mock<IZigNetEntitiesWriter>();
        //        zigNetEntitiesWriterMock.Setup(zewm => zewm.GetSuiteResultWithoutTracking(5)).Returns(new SuiteResult { SuiteId = 2 });
        //        zigNetEntitiesWriterMock.Setup(zewm => zewm.SaveTestResult(It.IsAny<TestResult>()))
        //            .Returns(new TestResult
        //            {
        //                TestResultID = 3,
        //                Test = new Test { TestID = 4 }
        //            });
        //        zigNetEntitiesWriterMock.Setup(zewm => zewm.GetLatestTestResults())
        //            .Returns(new List<LatestTestResult>{
        //                new LatestTestResult { SuiteId = 2, TestId = 4, PassingFromDateTime = DateTime.UtcNow }
        //            }.AsQueryable);
        //        zigNetEntitiesWriterMock.Setup(zewm => zewm.GetTest(4)).Returns(
        //            new Test
        //            {
        //                TestID = 4,
        //                TestCategories = new List<TestCategory>()
        //            }
        //        );

        //        var testResult = new ZigNetTestResult
        //        {
        //            Test = new ZigNetTest
        //            {
        //                TestID = 4,
        //                Name = "test 1",
        //                Categories = new List<ZigNetTestCategory>()
        //            },
        //            SuiteResult = new ZigNetSuiteResult { SuiteResultID = 5 },
        //            ResultType = ZigNetTestResultType.Fail,
        //            TestFailureDetails = new ZigNetTestFailureDetails()
        //        };

        //        var zigNetEntityFrameworkDatabase = new ZigNetEntityFrameworkDatabase(zigNetEntitiesWriterMock.Object);
        //        zigNetEntityFrameworkDatabase.SaveTestResult(testResult);
        //    }

        //    [TestMethod]
        //    public void IgnoresPassingFromDate()
        //    {
        //        var zigNetEntitiesWriterMock = new Mock<IZigNetEntitiesWriter>();
        //        zigNetEntitiesWriterMock.Setup(zewm => zewm.GetSuiteResultWithoutTracking(5)).Returns(new SuiteResult { SuiteId = 2 });
        //        zigNetEntitiesWriterMock.Setup(zewm => zewm.SaveTestResult(It.IsAny<TestResult>()))
        //            .Returns(new TestResult
        //            {
        //                TestResultID = 3,
        //                Test = new Test { TestID = 4 }
        //            });
        //        zigNetEntitiesWriterMock.Setup(zewm => zewm.GetLatestTestResults())
        //            .Returns(new List<LatestTestResult>{
        //                new LatestTestResult { SuiteId = 2, TestId = 4, PassingFromDateTime = DateTime.UtcNow }
        //            }.AsQueryable);
        //        zigNetEntitiesWriterMock.Setup(zewm => zewm.GetTest(4)).Returns(
        //            new Test
        //            {
        //                TestID = 4,
        //                TestCategories = new List<TestCategory>()
        //            }
        //        );

        //        var testResult = new ZigNetTestResult
        //        {
        //            Test = new ZigNetTest
        //            {
        //                TestID = 4,
        //                Name = "test 1",
        //                Categories = new List<ZigNetTestCategory>()
        //            },
        //            SuiteResult = new ZigNetSuiteResult { SuiteResultID = 5 },
        //            ResultType = ZigNetTestResultType.Pass
        //        };

        //        var zigNetEntityFrameworkDatabase = new ZigNetEntityFrameworkDatabase(zigNetEntitiesWriterMock.Object);
        //        zigNetEntityFrameworkDatabase.SaveTestResult(testResult);
        //    }

        //    [TestMethod]
        //    public void IgnoresFailingFromDate()
        //    {
        //        var zigNetEntitiesWriterMock = new Mock<IZigNetEntitiesWriter>();
        //        zigNetEntitiesWriterMock.Setup(zewm => zewm.GetSuiteResultWithoutTracking(5)).Returns(new SuiteResult { SuiteId = 2 });
        //        zigNetEntitiesWriterMock.Setup(zewm => zewm.SaveTestResult(It.IsAny<TestResult>()))
        //            .Returns(new TestResult
        //            {
        //                TestResultID = 3,
        //                Test = new Test { TestID = 4 }
        //            });
        //        zigNetEntitiesWriterMock.Setup(zewm => zewm.GetLatestTestResults())
        //            .Returns(new List<LatestTestResult>{
        //                new LatestTestResult { SuiteId = 2, TestId = 4, FailingFromDateTime = DateTime.UtcNow }
        //            }.AsQueryable);
        //        zigNetEntitiesWriterMock.Setup(zewm => zewm.GetTest(4)).Returns(
        //            new Test
        //            {
        //                TestID = 4,
        //                TestCategories = new List<TestCategory>()
        //            }
        //        );

        //        var testResult = new ZigNetTestResult
        //        {
        //            Test = new ZigNetTest
        //            {
        //                TestID = 4,
        //                Name = "test 1",
        //                Categories = new List<ZigNetTestCategory>()
        //            },
        //            SuiteResult = new ZigNetSuiteResult { SuiteResultID = 5 },
        //            ResultType = ZigNetTestResultType.Fail,
        //            TestFailureDetails = new ZigNetTestFailureDetails()
        //        };

        //        var zigNetEntityFrameworkDatabase = new ZigNetEntityFrameworkDatabase(zigNetEntitiesWriterMock.Object);
        //        zigNetEntityFrameworkDatabase.SaveTestResult(testResult);
        //    }

        //    [TestMethod]
        //    public void UpdatesFailingFromDateIfInconclusive()
        //    {
        //        var zigNetEntitiesWriterMock = new Mock<IZigNetEntitiesWriter>();
        //        zigNetEntitiesWriterMock.Setup(zewm => zewm.GetSuiteResultWithoutTracking(5)).Returns(new SuiteResult { SuiteId = 2 });
        //        zigNetEntitiesWriterMock.Setup(zewm => zewm.SaveTestResult(It.IsAny<TestResult>()))
        //            .Returns(new TestResult
        //            {
        //                TestResultID = 3,
        //                Test = new Test { TestID = 4 }
        //            });
        //        zigNetEntitiesWriterMock.Setup(zewm => zewm.GetLatestTestResults())
        //            .Returns(new List<LatestTestResult>{
        //                new LatestTestResult { SuiteId = 2, TestId = 4, PassingFromDateTime = DateTime.UtcNow }
        //            }.AsQueryable);
        //        zigNetEntitiesWriterMock.Setup(zewm => zewm.GetTest(4)).Returns(
        //            new Test
        //            {
        //                TestID = 4,
        //                TestCategories = new List<TestCategory>()
        //            }
        //        );

        //        var testResult = new ZigNetTestResult
        //        {
        //            Test = new ZigNetTest
        //            {
        //                TestID = 4,
        //                Name = "test 1",
        //                Categories = new List<ZigNetTestCategory>()
        //            },
        //            SuiteResult = new ZigNetSuiteResult { SuiteResultID = 5 },
        //            ResultType = ZigNetTestResultType.Inconclusive
        //        };

        //        var zigNetEntityFrameworkDatabase = new ZigNetEntityFrameworkDatabase(zigNetEntitiesWriterMock.Object);
        //        zigNetEntityFrameworkDatabase.SaveTestResult(testResult);
        //    }

        //    [TestMethod]
        //    public void IgnoresFailingFromDateIfInconclusive()
        //    {
        //        var zigNetEntitiesWriterMock = new Mock<IZigNetEntitiesWriter>();
        //        zigNetEntitiesWriterMock.Setup(zewm => zewm.GetSuiteResultWithoutTracking(5)).Returns(new SuiteResult { SuiteId = 2 });
        //        zigNetEntitiesWriterMock.Setup(zewm => zewm.SaveTestResult(It.IsAny<TestResult>()))
        //            .Returns(new TestResult
        //            {
        //                TestResultID = 3,
        //                Test = new Test { TestID = 4 }
        //            });
        //        zigNetEntitiesWriterMock.Setup(zewm => zewm.GetLatestTestResults())
        //            .Returns(new List<LatestTestResult>{
        //                new LatestTestResult { SuiteId = 2, TestId = 4, FailingFromDateTime = DateTime.UtcNow }
        //            }.AsQueryable);
        //        zigNetEntitiesWriterMock.Setup(zewm => zewm.GetTest(4)).Returns(
        //            new Test
        //            {
        //                TestID = 4,
        //                TestCategories = new List<TestCategory>()
        //            }
        //        );

        //        var testResult = new ZigNetTestResult
        //        {
        //            Test = new ZigNetTest
        //            {
        //                TestID = 4,
        //                Name = "test 1",
        //                Categories = new List<ZigNetTestCategory>()
        //            },
        //            SuiteResult = new ZigNetSuiteResult { SuiteResultID = 5 },
        //            ResultType = ZigNetTestResultType.Inconclusive
        //        };

        //        var zigNetEntityFrameworkDatabase = new ZigNetEntityFrameworkDatabase(zigNetEntitiesWriterMock.Object);
        //        zigNetEntityFrameworkDatabase.SaveTestResult(testResult);
        //    }
        }

        //[TestClass]
        //public class GetTestResultsForSuiteMethod
        //{
        //    [TestMethod]
        //    public void MapsTestResults()
        //    {
        //        var zigNetEntitiesWriterMock = new Mock<IZigNetEntitiesWriter>();
        //        zigNetEntitiesWriterMock.Setup(zewm => zewm.GetTestResults()).Returns(
        //            new List<TestResult> {
        //                new TestResult
        //                {
        //                    SuiteResult = new SuiteResult { SuiteId = 1 },
        //                    TestResultEndDateTime = new DateTime(2018, 3, 13, 15, 30, 27),
        //                    TestResultType = new TestResultType { TestResultTypeName = "Pass" },
        //                    Test = new Test { TestID = 2 }
        //                },
        //                new TestResult
        //                {
        //                    SuiteResult = new SuiteResult { SuiteId = 1 },
        //                    TestResultEndDateTime = new DateTime(2018, 3, 14, 15, 30, 27),
        //                    TestResultType = new TestResultType { TestResultTypeName = "Fail" },
        //                    Test = new Test { TestID = 2 }
        //                },
        //                new TestResult
        //                {
        //                    SuiteResult = new SuiteResult { SuiteId = 1 },
        //                    TestResultEndDateTime = new DateTime(2018, 3, 15, 15, 30, 27),
        //                    TestResultType = new TestResultType { TestResultTypeName = "Inconclusive" },
        //                    Test = new Test { TestID = 2 }
        //                },
        //                new TestResult
        //                {
        //                    SuiteResult = new SuiteResult { SuiteId = 1 },
        //                    TestResultEndDateTime = new DateTime(2018, 3, 16, 15, 30, 27),
        //                    TestResultType = new TestResultType { TestResultTypeName = "Pass" },
        //                    Test = new Test { TestID = 2 }
        //                }
        //            }.AsQueryable
        //        );

        //        var zigNetEntityFrameworkDatabase = new ZigNetEntityFrameworkDatabase(zigNetEntitiesWriterMock.Object);

        //        var testResultsForTestInSuite = zigNetEntityFrameworkDatabase.GetTestResultsForSuite(1);

        //        Assert.AreEqual(4, testResultsForTestInSuite.Count());
        //        Assert.IsTrue(testResultsForTestInSuite.All(tr => tr.Test.TestID == 2));
        //        Assert.IsTrue(testResultsForTestInSuite.All(tr => tr.Test.Categories == null));
        //    }

        //    [TestMethod]
        //    public void IgnoresTestResultsWithoutSuiteId()
        //    {
        //        var zigNetEntitiesWriterMock = new Mock<IZigNetEntitiesWriter>();
        //        zigNetEntitiesWriterMock.Setup(zewm => zewm.GetTestResults()).Returns(
        //            new List<TestResult> {
        //                new TestResult
        //                {
        //                    SuiteResult = new SuiteResult { SuiteId = 1 },
        //                    TestResultEndDateTime = new DateTime(2018, 3, 13, 15, 30, 27),
        //                    TestResultType = new TestResultType { TestResultTypeName = "Pass" },
        //                    Test = new Test { TestID = 2 }
        //                },
        //                new TestResult
        //                {
        //                    SuiteResult = new SuiteResult { SuiteId = 2 },
        //                    TestResultEndDateTime = new DateTime(2018, 3, 14, 15, 30, 27),
        //                    TestResultType = new TestResultType { TestResultTypeName = "Pass" },
        //                    Test = new Test { TestID = 2 }
        //                }
        //            }.AsQueryable
        //        );

        //        var zigNetEntityFrameworkDatabase = new ZigNetEntityFrameworkDatabase(zigNetEntitiesWriterMock.Object);

        //        var testResultsForTestInSuite = zigNetEntityFrameworkDatabase.GetTestResultsForSuite(1);

        //        Assert.AreEqual(1, testResultsForTestInSuite.Count());
        //        Assert.IsTrue(testResultsForTestInSuite.All(tr => tr.Test.TestID == 2));
        //    }

        //    [TestMethod]
        //    public void DoesNotThrowWhenZeroTestResultsReturned()
        //    {
        //        var zigNetEntitiesWriterMock = new Mock<IZigNetEntitiesWriter>();
        //        zigNetEntitiesWriterMock.Setup(zewm => zewm.GetTestResults()).Returns(
        //            new List<TestResult>()
        //            .AsQueryable
        //        );

        //        var zigNetEntityFrameworkDatabase = new ZigNetEntityFrameworkDatabase(zigNetEntitiesWriterMock.Object);

        //        var testResultsForTestInSuite = zigNetEntityFrameworkDatabase.GetTestResultsForSuite(1);

        //        Assert.AreEqual(0, testResultsForTestInSuite.Count());
        //    }
        //}

        //[TestClass]
        //public class GetLatestTestResultsMethod
        //{
        //    [TestMethod]
        //    public void MapsCorrectly()
        //    {
        //        var utcNow = DateTime.UtcNow;
        //        var zigNetEntitiesWriterMock = new Mock<IZigNetEntitiesWriter>();
        //        zigNetEntitiesWriterMock.Setup(zewm => zewm.GetLatestTestResults()).Returns(
        //            new List<LatestTestResult> { 
        //                new LatestTestResult 
        //                {
        //                    SuiteId = 1,
        //                    TestResultId = 2,
        //                    TestName = "test1",
        //                    PassingFromDateTime = utcNow,
        //                }
        //            }.AsQueryable);

        //        var zigNetEntityFrameworkDatabase = new ZigNetEntityFrameworkDatabase(zigNetEntitiesWriterMock.Object);
        //        var latestTestResults = zigNetEntityFrameworkDatabase.GetLatestTestResults(1).ToList();

        //        Assert.AreEqual(1, latestTestResults.Count);
        //        Assert.AreEqual(2, latestTestResults[0].TestResultID);
        //        Assert.AreEqual("test1", latestTestResults[0].TestName);
        //        Assert.AreEqual(utcNow, latestTestResults[0].PassingFromDate);
        //        Assert.IsNull(latestTestResults[0].FailingFromDate);
        //    }

        //    [TestMethod]
        //    public void DoesNotThrowWhenNoLatestTestResultsForSuite()
        //    {
        //        var utcNow = DateTime.UtcNow;
        //        var zigNetEntitiesWriterMock = new Mock<IZigNetEntitiesWriter>();
        //        zigNetEntitiesWriterMock.Setup(zewm => zewm.GetLatestTestResults()).Returns(
        //            new List<LatestTestResult>().AsQueryable);

        //        var zigNetEntityFrameworkDatabase = new ZigNetEntityFrameworkDatabase(zigNetEntitiesWriterMock.Object);
        //        var latestTestResults = zigNetEntityFrameworkDatabase.GetLatestTestResults(1).ToList();

        //        Assert.AreEqual(0, latestTestResults.Count);
        //    }

        //    [TestMethod]
        //    public void SortsByFailingTheLongestThenPassingTheShortest()
        //    {
        //        var utcNow = DateTime.UtcNow;
        //        var zigNetEntitiesWriterMock = new Mock<IZigNetEntitiesWriter>();
        //        zigNetEntitiesWriterMock.Setup(zewm => zewm.GetLatestTestResults()).Returns(
        //            new List<LatestTestResult> { 
        //                new LatestTestResult 
        //                {
        //                    SuiteId = 1,
        //                    TestResultId = 2,
        //                    TestName = "test passing the longest",
        //                    PassingFromDateTime = new DateTime(2018, 3, 1, 1, 00, 00),
        //                },
        //                new LatestTestResult 
        //                {
        //                    SuiteId = 1,
        //                    TestResultId = 3,
        //                    TestName = "test failing the longest",
        //                    FailingFromDateTime = new DateTime(2018, 3, 1, 1, 00, 00),
        //                },
        //                new LatestTestResult 
        //                {
        //                    SuiteId = 1,
        //                    TestResultId = 4,
        //                    TestName = "test passing the shortest",
        //                    PassingFromDateTime = new DateTime(2018, 3, 1, 1, 01, 00),
        //                },
        //                new LatestTestResult 
        //                {
        //                    SuiteId = 1,
        //                    TestResultId = 5,
        //                    TestName = "test failing the shortest",
        //                    FailingFromDateTime = new DateTime(2018, 3, 1, 1, 01, 00),
        //                }
        //            }.AsQueryable);

        //        var zigNetEntityFrameworkDatabase = new ZigNetEntityFrameworkDatabase(zigNetEntitiesWriterMock.Object);
        //        var latestTestResults = zigNetEntityFrameworkDatabase.GetLatestTestResults(1).ToList();

        //        Assert.AreEqual(4, latestTestResults.Count);
        //        Assert.AreEqual("test failing the longest", latestTestResults[0].TestName);
        //        Assert.AreEqual("test failing the shortest", latestTestResults[1].TestName);
        //        Assert.AreEqual("test passing the shortest", latestTestResults[2].TestName);
        //        Assert.AreEqual("test passing the longest", latestTestResults[3].TestName);
        //    }
        //}
    }
}
