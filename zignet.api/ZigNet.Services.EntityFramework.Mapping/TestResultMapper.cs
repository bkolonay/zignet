using System;
using ZigNet.Domain.Test;

namespace ZigNet.Services.EntityFramework.Mapping
{
    public class TestResultMapper : ITestResultMapper
    {
        // todo: could unit test this
        public int Map(TestResultType testResultType)
        {
            switch (testResultType)
            {
                case TestResultType.Fail:
                    return 1;
                case TestResultType.Inconclusive:
                    return 2;
                case TestResultType.Pass:
                    return 3;
                default:
                    throw new InvalidOperationException("Test Result Type not recognized");
            }
        }
    }
}
