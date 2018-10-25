using System;
using System.Collections.Generic;
using System.Linq;
using ZigNet.Api.Model;
using ZigNet.Domain.Suite;
using ZigNet.Domain.Test;
using ZigNet.Domain.Test.TestStep;

namespace ZigNet.Api.Mapping
{
    public class ZigNetApiMapper : IZigNetApiMapper
    {
        public Suite MapCreateSuiteModel(CreateSuiteModel createSuiteModel)
        {
            var suite = new Suite {
                Name = createSuiteModel.SuiteName,
                Categories = new List<SuiteCategory>()
            };

            if (createSuiteModel.SuiteCategories != null)
                foreach (var suiteCategoryModel in createSuiteModel.SuiteCategories)
                    suite.Categories.Add(new SuiteCategory { Name = suiteCategoryModel });

            return suite;
        }

        public TestResult MapCreateTestResultModel(CreateTestResultModel createTestResultModel)
        {
            if (!createTestResultModel.StartTime.HasValue)
                throw new ArgumentNullException("StartTime", "Test start time must have a value");
            if (!createTestResultModel.EndTime.HasValue)
                throw new ArgumentNullException("EndTime", "Test end time must have a value");


            var testResult = new TestResult
            {
                Test = new Test { Name = createTestResultModel.TestName, Categories = new List<TestCategory>() },
                SuiteResult = new SuiteResult { SuiteResultID = createTestResultModel.SuiteResultId },
                StartTime = createTestResultModel.StartTime.Value,
                EndTime = createTestResultModel.EndTime.Value,
                ResultType = createTestResultModel.TestResultType,
                TestFailureDetails = new TestFailureDetails {
                    FailureType = createTestResultModel.TestFailureType,
                    FailureDetailMessage = createTestResultModel.TestFailureDetails }
            };

            if (createTestResultModel.TestCategories != null)
                foreach (var testCategoryName in createTestResultModel.TestCategories)
                    testResult.Test.Categories.Add(new TestCategory { Name = testCategoryName });

            if (createTestResultModel.TestStepResults == null)
                testResult.TestStepResults = new List<TestStepResult>();
            else
                testResult.TestStepResults = createTestResultModel.TestStepResults;

            return testResult;
        }
    }
}
