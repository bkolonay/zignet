using System;
using ZigNet.Domain.Test;
using ZigNet.Services.DTOs;

namespace ZigNet.Services.EntityFramework.Mapping
{
    public class TestResultMapper : ITestResultMapper
    {
        // todo: unit test public interface

        public TemporaryTestResultDto ToTemporaryTestResult(TestResult testResult)
        {
            return new TemporaryTestResultDto
            {
                TestResultId = testResult.TestResultID,
                SuiteResultId = testResult.SuiteResult.SuiteResultID,
                SuiteId = testResult.SuiteResult.Suite.SuiteID,
                TestResultTypeId = Map(testResult.ResultType)
            };
        }

        public int Map(TestResultType testResultType)
        {
            switch (testResultType)
            {
                case TestResultType.Fail:
                    return 1;
                case TestResultType.Inconclusive:
                    return 2;
                case TestResultType.Pass:
                    return 3;
                default:
                    throw new InvalidOperationException("Test Result Type not recognized");
            }
        }

        public TestResultType Map(int dbTestResultTypeId)
        {
            switch (dbTestResultTypeId)
            {
                case 1:
                    return TestResultType.Fail;
                case 2:
                    return TestResultType.Inconclusive;
                case 3:
                    return TestResultType.Pass;
                default:
                    throw new InvalidOperationException("DB test result type ID not recognized");
            }
        }

        public LatestTestResultDto ToLatestTestResult(TestResult testResult)
        {
            return new LatestTestResultDto
            {
                TestResultID = testResult.TestResultID,
                SuiteId = testResult.SuiteResult.Suite.SuiteID,
                TestId = testResult.Test.TestID,
                TestName = testResult.Test.Name,
                SuiteName = testResult.SuiteResult.Suite.Name
            };
        }

        public TestFailureDurationDto ToTestFailureDuration(TestResult testResult)
        {
            return new TestFailureDurationDto
            {
                SuiteId = testResult.SuiteResult.Suite.SuiteID,
                TestId = testResult.Test.TestID,
                TestResultId = testResult.TestResultID
            };
        }
    }
}
