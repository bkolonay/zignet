using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Data.Entity;
using ZigNetSuite = ZigNet.Domain.Suite.Suite;
using ZigNetTest = ZigNet.Domain.Test.Test;
using ZigNetTestCategory = ZigNet.Domain.Test.TestCategory;

namespace ZigNet.Database.EntityFramework
{
    public class ZigNetEntitiesWrapper : IZigNetEntitiesWrapper, IDisposable
    {
        private ZigNetEntities _zigNetEntities;

        public ZigNetEntitiesWrapper()
        {
            _zigNetEntities = new ZigNetEntities();
            _zigNetEntities.Database.Log = s => Debug.WriteLine(s);
        }        

        public IQueryable<Suite> GetSuites()
        {
            return _zigNetEntities.Suites;
        }
        public IEnumerable<Test> GetTestsWithTestResultsForSuite(int suiteId)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            var testsForSuite = _zigNetEntities.Suites
                .AsNoTracking()
                .Include(s => s.Tests)
                .Single(s => s.SuiteID == suiteId)
                .Tests;

            stopwatch.Stop();
            var testsForSuiteSeconds = stopwatch.ElapsedMilliseconds / 1000.0;

            stopwatch.Reset();
            stopwatch.Start();

            var testResultsGroupedByTestForSuite = _zigNetEntities.TestResults
                .AsNoTracking()
                .Include(tr => tr.Test)
                .Where(tr => tr.SuiteResult.SuiteId == suiteId)
                .GroupBy(tr => tr.TestId)
                .ToList();

            stopwatch.Stop();
            var testResultsGroupedByTestForSuiteSeconds = stopwatch.ElapsedMilliseconds / 1000.0;

            var testsWithTestResults = new List<Test>();
            foreach (var groupedTestResult in testResultsGroupedByTestForSuite)
                if (testsForSuite.Any(t => t.TestID == groupedTestResult.Key))
                    testsWithTestResults.Add(new Test { TestID = groupedTestResult.Key, TestResults = groupedTestResult.ToList() });

            return testsWithTestResults;
        }

        public Suite GetSuite(int suiteId)
        {
            return _zigNetEntities.Suites.Single(s => s.SuiteID == suiteId);
        }

        public ZigNetSuite GetZigNetSuite(int suiteId)
        {
            return _zigNetEntities.Suites
                .Select(s => new ZigNetSuite { SuiteID = s.SuiteID })
                .Single(s => s.SuiteID == suiteId);
        }

        public IQueryable<SuiteResult> GetSuiteResults()
        {
            return _zigNetEntities.SuiteResults;
        }

        public SuiteResult GetSuiteResult(int suiteResultId)
        {
            return GetSuiteResults().Single(sr => sr.SuiteResultID == suiteResultId);
        }

        public SuiteResult GetSuiteResultWithoutTracking(int suiteResultId)
        {
            return GetSuiteResults()
                .AsNoTracking()
                .Single(sr => sr.SuiteResultID == suiteResultId);
        }

        public bool SuiteResultExists(int suiteResultId)
        {
            return _zigNetEntities.SuiteResults
                .AsNoTracking()
                .Any(sr => sr.SuiteResultID == suiteResultId);
        }

        public IQueryable<SuiteCategory> GetSuiteCategories()
        {
            return _zigNetEntities.SuiteCategories;
        }

        public IQueryable<TestCategory> GetTestCategories()
        {
            return _zigNetEntities.TestCategories;
        }

        public IQueryable<TestResult> GetTestResults()
        {
            return _zigNetEntities.TestResults;
        }

        public ZigNetTest GetTestOrDefault(string testName)
        {
            return _zigNetEntities.Tests
                .AsNoTracking()
                .Include(t => t.TestCategories)
                .Select(t => 
                    new ZigNetTest
                    {
                        TestID = t.TestID,
                        Name = t.TestName,
                        Categories = t.TestCategories.Select(tc => new ZigNetTestCategory { TestCategoryID = tc.TestCategoryID, Name = tc.CategoryName }).ToList()
                    }
                )
                .SingleOrDefault(t => t.Name == testName);
        }

        public Test GetTest(int testId)
        {
            return _zigNetEntities.Tests
                .Include(t => t.Suites)
                .Single(t => t.TestID == testId);
        }

        public TestFailureType GetTestFailureType(int testFailureTypeId)
        {
            return _zigNetEntities.TestFailureTypes
                .AsNoTracking()
                .Single(tft => tft.TestFailureTypeID == testFailureTypeId);
        }

        public int SaveSuite(Suite suite)
        {
            if (suite.SuiteID == 0)
                _zigNetEntities.Suites.Add(suite);
            _zigNetEntities.SaveChanges();
            return suite.SuiteID;
        }

        public int SaveSuiteResult(SuiteResult suiteResult)
        {
            if (suiteResult.SuiteResultID == 0)
                _zigNetEntities.SuiteResults.Add(suiteResult);
            _zigNetEntities.SaveChanges();
            return suiteResult.SuiteResultID;
        }

        public void SaveTestResult(TestResult testResult)
        {
            _zigNetEntities.TestResults.Add(testResult);
            _zigNetEntities.SaveChanges();
        }

        // disposal code copied directly from: https://docs.microsoft.com/en-us/aspnet/web-api/overview/advanced/dependency-injection
        protected void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_zigNetEntities != null)
                {
                    _zigNetEntities.Dispose();
                    _zigNetEntities = null;
                }
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
