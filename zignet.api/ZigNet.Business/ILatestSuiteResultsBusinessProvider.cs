using System.Collections.Generic;
using ZigNet.Database.DTOs;

namespace ZigNet.Business
{
    public interface ILatestSuiteResultsBusinessProvider
    {
        IEnumerable<SuiteSummary> GetLatest(bool group);
    }
}
