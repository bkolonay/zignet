using System;

namespace ZigNet.Database.DTOs
{
    public class SuiteSummary
    {
        public int SuiteID { get; set; }
        public string SuiteName { get; set; }
        public int TotalPassedTests { get; set; }
        public int TotalFailedTests { get; set; }
        public int TotalInconclusiveTests { get; set; }
        public DateTime? SuiteEndTime { get; set; }
    }
}
