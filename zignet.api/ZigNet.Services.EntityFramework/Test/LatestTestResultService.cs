using System;
using System.Collections.Generic;
using System.Linq;
using ZigNet.Database.EntityFramework;
using ZigNet.Services.DTOs;
using DbLatestTestResult = ZigNet.Database.EntityFramework.LatestTestResult;
using TestResultType = ZigNet.Domain.Test.TestResultType;

namespace ZigNet.Services.EntityFramework
{
    public class LatestTestResultService : ILatestTestResultService
    {
        private ZigNetEntities _db;

        public LatestTestResultService(IDbContext dbContext)
        {
            _db = dbContext.Get();
        }

        // note: class is not unit tested (change with caution)
        public IEnumerable<LatestTestResultDto> Get(int suiteId)
        {
            var dbLatestTestResultsForSuite = _db.LatestTestResults
                .AsNoTracking()
                .Where(l => l.SuiteId == suiteId);
            return Map(dbLatestTestResultsForSuite);
        }

        public IEnumerable<LatestTestResultDto> Get(int[] suiteIds)
        {
            var dbLatestTestResultsForSuites = _db.LatestTestResults
                .AsNoTracking()
                .Where(l => suiteIds.Any(s => s == l.SuiteId));
            return Map(dbLatestTestResultsForSuites);
        }

        public LatestTestResultDto Save(LatestTestResultDto latestTestResultDto, TestResultType testResultType, DateTime utcNow)
        {
            var dbLatestTestResult = _db.LatestTestResults
                .SingleOrDefault(t =>
                    t.SuiteId == latestTestResultDto.SuiteId &&
                    t.TestId == latestTestResultDto.TestId
                );

            var suiteChanged = false;
            if (dbLatestTestResult == null)
                dbLatestTestResult = new DbLatestTestResult
                {
                    SuiteId = latestTestResultDto.SuiteId,
                    TestId = latestTestResultDto.TestId,
                    TestName = latestTestResultDto.TestName,
                    SuiteName = latestTestResultDto.SuiteName,
                    SuiteApplicationName = latestTestResultDto.SuiteApplicationName,
                    SuiteEnvironmentNameAbbreviation = latestTestResultDto.SuiteEnvironmentNameAbbreviation
                };
            else
            {
                if (dbLatestTestResult.SuiteName != latestTestResultDto.SuiteName)
                {
                    dbLatestTestResult.SuiteName = latestTestResultDto.SuiteName;
                    suiteChanged = true;
                }
                if (dbLatestTestResult.SuiteApplicationName != latestTestResultDto.SuiteApplicationName)
                {
                    dbLatestTestResult.SuiteApplicationName = latestTestResultDto.SuiteApplicationName;
                    suiteChanged = true;
                }
                if (dbLatestTestResult.SuiteEnvironmentNameAbbreviation != latestTestResultDto.SuiteEnvironmentNameAbbreviation)
                {
                    dbLatestTestResult.SuiteEnvironmentNameAbbreviation = latestTestResultDto.SuiteEnvironmentNameAbbreviation;
                    suiteChanged = true;
                }
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
            else if (suiteChanged)
                SaveLatestTestResult(dbLatestTestResult);

            // todo: move to mapping class
            return new LatestTestResultDto
            {
                LatestTestResultID = dbLatestTestResult.LatestTestResultID,
                TestResultID = dbLatestTestResult.TestResultId,
                SuiteId = dbLatestTestResult.SuiteId,
                TestId = dbLatestTestResult.TestId,
                TestName = dbLatestTestResult.TestName,
                SuiteName = dbLatestTestResult.SuiteName,
                SuiteApplicationName = dbLatestTestResult.SuiteApplicationName,
                SuiteEnvironmentNameAbbreviation = dbLatestTestResult.SuiteEnvironmentNameAbbreviation,
                PassingFromDate = dbLatestTestResult.PassingFromDateTime,
                FailingFromDate = dbLatestTestResult.FailingFromDateTime
            };
        }

        private void SaveLatestTestResult(DbLatestTestResult latestTestResult)
        {
            if (latestTestResult.LatestTestResultID == 0)
                _db.LatestTestResults.Add(latestTestResult);
            _db.SaveChanges();
        }

        // todo: move to mapping class
        private IQueryable<LatestTestResultDto> Map(IQueryable<DbLatestTestResult> dbLatestTestResults)
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
