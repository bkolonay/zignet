using ZigNet.Domain.Suite;

namespace ZigNet.Business
{
    public interface ISuiteBusinessProvider
    {
        int StartSuite(int suiteId);
        int StartSuite(string applicationName, string suiteName, string environmentName);
        void StopSuite(int suiteResultId, SuiteResultType suiteResultType);
    }
}
