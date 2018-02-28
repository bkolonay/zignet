using System;
using ZigNet.Domain.Test;

namespace ZigNet.Api.Model
{
    public class CreateTestResultModel
    {
        public string TestName { get; set; }
        public int SuiteResultId { get; set; }
        public string[] TestCategories { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public TestResultType TestResultType { get; set; }
        public TestFailureType TestFailureType { get; set; }
        public string TestFailureDetails { get; set; }
    }
}