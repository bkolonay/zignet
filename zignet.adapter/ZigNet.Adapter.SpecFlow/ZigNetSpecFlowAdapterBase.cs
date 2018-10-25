using System;
using System.Collections.Generic;
using System.IO;
using ZigNet.Adapter.SpecFlow.Utility;
using ZigNet.Api.Model;
using ZigNet.Domain.Suite;
using ZigNet.Domain.Test;
using ZigNet.Domain.Test.TestStep;

namespace ZigNet.Adapter.SpecFlow
{
    public class ZigNetSpecFlowAdapterBase : IZigNetAdapter
    {
        private IZigNetApiHandler _zigNetApiHandler;
        private IFileService _fileService;
        private ISpecFlowContextWrapper _specFlowContext;
        private Stack<TestStepResult> _testStepResults;
        private readonly string _suiteResultIdFilePath;

        public ZigNetSpecFlowAdapterBase(IZigNetApiHandler zigNetApiHandler, IDirectoryService directoryService,
            IFileService fileService, ISpecFlowContextWrapper specFlowContext)
        {
            _zigNetApiHandler = zigNetApiHandler;
            _fileService = fileService;
            _specFlowContext = specFlowContext;
            _suiteResultIdFilePath = Path.Combine(directoryService.GetExecutingDirectory(), "suiteResultId.txt");
            _testStepResults = new Stack<TestStepResult>();
        }

        public int StartSuite(string applicationName, string suiteName, string environmentName)
        {
            var suiteResultId = _zigNetApiHandler.StartSuite(applicationName, suiteName, environmentName);
            _fileService.WriteStringToFile(_suiteResultIdFilePath, suiteResultId.ToString());
            return suiteResultId;
        }

        public void SaveTestResult(DateTime testStartTime)
        {
            var suiteResultId = int.Parse(_fileService.ReadStringFromFile(_suiteResultIdFilePath));

            var createTestResultModel = new CreateTestResultModel
            {
                SuiteResultId = suiteResultId,
                StartTime = testStartTime,
                EndTime = DateTime.UtcNow,
                TestName = _specFlowContext.GetScenarioTitle(),
                TestCategories = _specFlowContext.GetScenarioAndFeatureTags(),
                TestStepResults = _testStepResults.ToArray()
            };

            var specFlowException = _specFlowContext.GetScenarioTestError();
            if (specFlowException == null)
                createTestResultModel.TestResultType = TestResultType.Pass;
            else
            {
                createTestResultModel.TestResultType = TestResultType.Fail;
                if (specFlowException.GetType().Name.Contains("Assert"))
                    createTestResultModel.TestFailureType = TestFailureType.Assertion;
                else
                    createTestResultModel.TestFailureType = TestFailureType.Exception;
                createTestResultModel.TestFailureDetails = string.Format("Message: {0}, Inner Exception: {1}, Stack Trace: {2}",
                    specFlowException.Message, specFlowException.InnerException, specFlowException.StackTrace);
            }

            _zigNetApiHandler.SaveTestResult(createTestResultModel);
        }

        public void StartTestStep()
        {
            var utcNow = DateTime.UtcNow;

            _testStepResults.Push(new TestStepResult
            {
                TestStep = new TestStep { Name = _specFlowContext.GetTestStepName() },
                StartTime = utcNow
            });
        }

        public void StopTestStep()
        {
            var utcNow = DateTime.UtcNow;

            var testStepResult = _testStepResults.Peek();
            testStepResult.EndTime = utcNow;
        }

        public void StopSuite(SuiteResultType suiteResultType)
        {
            var suiteResultId = int.Parse(_fileService.ReadStringFromFile(_suiteResultIdFilePath));
            _zigNetApiHandler.StopSuite(suiteResultId, suiteResultType);
        }
    }
}
