using System;
using System.Collections.Generic;
using System.Linq;
using ZigNet.Database.EntityFramework;
using DbLatestTestResult = ZigNet.Database.EntityFramework.LatestTestResult;
using DtoLatestTestResult = ZigNet.Database.DTOs.LatestTestResult;
using DtoTestFailureDuration = ZigNet.Database.DTOs.TestFailureDuration;

namespace ZigNet.Services.EntityFramework
{
    public class TestResultService : ITestResultService
    {
        private ZigNetEntities _zigNetEntities;

        public TestResultService(IZigNetEntitiesWrapper zigNetEntitiesWrapper)
        {
            _zigNetEntities = zigNetEntitiesWrapper.Get();
        }

        public IEnumerable<DtoLatestTestResult> GetLatestResults(int suiteId, bool group)
        {
            var latestTestResults = new List<DbLatestTestResult>();

            if (group)
            {
                var suite = _zigNetEntities.Suites.AsNoTracking().Single(s => s.SuiteID == suiteId);
                var suites = _zigNetEntities.Suites.AsNoTracking()
                    .Where(s => s.EnvironmentId == suite.EnvironmentId && s.ApplicationId == suite.ApplicationId);
                foreach (var localSuite in suites)
                    latestTestResults.AddRange(_zigNetEntities.LatestTestResults.AsNoTracking().Where(l => l.SuiteId == localSuite.SuiteID));
            }
            else
                latestTestResults = _zigNetEntities.LatestTestResults.AsNoTracking().Where(l => l.SuiteId == suiteId).ToList();

            var allDatabaseTestFailureDurations = _zigNetEntities.TestFailureDurations.AsNoTracking().ToList();

            var latestTestResultDtos = new List<DtoLatestTestResult>();
            var utcNow = DateTime.UtcNow;
            foreach (var latestTestResult in latestTestResults)
            {
                var testFailureDurationLimit = utcNow.AddHours(-24);
                var databaseTestFailureDurationsForTestResult = allDatabaseTestFailureDurations.Where(tfd =>
                    (tfd.SuiteId == latestTestResult.SuiteId && tfd.TestId == latestTestResult.TestId) &&
                    (tfd.FailureEndDateTime > testFailureDurationLimit || tfd.FailureEndDateTime == null)
                );

                var testFailureDurations = new List<DtoTestFailureDuration>();
                foreach (var databaseTestFailureDuration in databaseTestFailureDurationsForTestResult)
                    testFailureDurations.Add(new DtoTestFailureDuration
                    {
                        FailureStart = databaseTestFailureDuration.FailureStartDateTime,
                        FailureEnd = databaseTestFailureDuration.FailureEndDateTime
                    });

                latestTestResultDtos.Add(new DtoLatestTestResult
                {
                    TestResultID = latestTestResult.TestResultId,
                    TestName = latestTestResult.TestName,
                    SuiteName = latestTestResult.SuiteName,
                    FailingFromDate = latestTestResult.FailingFromDateTime,
                    PassingFromDate = latestTestResult.PassingFromDateTime,
                    TestFailureDurations = testFailureDurations
                });
            }
            var passingLatestTestResultDtos = latestTestResultDtos.Where(ltr => ltr.PassingFromDate != null).OrderByDescending(ltr => ltr.PassingFromDate);
            var failingLatestTestResultDtos = latestTestResultDtos.Where(ltr => ltr.FailingFromDate != null).OrderBy(ltr => ltr.FailingFromDate).ToList();
            failingLatestTestResultDtos.AddRange(passingLatestTestResultDtos);

            return failingLatestTestResultDtos;
        }
    }
}
