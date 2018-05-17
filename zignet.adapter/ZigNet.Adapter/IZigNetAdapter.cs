using System;
using ZigNet.Domain.Suite;

namespace ZigNet.Adapter
{
    public interface IZigNetAdapter
    {
        int StartSuite(string applicationName, string suiteName, string environmentName);
        void SaveTestResult(DateTime testStartTime);
        void StopSuite(int suiteResultId, SuiteResultType suiteResultType);
    }
}
