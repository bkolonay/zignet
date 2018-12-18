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
        private ITestResultSaverService _testResultSaverService;

        public TestResultBusinessProvider(ITestResultService testResultService, ITestResultSaverService testResultSaverService)
        {
            _testResultService = testResultService;
            _testResultSaverService = testResultSaverService;
        }

        public IEnumerable<LatestTestResultDto> GetLatest(SuiteResultsFilter suiteResultsFilter)
        {
            return _testResultService.GetLatest(suiteResultsFilter);
        }

        public void Save(TestResult testResult)
        {
            if (string.IsNullOrWhiteSpace(testResult.Test.Name))
                throw new ArgumentNullException("TestName", "Test name cannot be null");
            foreach (var testStepResult in testResult.TestStepResults)
                if (string.IsNullOrWhiteSpace(testStepResult.TestStep.Name))
                    throw new ArgumentNullException("TestStepName", "Test step name cannot be null");

            _testResultSaverService.Save(testResult);
        }
    }
}
