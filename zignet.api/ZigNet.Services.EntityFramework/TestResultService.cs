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
            var dbLatestTestResults = new List<DbLatestTestResult>();

            if (group)
            {
                var suite = _zigNetEntities.Suites.AsNoTracking().Single(s => s.SuiteID == suiteId);
                var suites = _zigNetEntities.Suites.AsNoTracking()
                    .Where(s => s.EnvironmentId == suite.EnvironmentId && s.ApplicationId == suite.ApplicationId);
                foreach (var localSuite in suites)
                    dbLatestTestResults.AddRange(_zigNetEntities.LatestTestResults.AsNoTracking().Where(l => l.SuiteId == localSuite.SuiteID));
            }
            else
                dbLatestTestResults = _zigNetEntities.LatestTestResults.AsNoTracking().Where(l => l.SuiteId == suiteId).ToList();

            var dbTestFailureDurations = _zigNetEntities.TestFailureDurations.AsNoTracking().ToList();

            var dtoLatestTestResults = new List<DtoLatestTestResult>();
            var utcNow = DateTime.UtcNow;
            foreach (var dbLatestTestResult in dbLatestTestResults)
            {
                var testFailureDurationLimit = utcNow.AddHours(-24);
                var dbTestFailureDurationsForTestResult = 
                    dbTestFailureDurations.Where(t =>
                        (t.SuiteId == dbLatestTestResult.SuiteId && t.TestId == dbLatestTestResult.TestId) &&
                        (t.FailureEndDateTime > testFailureDurationLimit || t.FailureEndDateTime == null)
                );

                var dtoTestFailureDurations = new List<DtoTestFailureDuration>();
                foreach (var dbTestFailureDuration in dbTestFailureDurationsForTestResult)
                    dtoTestFailureDurations.Add(new DtoTestFailureDuration
                    {
                        FailureStart = dbTestFailureDuration.FailureStartDateTime,
                        FailureEnd = dbTestFailureDuration.FailureEndDateTime
                    });

                dtoLatestTestResults.Add(new DtoLatestTestResult
                {
                    TestResultID = dbLatestTestResult.TestResultId,
                    TestName = dbLatestTestResult.TestName,
                    SuiteName = dbLatestTestResult.SuiteName,
                    FailingFromDate = dbLatestTestResult.FailingFromDateTime,
                    PassingFromDate = dbLatestTestResult.PassingFromDateTime,
                    TestFailureDurations = dtoTestFailureDurations
                });
            }

            var passingDtoLatestTestResults = dtoLatestTestResults.Where(ltr => ltr.PassingFromDate != null).OrderByDescending(ltr => ltr.PassingFromDate);
            var failingDtoLatestTestResults = dtoLatestTestResults.Where(ltr => ltr.FailingFromDate != null).OrderBy(ltr => ltr.FailingFromDate).ToList();
            failingDtoLatestTestResults.AddRange(passingDtoLatestTestResults);

            return failingDtoLatestTestResults;
        }
    }
}
