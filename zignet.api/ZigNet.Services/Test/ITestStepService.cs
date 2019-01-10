using System.Collections.Generic;
using ZigNet.Domain.Test.TestStep;

namespace ZigNet.Services
{
    public interface ITestStepService
    {
        ICollection<TestStepResult> Save(int testId, int testResultId, IEnumerable<TestStepResult> testStepResults);
    }
}
