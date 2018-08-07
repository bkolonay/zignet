using System.Collections.Generic;
using System.Linq;
using ZigNet.Database.DTOs;
using ZigNetTest = ZigNet.Domain.Test.Test;

namespace ZigNet.Database.EntityFramework
{
    public interface IZigNetEntitiesReadOnly
    {
        IEnumerable<SuiteSummary> GetLatestSuiteResults();
        IEnumerable<SuiteSummary> GetLatestSuiteResultsGroupedByApplicationAndEnvironment();
        string GetSuiteName(int suiteId);
        string GetSuiteNameGroupedByApplicationAndEnvironment(int suiteId);
        Suite GetSuite(int suiteId);
        SuiteResult GetSuiteResult(int suiteResultId);
        ZigNetTest GetMappedTestWithCategoriesOrDefault(string testName);
        IQueryable<Suite> GetSuites();
        IQueryable<LatestTestResult> GetLatestTestResults();
        IQueryable<TestFailureDuration> GetTestFailureDurations();
    }
}
