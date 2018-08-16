using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using ZigNet.Database.EntityFramework;
using ZigNet.Services.DTOs;
using DbTest = ZigNet.Database.EntityFramework.Test;
using DbTestResult = ZigNet.Database.EntityFramework.TestResult;
using DbTestCategory = ZigNet.Database.EntityFramework.TestCategory;
using DbTestFailureDetail = ZigNet.Database.EntityFramework.TestFailureDetail;
using DbTestFailureType = ZigNet.Database.EntityFramework.TestFailureType;
using TestResult = ZigNet.Domain.Test.TestResult;
using TestResultType = ZigNet.Domain.Test.TestResultType;
using Test = ZigNet.Domain.Test.Test;
using TestCategory = ZigNet.Domain.Test.TestCategory;
using TestFailureType = ZigNet.Domain.Test.TestFailureType;
using TestFailureDetails = ZigNet.Domain.Test.TestFailureDetails;
using SuiteResult = ZigNet.Domain.Suite.SuiteResult;
using Suite = ZigNet.Domain.Suite.Suite;
using ZigNet.Services.EntityFramework.Mapping;

namespace ZigNet.Services.EntityFramework
{
    public class TestResultService : ITestResultService
    {
        private ZigNetEntities _zigNetEntities;
        private ISuiteService _suiteService;
        private ILatestTestResultsService _latestTestResultsService;
        private ITestFailureDurationService _testFailureDurationService;
        private ITestResultMapper _testResultMapper;
        private ITemporaryTestResultsService _temporaryTestResultsService;

        public TestResultService(IZigNetEntitiesWrapper zigNetEntitiesWrapper, ISuiteService suiteService,
            ILatestTestResultsService latestTestResultsService, ITestFailureDurationService testFailureDurationService,
            ITestResultMapper testResultMapper, ITemporaryTestResultsService temporaryTestResultsService)
        {
            _zigNetEntities = zigNetEntitiesWrapper.Get();
            _suiteService = suiteService;
            _latestTestResultsService = latestTestResultsService;
            _testFailureDurationService = testFailureDurationService;
            _temporaryTestResultsService = temporaryTestResultsService;
            _testResultMapper = testResultMapper;
        }

        public IEnumerable<LatestTestResultDto> GetLatestResults(int suiteId)
        {
            var latestTestResults = _latestTestResultsService.Get(suiteId).ToList();
            latestTestResults = AssignTestFailureDurations(latestTestResults);
            return Sort(latestTestResults);
        }

        public IEnumerable<LatestTestResultDto> GetLatestResultsGrouped(int suiteId)
        {
            var suite = _suiteService.Get(suiteId);
            var suiteIds = _suiteService.GetAll()
                .Where(s => s.EnvironmentId == suite.EnvironmentId && s.ApplicationId == suite.ApplicationId)
                .Select(s => s.SuiteID)
                .ToArray();

            var latestTestResults = _latestTestResultsService.Get(suiteIds).ToList();
            latestTestResults = AssignTestFailureDurations(latestTestResults);
            return Sort(latestTestResults);
        }

        private List<LatestTestResultDto> AssignTestFailureDurations(List<LatestTestResultDto> latestTestResults)
        {
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

            return latestTestResults;
        }
        private List<LatestTestResultDto> Sort(List<LatestTestResultDto> latestTestResults)
        {
            var passingLatestTestResults = latestTestResults.Where(t => t.PassingFromDate != null).OrderByDescending(t => t.PassingFromDate);
            var failingLatestTestResults = latestTestResults.Where(t => t.FailingFromDate != null).OrderBy(t => t.FailingFromDate).ToList();
            failingLatestTestResults.AddRange(passingLatestTestResults);
            return failingLatestTestResults;
        }

