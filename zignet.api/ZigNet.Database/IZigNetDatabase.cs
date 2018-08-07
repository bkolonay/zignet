using System.Collections.Generic;
using ZigNet.Database.DTOs;
using ZigNet.Domain.Suite;
using ZigNet.Domain.Test;

namespace ZigNet.Database
{
    public interface IZigNetDatabase
    {
        IEnumerable<LatestTestResult> GetLatestTestResults(int suiteId, bool groupResultsByApplicationAndEnvironment);
        void SaveTestResult(TestResult testResult);

        IEnumerable<Suite> GetMappedSuites();
        IEnumerable<SuiteCategory> GetSuiteCategoriesForSuite(int suiteId);
        int SaveSuite(Suite suite);
    }
}
