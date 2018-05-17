using ZigNet.Api.Model;
using ZigNet.Domain.Suite;

namespace ZigNet.Adapter
{
    public interface IZigNetApiHandler
    {
        int StartSuite(int suiteId);
        int StartSuite(string applicationName, string suiteName, string environmentName);
        void SaveTestResult(CreateTestResultModel createTestResultModel);
        void StopSuite(int suiteResultId, SuiteResultType suiteResultType);
    }
}
