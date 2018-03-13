using System.Collections.Generic;
using ZigNet.Business.Models;
using ZigNet.Domain.Suite;
using ZigNet.Domain.Test;

namespace ZigNet.Business
{
    public interface IZigNetBusiness
    {
        IEnumerable<SuiteSummary> GetLatestSuiteResults();
        int CreateSuite(Suite suite);
        void AddSuiteCategory(int suiteId, string suiteCategoryName);
        void DeleteSuiteCategory(int suiteId, string suiteCategoryName);
        int StartSuite(int suiteId);
        int StartSuite(string suiteName);
        void EndSuite(int suiteResultId, SuiteResultType suiteResultType);
        void SaveTestResult(TestResult testResult);
        IEnumerable<LatestTestResult> GetLatestTestResults(int suiteId);
    }
}
