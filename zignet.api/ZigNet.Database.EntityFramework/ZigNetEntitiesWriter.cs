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
    public class ZigNetEntitiesWriter : IZigNetEntitiesWriter
    {
        private ZigNetEntities _zigNetEntities;

        public ZigNetEntitiesWriter(IZigNetEntitiesSingleton zigNetEntitiesSingleton)
        {
            _zigNetEntities = zigNetEntitiesSingleton.GetInstance();
        }

        public int SaveSuiteResult(SuiteResult suiteResult)
        {
            if (suiteResult.SuiteResultID == 0)
                _zigNetEntities.SuiteResults.Add(suiteResult);
            _zigNetEntities.SaveChanges();
            return suiteResult.SuiteResultID;
        }
        public SuiteResult GetSuiteResult(int suiteResultId)
        {
            return GetSuiteResults().Single(sr => sr.SuiteResultID == suiteResultId);
        }
        public Test GetTestWithSuites(int testId)
        {
            return _zigNetEntities.Tests
                .Include(t => t.Suites)
                .Single(t => t.TestID == testId);
        }
        public IQueryable<TestCategory> GetTestCategories()
        {
            return _zigNetEntities.TestCategories;
        }
        public Suite GetSuite(int suiteId)
        {
            return _zigNetEntities.Suites.Single(s => s.SuiteID == suiteId);
        }
        public TestResult SaveTestResult(TestResult testResult)
        {
            _zigNetEntities.TestResults.Add(testResult);
            _zigNetEntities.SaveChanges();
            return testResult;
        }
        public IQueryable<LatestTestResult> GetLatestTestResults()
        {
            return _zigNetEntities.LatestTestResults;
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

        public IQueryable<TestResult> GetTestResults()
        {
            return _zigNetEntities.TestResults;
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

        public void SaveLatestTestResult(LatestTestResult latestTestResult)
        {
            if (latestTestResult.LatestTestResultID == 0)
                _zigNetEntities.LatestTestResults.Add(latestTestResult);
            _zigNetEntities.SaveChanges();
        }        
    }
}
