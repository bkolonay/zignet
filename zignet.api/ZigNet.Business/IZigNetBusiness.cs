﻿using System.Collections.Generic;
using ZigNet.Database.DTOs;
using ZigNet.Domain.Suite;
using ZigNet.Domain.Test;

namespace ZigNet.Business
{
    public interface IZigNetBusiness
    {
        IEnumerable<LatestTestResult> GetLatestTestResults(int suiteId, bool groupResultsByApplicationAndEnvironment);
        void SaveTestResult(TestResult testResult);

        int CreateSuite(Suite suite);
        void AddSuiteCategory(int suiteId, string suiteCategoryName);
        void DeleteSuiteCategory(int suiteId, string suiteCategoryName);
    }
}
