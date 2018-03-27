using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Data.Entity;
using ZigNetSuite = ZigNet.Domain.Suite.Suite;
using ZigNetTest = ZigNet.Domain.Test.Test;
using ZigNetTestCategory = ZigNet.Domain.Test.TestCategory;
using ZigNetSuiteResult = ZigNet.Domain.Suite.SuiteResult;
using ZigNetTestResultType = ZigNet.Domain.Test.TestResultType;
using ZigNet.Database.DTOs;

namespace ZigNet.Database.EntityFramework
{
    public class ZigNetEntitiesReadOnly : IZigNetEntitiesReadOnly
    {
        private ZigNetEntities _zigNetEntities;

        public ZigNetEntitiesReadOnly(IZigNetEntitiesWrapper zigNetEntitiesWrapper)
        {
            _zigNetEntities = zigNetEntitiesWrapper.Get();
        }

        public int GetSuiteId(string suiteName)
        {
            return _zigNetEntities.Suites
                .AsNoTracking()
                .Select(s => new { s.SuiteName, s.SuiteID })
                .Single(s => s.SuiteName == suiteName)
                .SuiteID;
        }
        public ZigNetTest GetMappedTestWithCategoriesOrDefault(string testName)
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
        public SuiteResult GetSuiteResult(int suiteResultId)
        {
            return _zigNetEntities.SuiteResults
                .AsNoTracking()
                .Single(sr => sr.SuiteResultID == suiteResultId);
        }
        public IQueryable<LatestTestResult> GetLatestTestResults()
        {
            return _zigNetEntities.LatestTestResults.AsNoTracking();
        }
        public IEnumerable<SuiteSummary> GetLatestSuiteResults()
        {
            var suites = _zigNetEntities.Suites
                .AsNoTracking()
                .Include(s => s.SuiteResults);

            var latestSuiteResults = new List<SuiteResult>();
            foreach (var suite in suites)
            {
                var latestSuiteResult = suite.SuiteResults.OrderByDescending(sr => sr.SuiteResultStartDateTime).FirstOrDefault();
                if (latestSuiteResult == null)
                    continue;
                latestSuiteResults.Add(latestSuiteResult);
            }

            var suiteSummaries = new List<SuiteSummary>();
            foreach (var suiteResult in latestSuiteResults)
            {
                var testResults = _zigNetEntities.TestResults
                    .AsNoTracking()
                    .Where(tr => tr.SuiteResultId == suiteResult.SuiteResultID);

                suiteSummaries.Add(
                    new SuiteSummary
                    {
                        SuiteID = suiteResult.Suite.SuiteID,
                        SuiteName = suiteResult.Suite.SuiteName,
                        TotalFailedTests = testResults.Where(t => t.TestResultTypeId == 1).Count(),
                        TotalInconclusiveTests = testResults.Where(t => t.TestResultTypeId == 2).Count(),
                        TotalPassedTests = testResults.Where(t => t.TestResultTypeId == 3).Count(),
                        SuiteEndTime = suiteResult.SuiteResultEndDateTime
                    }
                );
            }

            return suiteSummaries;
        }
    }
}
