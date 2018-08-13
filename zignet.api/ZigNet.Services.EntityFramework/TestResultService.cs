using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using ZigNet.Database.EntityFramework;
using DbLatestTestResult = ZigNet.Database.EntityFramework.LatestTestResult;
using DtoLatestTestResult = ZigNet.Services.DTOs.LatestTestResultDto;
using DomainTestResult = ZigNet.Domain.Test.TestResult;
using DomainTestResultType = ZigNet.Domain.Test.TestResultType;
using DomainTest = ZigNet.Domain.Test.Test;
using DomainTestCategory = ZigNet.Domain.Test.TestCategory;
using DomainTestFailureType = ZigNet.Domain.Test.TestFailureType;
using LatestTestResultDto = ZigNet.Services.DTOs.LatestTestResultDto;
using ZigNet.Services.DTOs;

namespace ZigNet.Services.EntityFramework
{
    public class TestResultService : ITestResultService
    {
        private ZigNetEntities _zigNetEntities;
        private ISuiteService _suiteService;
        private ILatestTestResultsService _latestTestResultsService;
        private ITestFailureDurationService _testFailureDurationService;

        public TestResultService(IZigNetEntitiesWrapper zigNetEntitiesWrapper, ISuiteService suiteService,
            ILatestTestResultsService latestTestResultsService, ITestFailureDurationService testFailureDurationService)
        {
            _zigNetEntities = zigNetEntitiesWrapper.Get();
            _suiteService = suiteService;
            _latestTestResultsService = latestTestResultsService;
            _testFailureDurationService = testFailureDurationService;
        }

        public IEnumerable<DtoLatestTestResult> GetLatestResults(int suiteId, bool group)
        {
            // todo: consider using private method for common code (or pull it into it's own service)
            //   if this works, could put boolean logic in business layer instead of here (e.g. 2 functions)

            var latestTestResults = new List<LatestTestResultDto>();

            if (group)
            {
                var suite = _suiteService.Get(suiteId);
                var suiteIds = _suiteService.GetAll()
                    .Where(s => s.EnvironmentId == suite.EnvironmentId && s.ApplicationId == suite.ApplicationId)
                    .Select(s => s.SuiteID)
                    .ToArray();
                latestTestResults = _latestTestResultsService.Get(suiteIds).ToList();
            }
            else
                latestTestResults = _latestTestResultsService.Get(suiteId).ToList();

            var testFailureDurations = _testFailureDurationService.GetAll().ToList();
            var utcNow = DateTime.UtcNow;
            for (var i = 0; i < latestTestResults.Count; i++)
            {
                var testFailureDurationLimit = utcNow.AddHours(-24);
                latestTestResults[i].TestFailureDurations = testFailureDurations
                    .Where(t =>
                            (t.SuiteId == latestTestResults[i].SuiteId && t.TestId == latestTestResults[i].TestId) &&
                            (t.FailureEnd > testFailureDurationLimit || t.FailureEnd == null))
                    .ToList();
            }

            var passingDtoLatestTestResults = latestTestResults.Where(t => t.PassingFromDate != null).OrderByDescending(t => t.PassingFromDate);
            var failingDtoLatestTestResults = latestTestResults.Where(t => t.FailingFromDate != null).OrderBy(t => t.FailingFromDate).ToList();
            failingDtoLatestTestResults.AddRange(passingDtoLatestTestResults);

            return failingDtoLatestTestResults;
        }

