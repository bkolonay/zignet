using System.Collections.Generic;
using System.Linq;
using ZigNetSuite = ZigNet.Domain.Suite.Suite;
using ZigNetTest = ZigNet.Domain.Test.Test;

namespace ZigNet.Database.EntityFramework
{
    public interface IZigNetEntitiesWriter
    {
        SuiteResult GetSuiteResult(int suiteResultId);
        Test GetTestWithSuites(int testId);
        IQueryable<TestCategory> GetTestCategories();
        Suite GetSuite(int suiteId);
        TestResult SaveTestResult(TestResult testResult);
        IQueryable<LatestTestResult> GetLatestTestResults();

        bool SuiteResultExists(int suiteResultId);
        ZigNetSuite GetZigNetSuite(int suiteId);
        IQueryable<Suite> GetSuites();
        IEnumerable<Test> GetTestsWithTestResultsForSuite(int suiteId);
        IQueryable<SuiteCategory> GetSuiteCategories();
        IQueryable<SuiteResult> GetSuiteResults();
        IQueryable<TestResult> GetTestResults();
        TestFailureType GetTestFailureType(int testFailureTypeId);
        
        int SaveSuiteResult(SuiteResult suiteResult);
        void SaveLatestTestResult(LatestTestResult latestTestResult);

        int SaveSuite(Suite suite);
    }
}
