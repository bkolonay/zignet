using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using ZigNetTest = ZigNet.Domain.Test.Test;
using ZigNetTestCategory = ZigNet.Domain.Test.TestCategory;
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

        public int GetSuiteId(string applicationName, string suiteName, string environmentName)
        {
            return _zigNetEntities.Suites
                .AsNoTracking()
                .Single(s => 
                    s.Application.ApplicationName == applicationName &&
                    s.SuiteName == suiteName &&
                    s.Environment.EnvironmentName == environmentName)
                .SuiteID;
        }
        public string GetSuiteName(int suiteId)
        {
            var suite = _zigNetEntities.Suites
                .AsNoTracking()
                .Include(s => s.Application.ApplicationName)
                .Include(s => s.Environment.EnvironmentName)
                .Select(s => new
                {
                    s.SuiteID,
                    s.SuiteName,
                    s.Application.ApplicationNameAbbreviation,
                    s.Environment.EnvironmentNameAbbreviation
                })
                .Single(s => s.SuiteID == suiteId);

            return string.Format("{0} {1} ({2})",
                            suite.ApplicationNameAbbreviation, suite.SuiteName, suite.EnvironmentNameAbbreviation);
        }
        public string GetSuiteNameGroupedByApplicationAndEnvironment(int suiteId)
        {
            var suite = _zigNetEntities.Suites
                .AsNoTracking()
                .Select(s => new { s.SuiteID, s.Application.ApplicationNameAbbreviation, s.Environment.EnvironmentNameAbbreviation })
                .Single(s => s.SuiteID == suiteId);

            return suite.ApplicationNameAbbreviation + " " + suite.EnvironmentNameAbbreviation;
        }
        public Suite GetSuite(int suiteId)
        {
            return _zigNetEntities.Suites
                .AsNoTracking()
                .Single(s => s.SuiteID == suiteId);
        }
        public SuiteResult GetSuiteResult(int suiteResultId)
        {
            return _zigNetEntities.SuiteResults
                .AsNoTracking()
                .Single(sr => sr.SuiteResultID == suiteResultId);
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
        public IQueryable<Suite> GetSuites()
        {
            return _zigNetEntities.Suites.AsNoTracking();
        }
        public IQueryable<LatestTestResult> GetLatestTestResults()
        {
            return _zigNetEntities.LatestTestResults.AsNoTracking();
        }
        public IQueryable<TestFailureDuration> GetTestFailureDurations()
        {
            return _zigNetEntities.TestFailureDurations.AsNoTracking();
        }
        public IEnumerable<SuiteSummary> GetLatestSuiteResults()
        {
            var suites = _zigNetEntities.Suites
                .AsNoTracking()
                .Include(s => s.Application.ApplicationName)
                .Include(s => s.Environment.EnvironmentName)
                .Select(s => new
                {
                    s.SuiteID,
                    s.SuiteName,
                    s.Application.ApplicationNameAbbreviation,
                    s.Environment.EnvironmentNameAbbreviation
                });

            var suiteSummaries = new List<SuiteSummary>();
            foreach (var suite in suites)
            {
                var temporaryTestResults = _zigNetEntities.TemporaryTestResults
                    .AsNoTracking()
                    .Where(ttr => ttr.SuiteId == suite.SuiteID);

                var firstTemporaryTestResult = temporaryTestResults.FirstOrDefault();
                var suiteEndTime = firstTemporaryTestResult == null ? null : firstTemporaryTestResult.SuiteResult.SuiteResultEndDateTime;

                suiteSummaries.Add(
                    new SuiteSummary
                    {
                        SuiteIds = new List<int> { suite.SuiteID },
                        SuiteName = string.Format("{0} {1} ({2})",
                            suite.ApplicationNameAbbreviation, suite.SuiteName, suite.EnvironmentNameAbbreviation),
                        TotalFailedTests = temporaryTestResults.Where(t => t.TestResultTypeId == 1).Count(),
                        TotalInconclusiveTests = temporaryTestResults.Where(t => t.TestResultTypeId == 2).Count(),
                        TotalPassedTests = temporaryTestResults.Where(t => t.TestResultTypeId == 3).Count(),
                        SuiteEndTime = suiteEndTime
                    }
                );
            }

            return suiteSummaries;
        }
        public IEnumerable<SuiteSummary> GetLatestSuiteResultsGroupedByApplicationAndEnvironment()
        {
            var suites = _zigNetEntities.Suites
                .Include(s => s.Application)
                .Include(s => s.Environment)
                .AsNoTracking();

            var suiteSummaryDictionary = new Dictionary<string, SuiteSummary>();
            foreach (var suite in suites)
            {
                var temporaryTestResults = _zigNetEntities.TemporaryTestResults
                    .AsNoTracking()
                    .Where(ttr => ttr.SuiteId == suite.SuiteID);

                var key = suite.Application.ApplicationNameAbbreviation + " " + suite.Environment.EnvironmentNameAbbreviation;
                if (suiteSummaryDictionary.ContainsKey(key))
                {
                    var existingSuiteSummary = suiteSummaryDictionary[key];
                    existingSuiteSummary.SuiteIds.Add(suite.SuiteID);
                    existingSuiteSummary.TotalFailedTests = existingSuiteSummary.TotalFailedTests +
                        temporaryTestResults.Where(t => t.TestResultTypeId == 1).Count();
                    existingSuiteSummary.TotalInconclusiveTests = existingSuiteSummary.TotalInconclusiveTests +
                        temporaryTestResults.Where(t => t.TestResultTypeId == 2).Count();
                    existingSuiteSummary.TotalPassedTests = existingSuiteSummary.TotalPassedTests +
                        temporaryTestResults.Where(t => t.TestResultTypeId == 3).Count();
                    suiteSummaryDictionary[key] = existingSuiteSummary;
                }
                else
                    suiteSummaryDictionary.Add(key,
                        new SuiteSummary
                        {
                            SuiteIds = new List<int> { suite.SuiteID },
                            SuiteName = key,
                            TotalFailedTests = temporaryTestResults.Where(t => t.TestResultTypeId == 1).Count(),
                            TotalInconclusiveTests = temporaryTestResults.Where(t => t.TestResultTypeId == 2).Count(),
                            TotalPassedTests = temporaryTestResults.Where(t => t.TestResultTypeId == 3).Count()
                        }
                    );
            }

            var suiteSummaries = new List<SuiteSummary>();
            var keys = suiteSummaryDictionary.Keys;
            foreach (var key in keys)
                suiteSummaries.Add(suiteSummaryDictionary[key]);

            return suiteSummaries;
        }
    }
}
