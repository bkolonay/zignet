using System.Collections.Generic;
using ZigNet.Database.DTOs;
using ZigNet.Domain.Suite;
using ZigNet.Domain.Test;

namespace ZigNet.Database
{
    public interface IZigNetDatabase
    {
        string GetSuiteName(int suiteId, bool groupSuiteNameByApplicationAndEnvironment);
        IEnumerable<LatestTestResult> GetLatestTestResults(int suiteId, bool groupResultsByApplicationAndEnvironment);
        IEnumerable<SuiteSummary> GetLatestSuiteResults(bool groupResultsByApplicationAndEnvironment);
        void SaveTestResult(TestResult testResult);

        IEnumerable<Suite> GetMappedSuites();
        IEnumerable<SuiteCategory> GetSuiteCategoriesForSuite(int suiteId);
        int SaveSuite(Suite suite);
    }
}
