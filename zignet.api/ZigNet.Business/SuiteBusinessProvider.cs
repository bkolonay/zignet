using System;
using ZigNet.Domain.Suite;
using ZigNet.Services;

namespace ZigNet.Business
{
    public class SuiteBusinessProvider : ISuiteBusinessProvider
    {
        private ITemporaryTestResultsService _temporaryTestResultsService;
        private ISuiteResultService _suiteResultService;
        private ISuiteService _suiteService;

        public SuiteBusinessProvider(ISuiteService suiteService, ITemporaryTestResultsService temporaryTestResultsService,
            ISuiteResultService suiteResultService)
        {
            _suiteService = suiteService;
            _temporaryTestResultsService = temporaryTestResultsService;
            _suiteResultService = suiteResultService;
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

        public string GetSuiteName(int suiteId, bool group)
        {
            if (group)
                return _suiteService.GetNameGrouped(suiteId).GetNameGrouped();
            else
                return _suiteService.GetName(suiteId).GetName();
        }
    }
}
