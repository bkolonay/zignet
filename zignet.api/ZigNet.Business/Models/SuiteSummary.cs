using System;

namespace ZigNet.Business.Models
{
    public class SuiteSummary
    {
        public string SuiteName { get; set; }
        public int TotalPassedTests { get; set; }
        public int TotalFailedTests { get; set; }
        public int TotalInconclusiveTests { get; set; }
        public DateTime? SuiteEndTime { get; set; }
    }
}