using System;
using System.Collections.Generic;
using ZigNet.Domain.Suite;
using ZigNet.Domain.Test.TestStep;

namespace ZigNet.Domain.Test
{
    public class TestResult
    {
        public int TestResultID { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public Test Test { get; set; }
        public TestResultType ResultType { get; set; }
        public TestFailureDetails TestFailureDetails { get; set; }
        public SuiteResult SuiteResult { get; set; }
        public ICollection<TestStepResult> TestStepResults { get; set; }
    }
}
