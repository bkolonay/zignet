using ZigNet.Domain.Test;
using ZigNet.Services.DTOs;

namespace ZigNet.Services.EntityFramework.Mapping
{
    public interface ITestResultMapper
    {
        TemporaryTestResultDto ToTemporaryTestResult(TestResult testResult);
        LatestTestResultDto ToLatestTestResult(TestResult testResult);
        int Map(TestResultType testResultType);
    }
}
