using System.Collections.Generic;
using System.Linq;
using ZigNet.Database.EntityFramework;
using ZigNet.Services.DTOs;

namespace ZigNet.Services.EntityFramework
{
    public class LatestTestResultsService : ILatestTestResultsService
    {
        private ZigNetEntities _zigNetEntities;

        public LatestTestResultsService(IZigNetEntitiesWrapper zigNetEntitiesWrapper)
        {
            _zigNetEntities = zigNetEntitiesWrapper.Get();
        }

        // todo: unit test 2 public methods
        public IEnumerable<LatestTestResultDto> Get(int[] suiteIds)
        {
            var latestTestResultsForSuites = _zigNetEntities.LatestTestResults
                .AsNoTracking()
                .Where(l => suiteIds.Any(s => s == l.SuiteId));
            return Map(latestTestResultsForSuites);
        }

        public IEnumerable<LatestTestResultDto> Get(int suiteId)
        {
            var latestTestResultsForSuite = _zigNetEntities.LatestTestResults
                .AsNoTracking()
                .Where(l => l.SuiteId == suiteId);
            return Map(latestTestResultsForSuite);
        }

        private IQueryable<LatestTestResultDto> Map(IQueryable<LatestTestResult> dbLatestTestResults)
        {
            return dbLatestTestResults
                .Select(l => new LatestTestResultDto
                {
                    SuiteId = l.SuiteId,
                    TestId = l.TestId,
                    TestResultID = l.TestResultId,
                    TestName = l.TestName,
                    SuiteName = l.SuiteName,
                    FailingFromDate = l.FailingFromDateTime,
                    PassingFromDate = l.PassingFromDateTime
                });
        }
    }
}
