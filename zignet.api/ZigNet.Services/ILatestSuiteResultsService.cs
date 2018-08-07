using System.Collections.Generic;
using ZigNet.Database.DTOs;

namespace ZigNet.Services
{
    public interface ILatestSuiteResultsService
    {
        IEnumerable<SuiteSummary> GetLatest();
        IEnumerable<SuiteSummary> GetLatestGrouped();
    }
}
