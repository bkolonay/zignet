using System;
using System.Collections.Generic;
using System.Linq;
using ZigNetSuite = ZigNet.Domain.Suite.Suite;
using ZigNetSuiteResult = ZigNet.Domain.Suite.SuiteResult;
using ZigNetSuiteCategory = ZigNet.Domain.Suite.SuiteCategory;
using ZigNetTestResult = ZigNet.Domain.Test.TestResult;
using ZigNetSuiteResultType = ZigNet.Domain.Suite.SuiteResultType;
using ZigNetTestResultType = ZigNet.Domain.Test.TestResultType;
using ZigNetTest = ZigNet.Domain.Test.Test;
using ZigNetTestCategory = ZigNet.Domain.Test.TestCategory;
using ZigNetTestFailureType = ZigNet.Domain.Test.TestFailureType;
using LatestTestResultDto = ZigNet.Database.DTOs.LatestTestResult;
using System.Diagnostics;

namespace ZigNet.Database.EntityFramework
{
    public class ZigNetEntityFrameworkDatabase : IZigNetDatabase
    {
        private IZigNetEntitiesWriter _zigNetEntitiesWriter;
        private IZigNetEntitiesReadOnly _zigNetEntitiesReadOnly;

        public ZigNetEntityFrameworkDatabase(IZigNetEntitiesWriter zigNetEntitiesWriter, IZigNetEntitiesReadOnly zigNetEntitiesReadOnly)
        {
            _zigNetEntitiesWriter = zigNetEntitiesWriter;
            _zigNetEntitiesReadOnly = zigNetEntitiesReadOnly;
        }

        public ZigNetSuite GetSuite(int suiteId)
        {
            return _zigNetEntitiesWriter.GetZigNetSuite(suiteId);
        }

        public bool SuiteResultExists(int suiteResultId)
        {
            return _zigNetEntitiesWriter.SuiteResultExists(suiteResultId);
        }

        public IEnumerable<ZigNetSuite> GetSuites()
        {
            var databaseSuites = _zigNetEntitiesWriter.GetSuites();

            var suites = new List<ZigNetSuite>();
            foreach (var databaseSuite in databaseSuites)
                suites.Add(MapDatabaseSuite(databaseSuite));

            return suites;
        }

        public IEnumerable<ZigNetSuiteCategory> GetSuiteCategoriesForSuite(int suiteId)
        {
            return MapDatabaseSuiteCategories(_zigNetEntitiesWriter.GetSuite(suiteId).SuiteCategories);
        }

        public IEnumerable<ZigNetSuiteResult> GetSuiteResultsForSuite(int suiteId)
        {
            var databaseSuiteResults = _zigNetEntitiesWriter.GetSuiteResults().Where(sr => sr.SuiteId == suiteId);

            var suiteResultsForSuite = new List<ZigNetSuiteResult>();
            foreach (var databaseSuiteResult in databaseSuiteResults)
                suiteResultsForSuite.Add(MapDatabaseSuiteResult(databaseSuiteResult));

            return suiteResultsForSuite;
        }

        public ZigNetSuiteResult GetSuiteResult(int suiteResultId)
        {
            var databaseSuiteResult = _zigNetEntitiesWriter.GetSuiteResult(suiteResultId);
            return MapDatabaseSuiteResult(databaseSuiteResult);
        }

        public IEnumerable<ZigNetTestResult> GetTestResultsForSuiteResult(int suiteResultId)
        {
            var databaseTestResults = _zigNetEntitiesWriter.GetTestResults().Where(tr => tr.SuiteResultId == suiteResultId);

            var testResultsForSuiteResult = new List<ZigNetTestResult>();
            foreach (var databaseTestResult in databaseTestResults)
            {
                testResultsForSuiteResult.Add(
                    new ZigNetTestResult
                    {
                        TestResultID = databaseTestResult.TestResultID,
                        ResultType = MapDatabaseTestResultType(databaseTestResult.TestResultType)
                    }
                );
            }

            return testResultsForSuiteResult;
        }

        public IEnumerable<ZigNetTestResult> GetTestResultsForSuite(int suiteId)
        {
            var databaseTestResults = _zigNetEntitiesWriter.GetTestResults().Where(tr => tr.SuiteResult.SuiteId == suiteId);
            var testResults = new List<ZigNetTestResult>();
            foreach (var databaseTestResult in databaseTestResults)
                testResults.Add(MapDatabaseTestResult(databaseTestResult));

            return testResults;
        }

