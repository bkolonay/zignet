using System.Collections.Generic;
using ZigNet.Services.DTOs;
using ZigNet.Domain.Test;

namespace ZigNet.Services
{
    public interface ITestResultService
    {
        IEnumerable<LatestTestResultDto> GetLatestResults(int suiteId);
        IEnumerable<LatestTestResultDto> GetLatestResultsGrouped(int suiteId);
        TestResult SaveTestResult(TestResult testResult);
    }
}
