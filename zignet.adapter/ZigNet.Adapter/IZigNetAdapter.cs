using System;
using ZigNet.Domain.Suite;

namespace ZigNet.Adapter
{
    public interface IZigNetAdapter
    {
        int StartSuite(string applicationName, string suiteName, string environmentName);
        void StartTestStep();
        void StopTestStep();
        void SaveTestResult(DateTime testStartTime);
        void StopSuite(SuiteResultType suiteResultType);
    }
}
