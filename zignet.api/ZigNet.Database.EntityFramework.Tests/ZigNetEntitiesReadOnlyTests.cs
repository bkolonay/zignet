using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZigNet.Database.EntityFramework.Tests
{
    public class ZigNetEntitiesReadOnlyTests
    {
        //[TestClass]
        //public class GetLatestSuiteResultsMethod
        //{
        //    [TestMethod]
        //    public void ReturnsLatestSuiteResultsFromOneSuite()
        //    {
        //        var zignetDatabaseMock = new Mock<IZigNetDatabase>();
        //        var suite = new Suite { SuiteID = 1, Name = "Suite 1" };
        //        var suites = new List<Suite> { suite };
        //        var suiteResults = new List<SuiteResult> {
        //            new SuiteResult { Suite = suite, StartTime = DateTime.Parse("12/19/2017 10:00am"), EndTime = DateTime.Parse("12/19/2017 10:05am")},
        //            new SuiteResult { Suite = suite, StartTime = DateTime.Parse("12/19/2017 10:02am"), EndTime = DateTime.Parse("12/19/2017 10:10am") },
        //            new SuiteResult { Suite = suite, StartTime = DateTime.Parse("12/19/2017 10:01am"), EndTime = DateTime.Parse("12/19/2017 10:03am") }
        //        };
        //        var testResults = new List<ZigNetTestResult>{
        //            new ZigNetTestResult { ResultType = TestResultType.Fail },
        //            new ZigNetTestResult { ResultType = TestResultType.Inconclusive },
        //            new ZigNetTestResult { ResultType = TestResultType.Pass },
        //            new ZigNetTestResult { ResultType = TestResultType.Pass },
        //        };

        //        zignetDatabaseMock.Setup(zdm => zdm.GetSuites()).Returns(suites);
        //        zignetDatabaseMock.Setup(zdm => zdm.GetSuiteResultsForSuite(It.IsAny<int>())).Returns(suiteResults);
        //        zignetDatabaseMock.Setup(zdm => zdm.GetTestResultsForSuiteResult(It.IsAny<int>())).Returns(testResults);

        //        var tests = new List<Test> { new Test() };
        //        var mockDbSet = new Mock<DbSet<Test>>();
        //        mockDbSet.As<IQueryable<Test>>().Setup()
        //        //DbSet

        //        var zigNetEntitiesSingletonMock = new Mock<IZigNetEntitiesWrapper>();
        //        zigNetEntitiesSingletonMock.Setup(m => m.Tests()).Returns(dbSet);

        //        var zigNetEntitiesReadOnly = new ZigNetEntitiesReadOnly(zigNetEntitiesSingletonMock.Object);

        //        var suiteSummaries = zigNetEntitiesReadOnly.GetLatestSuiteResults().ToList();

        //        Assert.AreEqual(1, suiteSummaries.Count());
        //        Assert.AreEqual(1, suiteSummaries[0].SuiteID);
        //        Assert.AreEqual("Suite 1", suiteSummaries[0].SuiteName);
        //        Assert.AreEqual(1, suiteSummaries[0].TotalFailedTests);
        //        Assert.AreEqual(1, suiteSummaries[0].TotalInconclusiveTests);
        //        Assert.AreEqual(2, suiteSummaries[0].TotalPassedTests);
        //        Assert.AreEqual(DateTime.Parse("12/19/2017 10:10am"), suiteSummaries[0].SuiteEndTime);
        //    }

            //[TestMethod]
            //public void DoesNotThrowWhenNoSuiteResultsForSuite()
            //{
            //    var zignetDatabaseMock = new Mock<IZigNetDatabase>();
            //    var suite = new Suite { SuiteID = 1, Name = "Suite 1" };
            //    var suites = new List<Suite> { suite };
            //    var suiteResults = new List<SuiteResult> { };
            //    var testResults = new List<ZigNetTestResult>{
            //        new ZigNetTestResult { ResultType = TestResultType.Fail },
            //        new ZigNetTestResult { ResultType = TestResultType.Inconclusive },
            //        new ZigNetTestResult { ResultType = TestResultType.Pass },
            //        new ZigNetTestResult { ResultType = TestResultType.Pass },
            //    };

            //    zignetDatabaseMock.Setup(zdm => zdm.GetSuites()).Returns(suites);
            //    zignetDatabaseMock.Setup(zdm => zdm.GetSuiteResultsForSuite(It.IsAny<int>())).Returns(suiteResults);
            //    zignetDatabaseMock.Setup(zdm => zdm.GetTestResultsForSuiteResult(It.IsAny<int>())).Returns(testResults);

            //    var zigNetBusiness = new ZigNetBusiness(zignetDatabaseMock.Object);

            //    var suiteSummaries = zigNetBusiness.GetLatestSuiteResults().ToList();

            //    Assert.AreEqual(0, suiteSummaries.Count());
            //}

            //[TestMethod]
            //public void DoesNotThrowWhenSuiteEndTimeNull()
            //{
            //    var zignetDatabaseMock = new Mock<IZigNetDatabase>();
            //    var suite = new Suite { SuiteID = 1, Name = "Suite 1" };
            //    var suites = new List<Suite> { suite };
            //    var suiteResults = new List<SuiteResult> {
            //        new SuiteResult { Suite = suite, StartTime = DateTime.Parse("12/19/2017 10:00am"), EndTime = DateTime.Parse("12/19/2017 10:05am")},
            //        new SuiteResult { Suite = suite, StartTime = DateTime.Parse("12/19/2017 10:02am") },
            //        new SuiteResult { Suite = suite, StartTime = DateTime.Parse("12/19/2017 10:01am"), EndTime = DateTime.Parse("12/19/2017 10:03am") }
            //    };
            //    var testResults = new List<ZigNetTestResult>{
            //        new ZigNetTestResult { ResultType = TestResultType.Fail },
            //        new ZigNetTestResult { ResultType = TestResultType.Inconclusive },
            //        new ZigNetTestResult { ResultType = TestResultType.Pass },
            //        new ZigNetTestResult { ResultType = TestResultType.Pass },
            //    };

            //    zignetDatabaseMock.Setup(zdm => zdm.GetSuites()).Returns(suites);
            //    zignetDatabaseMock.Setup(zdm => zdm.GetSuiteResultsForSuite(It.IsAny<int>())).Returns(suiteResults);
            //    zignetDatabaseMock.Setup(zdm => zdm.GetTestResultsForSuiteResult(It.IsAny<int>())).Returns(testResults);

            //    var zigNetBusiness = new ZigNetBusiness(zignetDatabaseMock.Object);

            //    var suiteSummaries = zigNetBusiness.GetLatestSuiteResults().ToList();
            //    var suiteSummary = suiteSummaries[0];

            //    Assert.AreEqual(1, suiteSummaries.Count());
            //    Assert.AreEqual(1, suiteSummary.SuiteID);
            //    Assert.AreEqual("Suite 1", suiteSummary.SuiteName);
            //    Assert.AreEqual(1, suiteSummary.TotalFailedTests);
            //    Assert.AreEqual(1, suiteSummary.TotalInconclusiveTests);
            //    Assert.AreEqual(2, suiteSummary.TotalPassedTests);
            //    Assert.IsNull(suiteSummary.SuiteEndTime);
            //}

            //[TestMethod]
            //public void DoesNotThrowWhenNoSuites()
            //{
            //    var zignetDatabaseMock = new Mock<IZigNetDatabase>();
            //    var suites = new List<Suite> { };

            //    zignetDatabaseMock.Setup(zdm => zdm.GetMappedSuites()).Returns(suites);

            //    var zigNetBusiness = new ZigNetBusiness(zignetDatabaseMock.Object);

            //    var suiteSummaries = zigNetBusiness.GetLatestSuiteResults().ToList();

            //    Assert.AreEqual(0, suiteSummaries.Count());
            //}

            //[TestMethod]
            //public void DoesNotThrowWhenNoTestResultsForSuiteResult()
            //{
            //    var zignetDatabaseMock = new Mock<IZigNetDatabase>();
            //    var suite = new Suite { SuiteID = 1, Name = "Suite 1" };
            //    var suites = new List<Suite> { suite };
            //    var suiteResults = new List<SuiteResult> {
            //        new SuiteResult { Suite = suite, StartTime = DateTime.Parse("12/19/2017 10:00am"), EndTime = DateTime.Parse("12/19/2017 10:05am")},
            //        new SuiteResult { Suite = suite, StartTime = DateTime.Parse("12/19/2017 10:02am"), EndTime = DateTime.Parse("12/19/2017 10:10am") },
            //        new SuiteResult { Suite = suite, StartTime = DateTime.Parse("12/19/2017 10:01am"), EndTime = DateTime.Parse("12/19/2017 10:03am") }
            //    };
            //    var testResults = new List<ZigNetTestResult> { };

            //    zignetDatabaseMock.Setup(zdm => zdm.GetSuites()).Returns(suites);
            //    zignetDatabaseMock.Setup(zdm => zdm.GetSuiteResultsForSuite(It.IsAny<int>())).Returns(suiteResults);
            //    zignetDatabaseMock.Setup(zdm => zdm.GetTestResultsForSuiteResult(It.IsAny<int>())).Returns(testResults);

            //    var zigNetBusiness = new ZigNetBusiness(zignetDatabaseMock.Object);

            //    var suiteSummaries = zigNetBusiness.GetLatestSuiteResults().ToList();
            //    var suiteSummary = suiteSummaries[0];

            //    Assert.AreEqual(1, suiteSummaries.Count());
            //    Assert.AreEqual(1, suiteSummary.SuiteID);
            //    Assert.AreEqual("Suite 1", suiteSummary.SuiteName);
            //    Assert.AreEqual(0, suiteSummary.TotalFailedTests);
            //    Assert.AreEqual(0, suiteSummary.TotalInconclusiveTests);
            //    Assert.AreEqual(0, suiteSummary.TotalPassedTests);
            //    Assert.AreEqual(DateTime.Parse("12/19/2017 10:10am"), suiteSummary.SuiteEndTime);
            //}
        //}
    }
}