        public IEnumerable<LatestTestResultDto> GetLatestTestResults(int suiteId)
        {
            var latestTestResults = _zigNetEntitiesWriter.GetLatestTestResults().Where(ltr => ltr.SuiteId == suiteId);

            var latestTestResultDtos = new List<LatestTestResultDto>();
            foreach (var latestTestResult in latestTestResults)
                latestTestResultDtos.Add(new LatestTestResultDto
                {
                    TestResultID = latestTestResult.TestResultId,
                    TestName = latestTestResult.TestName,
                    FailingFromDate = latestTestResult.FailingFromDateTime,
                    PassingFromDate = latestTestResult.PassingFromDateTime
                });
            var passingLatestTestResultDtos = latestTestResultDtos.Where(ltr => ltr.PassingFromDate != null).OrderByDescending(ltr => ltr.PassingFromDate);
            var failingLatestTestResultDtos = latestTestResultDtos.Where(ltr => ltr.FailingFromDate != null).OrderBy(ltr => ltr.FailingFromDate).ToList();
            failingLatestTestResultDtos.AddRange(passingLatestTestResultDtos);

            return failingLatestTestResultDtos;
        }

        public IEnumerable<ZigNetTest> GetTestsForSuite(int suiteId)
        {
            var suite = _zigNetEntitiesWriter.GetSuite(suiteId);
            var databaseTests = suite.Tests;

            var testsForSuite = new List<ZigNetTest>();
            foreach (var databaseTest in databaseTests)
                testsForSuite.Add(MapDatabaseTestShallow(databaseTest));

            return testsForSuite;
        }

        public ZigNetTest GetTestOrDefault(string testName)
        {
            return _zigNetEntitiesWriter.GetTestOrDefault(testName);
        }

        public int SaveSuite(ZigNetSuite suite)
        {
            Suite databaseSuite;
            if (suite.SuiteID == 0)
                databaseSuite = new Suite { SuiteName = suite.Name, SuiteCategories = new List<SuiteCategory>() };
            else
                databaseSuite = _zigNetEntitiesWriter.GetSuite(suite.SuiteID);

            databaseSuite.SuiteCategories.Clear();
            foreach (var suiteCategory in suite.Categories)
            {
                var existingDatabaseSuiteCategory = _zigNetEntitiesWriter.GetSuiteCategories().SingleOrDefault(sc => sc.CategoryName == suiteCategory.Name);
                if (existingDatabaseSuiteCategory != null)
                    databaseSuite.SuiteCategories.Add(existingDatabaseSuiteCategory);
                else
                    databaseSuite.SuiteCategories.Add(new SuiteCategory { CategoryName = suiteCategory.Name });
            }

            return _zigNetEntitiesWriter.SaveSuite(databaseSuite);
        }

        public int SaveSuiteResult(ZigNetSuiteResult suiteResult)
        {
            SuiteResult databaseSuiteResult;
            if (suiteResult.SuiteResultID == 0)
                databaseSuiteResult = new SuiteResult();
            else
                databaseSuiteResult = _zigNetEntitiesWriter.GetSuiteResult(suiteResult.SuiteResultID);

            databaseSuiteResult.SuiteId = suiteResult.Suite.SuiteID;
            databaseSuiteResult.SuiteResultStartDateTime = suiteResult.StartTime;
            databaseSuiteResult.SuiteResultTypeId = MapSuiteResultType(suiteResult.ResultType);
            databaseSuiteResult.SuiteResultEndDateTime = suiteResult.EndTime;

            return _zigNetEntitiesWriter.SaveSuiteResult(databaseSuiteResult);
        }

