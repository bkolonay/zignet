using System.Collections.Generic;
using ZigNet.Domain.Suite;
using ZigNet.Domain.Test;

namespace ZigNet.Database
{
    public interface IZigNetDatabase
    {
        IEnumerable<Suite> GetSuites();
        IEnumerable<SuiteCategory> GetSuiteCategoriesForSuite(int suiteId);
        IEnumerable<SuiteResult> GetSuiteResultsForSuite(int suiteId);
        SuiteResult GetSuiteResult(int suiteResultId);
        IEnumerable<TestResult> GetTestResultsForSuiteResult(int suiteResultId);
        IEnumerable<Test> GetTestsForSuite(int suiteId);
        TestResult GetLatestTestResultInSuite(int testId, int suiteId);
        Test GetTestOrDefault(string testName);
        int SaveSuite(Suite suite);
        int SaveSuiteResult(SuiteResult suiteResult);
        void SaveTestResult(TestResult testResult);
    }
}
