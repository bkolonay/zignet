using System;

namespace ZigNet.Business.Models
{
    public class LatestTestResult
    {
        public int TestResultID { get; set; }
        public string TestName { get; set; }
        public DateTime? FailingFromDate { get; set; }
        public DateTime? PassingFromDate { get; set; }
    }
}
