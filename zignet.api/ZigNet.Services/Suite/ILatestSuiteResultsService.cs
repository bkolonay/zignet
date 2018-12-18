using System.Collections.Generic;
using ZigNet.Services.DTOs;

namespace ZigNet.Services
{
    public interface ILatestSuiteResultsService
    {
        IEnumerable<SuiteSummary> GetLatest();
        IEnumerable<SuiteSummary> GetLatestGrouped();
        IEnumerable<SuiteSummary> GetLatest(SuiteResultsFilter suiteResultsFilter);
    }
}
