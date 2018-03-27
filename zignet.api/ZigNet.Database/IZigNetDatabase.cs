using System.Collections.Generic;
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
        IEnumerable<LatestTestResult> GetLatestTestResults(int suiteResultId);
        IEnumerable<Suite> GetSuites();
        IEnumerable<SuiteSummary> GetLatestSuiteResults();


        bool SuiteResultExists(int suiteResultId);
        IEnumerable<SuiteCategory> GetSuiteCategoriesForSuite(int suiteId);
        IEnumerable<SuiteResult> GetSuiteResultsForSuite(int suiteId);
        IEnumerable<TestResult> GetTestResultsForSuite(int suiteId);
        IEnumerable<TestResult> GetTestResultsForSuiteResult(int suiteResultId);
        IEnumerable<Test> GetTestsForSuite(int suiteId);
        SuiteResult GetSuiteResult(int suiteResultId);
        void SaveTestResult(TestResult testResult);

        int SaveSuite(Suite suite);
        
    }
}
