﻿using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ZigNet.Database;
using ZigNet.Domain.Suite;
using System.Collections.Generic;
using ZigNet.Domain.Test;
using ZigNetTestResult = ZigNet.Domain.Test.TestResult;

namespace ZigNet.Business.Tests
{
    public class ZigNetBusinessTests
    {
        [TestClass]
        public class GetLatestSuiteResultsMethod
        {
            [TestMethod]
            public void ReturnsLatestSuiteResultsFromOneSuite()
            {
                var zignetDatabaseMock = new Mock<IZigNetDatabase>();
                var suite = new Suite { SuiteID = 1, Name = "Suite 1" };
                var suites = new List<Suite> { suite };
                var suiteResults = new List<SuiteResult> {
                    new SuiteResult { Suite = suite, StartTime = DateTime.Parse("12/19/2017 10:00am"), EndTime = DateTime.Parse("12/19/2017 10:05am")},
                    new SuiteResult { Suite = suite, StartTime = DateTime.Parse("12/19/2017 10:02am"), EndTime = DateTime.Parse("12/19/2017 10:10am") },
                    new SuiteResult { Suite = suite, StartTime = DateTime.Parse("12/19/2017 10:01am"), EndTime = DateTime.Parse("12/19/2017 10:03am") }
                };
                var testResults = new List<ZigNetTestResult>{
                    new ZigNetTestResult { ResultType = TestResultType.Fail },
                    new ZigNetTestResult { ResultType = TestResultType.Inconclusive },
                    new ZigNetTestResult { ResultType = TestResultType.Pass },
                    new ZigNetTestResult { ResultType = TestResultType.Pass },
                };

                zignetDatabaseMock.Setup(zdm => zdm.GetSuites()).Returns(suites);
                zignetDatabaseMock.Setup(zdm => zdm.GetSuiteResultsForSuite(It.IsAny<int>())).Returns(suiteResults);
                zignetDatabaseMock.Setup(zdm => zdm.GetTestResultsForSuiteResult(It.IsAny<int>())).Returns(testResults);

                var zigNetBusiness = new ZigNetBusiness(zignetDatabaseMock.Object);

                var suiteSummaries = zigNetBusiness.GetLatestSuiteResults().ToList();
                var suiteSummary = suiteSummaries[0];

                Assert.AreEqual(1, suiteSummaries.Count());
                Assert.AreEqual(1, suiteSummary.SuiteID);
                Assert.AreEqual("Suite 1", suiteSummary.SuiteName);
                Assert.AreEqual(1, suiteSummary.TotalFailedTests);
                Assert.AreEqual(1, suiteSummary.TotalInconclusiveTests);
                Assert.AreEqual(2, suiteSummary.TotalPassedTests);
                Assert.AreEqual(DateTime.Parse("12/19/2017 10:10am"), suiteSummary.SuiteEndTime);
            }

            [TestMethod]
            public void DoesNotThrowWhenNoSuiteResultsForSuite()
            {
                var zignetDatabaseMock = new Mock<IZigNetDatabase>();
                var suite = new Suite { SuiteID = 1, Name = "Suite 1" };
                var suites = new List<Suite> { suite };
                var suiteResults = new List<SuiteResult> { };
                var testResults = new List<ZigNetTestResult>{
                    new ZigNetTestResult { ResultType = TestResultType.Fail },
                    new ZigNetTestResult { ResultType = TestResultType.Inconclusive },
                    new ZigNetTestResult { ResultType = TestResultType.Pass },
                    new ZigNetTestResult { ResultType = TestResultType.Pass },
                };

                zignetDatabaseMock.Setup(zdm => zdm.GetSuites()).Returns(suites);
                zignetDatabaseMock.Setup(zdm => zdm.GetSuiteResultsForSuite(It.IsAny<int>())).Returns(suiteResults);
                zignetDatabaseMock.Setup(zdm => zdm.GetTestResultsForSuiteResult(It.IsAny<int>())).Returns(testResults);

                var zigNetBusiness = new ZigNetBusiness(zignetDatabaseMock.Object);

                var suiteSummaries = zigNetBusiness.GetLatestSuiteResults().ToList();

                Assert.AreEqual(0, suiteSummaries.Count());
            }

