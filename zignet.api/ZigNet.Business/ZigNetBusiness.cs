using System;
using System.Collections.Generic;
using System.Linq;
using ZigNet.Database;
using ZigNet.Database.DTOs;
using ZigNet.Domain.Suite;
using ZigNet.Domain.Test;

namespace ZigNet.Business
{
    public class ZigNetBusiness : IZigNetBusiness
    {
        private IZigNetDatabase _zignetDatabase;
        private ITemporaryTestResultsService _temporaryTestResultsService;
        private ISuiteResultService _suiteResultService;
        private ISuiteService _suiteService;

        public ZigNetBusiness(IZigNetDatabase zigNetDatabase, ITemporaryTestResultsService temporaryTestResultsService,
            ISuiteResultService suiteResultService, ISuiteService suiteService)
        {
            _zignetDatabase = zigNetDatabase;
            _temporaryTestResultsService = temporaryTestResultsService;
            _suiteResultService = suiteResultService;
            _suiteService = suiteService;
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
            var suiteId = _suiteService.GetSuiteId(applicationName, suiteName, environmentName);
            return StartSuite(suiteId);
        }
        public void StopSuite(int suiteResultId, SuiteResultType suiteResultType)
        {
            _zignetDatabase.StopSuite(suiteResultId, suiteResultType);
        }
        public void SaveTestResult(TestResult testResult)
        {
            if (string.IsNullOrWhiteSpace(testResult.Test.Name))
                throw new ArgumentNullException("TestName", "Test name cannot be null");

            _zignetDatabase.SaveTestResult(testResult);
        }
        public string GetSuiteName(int suiteId, bool groupSuiteNameByApplicationAndEnvironment)
        {
            return _zignetDatabase.GetSuiteName(suiteId, groupSuiteNameByApplicationAndEnvironment);
        }
        public IEnumerable<LatestTestResult> GetLatestTestResults(int suiteId, bool groupResultsByApplicationAndEnvironment)
        {
            return _zignetDatabase.GetLatestTestResults(suiteId, groupResultsByApplicationAndEnvironment);
        }
        public IEnumerable<SuiteSummary> GetLatestSuiteResults(bool groupResultsByApplicationAndEnvironment)
        {
            return _zignetDatabase.GetLatestSuiteResults(groupResultsByApplicationAndEnvironment);
        }

        public int CreateSuite(Suite suite)
        {
            if (_zignetDatabase.GetMappedSuites().Any(s => s.Name == suite.Name))
                throw new InvalidOperationException("Suite with name already exists: " + suite.Name);

            return _zignetDatabase.SaveSuite(suite);
        }
        public void AddSuiteCategory(int suiteId, string suiteCategoryName)
        {
            if (string.IsNullOrWhiteSpace(suiteCategoryName))
                throw new InvalidOperationException("Suite category name cannot be null or empty string");

            var currentSuiteCategories = _zignetDatabase.GetSuiteCategoriesForSuite(suiteId);
            if (currentSuiteCategories.Any(sc => sc.Name == suiteCategoryName))
                return;

            var suite = _zignetDatabase.GetMappedSuites().Single(s => s.SuiteID == suiteId);
            suite.Categories.Add(new SuiteCategory { Name = suiteCategoryName });
            _zignetDatabase.SaveSuite(suite);
        }
        public void DeleteSuiteCategory(int suiteId, string suiteCategoryName)
        {
            if (string.IsNullOrWhiteSpace(suiteCategoryName))
                throw new InvalidOperationException("Suite category name cannot be null or empty string");

            var currentSuiteCategories = _zignetDatabase.GetSuiteCategoriesForSuite(suiteId);
            if (!currentSuiteCategories.Any(sc => sc.Name == suiteCategoryName))
                return;

            var suite = _zignetDatabase.GetMappedSuites().Single(s => s.SuiteID == suiteId);
            suite.Categories.Remove(suite.Categories.Single(sc => sc.Name == suiteCategoryName));
            _zignetDatabase.SaveSuite(suite);
        }
    }
}
