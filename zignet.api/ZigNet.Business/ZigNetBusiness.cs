using System;
using System.Collections.Generic;
using System.Linq;
using ZigNet.Business.Models;
using ZigNet.Database;
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
            var latestSuiteResults = new List<SuiteResult>();
            foreach (var suite in _zignetDatabase.GetSuites())
            {
                var latestSuiteResult = _zignetDatabase.GetSuiteResultsForSuite(suite.SuiteID).OrderByDescending(sr => sr.StartTime).FirstOrDefault();
                if (latestSuiteResult == null)
                    continue;
                latestSuiteResults.Add(latestSuiteResult);
            }

            var suiteSummaries = new List<SuiteSummary>();
            foreach (var suiteResult in latestSuiteResults)
            {
                var tests = _zignetDatabase.GetTestResultsForSuiteResult(suiteResult.SuiteResultID);

                suiteSummaries.Add(
                    new SuiteSummary
                    {
                        SuiteID = suiteResult.Suite.SuiteID,
                        SuiteName = suiteResult.Suite.Name,
                        TotalFailedTests = tests.Where(t => t.ResultType == TestResultType.Fail).Count(),
                        TotalInconclusiveTests = tests.Where(t => t.ResultType == TestResultType.Inconclusive).Count(),
                        TotalPassedTests = tests.Where(t => t.ResultType == TestResultType.Pass).Count(),
                        SuiteEndTime = suiteResult.EndTime
                    }
                );
            }

            return suiteSummaries;
        }

        public int CreateSuite(Suite suite)
        {
            if (_zignetDatabase.GetSuites().Any(s => s.Name == suite.Name))
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

            var suite = _zignetDatabase.GetSuites().Single(s => s.SuiteID == suiteId);
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

            var suite = _zignetDatabase.GetSuites().Single(s => s.SuiteID == suiteId);
            suite.Categories.Remove(suite.Categories.Single(sc => sc.Name == suiteCategoryName));
            _zignetDatabase.SaveSuite(suite);
        }
    }
}