            [TestMethod]
            public void DoesNotThrowWhenSuiteEndTimeNull()
            {
                var zignetDatabaseMock = new Mock<IZigNetDatabase>();
                var suite = new Suite { SuiteID = 1, Name = "Suite 1" };
                var suites = new List<Suite> { suite };
                var suiteResults = new List<SuiteResult> {
                    new SuiteResult { Suite = suite, StartTime = DateTime.Parse("12/19/2017 10:00am"), EndTime = DateTime.Parse("12/19/2017 10:05am")},
                    new SuiteResult { Suite = suite, StartTime = DateTime.Parse("12/19/2017 10:02am") },
                    new SuiteResult { Suite = suite, StartTime = DateTime.Parse("12/19/2017 10:01am"), EndTime = DateTime.Parse("12/19/2017 10:03am") }
                };
                var testResults = new List<ZigNetTestResult>{
                    new ZigNetTestResult { ResultType = TestResultType.Fail },
                    new ZigNetTestResult { ResultType = TestResultType.Inconclusive },
                    new ZigNetTestResult { ResultType = TestResultType.Pass },
                    new ZigNetTestResult { ResultType = TestResultType.Pass },
                };

                zignetDatabaseMock.Setup(zdm => zdm.GetSuites()).Returns(suites);
                zignetDatabaseMock.Setup(zdm => zdm.GetSuiteResultsForSuite(It.IsAny<int>())).Returns(suiteResults);
                zignetDatabaseMock.Setup(zdm => zdm.GetTestResultsForSuiteResult(It.IsAny<int>())).Returns(testResults);

                var zigNetBusiness = new ZigNetBusiness(zignetDatabaseMock.Object);

                var suiteSummaries = zigNetBusiness.GetLatestSuiteResults().ToList();
                var suiteSummary = suiteSummaries[0];

                Assert.AreEqual(1, suiteSummaries.Count());
                Assert.AreEqual(1, suiteSummary.SuiteID);
                Assert.AreEqual("Suite 1", suiteSummary.SuiteName);
                Assert.AreEqual(1, suiteSummary.TotalFailedTests);
                Assert.AreEqual(1, suiteSummary.TotalInconclusiveTests);
                Assert.AreEqual(2, suiteSummary.TotalPassedTests);
                Assert.IsNull(suiteSummary.SuiteEndTime);
            }

            [TestMethod]
            public void DoesNotThrowWhenNoSuites()
            {
                var zignetDatabaseMock = new Mock<IZigNetDatabase>();
                var suites = new List<Suite> { };

                zignetDatabaseMock.Setup(zdm => zdm.GetSuites()).Returns(suites);

                var zigNetBusiness = new ZigNetBusiness(zignetDatabaseMock.Object);

                var suiteSummaries = zigNetBusiness.GetLatestSuiteResults().ToList();

                Assert.AreEqual(0, suiteSummaries.Count());
            }

            [TestMethod]
            public void DoesNotThrowWhenNoTestResultsForSuiteResult()
            {
                var zignetDatabaseMock = new Mock<IZigNetDatabase>();
                var suite = new Suite { SuiteID = 1, Name = "Suite 1" };
                var suites = new List<Suite> { suite };
                var suiteResults = new List<SuiteResult> {
                    new SuiteResult { Suite = suite, StartTime = DateTime.Parse("12/19/2017 10:00am"), EndTime = DateTime.Parse("12/19/2017 10:05am")},
                    new SuiteResult { Suite = suite, StartTime = DateTime.Parse("12/19/2017 10:02am"), EndTime = DateTime.Parse("12/19/2017 10:10am") },
                    new SuiteResult { Suite = suite, StartTime = DateTime.Parse("12/19/2017 10:01am"), EndTime = DateTime.Parse("12/19/2017 10:03am") }
                };
                var testResults = new List<ZigNetTestResult> { };

                zignetDatabaseMock.Setup(zdm => zdm.GetSuites()).Returns(suites);
                zignetDatabaseMock.Setup(zdm => zdm.GetSuiteResultsForSuite(It.IsAny<int>())).Returns(suiteResults);
                zignetDatabaseMock.Setup(zdm => zdm.GetTestResultsForSuiteResult(It.IsAny<int>())).Returns(testResults);

                var zigNetBusiness = new ZigNetBusiness(zignetDatabaseMock.Object);

                var suiteSummaries = zigNetBusiness.GetLatestSuiteResults().ToList();
                var suiteSummary = suiteSummaries[0];

                Assert.AreEqual(1, suiteSummaries.Count());
                Assert.AreEqual(1, suiteSummary.SuiteID);
                Assert.AreEqual("Suite 1", suiteSummary.SuiteName);
                Assert.AreEqual(0, suiteSummary.TotalFailedTests);
                Assert.AreEqual(0, suiteSummary.TotalInconclusiveTests);
                Assert.AreEqual(0, suiteSummary.TotalPassedTests);
                Assert.AreEqual(DateTime.Parse("12/19/2017 10:10am"), suiteSummary.SuiteEndTime);
            }
        }

