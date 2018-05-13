using System.Linq;

namespace ZigNet.Database.EntityFramework
{
    public interface IZigNetEntitiesWriter
    {
        Suite GetSuite(int suiteId);
        SuiteResult GetSuiteResult(int suiteResultId);
        Test GetTestWithSuites(int testId);
        TestFailureType GetTestFailureType(int testFailureTypeId);
        IQueryable<TestCategory> GetTestCategories();
        IQueryable<LatestTestResult> GetLatestTestResults();
        IQueryable<TestFailureDuration> GetTestFailureDurations();
        int SaveSuiteResult(SuiteResult suiteResult);
        void SaveLatestTestResult(LatestTestResult latestTestResult);
        void SaveTestFailedDuration(TestFailureDuration testFailedDuration);
        TestResult SaveTestResult(TestResult testResult);
        void SaveTemporaryTestResult(TemporaryTestResult testResult);
        void DeleteAllTemporaryTestResultsForSuite(int suiteId);

        IQueryable<Suite> GetSuites();
        IQueryable<SuiteCategory> GetSuiteCategories();
        int SaveSuite(Suite suite);
    }
}
