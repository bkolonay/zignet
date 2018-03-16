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

        public int StartSuite(int suiteId)
        {
            var suiteResult = CreateNewSuiteResultForSuite(
                _zignetDatabase.GetSuites().Single(s => s.SuiteID == suiteId)
            );
            return _zignetDatabase.SaveSuiteResult(suiteResult);
        }

        public int StartSuite(string suiteName)
        {
            var suiteResult = CreateNewSuiteResultForSuite(
                _zignetDatabase.GetSuites().Single(s => s.Name == suiteName)
            );
            return _zignetDatabase.SaveSuiteResult(suiteResult);
        }

        public void EndSuite(int suiteResultId, SuiteResultType suiteResultType)
        {
            var suiteResult = _zignetDatabase.GetSuiteResult(suiteResultId);

            suiteResult.EndTime = DateTime.UtcNow;
            suiteResult.ResultType = suiteResultType;
            _zignetDatabase.SaveSuiteResult(suiteResult);
        }

        public void SaveTestResult(TestResult testResult)
        {
            if (string.IsNullOrWhiteSpace(testResult.Test.Name))
                throw new ArgumentNullException("TestName", "Test name cannot be null");

            var existingTestWithSameName = _zignetDatabase.GetTestOrDefault(testResult.Test.Name);
            if (existingTestWithSameName != null)
            {
                testResult.Test.TestID = existingTestWithSameName.TestID;
                testResult.Test.Categories = testResult.Test.Categories.Concat(existingTestWithSameName.Categories).ToList();
            }

            // this will throw if suite result passed in doesn't exist in the DB (it must exist)
            var testResultSuiteResult = _zignetDatabase.GetSuiteResult(testResult.SuiteResult.SuiteResultID);

            _zignetDatabase.SaveTestResult(testResult);
        }

        private SuiteResult CreateNewSuiteResultForSuite(Suite suite)
        {
            return new SuiteResult
            {
                Suite = suite,
                StartTime = DateTime.UtcNow,
                ResultType = SuiteResultType.Inconclusive
            };
        }

        public IEnumerable<LatestTestResult> GetLatestTestResults(int suiteId)
        {
            var latestTestResults = new List<LatestTestResult>();
            var testsForSuite = _zignetDatabase.GetTestsForSuite(suiteId);

            foreach (var test in testsForSuite)
            {
                var latestTestResult = new LatestTestResult { TestName = test.Name };

                var latestTestResultInSuite = _zignetDatabase.GetLatestTestResultInSuite(test.TestID, suiteId);

                if (latestTestResultInSuite.ResultType == TestResultType.Pass)
                {
                    var allTestResultsInSuiteForTest = _zignetDatabase.GetTestResultsForTestInSuite(test.TestID, suiteId).OrderByDescending(tr => tr.EndTime);
                    var lastFailedTestResult = allTestResultsInSuiteForTest.FirstOrDefault(tr => tr.ResultType == TestResultType.Fail);
                    if (lastFailedTestResult != null)
                    {
                        var testResultsAfterFailure = allTestResultsInSuiteForTest.Where(tr => tr.EndTime < lastFailedTestResult.EndTime);
                        var firstPassBeforeFailure = testResultsAfterFailure.FirstOrDefault(tr => tr.ResultType == TestResultType.Pass);
                        if (firstPassBeforeFailure == null)
                        {
                            latestTestResult.TestResultID = latestTestResultInSuite.TestResultID;
                            latestTestResult.PassingFromDate = latestTestResultInSuite.EndTime;
                        }
                        else
                        {
                            latestTestResult.TestResultID = firstPassBeforeFailure.TestResultID;
                            latestTestResult.PassingFromDate = firstPassBeforeFailure.EndTime;
                        }
                    }
                    else
                    {
                        var firstTimeTestPassed = allTestResultsInSuiteForTest.Last();
                        latestTestResult.TestResultID = firstTimeTestPassed.TestResultID;
                        latestTestResult.PassingFromDate = firstTimeTestPassed.EndTime;
                    }
                }
                else
                {
                    throw new NotImplementedException();
                }
                
                latestTestResults.Add(latestTestResult);
            }

            return latestTestResults;
        }
    }
}
