using System;
using System.Collections.Generic;

namespace ZigNet.Services.DTOs
{
    public class LatestTestResultDto
    {
        public int TestResultID { get; set; }
        public int TestId { get; set; }
        public string TestName { get; set; }
        public int SuiteId { get; set; }
        public string SuiteName { get; set; }
        public DateTime? FailingFromDate { get; set; }
        public DateTime? PassingFromDate { get; set; }
        public IEnumerable<TestFailureDurationDto> TestFailureDurations { get; set; }
    }
}
