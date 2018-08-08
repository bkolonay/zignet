using System.Collections.Generic;
using ZigNet.Database.DTOs;
using ZigNet.Domain.Test;

namespace ZigNet.Services
{
    public interface ITestResultService
    {
        IEnumerable<LatestTestResult> GetLatestResults(int suiteId, bool group);
        void SaveTestResult(TestResult testResult);


        Test GetMappedTestWithCategoriesOrDefault(string testName);
    }
}
