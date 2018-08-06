using System;
using System.Collections.Generic;
using System.Linq;
using ZigNetSuite = ZigNet.Domain.Suite.Suite;
using ZigNetSuiteCategory = ZigNet.Domain.Suite.SuiteCategory;
using ZigNetTestResult = ZigNet.Domain.Test.TestResult;
using ZigNetSuiteResultType = ZigNet.Domain.Suite.SuiteResultType;
using ZigNetTestResultType = ZigNet.Domain.Test.TestResultType;
using ZigNetTestFailureType = ZigNet.Domain.Test.TestFailureType;
using LatestTestResultDto = ZigNet.Database.DTOs.LatestTestResult;
using TestFailureDurationDto = ZigNet.Database.DTOs.TestFailureDuration;
using ZigNet.Database.DTOs;

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

        public void StopSuite(int suiteResultId, ZigNetSuiteResultType suiteResultType)
        {
            var databaseSuiteResult = _zigNetEntitiesWriter.GetSuiteResult(suiteResultId);
            databaseSuiteResult.SuiteResultEndDateTime = DateTime.UtcNow;
            databaseSuiteResult.SuiteResultTypeId = MapSuiteResultType(suiteResultType);
            _zigNetEntitiesWriter.SaveSuiteResult(databaseSuiteResult);
        }
        public string GetSuiteName(int suiteId, bool groupSuiteNameByApplicationAndEnvironment)
        {
            if (groupSuiteNameByApplicationAndEnvironment)
                return _zigNetEntitiesReadOnly.GetSuiteNameGroupedByApplicationAndEnvironment(suiteId);
            else
                return _zigNetEntitiesReadOnly.GetSuiteName(suiteId);
        }
        public IEnumerable<LatestTestResultDto> GetLatestTestResults(int suiteId, bool groupResultsByApplicationAndEnvironment)
        {
            var latestTestResults = new List<LatestTestResult>();

            if (groupResultsByApplicationAndEnvironment)
            {
                var suite = _zigNetEntitiesReadOnly.GetSuite(suiteId);
                var suites = _zigNetEntitiesReadOnly.GetSuites()
                    .Where(s => s.EnvironmentId == suite.EnvironmentId && s.ApplicationId == suite.ApplicationId);
                foreach (var localSuite in suites)
                    latestTestResults.AddRange(_zigNetEntitiesReadOnly.GetLatestTestResults().Where(ltr => ltr.SuiteId == localSuite.SuiteID));
            }
            else
                latestTestResults = _zigNetEntitiesReadOnly.GetLatestTestResults().Where(ltr => ltr.SuiteId == suiteId).ToList();

            var allDatabaseTestFailureDurations = _zigNetEntitiesReadOnly.GetTestFailureDurations().ToList();

            var latestTestResultDtos = new List<LatestTestResultDto>();
            var utcNow = DateTime.UtcNow;
            foreach (var latestTestResult in latestTestResults)
            {
                var testFailureDurationLimit = utcNow.AddHours(-24);
                var databaseTestFailureDurationsForTestResult = allDatabaseTestFailureDurations.Where(tfd =>
                    (tfd.SuiteId == latestTestResult.SuiteId && tfd.TestId == latestTestResult.TestId) &&
                    (tfd.FailureEndDateTime > testFailureDurationLimit || tfd.FailureEndDateTime == null)
                );

                var testFailureDurations = new List<TestFailureDurationDto>();
                foreach (var databaseTestFailureDuration in databaseTestFailureDurationsForTestResult)
                    testFailureDurations.Add(new TestFailureDurationDto
                    {
                        FailureStart = databaseTestFailureDuration.FailureStartDateTime,
                        FailureEnd = databaseTestFailureDuration.FailureEndDateTime
                    });

                latestTestResultDtos.Add(new LatestTestResultDto
                {
                    TestResultID = latestTestResult.TestResultId,
                    TestName = latestTestResult.TestName,
                    SuiteName = latestTestResult.SuiteName,
                    FailingFromDate = latestTestResult.FailingFromDateTime,
                    PassingFromDate = latestTestResult.PassingFromDateTime,
                    TestFailureDurations = testFailureDurations
                });
            }
            var passingLatestTestResultDtos = latestTestResultDtos.Where(ltr => ltr.PassingFromDate != null).OrderByDescending(ltr => ltr.PassingFromDate);
            var failingLatestTestResultDtos = latestTestResultDtos.Where(ltr => ltr.FailingFromDate != null).OrderBy(ltr => ltr.FailingFromDate).ToList();
            failingLatestTestResultDtos.AddRange(passingLatestTestResultDtos);

            return failingLatestTestResultDtos;
        }
        public IEnumerable<SuiteSummary> GetLatestSuiteResults(bool groupResultsByApplicationAndEnvironment)
        {
            if (groupResultsByApplicationAndEnvironment)
                return _zigNetEntitiesReadOnly.GetLatestSuiteResultsGroupedByApplicationAndEnvironment();
            else
                return _zigNetEntitiesReadOnly.GetLatestSuiteResults();

        }
        public void SaveTestResult(ZigNetTestResult testResult)
        {
            var existingTestWithSameName = _zigNetEntitiesReadOnly.GetMappedTestWithCategoriesOrDefault(testResult.Test.Name);
            if (existingTestWithSameName != null)
            {
                testResult.Test.TestID = existingTestWithSameName.TestID;
                testResult.Test.Categories = testResult.Test.Categories.Concat(existingTestWithSameName.Categories).ToList();
            }

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
                databaseTestResult.Test = _zigNetEntitiesWriter.GetTestWithSuites(testResult.Test.TestID);
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

            var suiteResult = _zigNetEntitiesReadOnly.GetSuiteResult(testResult.SuiteResult.SuiteResultID);
            if (!databaseTestResult.Test.Suites.Any(s => s.SuiteID == suiteResult.SuiteId))
            {
                var localSuite = _zigNetEntitiesWriter.GetSuite(suiteResult.SuiteId);
                databaseTestResult.Test.Suites.Add(localSuite);
            }

            var savedTestResult = _zigNetEntitiesWriter.SaveTestResult(databaseTestResult);
            _zigNetEntitiesWriter.SaveTemporaryTestResult(new TemporaryTestResult
            {
                TestResultId = savedTestResult.TestResultID,
                SuiteResultId = testResult.SuiteResult.SuiteResultID,
                SuiteId = suiteResult.SuiteId,
                TestResultTypeId = databaseTestResult.TestResultTypeId
            });

            var databaseLatestTestResult = _zigNetEntitiesWriter.GetLatestTestResults()
                .SingleOrDefault(ltr =>
                    ltr.SuiteId == suiteResult.SuiteId &&
                    ltr.TestId == databaseTestResult.Test.TestID
                );
            var suite = _zigNetEntitiesReadOnly.GetSuite(suiteResult.SuiteId);

            var suiteNameChanged = false;
            if (databaseLatestTestResult == null)
                databaseLatestTestResult = new LatestTestResult
                {
                    SuiteId = suiteResult.SuiteId,
                    TestId = savedTestResult.Test.TestID,
                    TestName = testResult.Test.Name,
                    SuiteName = suite.SuiteName
                };
            else if (databaseLatestTestResult.SuiteName != suite.SuiteName)
            {
                databaseLatestTestResult.SuiteName = suite.SuiteName;
                suiteNameChanged = true;
            }

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
            else if (suiteNameChanged)
                _zigNetEntitiesWriter.SaveLatestTestResult(databaseLatestTestResult);

            var latestDatabaseTestFailedDuration = _zigNetEntitiesWriter.GetTestFailureDurations()
                .OrderByDescending(tfd => tfd.FailureStartDateTime)
                .FirstOrDefault(tfd =>
                    tfd.SuiteId == suiteResult.SuiteId &&
                    tfd.TestId == databaseTestResult.Test.TestID
                );
            if (testResult.ResultType == ZigNetTestResultType.Pass
                && latestDatabaseTestFailedDuration != null
                && latestDatabaseTestFailedDuration.FailureStartDateTime != null && latestDatabaseTestFailedDuration.FailureEndDateTime == null)
            {
                latestDatabaseTestFailedDuration.FailureEndDateTime = utcNow;
                _zigNetEntitiesWriter.SaveTestFailedDuration(latestDatabaseTestFailedDuration);
            }
            else if (testResult.ResultType == ZigNetTestResultType.Fail || testResult.ResultType == ZigNetTestResultType.Inconclusive)
            {
                if (latestDatabaseTestFailedDuration == null || latestDatabaseTestFailedDuration.FailureEndDateTime != null)
                {
                    var newTestFailedDuration = new TestFailureDuration
                    {
                        SuiteId = suiteResult.SuiteId,
                        TestId = savedTestResult.Test.TestID,
                        TestResultId = savedTestResult.TestResultID,
                        FailureStartDateTime = utcNow
                    };
                    _zigNetEntitiesWriter.SaveTestFailedDuration(newTestFailedDuration);
                }
            }
        }

        public IEnumerable<ZigNetSuite> GetMappedSuites()
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
        private IEnumerable<ZigNetSuiteCategory> MapDatabaseSuiteCategories(IEnumerable<SuiteCategory> databaseSuiteCategories)
        {
            var suiteCategories = new List<ZigNetSuiteCategory>();
            foreach (var databaseSuiteCategory in databaseSuiteCategories)
                suiteCategories.Add(MapDatabaseSuiteCategory(databaseSuiteCategory));
            return suiteCategories;
        }
    }
}
