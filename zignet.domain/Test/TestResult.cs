using System;
using ZigNet.Domain.Suite;

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
    }
}
