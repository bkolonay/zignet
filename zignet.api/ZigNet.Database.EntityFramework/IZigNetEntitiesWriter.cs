using System.Collections.Generic;
using System.Linq;
using ZigNetSuite = ZigNet.Domain.Suite.Suite;
using ZigNetTest = ZigNet.Domain.Test.Test;

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
