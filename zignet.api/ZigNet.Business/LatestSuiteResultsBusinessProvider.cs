using System.Collections.Generic;
using System.Linq;
using ZigNet.Database.DTOs;
using ZigNet.Services;

namespace ZigNet.Business
{
    public class LatestSuiteResultsBusinessProvider : ILatestSuiteResultsBusinessProvider
    {
        private ILatestSuiteResultsService _latestSuiteResultsService;

        public LatestSuiteResultsBusinessProvider(ILatestSuiteResultsService latestSuiteResultsService)
        {
            _latestSuiteResultsService = latestSuiteResultsService;
        }

        public IEnumerable<SuiteSummary> GetLatest(bool group, bool includeDebug)
        {
            var suiteSummaries = group ? _latestSuiteResultsService.GetLatestGrouped() : _latestSuiteResultsService.GetLatest();
            return includeDebug ? suiteSummaries : suiteSummaries.Where(s => !s.SuiteName.Contains("(D)"));
        }
    }
}
