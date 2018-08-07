using System.Data.Entity;
using System.Collections.Generic;
using ZigNet.Database.DTOs;
using ZigNet.Database.EntityFramework;
using System.Linq;

namespace ZigNet.Services.EntityFramework
{
    public class LatestSuiteResultsService : ILatestSuiteResultsService
    {
        private ZigNetEntities _zigNetEntities;

        public LatestSuiteResultsService(IZigNetEntitiesWrapper zigNetEntitiesWrapper)
        {
            _zigNetEntities = zigNetEntitiesWrapper.Get();
        }

        public IEnumerable<SuiteSummary> GetLatest()
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

            var allTemporaryTestResults = _zigNetEntities.TemporaryTestResults.AsNoTracking().ToList();

            var suiteSummaries = new List<SuiteSummary>();
            foreach (var suite in suites)
            {
                var temporaryTestResultsForSuite = allTemporaryTestResults
                    .Where(ttr => ttr.SuiteId == suite.SuiteID);

                var firstTemporaryTestResult = temporaryTestResultsForSuite.FirstOrDefault();
                var suiteEndTime = firstTemporaryTestResult == null ? null : firstTemporaryTestResult.SuiteResult.SuiteResultEndDateTime;

                suiteSummaries.Add(
                    new SuiteSummary
                    {
                        SuiteIds = new List<int> { suite.SuiteID },
                        SuiteName = string.Format("{0} {1} ({2})",
                            suite.ApplicationNameAbbreviation, suite.SuiteName, suite.EnvironmentNameAbbreviation),
                        TotalFailedTests = temporaryTestResultsForSuite.Where(t => t.TestResultTypeId == 1).Count(),
                        TotalInconclusiveTests = temporaryTestResultsForSuite.Where(t => t.TestResultTypeId == 2).Count(),
                        TotalPassedTests = temporaryTestResultsForSuite.Where(t => t.TestResultTypeId == 3).Count(),
                        SuiteEndTime = suiteEndTime
                    }
                );
            }

            return suiteSummaries;
        }

        public IEnumerable<SuiteSummary> GetLatestGrouped()
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
