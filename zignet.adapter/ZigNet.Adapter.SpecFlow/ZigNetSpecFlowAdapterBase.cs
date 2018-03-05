using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using ZigNet.Adapter.SpecFlow.Utility;
using ZigNet.Api.Model;
using ZigNet.Domain.Suite;
using ZigNet.Domain.Test;

namespace ZigNet.Adapter.SpecFlow
{
    public class ZigNetSpecFlowAdapterBase : IZigNetAdapter
    {
        private IZigNetApiHandler _zigNetApiHandler;
        private IFileService _fileService;
        private ISpecFlowContextWrapper _specFlowContext;
        private string _suiteResultIdFilePath;

        public ZigNetSpecFlowAdapterBase(IZigNetApiHandler zigNetApiHandler, IDirectoryService directoryService,
            IFileService fileService, ISpecFlowContextWrapper specFlowContext)
        {
            _zigNetApiHandler = zigNetApiHandler;
            _fileService = fileService;
            _specFlowContext = specFlowContext;
            _suiteResultIdFilePath = Path.Combine(directoryService.GetExecutingDirectory(), "suiteResultId.txt");
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
                TestCategories = _specFlowContext.GetScenarioAndFeatureTags()
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

        public int StartSuite(string suiteName)
        {
            var suiteResultId = _zigNetApiHandler.StartSuite(suiteName);
            _fileService.WriteStringToFile(_suiteResultIdFilePath, suiteResultId.ToString());
            return suiteResultId;
        }

        public void StopSuite(int suiteResultId, SuiteResultType suiteResultType)
        {
            _zigNetApiHandler.StopSuite(suiteResultId, suiteResultType);
        }
    }
}