        public void SaveTestResult(ZigNetTestResult testResult)
        {
            var databaseTestResult = new TestResult
            {
                SuiteResultId = testResult.SuiteResult.SuiteResultID,
                TestResultStartDateTime = testResult.StartTime,
                TestResultEndDateTime = testResult.EndTime,
                TestResultTypeId = MapTestResultType(testResult.ResultType)
            };

            if (testResult.ResultType == ZigNetTestResultType.Fail)
            {
                databaseTestResult.TestFailureTypes.Add(GetTestFailureType(testResult.TestFailureDetails.FailureType));
                if (!string.IsNullOrWhiteSpace(testResult.TestFailureDetails.FailureDetailMessage))
                    databaseTestResult.TestFailureDetails.Add(new TestFailureDetail { TestFailureDetail1 = testResult.TestFailureDetails.FailureDetailMessage });
            }

            if (testResult.Test.TestID != 0)
                databaseTestResult.Test = _zigNetEntitiesWriter.GetTest(testResult.Test.TestID);
            else
                databaseTestResult.Test = new Test { TestName = testResult.Test.Name, TestCategories = new List<TestCategory>() };

            databaseTestResult.Test.TestCategories.Clear();
            var existingDatabaseTestCategories = _zigNetEntitiesWriter.GetTestCategories().OrderBy(tc => tc.TestCategoryID).ToList();
            foreach (var testCategory in testResult.Test.Categories)
            {
                // use FirstOrDefault instead of SingleOrDefault because first-run multi-threaded tests end up inserting duplicate categories
                // (before the check for duplicates happens)
                var existingDatabaseTestCategory = existingDatabaseTestCategories
                    .FirstOrDefault(tc => tc.CategoryName == testCategory.Name);
                if (existingDatabaseTestCategory != null)
                    databaseTestResult.Test.TestCategories.Add(existingDatabaseTestCategory);
                else
                    databaseTestResult.Test.TestCategories.Add(new TestCategory { CategoryName = testCategory.Name });
            }

            var suiteResult = _zigNetEntitiesWriter.GetSuiteResultWithoutTracking(testResult.SuiteResult.SuiteResultID);
            if (!databaseTestResult.Test.Suites.Any(s => s.SuiteID == suiteResult.SuiteId))
            {
                var suite = _zigNetEntitiesWriter.GetSuite(suiteResult.SuiteId);
                databaseTestResult.Test.Suites.Add(suite);
            }

            var savedTestResult = _zigNetEntitiesWriter.SaveTestResult(databaseTestResult);

            var databaseLatestTestResult = _zigNetEntitiesWriter.GetLatestTestResults()
                .SingleOrDefault(ltr =>
                    ltr.SuiteId == suiteResult.SuiteId &&
                    ltr.TestId == databaseTestResult.Test.TestID
                );
            if (databaseLatestTestResult == null)
                databaseLatestTestResult = new LatestTestResult
                {
                    SuiteId = suiteResult.SuiteId,
                    TestId = savedTestResult.Test.TestID,
                    TestName = testResult.Test.Name
                };
            var utcNow = DateTime.UtcNow;
            if (testResult.ResultType == ZigNetTestResultType.Pass && databaseLatestTestResult.PassingFromDateTime == null)
            {
                databaseLatestTestResult.TestResultId = savedTestResult.TestResultID;
                databaseLatestTestResult.PassingFromDateTime = utcNow;
                databaseLatestTestResult.FailingFromDateTime = null;
                _zigNetEntitiesWriter.SaveLatestTestResult(databaseLatestTestResult);
            }
            else if ((testResult.ResultType == ZigNetTestResultType.Fail || testResult.ResultType == ZigNetTestResultType.Inconclusive) 
                      && databaseLatestTestResult.FailingFromDateTime == null)
            {
                databaseLatestTestResult.TestResultId = savedTestResult.TestResultID;
                databaseLatestTestResult.FailingFromDateTime = utcNow;
                databaseLatestTestResult.PassingFromDateTime = null;
                _zigNetEntitiesWriter.SaveLatestTestResult(databaseLatestTestResult);
            }
        }

        private TestFailureType GetTestFailureType(ZigNetTestFailureType zigNetTestFailureType)
        {
            switch (zigNetTestFailureType)
            {
                case ZigNetTestFailureType.Exception:
                    return _zigNetEntitiesWriter.GetTestFailureType(2);
                case ZigNetTestFailureType.Assertion:
                    return _zigNetEntitiesWriter.GetTestFailureType(1);
                default:
                    throw new InvalidOperationException("Test failure type not recognized");
            }
        }

        private ZigNetSuiteResult MapDatabaseSuiteResult(SuiteResult databaseSuiteResult)
        {
            return new ZigNetSuiteResult
            {
                SuiteResultID = databaseSuiteResult.SuiteResultID,
                StartTime = databaseSuiteResult.SuiteResultStartDateTime,
                EndTime = databaseSuiteResult.SuiteResultEndDateTime,
                ResultType = MapDatabaseSuiteResultType(databaseSuiteResult.SuiteResultType),
                Suite = MapDatabaseSuite(databaseSuiteResult.Suite)
            };
        }

        private int MapSuiteResultType(ZigNetSuiteResultType zigNetSuiteResultType)
        {
            switch (zigNetSuiteResultType)
            {
                case ZigNetSuiteResultType.Fail:
                    return 1;
                case ZigNetSuiteResultType.Inconclusive:
                    return 2;
                case ZigNetSuiteResultType.Pass:
                    return 3;
                default:
                    throw new InvalidOperationException("Suite result type not recognized");
            }
        }

