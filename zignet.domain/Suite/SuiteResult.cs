using System;
using System.Collections.Generic;
using ZigNet.Domain.Test;

namespace ZigNet.Domain.Suite
{
    public class SuiteResult
    {
        public int SuiteResultID { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public Suite Suite { get; set; }
        public SuiteResultType ResultType { get; set; }
        public ICollection<TestResult> TestResults { get; set; }
    }
}
