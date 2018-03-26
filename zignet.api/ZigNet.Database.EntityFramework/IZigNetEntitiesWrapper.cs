using System.Collections.Generic;
using System.Linq;

namespace ZigNet.Database.EntityFramework
{
    public interface IZigNetEntitiesWrapper
    {
        IQueryable<Suite> GetSuites();
        IEnumerable<Test> GetTestsWithTestResultsForSuite(int suiteId);
        Suite GetSuite(int suiteId);
        IQueryable<SuiteCategory> GetSuiteCategories();
        IQueryable<TestCategory> GetTestCategories();
        IQueryable<SuiteResult> GetSuiteResults();
        SuiteResult GetSuiteResult(int suiteResultId);
        IQueryable<TestResult> GetTestResults();
        Test GetTestOrDefault(string testName);
        Test GetTest(int testId);
        TestFailureType GetTestFailureType(int testFailureTypeId);
        int SaveSuite(Suite suite);
        int SaveSuiteResult(SuiteResult suiteResult);
        void SaveTestResult(TestResult testResult);
    }
}
