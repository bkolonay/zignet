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

        public ZigNetBusiness(IZigNetDatabase zigNetDatabase)
        {
            _zignetDatabase = zigNetDatabase;
        }

        public int StartSuite(int suiteId)
        {
            return _zignetDatabase.StartSuite(suiteId);
        }
        public int StartSuite(string suiteName)
        {
            return _zignetDatabase.StartSuite(suiteName);
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
        public IEnumerable<LatestTestResult> GetLatestTestResults(int suiteId)
        {
            var databaseLatestTestResults = _zignetDatabase.GetLatestTestResults(suiteId);

            var latestTestResults = new List<LatestTestResult>();
            foreach (var databaseLatestTestResult in databaseLatestTestResults)
            {
                latestTestResults.Add(new LatestTestResult
                {
                    TestResultID = databaseLatestTestResult.TestResultID,
                    TestName = databaseLatestTestResult.TestName,
                    FailingFromDate = databaseLatestTestResult.FailingFromDate,
                    PassingFromDate = databaseLatestTestResult.PassingFromDate
                });
            }
            return latestTestResults;
        }
        public IEnumerable<SuiteSummary> GetLatestSuiteResults()
        {
            return _zignetDatabase.GetLatestSuiteResults();
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
