using System.Collections.Generic;
using System.Linq;
using ZigNet.Database.DTOs;
using ZigNetTest = ZigNet.Domain.Test.Test;

namespace ZigNet.Database.EntityFramework
{
    public interface IZigNetEntitiesReadOnly
    {
        int GetSuiteId(string suiteName);
        string GetSuiteName(int suiteId);
        ZigNetTest GetMappedTestWithCategoriesOrDefault(string testName);
        SuiteResult GetSuiteResult(int suiteResultId);
        IQueryable<LatestTestResult> GetLatestTestResults();
        IEnumerable<SuiteSummary> GetLatestSuiteResults();
    }
}
