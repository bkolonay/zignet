using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using ZigNet.Database.EntityFramework;
using DbTest = ZigNet.Database.EntityFramework.Test;
using DbTestResult = ZigNet.Database.EntityFramework.TestResult;
using DbTestCategory = ZigNet.Database.EntityFramework.TestCategory;
using DbTestFailureDetail = ZigNet.Database.EntityFramework.TestFailureDetail;
using TestResult = ZigNet.Domain.Test.TestResult;
using TestResultType = ZigNet.Domain.Test.TestResultType;
using Test = ZigNet.Domain.Test.Test;
using TestCategory = ZigNet.Domain.Test.TestCategory;
using TestFailureDetails = ZigNet.Domain.Test.TestFailureDetails;
using SuiteResult = ZigNet.Domain.Suite.SuiteResult;
using Suite = ZigNet.Domain.Suite.Suite;
using Application = ZigNet.Domain.Suite.Application;
using Environment = ZigNet.Domain.Suite.Environment;
using ZigNet.Services.EntityFramework.Mapping;
using ZigNet.Services.DTOs;

namespace ZigNet.Services.EntityFramework
{
    public class TestResultSaverService : ITestResultSaverService
    {
        private ZigNetEntities _db;
        private ILatestTestResultService _latestTestResultService;
        private ITemporaryTestResultService _temporaryTestResultService;
        private ITestFailureDurationService _testFailureDurationService;
        private ITestStepService _testStepService;
        private ITestResultMapper _testResultMapper;
        private ISuiteService _suiteService;

        public TestResultSaverService(IDbContext dbContext, ILatestTestResultService latestTestResultService,
            ITemporaryTestResultService temporaryTestResultService, ITestFailureDurationService testFailureDurationService,
            ITestStepService testStepService, ITestResultMapper testResultMapper, ISuiteService suiteService)
        {
            _db = dbContext.Get();
            _latestTestResultService = latestTestResultService;
            _temporaryTestResultService = temporaryTestResultService;
            _testFailureDurationService = testFailureDurationService;
            _testStepService = testStepService;
            _testResultMapper = testResultMapper;
            _suiteService = suiteService;
        }

        public TestResult Save(TestResult testResult)
        {
            var existingTest = _db.Tests
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
                .SingleOrDefault(t => t.Name == testResult.Test.Name);

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
                TestResultTypeId = _testResultMapper.ToDbTestResultTypeId(testResult.ResultType)
            };

            if (testResult.ResultType == TestResultType.Fail)
            {
                var testFailureTypeId = _testResultMapper.ToDbTestFailureTypeId(testResult.TestFailureDetails.FailureType);
                var testFailureType = _db.TestFailureTypes.Single(t => t.TestFailureTypeID == testFailureTypeId);
                dbTestResult.TestFailureTypes.Add(testFailureType);
                if (!string.IsNullOrWhiteSpace(testResult.TestFailureDetails.FailureDetailMessage))
                    dbTestResult.TestFailureDetails.Add(
                        new DbTestFailureDetail { TestFailureDetail1 = testResult.TestFailureDetails.FailureDetailMessage });
            }

            if (testResult.Test.TestID != 0)
                dbTestResult.Test = _db.Tests
                    .Include(t => t.Suites)
                    .Single(t => t.TestID == testResult.Test.TestID);
            else
                dbTestResult.Test = new DbTest
                {
                    TestName = testResult.Test.Name,
                    TestCategories = new List<DbTestCategory>()
                };

            dbTestResult.Test.TestCategories.Clear();
            var dbTestCategories = _db.TestCategories.ToList();
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

            var suiteResult = _db.SuiteResults
                .AsNoTracking()
                .Single(sr => sr.SuiteResultID == testResult.SuiteResult.SuiteResultID);
            if (!dbTestResult.Test.Suites.Any(s => s.SuiteID == suiteResult.SuiteId))
                dbTestResult.Test.Suites.Add(
                    _db.Suites
                        .Single(s => s.SuiteID == suiteResult.SuiteId));

            _db.TestResults.Add(dbTestResult);
            _db.SaveChanges();

            var suiteDto = _suiteService.GetAll().Single(s => s.SuiteID == suiteResult.SuiteId);
            var savedTestResult = Map(dbTestResult, suiteDto);

            _testStepService.Save(savedTestResult.TestResultID, testResult.TestStepResults);

            _temporaryTestResultService.Save(_testResultMapper.ToTemporaryTestResult(savedTestResult));

            var utcNow = DateTime.UtcNow;
            _latestTestResultService.Save(
                _testResultMapper.ToLatestTestResult(savedTestResult), savedTestResult.ResultType, utcNow);

            _testFailureDurationService.Save(
                _testResultMapper.ToTestFailureDuration(savedTestResult), savedTestResult.ResultType, utcNow);

            return savedTestResult;
        }

        private TestResult Map(DbTestResult dbTestResult, SuiteDto suiteDto)
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
                    Suite = new Suite {
                        SuiteID = suiteDto.SuiteID,
                        Name = suiteDto.Name,
                        Application = new Application { Name = suiteDto.ApplicationName },
                        Environment = new Environment { Abbreviation = suiteDto.EnvironmentNameAbbreviation }
                    }
                },
                ResultType = _testResultMapper.ToTestResultType(dbTestResult.TestResultTypeId),
                StartTime = dbTestResult.TestResultStartDateTime,
                EndTime = dbTestResult.TestResultEndDateTime
            };

            if (testResult.ResultType == TestResultType.Fail)
                testResult.TestFailureDetails = new TestFailureDetails
                {
                    FailureDetailMessage = dbTestResult.TestFailureDetails.Count == 0 ? null : dbTestResult.TestFailureDetails.First().TestFailureDetail1,
                    FailureType = _testResultMapper.ToTestFailureType(dbTestResult.TestFailureTypes.First().TestFailureTypeID)
                };

            foreach (var dbSuite in dbTestResult.Test.Suites)
                testResult.Test.Suites.Add(new Suite { SuiteID = dbSuite.SuiteID });
            foreach (var dbTestCategory in dbTestResult.Test.TestCategories)
                testResult.Test.Categories.Add(new TestCategory { TestCategoryID = dbTestCategory.TestCategoryID, Name = dbTestCategory.CategoryName });

            return testResult;
        }
    }
}
