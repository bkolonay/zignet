using System.Collections.Generic;
using ZigNet.Domain.Suite;
using ZigNet.Services;
using ZigNet.Services.DTOs;

namespace ZigNet.Business
{
    public interface ISuiteBusinessProvider
    {
        int StartSuite(int suiteId);
        int StartSuite(string applicationName, string suiteName, string environmentName);
        void StopSuite(int suiteResultId, SuiteResultType suiteResultType);
        string GetSuiteName(int suiteId, bool group);
        IEnumerable<SuiteSummary> GetLatest(bool group, bool includeDebug);
        IEnumerable<SuiteSummary> GetLatest(SuiteResultsFilter suiteResultsFilter);
    }
}
