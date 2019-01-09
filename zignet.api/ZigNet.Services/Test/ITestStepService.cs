using System.Collections.Generic;
using ZigNet.Domain.Test.TestStep;

namespace ZigNet.Services
{
    public interface ITestStepService
    {
        void Save(int testId, int testResultId, IEnumerable<TestStepResult> testStepResults);
    }
}