        [TestClass]
        public class CreateSuiteMethod
        {
            [TestMethod]
            public void CreatesSuite()
            {
                var zignetDatabase = new Mock<IZigNetDatabase>();
                zignetDatabase.Setup(zd => zd.SaveSuite(It.IsAny<Suite>())).Returns(1);

                var zigNetBusiness = new ZigNetBusiness(zignetDatabase.Object);
                var suiteId = zigNetBusiness.CreateSuite(new Suite());

                Assert.AreEqual(1, suiteId);
            }

            [TestMethod]
            [ExpectedException(typeof(InvalidOperationException))]
            public void ThrowsWhenSuiteHasSameName()
            {
                var zignetDatabase = new Mock<IZigNetDatabase>();
                zignetDatabase.Setup(zd => zd.GetSuites()).Returns(new List<Suite> { new Suite { Name = "suite 1" } });

                var suite = new Suite
                {
                    Name = "suite 1"
                };

                var zigNetBusiness = new ZigNetBusiness(zignetDatabase.Object);
                var suiteId = zigNetBusiness.CreateSuite(suite);
            }
        }

        [TestClass]
        public class AddSuiteCategoryMethod
        {
            [TestMethod]
            public void DoesNotThrowWhenSuiteCategoryAlreadyAssignedToSuite()
            {
                var zignetDatabase = new Mock<IZigNetDatabase>();
                zignetDatabase.Setup(zd => zd.GetSuiteCategoriesForSuite(1)).Returns(new List<SuiteCategory> {
                    new SuiteCategory { Name = "suite category 1" }
                });

                var zigNetBusiness = new ZigNetBusiness(zignetDatabase.Object);
                zigNetBusiness.AddSuiteCategory(1, "suite category 1");
            }

            [TestMethod]
            public void DoesNotThrowWhenSuiteCategoryAssignedToSuite()
            {
                var zignetDatabase = new Mock<IZigNetDatabase>();
                zignetDatabase.Setup(zd => zd.GetSuites()).Returns(new List<Suite> {
                    new Suite { SuiteID = 1, Categories = new List<SuiteCategory>() }
                });

                var zigNetBusiness = new ZigNetBusiness(zignetDatabase.Object);
                zigNetBusiness.AddSuiteCategory(1, "suite category 2");
            }

            [TestMethod]
            [ExpectedException(typeof(InvalidOperationException))]
            public void ThrowsWhenSuiteIdDoesNotExist()
            {
                var zignetDatabase = new Mock<IZigNetDatabase>();

                var zigNetBusiness = new ZigNetBusiness(zignetDatabase.Object);
                zigNetBusiness.AddSuiteCategory(99, "suite category 1");
            }

            [TestMethod]
            [ExpectedException(typeof(InvalidOperationException))]
            public void ThrowsWhenSuiteCategoryNull()
            {
                var zignetDatabase = new Mock<IZigNetDatabase>();

                var zigNetBusiness = new ZigNetBusiness(zignetDatabase.Object);
                zigNetBusiness.AddSuiteCategory(1, null);
            }
        }

        [TestClass]
        public class DeleteSuiteCategoryMethod
        {
            [TestMethod]
            public void DoesNotThrowWhenSuiteCategoryNotAssignedToSuite()
            {
                var zignetDatabase = new Mock<IZigNetDatabase>();
                zignetDatabase.Setup(zd => zd.GetSuiteCategoriesForSuite(1)).Returns(new List<SuiteCategory> {
                    new SuiteCategory { Name = "suite category 1" }
                });

                var zigNetBusiness = new ZigNetBusiness(zignetDatabase.Object);
                zigNetBusiness.DeleteSuiteCategory(1, "suite category 2");
            }

