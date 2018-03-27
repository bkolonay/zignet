using System.Collections.Generic;
using System.Linq;
using ZigNetSuite = ZigNet.Domain.Suite.Suite;
using ZigNetTest = ZigNet.Domain.Test.Test;

namespace ZigNet.Database.EntityFramework
{
    public interface IZigNetEntitiesReadOnly
    {
        int GetSuiteId(string suiteName);
        ZigNetTest GetMappedTestWithCategoriesOrDefault(string testName);
        SuiteResult GetSuiteResult(int suiteResultId);
        IQueryable<LatestTestResult> GetLatestTestResults();

        bool SuiteResultExists(int suiteResultId);
        ZigNetSuite GetZigNetSuite(int suiteId);
        IQueryable<Suite> GetSuites();
        IEnumerable<Test> GetTestsWithTestResultsForSuite(int suiteId);
        IQueryable<SuiteCategory> GetSuiteCategories();
        IQueryable<TestCategory> GetTestCategories();
        IQueryable<SuiteResult> GetSuiteResults();
        IQueryable<TestResult> GetTestResults();
        TestFailureType GetTestFailureType(int testFailureTypeId);
    }
}