        public void SaveTestResult(DomainTestResult testResult)
        {
            var existingTest = GetMappedTestWithCategoriesOrDefault(testResult.Test.Name);
            if (existingTest != null)
            {
                testResult.Test.TestID = existingTest.TestID;
                testResult.Test.Categories = testResult.Test.Categories.Concat(existingTest.Categories).ToList();
            }

            var dbTestResult = new TestResult
            {
                SuiteResultId = testResult.SuiteResult.SuiteResultID,
                TestResultStartDateTime = testResult.StartTime,
                TestResultEndDateTime = testResult.EndTime,
                TestResultTypeId = MapTestResultType(testResult.ResultType)
            };

            if (testResult.ResultType == DomainTestResultType.Fail)
            {
                dbTestResult.TestFailureTypes.Add(GetTestFailureType(testResult.TestFailureDetails.FailureType));
                if (!string.IsNullOrWhiteSpace(testResult.TestFailureDetails.FailureDetailMessage))
                    dbTestResult.TestFailureDetails.Add(new TestFailureDetail { TestFailureDetail1 = testResult.TestFailureDetails.FailureDetailMessage });
            }

            if (testResult.Test.TestID != 0)
                dbTestResult.Test = _zigNetEntities.Tests
                    .Include(t => t.Suites)
                    .Single(t => t.TestID == testResult.Test.TestID);
            else
                dbTestResult.Test = new Test { TestName = testResult.Test.Name, TestCategories = new List<TestCategory>() };

            dbTestResult.Test.TestCategories.Clear();
            var dbTestCategories = _zigNetEntities.TestCategories.OrderBy(c => c.TestCategoryID).ToList();
            foreach (var testCategory in testResult.Test.Categories)
            {
                // use FirstOrDefault instead of SingleOrDefault because first-run multi-threaded tests end up inserting duplicate categories
                // (before the check for duplicates happens)
                var existingDbTestCategory = dbTestCategories
                    .FirstOrDefault(c => c.CategoryName == testCategory.Name);
                if (existingDbTestCategory != null)
                    dbTestResult.Test.TestCategories.Add(existingDbTestCategory);
                else
                    dbTestResult.Test.TestCategories.Add(new TestCategory { CategoryName = testCategory.Name });
            }

            var suiteResult = _zigNetEntities.SuiteResults
                .AsNoTracking()
                .Single(sr => sr.SuiteResultID == testResult.SuiteResult.SuiteResultID);
            if (!dbTestResult.Test.Suites.Any(s => s.SuiteID == suiteResult.SuiteId))
            {
                var localSuite = _zigNetEntities.Suites.Single(s => s.SuiteID == suiteResult.SuiteId);                
                dbTestResult.Test.Suites.Add(localSuite);
            }

            _zigNetEntities.TestResults.Add(dbTestResult);
            _zigNetEntities.SaveChanges();
            
            _zigNetEntities.TemporaryTestResults.Add(new TemporaryTestResult
            {
                TestResultId = dbTestResult.TestResultID,
                SuiteResultId = testResult.SuiteResult.SuiteResultID,
                SuiteId = suiteResult.SuiteId,
                TestResultTypeId = dbTestResult.TestResultTypeId
            });
            _zigNetEntities.SaveChanges();

            var dbLatestTestResult = _zigNetEntities.LatestTestResults
                .SingleOrDefault(t =>
                    t.SuiteId == suiteResult.SuiteId &&
                    t.TestId == dbTestResult.Test.TestID
                );
            var suite = _zigNetEntities.Suites
                .AsNoTracking()
                .Single(s => s.SuiteID == suiteResult.SuiteId);

            var suiteNameChanged = false;
            if (dbLatestTestResult == null)
                dbLatestTestResult = new DbLatestTestResult
                {
                    SuiteId = suiteResult.SuiteId,
                    TestId = dbTestResult.Test.TestID,
                    TestName = testResult.Test.Name,
                    SuiteName = suite.SuiteName
                };
            else if (dbLatestTestResult.SuiteName != suite.SuiteName)
            {
                dbLatestTestResult.SuiteName = suite.SuiteName;
                suiteNameChanged = true;
            }

            var utcNow = DateTime.UtcNow;
            if (testResult.ResultType == DomainTestResultType.Pass && dbLatestTestResult.PassingFromDateTime == null)
            {
                dbLatestTestResult.TestResultId = dbTestResult.TestResultID;
                dbLatestTestResult.PassingFromDateTime = utcNow;
                dbLatestTestResult.FailingFromDateTime = null;
                SaveLatestTestResult(dbLatestTestResult);
            }
            else if ((testResult.ResultType == DomainTestResultType.Fail || testResult.ResultType == DomainTestResultType.Inconclusive)
                      && dbLatestTestResult.FailingFromDateTime == null)
            {
                dbLatestTestResult.TestResultId = dbTestResult.TestResultID;
                dbLatestTestResult.FailingFromDateTime = utcNow;
                dbLatestTestResult.PassingFromDateTime = null;
                SaveLatestTestResult(dbLatestTestResult);
            }
            else if (suiteNameChanged)
                SaveLatestTestResult(dbLatestTestResult);

            var latestDatabaseTestFailedDuration = _zigNetEntities.TestFailureDurations
                .OrderByDescending(f => f.FailureStartDateTime)
                .FirstOrDefault(f =>
                    f.SuiteId == suiteResult.SuiteId &&
                    f.TestId == dbTestResult.Test.TestID
                );
            if (testResult.ResultType == DomainTestResultType.Pass
                && latestDatabaseTestFailedDuration != null
                && latestDatabaseTestFailedDuration.FailureStartDateTime != null && latestDatabaseTestFailedDuration.FailureEndDateTime == null)
            {
                latestDatabaseTestFailedDuration.FailureEndDateTime = utcNow;
                SaveTestFailedDuration(latestDatabaseTestFailedDuration);
            }
            else if (testResult.ResultType == DomainTestResultType.Fail || testResult.ResultType == DomainTestResultType.Inconclusive)
            {
                if (latestDatabaseTestFailedDuration == null || latestDatabaseTestFailedDuration.FailureEndDateTime != null)
                {
                    var newTestFailedDuration = new TestFailureDuration
                    {
                        SuiteId = suiteResult.SuiteId,
                        TestId = dbTestResult.Test.TestID,
                        TestResultId = dbTestResult.TestResultID,
                        FailureStartDateTime = utcNow
                    };
                    SaveTestFailedDuration(newTestFailedDuration);
                }
            }
        }

