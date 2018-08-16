using ZigNet.Domain.Suite;

namespace ZigNet.Services
{
    public interface ISuiteResultService
    {
        SuiteResult Get(int suiteResultId);
        int SaveSuiteResult(SuiteResult suiteResult);
    }
}
