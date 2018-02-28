using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using TechTalk.SpecFlow;
using ZigNet.Api.Model;
using ZigNet.Domain.Suite;
using ZigNet.Domain.Test;

namespace ZigNet.Adapter.SpecFlow
{
    public class ZigNetSpecFlowAdapterBase : IZigNetAdapter
    {
        private IZigNetApiHandler _zigNetApiHandler;
        private string _suiteResultIdFilePath;

        public ZigNetSpecFlowAdapterBase(IZigNetApiHandler zigNetApiHandler)
        {
            _zigNetApiHandler = zigNetApiHandler;
            _suiteResultIdFilePath = Path.Combine(
                Path.GetDirectoryName(
                new Uri(Assembly.GetExecutingAssembly().GetName().CodeBase).LocalPath),
                "suiteResultId.txt"
            );
        }

        public void SaveTestResult(DateTime testStartTime)
        {
            int suiteResultId;
            using (StreamReader streamReader = new StreamReader(_suiteResultIdFilePath))
                suiteResultId = int.Parse(streamReader.ReadToEnd());

            var testCategories = new List<string>();
            foreach (var tag in ScenarioContext.Current.ScenarioInfo.Tags)
                testCategories.Add(tag.ToString());
            foreach (var tag in FeatureContext.Current.FeatureInfo.Tags)
                testCategories.Add(tag.ToString());

            var createTestResultModel = new CreateTestResultModel
            {
                SuiteResultId = suiteResultId,
                StartTime = testStartTime,
                EndTime = DateTime.UtcNow,
                TestName = ScenarioContext.Current.ScenarioInfo.Title,
                TestCategories = testCategories.ToArray()
            };

            var specFlowException = ScenarioContext.Current.TestError;
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
            using (StreamWriter streamWriter = new StreamWriter(_suiteResultIdFilePath))
            {
                streamWriter.Write(suiteResultId);
            }
            return suiteResultId;
        }

        public void StopSuite(int suiteResultId, SuiteResultType suiteResultType)
        {
            _zigNetApiHandler.StopSuite(suiteResultId, suiteResultType);
        }
    }
}
