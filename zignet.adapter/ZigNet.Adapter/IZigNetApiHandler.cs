using ZigNet.Api.Model;
using ZigNet.Domain.Suite;

namespace ZigNet.Adapter
{
    public interface IZigNetApiHandler
    {
        int StartSuite(int suiteId);
        int StartSuite(string suiteName);
        void SaveTestResult(CreateTestResultModel createTestResultModel);
        void StopSuite(int suiteResultId, SuiteResultType suiteResultType);
    }
}
