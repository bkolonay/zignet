using System;
using System.Collections.Generic;
using System.Linq;
using ZigNet.Database.EntityFramework;
using ZigNet.Services.DTOs;
using ZigNet.Services.EntityFramework.Mapping;

namespace ZigNet.Services.EntityFramework
{
    public class TestResultService : ITestResultService
    {
        private ZigNetEntities _zigNetEntities;
        private ISuiteService _suiteService;
        private ILatestTestResultsService _latestTestResultsService;
        private ITestFailureDurationService _testFailureDurationService;
        private ITestResultMapper _testResultMapper;
        private ITemporaryTestResultsService _temporaryTestResultsService;

        // todo: rename class to LatestTestResultService
        public TestResultService(IZigNetEntitiesWrapper zigNetEntitiesWrapper, ISuiteService suiteService,
            ILatestTestResultsService latestTestResultsService, ITestFailureDurationService testFailureDurationService,
            ITestResultMapper testResultMapper, ITemporaryTestResultsService temporaryTestResultsService)
        {
            _zigNetEntities = zigNetEntitiesWrapper.Get();
            _suiteService = suiteService;
            _latestTestResultsService = latestTestResultsService;
            _testFailureDurationService = testFailureDurationService;
            _temporaryTestResultsService = temporaryTestResultsService;
            _testResultMapper = testResultMapper;
        }

        public IEnumerable<LatestTestResultDto> GetLatestResults(int suiteId)
        {
            var latestTestResults = _latestTestResultsService.Get(suiteId).ToList();
            latestTestResults = AssignTestFailureDurations(latestTestResults);
            return Sort(latestTestResults);
        }

        public IEnumerable<LatestTestResultDto> GetLatestResultsGrouped(int suiteId)
        {
            var suite = _suiteService.Get(suiteId);
            var suiteIds = _suiteService.GetAll()
                .Where(s => s.EnvironmentId == suite.EnvironmentId && s.ApplicationId == suite.ApplicationId)
                .Select(s => s.SuiteID)
                .ToArray();

            var latestTestResults = _latestTestResultsService.Get(suiteIds).ToList();
            latestTestResults = AssignTestFailureDurations(latestTestResults);
            return Sort(latestTestResults);
        }

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
