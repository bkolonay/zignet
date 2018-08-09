using System.Collections.Generic;
using ZigNet.Services.DTOs;
using ZigNet.Domain.Test;

namespace ZigNet.Business
{
    public interface ITestResultBusinessProvider
    {
        IEnumerable<LatestTestResult> GetLatestResults(int suiteId, bool group);
        void SaveTestResult(TestResult testResult);
    }
}
