using System;
using System.Collections.Generic;
using ZigNet.Services.DTOs;
using ZigNet.Domain.Test;
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

        public void SaveTestResult(TestResult testResult)
        {
            if (string.IsNullOrWhiteSpace(testResult.Test.Name))
                throw new ArgumentNullException("TestName", "Test name cannot be null");

            _testResultService.SaveTestResult(testResult);
        }
    }
}
