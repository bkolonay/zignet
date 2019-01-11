using System;
using System.Collections.Generic;
using System.Linq;
using ZigNet.Database.EntityFramework;
using ZigNet.Services.DTOs;

namespace ZigNet.Services.EntityFramework
{
    public class TestResultService : ITestResultService
    {
        private ZigNetEntities _db;
        private ILatestTestResultService _latestTestResultService;
        private ITestFailureDurationService _testFailureDurationService;
        private ISuiteService _suiteService;

        public TestResultService(IDbContext dbContext, ILatestTestResultService latestTestResultsService,
            ITestFailureDurationService testFailureDurationService, ISuiteService suiteService)
        {
            _db = dbContext.Get();
            _latestTestResultService = latestTestResultsService;
            _testFailureDurationService = testFailureDurationService;
            _suiteService = suiteService;
        }

        public IEnumerable<LatestTestResultDto> GetLatest(SuiteResultsFilter suiteResultsFilter)
        {
            IEnumerable<SuiteDto> suites;

            if (suiteResultsFilter.Applications == null || suiteResultsFilter.Applications.Length == 0)
                suites = _suiteService.GetAll();
            else
                suites = _suiteService.GetAll().Where(s => suiteResultsFilter.Applications.Contains(s.ApplicationName));

            if (suiteResultsFilter.Environments != null && suiteResultsFilter.Environments.Length > 0)
                suites = suites.Where(s => suiteResultsFilter.Environments.Contains(s.EnvironmentNameAbbreviation));

            if (suiteResultsFilter.Suites != null && suiteResultsFilter.Suites.Length > 0)
                suites = suites.Where(s => suiteResultsFilter.Suites.Contains(s.Name));

            if (!suiteResultsFilter.Debug)
                suites = suites.Where(s => !s.Name.Contains("(D)"));

            var suiteIds = suites.Select(s => s.SuiteID).ToArray();

            var latestTestResults = _latestTestResultService.Get(suiteIds).ToList();
            latestTestResults = AssignTestFailureDurations(latestTestResults);
            return Sort(latestTestResults);
        }

        // below saved for getting by single suitid
        //public IEnumerable<LatestTestResultDto> GetLatest(int suiteId)
        //{
        //    var latestTestResults = _latestTestResultService.Get(suiteId).ToList();
        //    latestTestResults = AssignTestFailureDurations(latestTestResults);
        //    return Sort(latestTestResults);
        //}

        private List<LatestTestResultDto> AssignTestFailureDurations(List<LatestTestResultDto> latestTestResults)
        {
            var testFailureDurations = _testFailureDurationService.GetAll().ToList();
            var utcNow = DateTime.UtcNow;
            for (var i = 0; i < latestTestResults.Count; i++)
            {
                var testFailureDurationLimit = utcNow.AddHours(-24);
                latestTestResults[i].TestFailureDurations = testFailureDurations
                    .Where(t =>
                            (t.SuiteId == latestTestResults[i].SuiteId && t.TestId == latestTestResults[i].TestId) &&
                            (t.FailureEnd > testFailureDurationLimit || t.FailureEnd == null))
                    .ToList();
            }

            return latestTestResults;
        }
        private List<LatestTestResultDto> Sort(List<LatestTestResultDto> latestTestResults)
        {
            var passingLatestTestResults = latestTestResults.Where(t => t.PassingFromDate != null).OrderByDescending(t => t.PassingFromDate);
            var failingLatestTestResults = latestTestResults.Where(t => t.FailingFromDate != null).OrderBy(t => t.FailingFromDate).ToList();
            failingLatestTestResults.AddRange(passingLatestTestResults);
            return failingLatestTestResults;
        }
    }
}
