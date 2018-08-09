using System.Collections.Generic;
using ZigNet.Services.DTOs;

namespace ZigNet.Business
{
    public interface ILatestSuiteResultsBusinessProvider
    {
        IEnumerable<SuiteSummary> GetLatest(bool group, bool includeDebug);
    }
}
