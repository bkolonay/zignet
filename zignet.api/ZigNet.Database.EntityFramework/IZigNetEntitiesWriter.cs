using System.Collections.Generic;
using System.Linq;
using ZigNetSuite = ZigNet.Domain.Suite.Suite;
using ZigNetTest = ZigNet.Domain.Test.Test;

namespace ZigNet.Database.EntityFramework
{
    public interface IZigNetEntitiesWriter
    {
        bool SuiteResultExists(int suiteResultId);
        ZigNetSuite GetZigNetSuite(int suiteId);
        IQueryable<Suite> GetSuites();
        IEnumerable<Test> GetTestsWithTestResultsForSuite(int suiteId);
        Suite GetSuite(int suiteId);
        IQueryable<SuiteCategory> GetSuiteCategories();
        IQueryable<TestCategory> GetTestCategories();
        IQueryable<SuiteResult> GetSuiteResults();
        SuiteResult GetSuiteResult(int suiteResultId);
        SuiteResult GetSuiteResultWithoutTracking(int suiteResultId);
        IQueryable<TestResult> GetTestResults();
        ZigNetTest GetTestOrDefault(string testName);
        Test GetTest(int testId);
        TestFailureType GetTestFailureType(int testFailureTypeId);
        IQueryable<LatestTestResult> GetLatestTestResults();
        int SaveSuite(Suite suite);
        int SaveSuiteResult(SuiteResult suiteResult);
        TestResult SaveTestResult(TestResult testResult);
        void SaveLatestTestResult(LatestTestResult latestTestResult);
    }
}