        private DomainTest GetMappedTestWithCategoriesOrDefault(string testName)
        {
            return _zigNetEntities.Tests
                .AsNoTracking()
                .Include(t => t.TestCategories)
                .Select(t =>
                    new DomainTest
                    {
                        TestID = t.TestID,
                        Name = t.TestName,
                        Categories = t.TestCategories.Select(tc => new DomainTestCategory { TestCategoryID = tc.TestCategoryID, Name = tc.CategoryName }).ToList()
                    }
                )
                .SingleOrDefault(t => t.Name == testName);
        }
        private int MapTestResultType(DomainTestResultType domainTestResultType)
        {
            switch (domainTestResultType)
            {
                case DomainTestResultType.Fail:
                    return 1;
                case DomainTestResultType.Inconclusive:
                    return 2;
                case DomainTestResultType.Pass:
                    return 3;
                default:
                    throw new InvalidOperationException("Test result type not recognized");
            }
        }
        private TestFailureType GetTestFailureType(DomainTestFailureType zigNetTestFailureType)
        {
            switch (zigNetTestFailureType)
            {
                case DomainTestFailureType.Exception:
                    return _zigNetEntities.TestFailureTypes.Single(t => t.TestFailureTypeID == 2);
                case DomainTestFailureType.Assertion:
                    return _zigNetEntities.TestFailureTypes.Single(t => t.TestFailureTypeID == 1);
                default:
                    throw new InvalidOperationException("Test failure type not recognized");
            }
        }
        private void SaveLatestTestResult(DbLatestTestResult latestTestResult)
        {
            if (latestTestResult.LatestTestResultID == 0)
                _zigNetEntities.LatestTestResults.Add(latestTestResult);
            _zigNetEntities.SaveChanges();
        }
        private void SaveTestFailedDuration(TestFailureDuration testFailedDuration)
        {
            if (testFailedDuration.TestFailureDurationID == 0)
                _zigNetEntities.TestFailureDurations.Add(testFailedDuration);
            _zigNetEntities.SaveChanges();
        }
    }
}
