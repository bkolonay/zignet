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
        private ZigNetEntities _zigNetEntities;

        public TestFailureDurationService(IZigNetEntitiesWrapper zigNetEntitiesWrapper)
        {
            _zigNetEntities = zigNetEntitiesWrapper.Get();
        }

        // note: function is not unit tested (change with caution)
        public IEnumerable<TestFailureDurationDto> GetAll()
        {
            return _zigNetEntities.TestFailureDurations
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
            var latestDbTestFailedDuration = _zigNetEntities.TestFailureDurations
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
                _zigNetEntities.TestFailureDurations.Add(testFailedDuration);
            _zigNetEntities.SaveChanges();
        }
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
