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

        public IEnumerable<LatestTestResultDto> GetLatest(int suiteId, bool group)
        {
            return group ? _testResultService.GetLatestGrouped(suiteId) : _testResultService.GetLatest(suiteId);
        }

        public void Save(TestResult testResult)
        {
            if (string.IsNullOrWhiteSpace(testResult.Test.Name))
                throw new ArgumentNullException("TestName", "Test name cannot be null");

            _testResultSaverService.Save(testResult);
        }
    }
}
