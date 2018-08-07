using System.Collections.Generic;
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

        public IEnumerable<SuiteSummary> GetLatest(bool group)
        {
            if (group)
                return _latestSuiteResultsService.GetLatestGrouped();
            else
                return _latestSuiteResultsService.GetLatest();
        }
    }
}
