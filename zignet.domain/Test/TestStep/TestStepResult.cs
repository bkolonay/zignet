using System;

namespace ZigNet.Domain.Test.TestStep
{
    public class TestStepResult
    {
        public int TestStepResultID { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public TestStep TestStep { get; set; }
        public TestStepResultType ResultType { get; set; }
        public TestResult TestResult { get; set; }
    }
}