            [TestMethod]
            public void DoesNotThrowWhenSuiteCategoryDeletedFromSuite()
            {
                var zignetDatabase = new Mock<IZigNetDatabase>();
                zignetDatabase.Setup(zd => zd.GetSuiteCategoriesForSuite(1)).Returns(new List<SuiteCategory> {
                    new SuiteCategory { Name = "suite category 1" }
                });
                zignetDatabase.Setup(zd => zd.GetSuites()).Returns(new List<Suite> {
                    new Suite {
                        SuiteID = 1,
                        Categories = new List<SuiteCategory> {
                            new SuiteCategory { Name = "suite category 1" }
                        }

                    }
                });

                var zigNetBusiness = new ZigNetBusiness(zignetDatabase.Object);
                zigNetBusiness.DeleteSuiteCategory(1, "suite category 1");
            }

            [TestMethod]
            [ExpectedException(typeof(InvalidOperationException))]
            public void ThrowsWhenSuiteIdDoesNotExist()
            {
                var zignetDatabase = new Mock<IZigNetDatabase>();
                zignetDatabase.Setup(zd => zd.GetSuiteCategoriesForSuite(99)).Returns(new List<SuiteCategory> {
                    new SuiteCategory { Name = "suite category 1" }
                });

                var zigNetBusiness = new ZigNetBusiness(zignetDatabase.Object);
                zigNetBusiness.DeleteSuiteCategory(99, "suite category 1");
            }

            [TestMethod]
            [ExpectedException(typeof(InvalidOperationException))]
            public void ThrowsWhenSuiteCategoryNull()
            {
                var zignetDatabase = new Mock<IZigNetDatabase>();

                var zigNetBusiness = new ZigNetBusiness(zignetDatabase.Object);
                zigNetBusiness.DeleteSuiteCategory(1, null);
            }
        }

        [TestClass]
        public class StartSuiteByIdMethod
        {
            [TestMethod]
            public void ReturnsSuiteResultId()
            {
                var zignetDatabase = new Mock<IZigNetDatabase>();
                zignetDatabase.Setup(zd => zd.GetSuites()).Returns(new List<Suite> { new Suite { SuiteID = 1 } });
                zignetDatabase.Setup(zd => zd.SaveSuiteResult(It.IsAny<SuiteResult>())).Returns(2);

                var zigNetBusiness = new ZigNetBusiness(zignetDatabase.Object);
                var suiteResultId = zigNetBusiness.StartSuite(1);

                Assert.AreEqual(2, suiteResultId);
            }

            [TestMethod]
            [ExpectedException(typeof(InvalidOperationException))]
            public void ThrowsWhenSuiteIdDoesNotExist()
            {
                var zignetDatabase = new Mock<IZigNetDatabase>();

                var zigNetBusiness = new ZigNetBusiness(zignetDatabase.Object);
                zigNetBusiness.StartSuite(1);
            }
        }

        [TestClass]
        public class StartSuiteByNameMethod
        {
            [TestMethod]
            public void ReturnsSuiteResultId()
            {
                var zignetDatabase = new Mock<IZigNetDatabase>();
                zignetDatabase.Setup(zd => zd.GetSuites()).Returns(new List<Suite> { new Suite { Name = "suite 1" } });
                zignetDatabase.Setup(zd => zd.SaveSuiteResult(It.IsAny<SuiteResult>())).Returns(3);

                var zigNetBusiness = new ZigNetBusiness(zignetDatabase.Object);
                var suiteResultId = zigNetBusiness.StartSuite("suite 1");

                Assert.AreEqual(3, suiteResultId);
            }

            [TestMethod]
            [ExpectedException(typeof(InvalidOperationException))]
            public void ThrowsWhenSuiteIdDoesNotExist()
            {
                var zignetDatabase = new Mock<IZigNetDatabase>();

                var zigNetBusiness = new ZigNetBusiness(zignetDatabase.Object);
                zigNetBusiness.StartSuite("suite 2");
            }
        }

        [TestClass]
        public class EndSuiteMethod
        {
            [TestMethod]
            public void DoesNotThrowWhenSuiteResultAndResultTypePassed()
            {
                var zignetDatabase = new Mock<IZigNetDatabase>();
                zignetDatabase.Setup(zd => zd.GetSuiteResult(1)).Returns(new SuiteResult { SuiteResultID = 1 });

                var zigNetBusiness = new ZigNetBusiness(zignetDatabase.Object);
                zigNetBusiness.EndSuite(1, SuiteResultType.Fail);
            }

