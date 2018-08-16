using ZigNet.Domain.Test;
using ZigNet.Services.DTOs;

namespace ZigNet.Services.EntityFramework.Mapping
{
    public interface ITestResultMapper
    {
        TemporaryTestResultDto ToTemporaryTestResult(TestResult testResult);
        LatestTestResultDto ToLatestTestResult(TestResult testResult);
        TestFailureDurationDto ToTestFailureDuration(TestResult testResult);
        int ToDbTestResultTypeId(TestResultType testResultType);
        TestResultType ToTestResultType(int dbTestResultTypeId);
        TestFailureType ToTestFailureType(int dbTestFailureTypeId);
        int ToDbTestFailureTypeId(TestFailureType testFailureType);
    }
}
