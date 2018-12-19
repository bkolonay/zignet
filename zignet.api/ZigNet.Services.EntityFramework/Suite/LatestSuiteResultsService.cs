using System.Collections.Generic;
using ZigNet.Services.DTOs;
using ZigNet.Database.EntityFramework;
using System.Linq;

namespace ZigNet.Services.EntityFramework
{
    public class LatestSuiteResultsService : ILatestSuiteResultsService
    {
        private ZigNetEntities _db;
        private ISuiteService _suiteService;

        public LatestSuiteResultsService(IDbContext dbContext, ISuiteService suiteService)
        {
            _db = dbContext.Get();
            _suiteService = suiteService;
        }

        public IEnumerable<SuiteSummary> GetLatest()
        {
            var suites = _suiteService.GetAll();
            var allTemporaryTestResults = _db.TemporaryTestResults.AsNoTracking().ToList();

            var suiteSummaries = new List<SuiteSummary>();
            foreach (var suite in suites)
            {
                var temporaryTestResultsForSuite = allTemporaryTestResults.Where(t => t.SuiteId == suite.SuiteID);

                var firstTemporaryTestResult = temporaryTestResultsForSuite.FirstOrDefault();
                var suiteEndTime = firstTemporaryTestResult == null ? null : firstTemporaryTestResult.SuiteResult.SuiteResultEndDateTime;

                suiteSummaries.Add(
                    new SuiteSummary
                    {
                        SuiteIds = new List<int> { suite.SuiteID },
                        SuiteName = suite.GetName(),
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
            var suites = _suiteService.GetAll();
            var allTemporaryTestResults = _db.TemporaryTestResults.AsNoTracking().ToList();

            var suiteSummaryDictionary = new Dictionary<string, SuiteSummary>();
            foreach (var suite in suites)
            {
                var temporaryTestResultsForSuite = allTemporaryTestResults.Where(t => t.SuiteId == suite.SuiteID);

                var key = suite.GetNameGrouped();
                if (suiteSummaryDictionary.ContainsKey(key))
                {
                    var existingSuiteSummary = suiteSummaryDictionary[key];

                    existingSuiteSummary.SuiteIds.Add(suite.SuiteID);
                    existingSuiteSummary.TotalFailedTests = 
                        existingSuiteSummary.TotalFailedTests + temporaryTestResultsForSuite.Where(t => t.TestResultTypeId == 1).Count();
                    existingSuiteSummary.TotalInconclusiveTests = 
                        existingSuiteSummary.TotalInconclusiveTests + temporaryTestResultsForSuite.Where(t => t.TestResultTypeId == 2).Count();
                    existingSuiteSummary.TotalPassedTests =
                        existingSuiteSummary.TotalPassedTests + temporaryTestResultsForSuite.Where(t => t.TestResultTypeId == 3).Count();

                    suiteSummaryDictionary[key] = existingSuiteSummary;
                }
                else
                    suiteSummaryDictionary.Add(key,
                        new SuiteSummary
                        {
                            SuiteIds = new List<int> { suite.SuiteID },
                            SuiteName = key,
                            TotalFailedTests = temporaryTestResultsForSuite.Where(t => t.TestResultTypeId == 1).Count(),
                            TotalInconclusiveTests = temporaryTestResultsForSuite.Where(t => t.TestResultTypeId == 2).Count(),
                            TotalPassedTests = temporaryTestResultsForSuite.Where(t => t.TestResultTypeId == 3).Count()
                        }
                    );
            }

            var suiteSummaries = new List<SuiteSummary>();
            foreach (var key in suiteSummaryDictionary.Keys)
                suiteSummaries.Add(suiteSummaryDictionary[key]);

            return suiteSummaries;
        }

        // todo: unit test
        public IEnumerable<SuiteSummary> GetLatest(SuiteResultsFilter suiteResultsFilter)
        {
            IEnumerable<SuiteDto> suites;

            // todo: is there any performance difference if we don't get debug suites?

            if (suiteResultsFilter.Applications == null || suiteResultsFilter.Applications.Length == 0)
                suites = _suiteService.GetAll();
            else
                suites = _suiteService.GetAll().Where(s => suiteResultsFilter.Applications.Contains(s.ApplicationName));             

            var allTemporaryTestResults = _db.TemporaryTestResults.AsNoTracking().ToList();

            var suiteSummaryDictionary = new Dictionary<string, SuiteSummary>();
            foreach (var suite in suites)
            {
                var temporaryTestResultsForSuite = allTemporaryTestResults.Where(t => t.SuiteId == suite.SuiteID);

                var key = suite.ApplicationName;
                if (suiteSummaryDictionary.ContainsKey(key))
                {
                    var existingSuiteSummary = suiteSummaryDictionary[key];

                    existingSuiteSummary.SuiteIds.Add(suite.SuiteID);
                    existingSuiteSummary.TotalFailedTests =
                        existingSuiteSummary.TotalFailedTests + temporaryTestResultsForSuite.Where(t => t.TestResultTypeId == 1).Count();
                    existingSuiteSummary.TotalInconclusiveTests =
                        existingSuiteSummary.TotalInconclusiveTests + temporaryTestResultsForSuite.Where(t => t.TestResultTypeId == 2).Count();
                    existingSuiteSummary.TotalPassedTests =
                        existingSuiteSummary.TotalPassedTests + temporaryTestResultsForSuite.Where(t => t.TestResultTypeId == 3).Count();

                    suiteSummaryDictionary[key] = existingSuiteSummary;
                }
                else
                    suiteSummaryDictionary.Add(key,
                        new SuiteSummary
                        {
                            SuiteIds = new List<int> { suite.SuiteID },
                            SuiteName = key,
                            TotalFailedTests = temporaryTestResultsForSuite.Where(t => t.TestResultTypeId == 1).Count(),
                            TotalInconclusiveTests = temporaryTestResultsForSuite.Where(t => t.TestResultTypeId == 2).Count(),
                            TotalPassedTests = temporaryTestResultsForSuite.Where(t => t.TestResultTypeId == 3).Count()
                        }
                    );
            }

            var suiteSummaries = new List<SuiteSummary>();
            foreach (var key in suiteSummaryDictionary.Keys)
                suiteSummaries.Add(suiteSummaryDictionary[key]);

            return suiteSummaries;
        }
    }
}
