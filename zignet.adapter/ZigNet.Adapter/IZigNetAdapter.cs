using System;
using ZigNet.Domain.Suite;

namespace ZigNet.Adapter
{
    public interface IZigNetAdapter
    {
        int StartSuite(string suiteName);
        void SaveTestResult(DateTime testStartTime);
        void StopSuite(int suiteResultId, SuiteResultType suiteResultType);
    }
}
