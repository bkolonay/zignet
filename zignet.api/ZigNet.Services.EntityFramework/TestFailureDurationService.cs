using System.Collections.Generic;
using System.Linq;
using ZigNet.Database.EntityFramework;
using ZigNet.Services.DTOs;

namespace ZigNet.Services.EntityFramework
{
    public class TestFailureDurationService : ITestFailureDurationService
    {
        private ZigNetEntities _zigNetEntities;

        public TestFailureDurationService(IZigNetEntitiesWrapper zigNetEntitiesWrapper)
        {
            _zigNetEntities = zigNetEntitiesWrapper.Get();
        }

        // note: class is not unit tested (change with caution)
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
    }
}
