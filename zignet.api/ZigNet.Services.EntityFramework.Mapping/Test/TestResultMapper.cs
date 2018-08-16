using System;
using ZigNet.Domain.Test;
using ZigNet.Services.DTOs;

namespace ZigNet.Services.EntityFramework.Mapping
{
    public class TestResultMapper : ITestResultMapper
    {
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

        public TemporaryTestResultDto ToTemporaryTestResult(TestResult testResult)
        {
            return new TemporaryTestResultDto
            {
                TestResultId = testResult.TestResultID,
                SuiteResultId = testResult.SuiteResult.SuiteResultID,
                SuiteId = testResult.SuiteResult.Suite.SuiteID,
                TestResultTypeId = ToDbTestResultTypeId(testResult.ResultType)
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

        public TestResultType ToTestResultType(int dbTestResultTypeId)
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

        public int ToDbTestResultTypeId(TestResultType testResultType)
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

        public TestFailureType ToTestFailureType(int dbTestFailureTypeId)
        {
            switch (dbTestFailureTypeId)
            {
                case 1:
                    return TestFailureType.Assertion;
                case 2:
                    return TestFailureType.Exception;
                default:
                    throw new InvalidOperationException("DB test failure type ID not recognized");
            }
        }

        public int ToDbTestFailureTypeId(TestFailureType testFailureType)
        {
            switch (testFailureType)
            {
                case TestFailureType.Assertion:
                    return 1;
                case TestFailureType.Exception:
                    return 2;
                default:
                    throw new InvalidOperationException("Test Failure Type not recognized");
            }
        }
    }
}