        public TestResult SaveTestResult(TestResult testResult)
        {
            // todo: move entire function to new service (there are too many dependencies)

            var existingTest = GetMappedTestWithCategoriesOrDefault(testResult.Test.Name);
            if (existingTest != null)
            {
                testResult.Test.TestID = existingTest.TestID;
                testResult.Test.Categories = testResult.Test.Categories.Concat(existingTest.Categories).ToList();
            }

            var dbTestResult = new DbTestResult
            {
                SuiteResultId = testResult.SuiteResult.SuiteResultID,
                TestResultStartDateTime = testResult.StartTime,
                TestResultEndDateTime = testResult.EndTime,
                TestResultTypeId = _testResultMapper.Map(testResult.ResultType)
            };

            if (testResult.ResultType == TestResultType.Fail)
            {
                dbTestResult.TestFailureTypes.Add(GetTestFailureType(testResult.TestFailureDetails.FailureType));
                if (!string.IsNullOrWhiteSpace(testResult.TestFailureDetails.FailureDetailMessage))
                    dbTestResult.TestFailureDetails.Add(
                        new DbTestFailureDetail {
                            TestFailureDetail1 = testResult.TestFailureDetails.FailureDetailMessage
                        });
            }

            if (testResult.Test.TestID != 0)
                dbTestResult.Test = _zigNetEntities.Tests
                    .Include(t => t.Suites)
                    .Single(t => t.TestID == testResult.Test.TestID);
            else
                dbTestResult.Test = new DbTest {
                    TestName = testResult.Test.Name,
                    TestCategories = new List<DbTestCategory>()
                };

            dbTestResult.Test.TestCategories.Clear();
            var dbTestCategories = _zigNetEntities.TestCategories.OrderBy(c => c.TestCategoryID).ToList(); // todo: remove ordering categories
            foreach (var testCategory in testResult.Test.Categories)
            {
                // use FirstOrDefault instead of SingleOrDefault because first-run multi-threaded tests end up inserting duplicate categories
                // (before the check for duplicates happens)
                var existingDbTestCategory = dbTestCategories
                    .FirstOrDefault(c => c.CategoryName == testCategory.Name);
                if (existingDbTestCategory != null)
                    dbTestResult.Test.TestCategories.Add(existingDbTestCategory);
                else
                    dbTestResult.Test.TestCategories.Add(new DbTestCategory { CategoryName = testCategory.Name });
            }

            var suiteResult = _zigNetEntities.SuiteResults
                .AsNoTracking()
                .Single(sr => sr.SuiteResultID == testResult.SuiteResult.SuiteResultID);
            if (!dbTestResult.Test.Suites.Any(s => s.SuiteID == suiteResult.SuiteId))
                dbTestResult.Test.Suites.Add(
                    _zigNetEntities.Suites
                        .Single(s => s.SuiteID == suiteResult.SuiteId));

            _zigNetEntities.TestResults.Add(dbTestResult);
            _zigNetEntities.SaveChanges();

            var savedTestResult = Map(dbTestResult, suiteResult.SuiteId);

            _temporaryTestResultsService.Save(
                _testResultMapper.ToTemporaryTestResult(savedTestResult));

            var utcNow = DateTime.UtcNow;
            savedTestResult.SuiteResult.Suite.Name = _zigNetEntities.Suites
                .AsNoTracking()
                .Single(s => s.SuiteID == suiteResult.SuiteId)
                .SuiteName;
            _latestTestResultsService.Save(
                _testResultMapper.ToLatestTestResult(savedTestResult), savedTestResult.ResultType, utcNow);

            // todo: move to mapping class
            var testFailureDurationDto = new TestFailureDurationDto
            {
                SuiteId = savedTestResult.SuiteResult.Suite.SuiteID,
                TestId = savedTestResult.Test.TestID,
                TestResultId = savedTestResult.TestResultID
            };

            _testFailureDurationService.Save(testFailureDurationDto, savedTestResult.ResultType, utcNow);

            return savedTestResult;
        }

        private TestResult Map(DbTestResult dbTestResult, int suiteId)
        {
            var testResult = new TestResult
            {
                TestResultID = dbTestResult.TestResultID,
                Test = new Test
                {
                    TestID = dbTestResult.Test.TestID,
                    Name = dbTestResult.Test.TestName,
                    Suites = new List<Suite>(),
                    Categories = new List<TestCategory>()
                },
                SuiteResult = new SuiteResult
                {
                    SuiteResultID = dbTestResult.SuiteResultId,
                    Suite = new Suite { SuiteID = suiteId }
                },
                ResultType = MapTestResultType(dbTestResult.TestResultTypeId),
                StartTime = dbTestResult.TestResultStartDateTime,
                EndTime = dbTestResult.TestResultEndDateTime
            };
            if (testResult.ResultType == TestResultType.Fail)
                testResult.TestFailureDetails = new TestFailureDetails
                {
                    FailureDetailMessage = dbTestResult.TestFailureDetails.Count == 0 ? null : dbTestResult.TestFailureDetails.First().TestFailureDetail1,
                    FailureType = MapTestFailureType(dbTestResult.TestFailureTypes.First().TestFailureTypeID)
                };
            foreach (var dbSuite in dbTestResult.Test.Suites)
                testResult.Test.Suites.Add(new Suite { SuiteID = dbSuite.SuiteID });
            foreach (var dbTestCategory in dbTestResult.Test.TestCategories)
                testResult.Test.Categories.Add(new TestCategory { TestCategoryID = dbTestCategory.TestCategoryID, Name = dbTestCategory.CategoryName });

            return testResult;
        }

        private Test GetMappedTestWithCategoriesOrDefault(string testName)
        {
            return _zigNetEntities.Tests
                .AsNoTracking()
                .Include(t => t.TestCategories)
                .Select(t =>
                    new Test
                    {
                        TestID = t.TestID,
                        Name = t.TestName,
                        Categories = t.TestCategories.Select(tc => new TestCategory { TestCategoryID = tc.TestCategoryID, Name = tc.CategoryName }).ToList()
                    }
                )
                .SingleOrDefault(t => t.Name == testName);
        }
        private DbTestFailureType GetTestFailureType(TestFailureType zigNetTestFailureType)
        {
            switch (zigNetTestFailureType)
            {
                case TestFailureType.Exception:
                    return _zigNetEntities.TestFailureTypes.Single(t => t.TestFailureTypeID == 2);
                case TestFailureType.Assertion:
                    return _zigNetEntities.TestFailureTypes.Single(t => t.TestFailureTypeID == 1);
                default:
                    throw new InvalidOperationException("Test failure type not recognized");
            }
        }

        // todo: move 2 below to mapping classes
        private TestResultType MapTestResultType(int dbTestResultTypeId)
        {
            switch (dbTestResultTypeId)
            {
                case 1:
                    return TestResultType.Fail;
                case 2:
                    return TestResultType.Inconclusive;
                case 3:
                    return TestResultType.Pass;
                default:
                    throw new InvalidOperationException("DB test result type ID not recognized");
            }
        }
        private TestFailureType MapTestFailureType(int dbTestFailureTypeId)
        {
            switch (dbTestFailureTypeId)
            {
                case 1:
                    return TestFailureType.Assertion;
                case 2:
                    return TestFailureType.Exception;
                default:
                    throw new InvalidOperationException("DB test failure type ID not recognized");
            }
        }
    }
}
