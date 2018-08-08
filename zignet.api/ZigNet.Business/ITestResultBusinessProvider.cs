using System.Collections.Generic;
using ZigNet.Database.DTOs;
using ZigNet.Domain.Test;

namespace ZigNet.Business
{
    public interface ITestResultBusinessProvider
    {
        IEnumerable<LatestTestResult> GetLatestResults(int suiteId, bool group);
        void SaveTestResult(TestResult testResult);
    }
}
