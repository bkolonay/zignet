using ZigNet.Domain.Test;
using ZigNet.Services.DTOs;

namespace ZigNet.Services.EntityFramework.Mapping
{
    public interface ITestResultMapper
    {
        TemporaryTestResultDto ToTemporaryTestResult(TestResult testResult);
        LatestTestResultDto ToLatestTestResult(TestResult testResult);
        TestFailureDurationDto ToTestFailureDuration(TestResult testResult);
        TestFailureType ToTestFailureType(int dbTestFailureTypeId);
        int Map(TestResultType testResultType);
        TestResultType Map(int dbTestResultTypeId);
    }
}
