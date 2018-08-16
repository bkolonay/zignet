using System;
using System.Collections.Generic;
using System.Linq;
using ZigNet.Database.EntityFramework;
using ZigNet.Services.DTOs;
using TestResultType = ZigNet.Domain.Test.TestResultType;

namespace ZigNet.Services.EntityFramework
{
    public class LatestTestResultsService : ILatestTestResultsService
    {
        private ZigNetEntities _zigNetEntities;

        public LatestTestResultsService(IDbContext zigNetEntitiesWrapper)
        {
            _zigNetEntities = zigNetEntitiesWrapper.Get();
        }

        // note: class is not unit tested (change with caution)
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

        public LatestTestResultDto Save(LatestTestResultDto latestTestResultDto, TestResultType testResultType, DateTime utcNow)
        {
            var dbLatestTestResult = _zigNetEntities.LatestTestResults
                .SingleOrDefault(t =>
                    t.SuiteId == latestTestResultDto.SuiteId &&
                    t.TestId == latestTestResultDto.TestId
                );

            var suiteNameChanged = false;
            if (dbLatestTestResult == null)
                dbLatestTestResult = new LatestTestResult
                {
                    SuiteId = latestTestResultDto.SuiteId,
                    TestId = latestTestResultDto.TestId,
                    TestName = latestTestResultDto.TestName,
                    SuiteName = latestTestResultDto.SuiteName
                };
            else if (dbLatestTestResult.SuiteName != latestTestResultDto.SuiteName)
            {
                dbLatestTestResult.SuiteName = latestTestResultDto.SuiteName;
                suiteNameChanged = true;
            }

            if (testResultType == TestResultType.Pass && dbLatestTestResult.PassingFromDateTime == null)
            {
                dbLatestTestResult.TestResultId = latestTestResultDto.TestResultID;
                dbLatestTestResult.PassingFromDateTime = utcNow;
                dbLatestTestResult.FailingFromDateTime = null;
                SaveLatestTestResult(dbLatestTestResult);
            }
            else if ((testResultType == TestResultType.Fail || testResultType == TestResultType.Inconclusive)
                      && dbLatestTestResult.FailingFromDateTime == null)
            {
                dbLatestTestResult.TestResultId = latestTestResultDto.TestResultID;
                dbLatestTestResult.FailingFromDateTime = utcNow;
                dbLatestTestResult.PassingFromDateTime = null;
                SaveLatestTestResult(dbLatestTestResult);
            }
            else if (suiteNameChanged)
                SaveLatestTestResult(dbLatestTestResult);

            return new LatestTestResultDto
            {
                LatestTestResultID = dbLatestTestResult.LatestTestResultID,
                TestResultID = dbLatestTestResult.TestResultId,
                SuiteId = dbLatestTestResult.SuiteId,
                TestId = dbLatestTestResult.TestId,
                TestName = dbLatestTestResult.TestName,
                SuiteName = dbLatestTestResult.SuiteName,
                PassingFromDate = dbLatestTestResult.PassingFromDateTime,
                FailingFromDate = dbLatestTestResult.FailingFromDateTime
            };
        }

        private void SaveLatestTestResult(LatestTestResult latestTestResult)
        {
            if (latestTestResult.LatestTestResultID == 0)
                _zigNetEntities.LatestTestResults.Add(latestTestResult);
            _zigNetEntities.SaveChanges();
        }
    }
}
