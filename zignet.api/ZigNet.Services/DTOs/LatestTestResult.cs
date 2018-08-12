﻿using System;
using System.Collections.Generic;

namespace ZigNet.Services.DTOs
{
    public class LatestTestResult
    {
        public int TestResultID { get; set; }
        public string TestName { get; set; }
        public string SuiteName { get; set; }
        public DateTime? FailingFromDate { get; set; }
        public DateTime? PassingFromDate { get; set; }
        public IEnumerable<TestFailureDuration> TestFailureDurations { get; set; }
    }
}