            [TestMethod]
            [ExpectedException(typeof(InvalidOperationException))]
            public void ThrowsWhenSuiteResultIdDoesNotExist()
            {
                var zignetDatabase = new Mock<IZigNetDatabase>();
                zignetDatabase.Setup(zd => zd.GetSuiteResult(1)).Throws(new InvalidOperationException());

                var zigNetBusiness = new ZigNetBusiness(zignetDatabase.Object);
                zigNetBusiness.EndSuite(1, SuiteResultType.Fail);
            }
        }

        [TestClass]
        public class SaveTestResultMethod
        {
            [TestMethod]
            [ExpectedException(typeof(ArgumentNullException))]
            public void ThrowsWhenTestNameNull()
            {
                var zignetDatabase = new Mock<IZigNetDatabase>();

                var testResult = new ZigNetTestResult { Test = new Test() };

                var zigNetBusiness = new ZigNetBusiness(zignetDatabase.Object);
                zigNetBusiness.SaveTestResult(testResult);
            }

            [TestMethod]
            public void AssignsTestIdWhenTestWithSameNameExists()
            {
                var zignetDatabase = new Mock<IZigNetDatabase>();
                zignetDatabase.Setup(zd => zd.GetTestOrDefault("test 1")).Returns(new Test { Name = "test 1", TestID = 1, Categories = new List<TestCategory>() });

                var testResult = new ZigNetTestResult { Test = new Test { Name = "test 1", Categories = new List<TestCategory>() }, SuiteResult = new SuiteResult() };

                var zigNetBusiness = new ZigNetBusiness(zignetDatabase.Object);
                zigNetBusiness.SaveTestResult(testResult);
            }

            [TestMethod]
            public void DoesNotAssignsTestIdWhenTestWithSameNameDoesNotExist()
            {
                var zignetDatabase = new Mock<IZigNetDatabase>();

                var testResult = new ZigNetTestResult { Test = new Test { Name = "test 1" }, SuiteResult = new SuiteResult() };

                var zigNetBusiness = new ZigNetBusiness(zignetDatabase.Object);
                zigNetBusiness.SaveTestResult(testResult);
            }

            [TestMethod]
            [ExpectedException(typeof(InvalidOperationException))]
            public void ThrowsIfSuiteResultNull()
            {
                var zignetDatabase = new Mock<IZigNetDatabase>();
                zignetDatabase.Setup(zd => zd.GetSuiteResult(1)).Throws(new InvalidOperationException());

                var testResult = new ZigNetTestResult { Test = new Test { Name = "test 1" }, SuiteResult = new SuiteResult { SuiteResultID = 1 } };

                var zigNetBusiness = new ZigNetBusiness(zignetDatabase.Object);
                zigNetBusiness.SaveTestResult(testResult);
            }

            [TestMethod]
            public void CopiesExistingTestCategories()
            {
                var zignetDatabase = new Mock<IZigNetDatabase>();
                zignetDatabase.Setup(zd => zd.GetTestOrDefault("test 1")).Returns(
                    new Test
                    {
                        Name = "test 1",
                        TestID = 1,
                        Categories = new List<TestCategory> {
                            new TestCategory { TestCategoryID = 1, Name = "test category 1" } }
                    }
                );

                var testResult = new ZigNetTestResult
                {
                    Test = new Test
                    {
                        Name = "test 1",
                        Categories = new List<TestCategory>()
                    },
                    SuiteResult = new SuiteResult()
                };

                var zigNetBusiness = new ZigNetBusiness(zignetDatabase.Object);
                zigNetBusiness.SaveTestResult(testResult);
            }

            [TestMethod]
            public void MergesNewAndExistingTestCategories()
            {
                var zignetDatabase = new Mock<IZigNetDatabase>();
                zignetDatabase.Setup(zd => zd.GetTestOrDefault("test 1")).Returns(
                    new Test
                    {
                        Name = "test 1",
                        TestID = 1,
                        Categories = new List<TestCategory> {
                            new TestCategory { TestCategoryID = 1, Name = "test category 1" } }
                    }
                );

                var testResult = new ZigNetTestResult
                {
                    Test = new Test
                    {
                        Name = "test 1",
                        Categories = new List<TestCategory> { new TestCategory { Name = "test category 2" } }
                    },
                    SuiteResult = new SuiteResult()
                };

                var zigNetBusiness = new ZigNetBusiness(zignetDatabase.Object);
                zigNetBusiness.SaveTestResult(testResult);
            }
        }
    }
}
