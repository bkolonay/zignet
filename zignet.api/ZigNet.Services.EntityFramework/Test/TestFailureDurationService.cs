using System;
using System.Collections.Generic;
using System.Linq;
using ZigNet.Database.EntityFramework;
using ZigNet.Services.DTOs;
using DbTestFailureDuration = ZigNet.Database.EntityFramework.TestFailureDuration;
using TestResultType = ZigNet.Domain.Test.TestResultType;

namespace ZigNet.Services.EntityFramework
{
    public class TestFailureDurationService : ITestFailureDurationService
    {
        private ZigNetEntities _db;

        public TestFailureDurationService(IDbContext dbContext)
        {
            _db = dbContext.Get();
        }

        // note: function is not unit tested (change with caution)
        public IEnumerable<TestFailureDurationDto> GetAll()
        {
            return _db.TestFailureDurations
                .AsNoTracking()
                .Select(f => new TestFailureDurationDto
                {
                    TestId = f.TestId,
                    SuiteId = f.SuiteId,
                    FailureEnd = f.FailureEndDateTime,
                    FailureStart = f.FailureStartDateTime
                });
        }

        public TestFailureDurationDto Save(TestFailureDurationDto testFailureDurationDto, TestResultType testResultType, DateTime utcNow)
        {
            var latestDbTestFailedDuration = _db.TestFailureDurations
                .OrderByDescending(f => f.FailureStartDateTime)
                .FirstOrDefault(f =>
                    f.SuiteId == testFailureDurationDto.SuiteId &&
                    f.TestId == testFailureDurationDto.TestId
                );

            if (testResultType == TestResultType.Pass
                && latestDbTestFailedDuration != null
                && latestDbTestFailedDuration.FailureStartDateTime != null && latestDbTestFailedDuration.FailureEndDateTime == null)
            {
                latestDbTestFailedDuration.TestResultId = testFailureDurationDto.TestResultId;
                latestDbTestFailedDuration.FailureEndDateTime = utcNow;
                SaveTestFailedDuration(latestDbTestFailedDuration);
                return Map(latestDbTestFailedDuration);
            }
            else if (testResultType == TestResultType.Fail || testResultType == TestResultType.Inconclusive)
            {
                if (latestDbTestFailedDuration == null || latestDbTestFailedDuration.FailureEndDateTime != null)
                {
                    var newDbTestFailedDuration = new DbTestFailureDuration
                    {
                        SuiteId = testFailureDurationDto.SuiteId,
                        TestId = testFailureDurationDto.TestId,
                        TestResultId = testFailureDurationDto.TestResultId,
                        FailureStartDateTime = utcNow
                    };
                    SaveTestFailedDuration(newDbTestFailedDuration);
                    return Map(newDbTestFailedDuration);
                }
            }

            return null;
        }

        private void SaveTestFailedDuration(DbTestFailureDuration testFailedDuration)
        {
            if (testFailedDuration.TestFailureDurationID == 0)
                _db.TestFailureDurations.Add(testFailedDuration);
            _db.SaveChanges();
        }

        // todo: move to mapping class
        private TestFailureDurationDto Map(DbTestFailureDuration dbTestFailureDuration)
        {
            return new TestFailureDurationDto
            {
                TestFailureDurationID = dbTestFailureDuration.TestFailureDurationID,
                TestResultId = dbTestFailureDuration.TestResultId,
                TestId = dbTestFailureDuration.TestId,
                SuiteId = dbTestFailureDuration.SuiteId,
                FailureEnd = dbTestFailureDuration.FailureEndDateTime,
                FailureStart = dbTestFailureDuration.FailureStartDateTime
            };
        }
    }
}
