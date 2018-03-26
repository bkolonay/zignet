using System.Collections.Generic;
using ZigNet.Database.DTOs;
using ZigNet.Domain.Suite;
using ZigNet.Domain.Test;

namespace ZigNet.Database
{
    public interface IZigNetDatabase
    {
        bool SuiteResultExists(int suiteResultId);
        Suite GetSuite(int suiteId);
        IEnumerable<Suite> GetSuites();
        IEnumerable<SuiteCategory> GetSuiteCategoriesForSuite(int suiteId);
        IEnumerable<SuiteResult> GetSuiteResultsForSuite(int suiteId);
        IEnumerable<TestResult> GetTestResultsForSuite(int suiteId);
        IEnumerable<TestResult> GetTestResultsForSuiteResult(int suiteResultId);
        IEnumerable<ZigNet.Database.DTOs.LatestTestResult> GetLatestTestResults(int suiteResultId);
        IEnumerable<Test> GetTestsForSuite(int suiteId);
        SuiteResult GetSuiteResult(int suiteResultId);
        Test GetTestOrDefault(string testName);
        int SaveSuite(Suite suite);
        int SaveSuiteResult(SuiteResult suiteResult);
        void SaveTestResult(TestResult testResult);
    }
}
