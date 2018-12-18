using System;
using System.Collections.Generic;
using System.Linq;
using ZigNet.Api.Model;
using ZigNet.Domain.Suite;
using ZigNet.Services;
using ZigNet.Services.DTOs;

namespace ZigNet.Business
{
    public class SuiteBusinessProvider : ISuiteBusinessProvider
    {
        private ILatestSuiteResultsService _latestSuiteResultsService;
        private ISuiteResultService _suiteResultService;
        private ISuiteService _suiteService;
        private ITemporaryTestResultService _temporaryTestResultsService;        

        public SuiteBusinessProvider(ILatestSuiteResultsService latestSuiteResultsService, ISuiteResultService suiteResultService,
            ISuiteService suiteService, ITemporaryTestResultService temporaryTestResultsService)
        {
            _latestSuiteResultsService = latestSuiteResultsService;
            _suiteResultService = suiteResultService;
            _suiteService = suiteService;
            _temporaryTestResultsService = temporaryTestResultsService;
        }

        public int StartSuite(int suiteId)
        {
            _temporaryTestResultsService.DeleteAll(suiteId);
            return _suiteResultService.SaveSuiteResult(
                new SuiteResult
                {
                    Suite = new Suite { SuiteID = suiteId },
                    StartTime = DateTime.UtcNow,
                    ResultType = SuiteResultType.Inconclusive
                });
        }

        public int StartSuite(string applicationName, string suiteName, string environmentName)
        {
            var suiteId = _suiteService.GetId(applicationName, suiteName, environmentName);
            return StartSuite(suiteId);
        }

        public void StopSuite(int suiteResultId, SuiteResultType suiteResultType)
        {
            var suiteResult = _suiteResultService.Get(suiteResultId);
            suiteResult.EndTime = DateTime.UtcNow;
            suiteResult.ResultType = suiteResultType;
            _suiteResultService.SaveSuiteResult(suiteResult);
        }

        // todo: delete if not used any more
        public string GetSuiteName(int suiteId, bool group)
        {
            var suite = _suiteService.Get(suiteId);
            return group ? suite.GetNameGrouped() : suite.GetName();
        }

        public IEnumerable<SuiteSummary> GetLatest(SuiteResultsFilter suiteResultsFilter)
        {
            if (suiteResultsFilter == null)
                suiteResultsFilter = new SuiteResultsFilter();

            var suiteSummaries = _latestSuiteResultsService.GetLatest(suiteResultsFilter);
            return suiteResultsFilter.Debug ? suiteSummaries : suiteSummaries.Where(s => !s.SuiteName.Contains("(D)"));
        }
    }
}
