﻿using System.Collections.Generic;
using ZigNet.Database.DTOs;
using ZigNet.Domain.Suite;
using ZigNet.Domain.Test;

namespace ZigNet.Database
{
    public interface IZigNetDatabase
    {
        int StartSuite(int suiteId);
        int StartSuite(string suiteName);
        void StopSuite(int suiteResultId, SuiteResultType suiteResultType);
        string GetSuiteName(int suiteId);
        IEnumerable<LatestTestResult> GetLatestTestResults(int suiteResultId);
        IEnumerable<SuiteSummary> GetLatestSuiteResults();
        void SaveTestResult(TestResult testResult);

        IEnumerable<Suite> GetMappedSuites();
        IEnumerable<SuiteCategory> GetSuiteCategoriesForSuite(int suiteId);
        int SaveSuite(Suite suite);
    }
}
