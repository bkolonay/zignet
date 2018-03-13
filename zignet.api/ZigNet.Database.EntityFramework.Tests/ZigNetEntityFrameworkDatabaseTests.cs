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
                var zigNetEntitiesWrapperMock = new Mock<IZigNetEntitiesWrapper>();
                zigNetEntitiesWrapperMock.Setup(zewm => zewm.SaveSuite(It.IsAny<Suite>())).Returns(1);

                var suite = new ZigNetSuite { Name = "new suite", Categories = new List<ZigNetSuiteCategory>() };

                var zigNetEntityFrameworkDatabase = new ZigNetEntityFrameworkDatabase(zigNetEntitiesWrapperMock.Object);
                var suiteId = zigNetEntityFrameworkDatabase.SaveSuite(suite);

                Assert.AreEqual(1, suiteId);
            }

            [TestMethod]
            public void SavesNewSuiteWithEmptyCategoryList()
            {
                var zigNetEntitiesWrapperMock = new Mock<IZigNetEntitiesWrapper>();
                zigNetEntitiesWrapperMock.Setup(zewm => zewm.SaveSuite(It.IsAny<Suite>())).Returns(1);

                var suite = new ZigNetSuite { Name = "new suite", Categories = new List<ZigNetSuiteCategory>() };

                var zigNetEntityFrameworkDatabase = new ZigNetEntityFrameworkDatabase(zigNetEntitiesWrapperMock.Object);
                var suiteId = zigNetEntityFrameworkDatabase.SaveSuite(suite);

                Assert.AreEqual(1, suiteId);
            }

            [TestMethod]
            public void SavesNewSuiteWithOnlyNewCategories()
            {
                var zigNetEntitiesWrapperMock = new Mock<IZigNetEntitiesWrapper>();
                zigNetEntitiesWrapperMock.Setup(zewm => zewm.SaveSuite(It.IsAny<Suite>())).Returns(1);
                zigNetEntitiesWrapperMock.Setup(zewm => zewm.GetSuiteCategories()).Returns(new List<SuiteCategory>().AsQueryable());

                var suite = new ZigNetSuite
                {
                    Name = "new suite",
                    Categories = new List<ZigNetSuiteCategory> {
                        new ZigNetSuiteCategory { Name = "suite category 1" }
                    }
                };

                var zigNetEntityFrameworkDatabase = new ZigNetEntityFrameworkDatabase(zigNetEntitiesWrapperMock.Object);
                var suiteId = zigNetEntityFrameworkDatabase.SaveSuite(suite);

                Assert.AreEqual(1, suiteId);
            }

            [TestMethod]
            public void SavesNewSuiteWithOnlyExistingCategories()
            {
                var zigNetEntitiesWrapperMock = new Mock<IZigNetEntitiesWrapper>();
                zigNetEntitiesWrapperMock.Setup(zewm => zewm.SaveSuite(It.IsAny<Suite>())).Returns(1);
                zigNetEntitiesWrapperMock.Setup(zewm => zewm.GetSuiteCategories()).Returns(new List<SuiteCategory> {
                    new SuiteCategory { CategoryName = "suite category 1" }
                }.AsQueryable());

                var suite = new ZigNetSuite
                {
                    Name = "new suite",
                    Categories = new List<ZigNetSuiteCategory> {
                        new ZigNetSuiteCategory { Name = "suite category 1" }
                    }
                };

                var zigNetEntityFrameworkDatabase = new ZigNetEntityFrameworkDatabase(zigNetEntitiesWrapperMock.Object);
                var suiteId = zigNetEntityFrameworkDatabase.SaveSuite(suite);

                Assert.AreEqual(1, suiteId);
            }

            [TestMethod]
            public void SavesNewSuiteWithNewAndExistingCategories()
            {
                var zigNetEntitiesWrapperMock = new Mock<IZigNetEntitiesWrapper>();
                zigNetEntitiesWrapperMock.Setup(zewm => zewm.SaveSuite(It.IsAny<Suite>())).Returns(1);
                zigNetEntitiesWrapperMock.Setup(zewm => zewm.GetSuiteCategories()).Returns(new List<SuiteCategory> {
                    new SuiteCategory { CategoryName = "suite category 1" },
                    new SuiteCategory { CategoryName = "suite category 2" },
                }.AsQueryable());

                var suite = new ZigNetSuite
                {
                    Name = "new suite",
                    Categories = new List<ZigNetSuiteCategory> {
                        new ZigNetSuiteCategory { Name = "suite category 1" },
                        new ZigNetSuiteCategory { Name = "suite category 3" }
                    }
                };

                var zigNetEntityFrameworkDatabase = new ZigNetEntityFrameworkDatabase(zigNetEntitiesWrapperMock.Object);
                var suiteId = zigNetEntityFrameworkDatabase.SaveSuite(suite);

                Assert.AreEqual(1, suiteId);
            }

            [TestMethod]
            public void SavesExistingSuiteWithExistingCategory()
            {
                var zigNetEntitiesWrapperMock = new Mock<IZigNetEntitiesWrapper>();
                zigNetEntitiesWrapperMock.Setup(zewm => zewm.GetSuite(1)).Returns(new Suite { SuiteID = 1 });
                zigNetEntitiesWrapperMock.Setup(zewm => zewm.GetSuiteCategories()).Returns(new List<SuiteCategory> {
                    new SuiteCategory { SuiteCategoryID = 1, CategoryName = "suite category 1" }
                }.AsQueryable());
                zigNetEntitiesWrapperMock.Setup(zewm => zewm.SaveSuite(It.IsAny<Suite>())).Returns(1);

                var suite = new ZigNetSuite
                {
                    SuiteID = 1,
                    Name = "existing suite",
                    Categories = new List<ZigNetSuiteCategory> {
                        new ZigNetSuiteCategory { Name = "suite category 1" }
                    }
                };

                var zigNetEntityFrameworkDatabase = new ZigNetEntityFrameworkDatabase(zigNetEntitiesWrapperMock.Object);
                var suiteId = zigNetEntityFrameworkDatabase.SaveSuite(suite);

                Assert.AreEqual(1, suiteId);
            }

            [TestMethod]
            public void SavesExistingSuiteWithNewCategory()
            {
                var zigNetEntitiesWrapperMock = new Mock<IZigNetEntitiesWrapper>();
                zigNetEntitiesWrapperMock.Setup(zewm => zewm.GetSuite(1)).Returns(new Suite { SuiteID = 1 });
                zigNetEntitiesWrapperMock.Setup(zewm => zewm.GetSuiteCategories()).Returns(new List<SuiteCategory> {
                    new SuiteCategory { SuiteCategoryID = 1, CategoryName = "suite category 1" }
                }.AsQueryable());
                zigNetEntitiesWrapperMock.Setup(zewm => zewm.SaveSuite(It.IsAny<Suite>())).Returns(1);

                var suite = new ZigNetSuite
                {
                    SuiteID = 1,
                    Name = "existing suite",
                    Categories = new List<ZigNetSuiteCategory> {
                        new ZigNetSuiteCategory { Name = "suite category 2" }
                    }
                };

                var zigNetEntityFrameworkDatabase = new ZigNetEntityFrameworkDatabase(zigNetEntitiesWrapperMock.Object);
                var suiteId = zigNetEntityFrameworkDatabase.SaveSuite(suite);

                Assert.AreEqual(1, suiteId);
            }
        }

        [TestClass]
        public class SaveSuiteResultMethod
        {
            [TestMethod]
            public void SavesNewSuiteResult()
            {
                var zigNetEntitiesWrapperMock = new Mock<IZigNetEntitiesWrapper>();
                zigNetEntitiesWrapperMock.Setup(zewm => zewm.SaveSuiteResult(It.IsAny<SuiteResult>())).Returns(1);

                var suiteResult = new ZigNetSuiteResult
                {
                    Suite = new ZigNetSuite { SuiteID = 2 },
                    StartTime = DateTime.UtcNow,
                    ResultType = ZigNetSuiteResultType.Inconclusive
                };

                var zigNetEntityFrameworkDatabase = new ZigNetEntityFrameworkDatabase(zigNetEntitiesWrapperMock.Object);
                var suiteResultId = zigNetEntityFrameworkDatabase.SaveSuiteResult(suiteResult);

                Assert.AreEqual(1, suiteResultId);
            }

            [TestMethod]
            public void SavesExistingSuiteResult()
            {
                var zigNetEntitiesWrapperMock = new Mock<IZigNetEntitiesWrapper>();
                var suiteResult = new ZigNetSuiteResult
                {
                    SuiteResultID = 2,
                    Suite = new ZigNetSuite { SuiteID = 3 },
                    StartTime = DateTime.UtcNow,
                    EndTime = DateTime.UtcNow,
                    ResultType = ZigNetSuiteResultType.Pass
                };
                zigNetEntitiesWrapperMock.Setup(zewm => zewm.GetSuiteResult(2)).Returns(new SuiteResult { SuiteResultID = 2 });
                zigNetEntitiesWrapperMock.Setup(zewm => zewm.SaveSuiteResult(It.IsAny<SuiteResult>())).Returns(2);

                var zigNetEntityFrameworkDatabase = new ZigNetEntityFrameworkDatabase(zigNetEntitiesWrapperMock.Object);
                var suiteResultId = zigNetEntityFrameworkDatabase.SaveSuiteResult(suiteResult);

                Assert.AreEqual(2, suiteResultId);
            }
        }

        [TestClass]
        public class GetSuiteResultMethod
        {
            [TestMethod]
            public void MapsDatabaseSuiteResult()
            {
                var startTime = DateTime.UtcNow;
                var endTime = DateTime.UtcNow;

                var zigNetEntitiesWrapperMock = new Mock<IZigNetEntitiesWrapper>();
                zigNetEntitiesWrapperMock.Setup(zewm => zewm.GetSuiteResult(1)).Returns(
                    new SuiteResult
                    {
                        SuiteResultID = 1,
                        SuiteResultStartDateTime = startTime,
                        SuiteResultEndDateTime = endTime,
                        SuiteResultType = new SuiteResultType { SuiteResultTypeName = "Fail" },
                        Suite = new Suite
                        {
                            SuiteID = 2,
                            SuiteCategories = new List<SuiteCategory> { new SuiteCategory { CategoryName = "suite category 1" } }
                        }
                    }
                );

                var zigNetEntityFrameworkDatabase = new ZigNetEntityFrameworkDatabase(zigNetEntitiesWrapperMock.Object);
                var suiteResult = zigNetEntityFrameworkDatabase.GetSuiteResult(1);

                Assert.AreEqual(1, suiteResult.SuiteResultID);
                Assert.AreEqual(startTime, suiteResult.StartTime);
                Assert.AreEqual(endTime, suiteResult.EndTime);
                Assert.AreEqual(ZigNetSuiteResultType.Fail, suiteResult.ResultType);
                Assert.AreEqual(2, suiteResult.Suite.SuiteID);
                Assert.AreEqual("suite category 1", suiteResult.Suite.Categories.ElementAt(0).Name);
            }

            [TestMethod]
            public void DoesNotThrowWhenEndDateTimeNull()
            {
                var zigNetEntitiesWrapperMock = new Mock<IZigNetEntitiesWrapper>();
                zigNetEntitiesWrapperMock.Setup(zewm => zewm.GetSuiteResult(1)).Returns(
                    new SuiteResult
                    {
                        SuiteResultID = 1,
                        SuiteResultEndDateTime = null,
                        SuiteResultType = new SuiteResultType { SuiteResultTypeName = "Fail" },
                        Suite = new Suite()
                    }
                );

                var zigNetEntityFrameworkDatabase = new ZigNetEntityFrameworkDatabase(zigNetEntitiesWrapperMock.Object);
                var suiteResult = zigNetEntityFrameworkDatabase.GetSuiteResult(1);

                Assert.AreEqual(1, suiteResult.SuiteResultID);
                Assert.IsNull(suiteResult.EndTime);
            }
        }

        [TestClass]
        public class GetTestMethod
        {
            [TestMethod]
            public void MapsDatabaseTest()
            {
                var zigNetEntitiesWrapperMock = new Mock<IZigNetEntitiesWrapper>();
                zigNetEntitiesWrapperMock.Setup(zewm => zewm.GetTestOrDefault("test 1")).Returns(
                    new Test
                    {
                        TestID = 1,
                        TestName = "test 1",
                        TestCategories = new List<TestCategory> { new TestCategory { CategoryName = "test category 1" } }
                    }
                );

                var zigNetEntityFrameworkDatabase = new ZigNetEntityFrameworkDatabase(zigNetEntitiesWrapperMock.Object);
                var test = zigNetEntityFrameworkDatabase.GetTestOrDefault("test 1");

                Assert.AreEqual(1, test.TestID);
                Assert.AreEqual("test 1", test.Name);
                Assert.AreEqual("test category 1", test.Categories.ElementAt(0).Name);
            }

            [TestMethod]
            public void ReturnsNullWhenTestDoesNotExist()
            {
                var zigNetEntitiesWrapperMock = new Mock<IZigNetEntitiesWrapper>();
                zigNetEntitiesWrapperMock.Setup(zewm => zewm.GetTestOrDefault("test 1")).Returns((Test)null);

                var zigNetEntityFrameworkDatabase = new ZigNetEntityFrameworkDatabase(zigNetEntitiesWrapperMock.Object);
                var test = zigNetEntityFrameworkDatabase.GetTestOrDefault("test 1");

                Assert.IsNull(test);
            }

            [TestMethod]
            public void MapsTestWithEmptyCategoryList()
            {
                var zigNetEntitiesWrapperMock = new Mock<IZigNetEntitiesWrapper>();
                zigNetEntitiesWrapperMock.Setup(zewm => zewm.GetTestOrDefault("test 1")).Returns(
                    new Test
                    {
                        TestID = 1,
                        TestName = "test 1",
                        TestCategories = new List<TestCategory>()
                    }
                );

                var zigNetEntityFrameworkDatabase = new ZigNetEntityFrameworkDatabase(zigNetEntitiesWrapperMock.Object);
                var test = zigNetEntityFrameworkDatabase.GetTestOrDefault("test 1");

                Assert.AreEqual(1, test.TestID);
                Assert.AreEqual("test 1", test.Name);
                Assert.AreEqual(0, test.Categories.Count);
            }
        }

        [TestClass]
        public class SaveTestResultMethod
        {
            [TestMethod]
            public void SavesFailedTestResultWithDetails()
            {
                var zigNetEntitiesWrapperMock = new Mock<IZigNetEntitiesWrapper>();
                zigNetEntitiesWrapperMock.Setup(zewm => zewm.GetTestFailureType(2)).Returns(
                    new TestFailureType
                    {
                        TestFailureTypeID = 2,
                        TestFailureTypeName = "Exception"
                    }
                );
                zigNetEntitiesWrapperMock.Setup(zewm => zewm.GetTestCategories()).Returns(
                    new List<TestCategory> { new TestCategory { TestCategoryID = 1, CategoryName = "test category 1" } }.AsQueryable);
                zigNetEntitiesWrapperMock.Setup(zewm => zewm.GetSuiteResult(1)).Returns(new SuiteResult { SuiteId = 2 });
                zigNetEntitiesWrapperMock.Setup(zewm => zewm.GetSuite(2)).Returns(new Suite { SuiteID = 2 });

                var testResult = new ZigNetTestResult
                {
                    Test = new ZigNetTest { Name = "test 1", Categories = new List<ZigNetTestCategory> { new ZigNetTestCategory { Name = "test category 1" } } },
                    SuiteResult = new ZigNetSuiteResult { SuiteResultID = 1 },
                    StartTime = DateTime.UtcNow,
                    EndTime = DateTime.UtcNow,
                    ResultType = ZigNetTestResultType.Fail,
                    TestFailureDetails = new ZigNetTestFailureDetails
                    {
                        FailureType = ZigNetTestFailureType.Exception,
                        FailureDetailMessage = "failed by exception at line 5"
                    }
                };

                var zigNetEntityFrameworkDatabase = new ZigNetEntityFrameworkDatabase(zigNetEntitiesWrapperMock.Object);
                zigNetEntityFrameworkDatabase.SaveTestResult(testResult);
            }

            [TestMethod]
            public void SavesExistingTestPassedTestResult()
            {
                var zigNetEntitiesWrapperMock = new Mock<IZigNetEntitiesWrapper>();
                zigNetEntitiesWrapperMock.Setup(zewm => zewm.GetTestCategories()).Returns(
                    new List<TestCategory> {
                        new TestCategory { TestCategoryID = 1, CategoryName = "test category 1" }
                    }.AsQueryable
                );
                zigNetEntitiesWrapperMock.Setup(zewm => zewm.GetTest(1)).Returns(
                    new Test
                    {
                        TestID = 1,
                        TestCategories = new List<TestCategory> { new TestCategory { TestCategoryID = 1, CategoryName = "test category 1" } },
                        Suites = new List<Suite> { new Suite { SuiteID = 2 } }
                    }
                );
                zigNetEntitiesWrapperMock.Setup(zewm => zewm.GetSuiteResult(1)).Returns(new SuiteResult { SuiteId = 2 });

                var testResult = new ZigNetTestResult
                {
                    Test = new ZigNetTest
                    {
                        TestID = 1,
                        Name = "test 1",
                        Categories = new List<ZigNetTestCategory> { new ZigNetTestCategory { Name = "test category 1" } }
                    },
                    SuiteResult = new ZigNetSuiteResult { SuiteResultID = 1 },
                    StartTime = DateTime.UtcNow,
                    EndTime = DateTime.UtcNow,
                    ResultType = ZigNetTestResultType.Pass,
                };

                var zigNetEntityFrameworkDatabase = new ZigNetEntityFrameworkDatabase(zigNetEntitiesWrapperMock.Object);
                zigNetEntityFrameworkDatabase.SaveTestResult(testResult);
            }

            [TestMethod]
            public void SavesExistingTestFailedTestResult()
            {
                var zigNetEntitiesWrapperMock = new Mock<IZigNetEntitiesWrapper>();
                zigNetEntitiesWrapperMock.Setup(zewm => zewm.GetTestFailureType(1)).Returns(
                    new TestFailureType
                    {
                        TestFailureTypeID = 1,
                        TestFailureTypeName = "Assertion"
                    }
                );
                zigNetEntitiesWrapperMock.Setup(zewm => zewm.GetTestCategories()).Returns(
                    new List<TestCategory> {
                        new TestCategory { TestCategoryID = 1, CategoryName = "test category 1" }
                    }.AsQueryable
                );
                zigNetEntitiesWrapperMock.Setup(zewm => zewm.GetTest(1)).Returns(
                    new Test
                    {
                        TestID = 1,
                        TestCategories = new List<TestCategory> { new TestCategory { TestCategoryID = 1, CategoryName = "test category 1" } },
                        Suites = new List<Suite> { new Suite { SuiteID = 2 } }
                    }
                );
                zigNetEntitiesWrapperMock.Setup(zewm => zewm.GetSuiteResult(1)).Returns(new SuiteResult { SuiteId = 2 });

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
                    SuiteResult = new ZigNetSuiteResult { SuiteResultID = 1 },
                    StartTime = DateTime.UtcNow,
                    EndTime = DateTime.UtcNow,
                    ResultType = ZigNetTestResultType.Fail,
                    TestFailureDetails = new ZigNetTestFailureDetails
                    {
                        FailureType = ZigNetTestFailureType.Assertion
                    }
                };

                var zigNetEntityFrameworkDatabase = new ZigNetEntityFrameworkDatabase(zigNetEntitiesWrapperMock.Object);
                zigNetEntityFrameworkDatabase.SaveTestResult(testResult);
            }

            [TestMethod]
            public void DoesNotThrowWhenClearingAllTestCategories()
            {
                var zigNetEntitiesWrapperMock = new Mock<IZigNetEntitiesWrapper>();
                zigNetEntitiesWrapperMock.Setup(zewm => zewm.GetTest(1)).Returns(
                    new Test
                    {
                        TestID = 1,
                        TestCategories = new List<TestCategory> { new TestCategory { TestCategoryID = 1, CategoryName = "test category 1" } },
                        Suites = new List<Suite> { new Suite { SuiteID = 2 } }
                    }
                );
                zigNetEntitiesWrapperMock.Setup(zewm => zewm.GetSuiteResult(1)).Returns(new SuiteResult { SuiteId = 2 });

                var testResult = new ZigNetTestResult
                {
                    Test = new ZigNetTest
                    {
                        TestID = 1,
                        Name = "test 1",
                        Categories = new List<ZigNetTestCategory>()
                    },
                    SuiteResult = new ZigNetSuiteResult { SuiteResultID = 1 },
                    StartTime = DateTime.UtcNow,
                    EndTime = DateTime.UtcNow,
                    ResultType = ZigNetTestResultType.Pass
                };

                var zigNetEntityFrameworkDatabase = new ZigNetEntityFrameworkDatabase(zigNetEntitiesWrapperMock.Object);
                zigNetEntityFrameworkDatabase.SaveTestResult(testResult);
            }
        }

        [TestClass]
        public class GetLatestTestResultInSuiteMethod
        {
            [TestMethod]
            public void GetsLatestTestResultInMultipleSuiteResults()
            {
                var zigNetEntitiesWrapperMock = new Mock<IZigNetEntitiesWrapper>();
                zigNetEntitiesWrapperMock.Setup(zewm => zewm.GetSuiteResults()).Returns(
                    new List<SuiteResult> {
                        new SuiteResult
                        {
                            SuiteId = 1,
                            TestResults = new List<TestResult>
                            {
                                new TestResult
                                {
                                    TestId = 2,
                                    TestResultEndDateTime = new DateTime(2018, 3, 13, 15, 30, 27),
                                    TestResultType = new TestResultType { TestResultTypeName = "Pass" },
                                    Test = new Test { TestID = 2 }
                                },
                                new TestResult
                                {
                                    TestId = 2,
                                    TestResultEndDateTime = new DateTime(2018, 3, 14, 15, 30, 27),
                                    TestResultType = new TestResultType { TestResultTypeName = "Pass" },
                                    Test = new Test { TestID = 2 }
                                }
                            }
                        },
                        new SuiteResult
                        {
                            SuiteId = 1,
                            TestResults = new List<TestResult>
                            {
                                new TestResult
                                {
                                    TestId = 2,
                                    TestResultEndDateTime = new DateTime(2018, 3, 15, 15, 30, 27),
                                    TestResultType = new TestResultType { TestResultTypeName = "Pass" },
                                    Test = new Test { TestID = 2 }
                                },
                                new TestResult
                                {
                                    TestId = 2,
                                    TestResultEndDateTime = new DateTime(2018, 3, 16, 15, 30, 27),
                                    TestResultType = new TestResultType { TestResultTypeName = "Pass" },
                                    Test = new Test { TestID = 2 }
                                }
                            }
                        }
                    }.AsQueryable
                );

                var zigNetEntityFrameworkDatabase = new ZigNetEntityFrameworkDatabase(zigNetEntitiesWrapperMock.Object);

                var latestTestResultInSuite = zigNetEntityFrameworkDatabase.GetLatestTestResultInSuite(2, 1);

                Assert.AreEqual(2, latestTestResultInSuite.Test.TestID);
                Assert.AreEqual(new DateTime(2018, 3, 16, 15, 30, 27), latestTestResultInSuite.EndTime);
            }

            [TestMethod]
            public void IgnoresTestResultsWithoutTestId()
            {
                var zigNetEntitiesWrapperMock = new Mock<IZigNetEntitiesWrapper>();
                zigNetEntitiesWrapperMock.Setup(zewm => zewm.GetSuiteResults()).Returns(
                    new List<SuiteResult> {
                        new SuiteResult
                        {
                            SuiteId = 1,
                            TestResults = new List<TestResult>
                            {
                                new TestResult
                                {
                                    TestId = 2,
                                    TestResultEndDateTime = new DateTime(2018, 3, 13, 15, 30, 27),
                                    TestResultType = new TestResultType { TestResultTypeName = "Pass" },
                                    Test = new Test { TestID = 2 }
                                },
                                new TestResult
                                {
                                    TestId = 3,
                                    TestResultEndDateTime = new DateTime(2018, 3, 14, 15, 30, 27),
                                    TestResultType = new TestResultType { TestResultTypeName = "Pass" },
                                    Test = new Test { TestID = 2 }
                                }
                            }
                        }
                    }.AsQueryable
                );

                var zigNetEntityFrameworkDatabase = new ZigNetEntityFrameworkDatabase(zigNetEntitiesWrapperMock.Object);

                var latestTestResultInSuite = zigNetEntityFrameworkDatabase.GetLatestTestResultInSuite(2, 1);

                Assert.AreEqual(2, latestTestResultInSuite.Test.TestID);
                Assert.AreEqual(new DateTime(2018, 3, 13, 15, 30, 27), latestTestResultInSuite.EndTime);
            }

            [TestMethod]
            public void DoesNotThrowWhenZeroSuiteResultsReturned()
            {
                var zigNetEntitiesWrapperMock = new Mock<IZigNetEntitiesWrapper>();
                zigNetEntitiesWrapperMock.Setup(zewm => zewm.GetSuiteResults()).Returns(new List<SuiteResult>().AsQueryable);

                var zigNetEntityFrameworkDatabase = new ZigNetEntityFrameworkDatabase(zigNetEntitiesWrapperMock.Object);

                var latestTestResultInSuite = zigNetEntityFrameworkDatabase.GetLatestTestResultInSuite(2, 1);

                Assert.IsNull(latestTestResultInSuite);
            }

            [TestMethod]
            public void DoesNotThrowWhenZeroTestResultsReturned()
            {
                var zigNetEntitiesWrapperMock = new Mock<IZigNetEntitiesWrapper>();
                zigNetEntitiesWrapperMock.Setup(zewm => zewm.GetSuiteResults()).Returns(
                    new List<SuiteResult> {
                        new SuiteResult
                        {
                            SuiteId = 1,
                            TestResults = new List<TestResult>()
                        }
                    }.AsQueryable
                );

                var zigNetEntityFrameworkDatabase = new ZigNetEntityFrameworkDatabase(zigNetEntitiesWrapperMock.Object);

                var latestTestResultInSuite = zigNetEntityFrameworkDatabase.GetLatestTestResultInSuite(2, 1);

                Assert.IsNull(latestTestResultInSuite);
            }
        }
    }
}
