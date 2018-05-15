using System.Collections.Generic;
using ZigNet.Database.DTOs;
using ZigNet.Domain.Suite;
using ZigNet.Domain.Test;

namespace ZigNet.Business
{
    public interface IZigNetBusiness
    {
        int StartSuite(int suiteId);
        int StartSuite(string suiteName);
        void StopSuite(int suiteResultId, SuiteResultType suiteResultType);
        string GetSuiteName(int suiteId, bool groupSuiteNameByApplicationAndEnvironment);
        IEnumerable<LatestTestResult> GetLatestTestResults(int suiteId, bool groupResultsByApplicationAndEnvironment);
        IEnumerable<SuiteSummary> GetLatestSuiteResults(bool groupResultsByApplicationAndEnvironment);
        void SaveTestResult(TestResult testResult);

        int CreateSuite(Suite suite);
        void AddSuiteCategory(int suiteId, string suiteCategoryName);
        void DeleteSuiteCategory(int suiteId, string suiteCategoryName);
    }
}
