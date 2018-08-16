using System;

namespace ZigNet.Services.DTOs
{
    public class TestFailureDurationDto
    {
        public int TestFailureDurationID { get; set; }
        public int TestId { get; set; }
        public int SuiteId { get; set; }
        public int TestResultId { get; set; }
        public DateTime? FailureStart { get; set; }
        public DateTime? FailureEnd { get; set; }
    }
}
