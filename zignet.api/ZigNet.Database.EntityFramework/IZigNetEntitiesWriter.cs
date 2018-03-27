using System.Linq;

namespace ZigNet.Database.EntityFramework
{
    public interface IZigNetEntitiesWriter
    {
        Suite GetSuite(int suiteId);
        Test GetTestWithSuites(int testId);
        SuiteResult GetSuiteResult(int suiteResultId);        
        TestFailureType GetTestFailureType(int testFailureTypeId);
        IQueryable<LatestTestResult> GetLatestTestResults();
        IQueryable<TestCategory> GetTestCategories();
        TestResult SaveTestResult(TestResult testResult);
        int SaveSuiteResult(SuiteResult suiteResult);
        void SaveLatestTestResult(LatestTestResult latestTestResult);

        IQueryable<Suite> GetSuites();
        IQueryable<SuiteCategory> GetSuiteCategories();
        int SaveSuite(Suite suite);
    }
}
