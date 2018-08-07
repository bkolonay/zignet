using System.Collections.Generic;
using ZigNet.Database.DTOs;
using ZigNet.Services;

namespace ZigNet.Business
{
    public class TestResultBusinessProvider : ITestResultBusinessProvider
    {
        private ITestResultService _testResultService;

        public TestResultBusinessProvider(ITestResultService testResultService)
        {
            _testResultService = testResultService;
        }

        public IEnumerable<LatestTestResult> GetLatestResults(int suiteId, bool group)
        {
            return _testResultService.GetLatestResults(suiteId, group);
        }
    }
}