        private int MapTestResultType(ZigNetTestResultType zigNetTestResultType)
        {
            switch (zigNetTestResultType)
            {
                case ZigNetTestResultType.Fail:
                    return 1;
                case ZigNetTestResultType.Inconclusive:
                    return 2;
                case ZigNetTestResultType.Pass:
                    return 3;
                default:
                    throw new InvalidOperationException("Test result type not recognized");
            }
        }

        private ICollection<ZigNetSuiteCategory> MapDatabaseSuiteCategories(Suite suite)
        {
            var databaseSuiteCategories = suite.SuiteCategories;

            var suiteCategories = new List<ZigNetSuiteCategory>();
            foreach (var databaseSuiteCategory in databaseSuiteCategories)
                suiteCategories.Add(MapDatabaseSuiteCategory(databaseSuiteCategory));

            return suiteCategories;
        }

        private ICollection<ZigNetTestCategory> MapDatabaseTestCategories(Test test)
        {
            var databaseTestCategories = test.TestCategories;

            var testCategories = new List<ZigNetTestCategory>();
            foreach (var databaseTestCategory in databaseTestCategories)
                testCategories.Add(MapDatabaseTestCategory(databaseTestCategory));

            return testCategories;
        }

        private ZigNetSuiteResultType MapDatabaseSuiteResultType(SuiteResultType suiteResultType)
        {
            switch (suiteResultType.SuiteResultTypeName)
            {
                case "Pass":
                    return ZigNetSuiteResultType.Pass;
                case "Fail":
                    return ZigNetSuiteResultType.Fail;
                case "Inconclusive":
                    return ZigNetSuiteResultType.Inconclusive;
                default:
                    throw new InvalidOperationException("Suite result type not recognized");
            }
        }

        private ZigNetTestResultType MapDatabaseTestResultType(TestResultType testResultType)
        {
            switch (testResultType.TestResultTypeName)
            {
                case "Pass":
                    return ZigNetTestResultType.Pass;
                case "Fail":
                    return ZigNetTestResultType.Fail;
                case "Inconclusive":
                    return ZigNetTestResultType.Inconclusive;
                default:
                    throw new InvalidOperationException("Test result type not recognized");
            }
        }

        private ZigNetSuite MapDatabaseSuite(Suite databaseSuite)
        {
            return new ZigNetSuite
            {
                SuiteID = databaseSuite.SuiteID,
                Name = databaseSuite.SuiteName,
                Categories = MapDatabaseSuiteCategories(databaseSuite)
            };
        }

        private ZigNetSuiteCategory MapDatabaseSuiteCategory(SuiteCategory databaseSuiteCategory)
        {
            return new ZigNetSuiteCategory
            {
                SuiteCategoryID = databaseSuiteCategory.SuiteCategoryID,
                Name = databaseSuiteCategory.CategoryName
            };
        }

        private ZigNetTestCategory MapDatabaseTestCategory(TestCategory databaseTestCategory)
        {
            return new ZigNetTestCategory
            {
                TestCategoryID = databaseTestCategory.TestCategoryID,
                Name = databaseTestCategory.CategoryName
            };
        }

        private IEnumerable<ZigNetSuiteCategory> MapDatabaseSuiteCategories(IEnumerable<SuiteCategory> databaseSuiteCategories)
        {
            var suiteCategories = new List<ZigNetSuiteCategory>();
            foreach (var databaseSuiteCategory in databaseSuiteCategories)
                suiteCategories.Add(MapDatabaseSuiteCategory(databaseSuiteCategory));
            return suiteCategories;
        }

        private SuiteCategory MapSuiteCategory(ZigNetSuiteCategory suiteCategory)
        {
            return new SuiteCategory
            {
                SuiteCategoryID = suiteCategory.SuiteCategoryID,
                CategoryName = suiteCategory.Name
            };
        }

        private ZigNetTest MapDatabaseTest(Test databaseTest)
        {
            var zigNetTest = MapDatabaseTestShallow(databaseTest);
            zigNetTest.Categories = MapDatabaseTestCategories(databaseTest);
            return zigNetTest;
        }

        private ZigNetTest MapDatabaseTestShallow(Test databaseTest)
        {
            return new ZigNetTest
            {
                TestID = databaseTest.TestID,
                Name = databaseTest.TestName
            };
        }

        private ZigNetTestResult MapDatabaseTestResult(TestResult testResult)
        {
            return new ZigNetTestResult
            {
                TestResultID = testResult.TestResultID,
                StartTime = testResult.TestResultStartDateTime,
                EndTime = testResult.TestResultEndDateTime,
                ResultType = MapDatabaseTestResultType(testResult.TestResultType),
                Test = MapDatabaseTestShallow(testResult.Test)
            };
        }
    }
}
