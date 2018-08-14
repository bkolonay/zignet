﻿using System;
using ZigNet.Domain.Test;
using ZigNet.Services.DTOs;

namespace ZigNet.Services.EntityFramework.Mapping
{
    public class TestResultMapper : ITestResultMapper
    {
        // todo: unit test public interface

        public TemporaryTestResultDto Map(TestResult testResult)
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
    }
}
