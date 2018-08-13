using System;

namespace ZigNet.Services.DTOs
{
    public class TestFailureDurationDto
    {
        public int TestId { get; set; }
        public int SuiteId { get; set; }
        public DateTime? FailureStart { get; set; }
        public DateTime? FailureEnd { get; set; }
    }
}
