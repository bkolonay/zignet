using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using ZigNet.Database.EntityFramework;
using DbLatestTestResult = ZigNet.Database.EntityFramework.LatestTestResult;
using DtoLatestTestResult = ZigNet.Database.DTOs.LatestTestResult;
using DtoTestFailureDuration = ZigNet.Database.DTOs.TestFailureDuration;
using DomainTestResult = ZigNet.Domain.Test.TestResult;
using DomainTestResultType = ZigNet.Domain.Test.TestResultType;
using DomainTest = ZigNet.Domain.Test.Test;
using DomainTestCategory = ZigNet.Domain.Test.TestCategory;
using DomainTestFailureType = ZigNet.Domain.Test.TestFailureType;

namespace ZigNet.Services.EntityFramework
{
    public class TestResultService : ITestResultService
    {
        private ZigNetEntities _zigNetEntities;

        public TestResultService(IZigNetEntitiesWrapper zigNetEntitiesWrapper)
        {
            _zigNetEntities = zigNetEntitiesWrapper.Get();
        }

        public IEnumerable<DtoLatestTestResult> GetLatestResults(int suiteId, bool group)
        {
            var dbLatestTestResults = new List<DbLatestTestResult>();

            if (group)
            {
                var suite = _zigNetEntities.Suites.AsNoTracking().Single(s => s.SuiteID == suiteId);
                var suites = _zigNetEntities.Suites.AsNoTracking()
                    .Where(s => s.EnvironmentId == suite.EnvironmentId && s.ApplicationId == suite.ApplicationId);
                foreach (var localSuite in suites)
                    dbLatestTestResults.AddRange(_zigNetEntities.LatestTestResults.AsNoTracking().Where(l => l.SuiteId == localSuite.SuiteID));
            }
            else
                dbLatestTestResults = _zigNetEntities.LatestTestResults.AsNoTracking().Where(l => l.SuiteId == suiteId).ToList();

            var dbTestFailureDurations = _zigNetEntities.TestFailureDurations.AsNoTracking().ToList();

            var dtoLatestTestResults = new List<DtoLatestTestResult>();
            var utcNow = DateTime.UtcNow;
            foreach (var dbLatestTestResult in dbLatestTestResults)
            {
                var testFailureDurationLimit = utcNow.AddHours(-24);
                var dbTestFailureDurationsForTestResult = 
                    dbTestFailureDurations.Where(t =>
                        (t.SuiteId == dbLatestTestResult.SuiteId && t.TestId == dbLatestTestResult.TestId) &&
                        (t.FailureEndDateTime > testFailureDurationLimit || t.FailureEndDateTime == null)
                );

                var dtoTestFailureDurations = new List<DtoTestFailureDuration>();
                foreach (var dbTestFailureDuration in dbTestFailureDurationsForTestResult)
                    dtoTestFailureDurations.Add(new DtoTestFailureDuration
                    {
                        FailureStart = dbTestFailureDuration.FailureStartDateTime,
                        FailureEnd = dbTestFailureDuration.FailureEndDateTime
                    });

                dtoLatestTestResults.Add(new DtoLatestTestResult
                {
                    TestResultID = dbLatestTestResult.TestResultId,
                    TestName = dbLatestTestResult.TestName,
                    SuiteName = dbLatestTestResult.SuiteName,
                    FailingFromDate = dbLatestTestResult.FailingFromDateTime,
                    PassingFromDate = dbLatestTestResult.PassingFromDateTime,
                    TestFailureDurations = dtoTestFailureDurations
                });
            }

            var passingDtoLatestTestResults = dtoLatestTestResults.Where(ltr => ltr.PassingFromDate != null).OrderByDescending(ltr => ltr.PassingFromDate);
            var failingDtoLatestTestResults = dtoLatestTestResults.Where(ltr => ltr.FailingFromDate != null).OrderBy(ltr => ltr.FailingFromDate).ToList();
            failingDtoLatestTestResults.AddRange(passingDtoLatestTestResults);

            return failingDtoLatestTestResults;
        }

        public void SaveTestResult(DomainTestResult testResult)
        {
            var existingTestWithSameName = GetMappedTestWithCategoriesOrDefault(testResult.Test.Name);
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

            if (testResult.ResultType == DomainTestResultType.Fail)
            {
                databaseTestResult.TestFailureTypes.Add(GetTestFailureType(testResult.TestFailureDetails.FailureType));
                if (!string.IsNullOrWhiteSpace(testResult.TestFailureDetails.FailureDetailMessage))
                    databaseTestResult.TestFailureDetails.Add(new TestFailureDetail { TestFailureDetail1 = testResult.TestFailureDetails.FailureDetailMessage });
            }

            if (testResult.Test.TestID != 0)
                databaseTestResult.Test = _zigNetEntities.Tests
                    .Include(t => t.Suites)
                    .Single(t => t.TestID == testResult.Test.TestID);
            else
                databaseTestResult.Test = new Test { TestName = testResult.Test.Name, TestCategories = new List<TestCategory>() };

            databaseTestResult.Test.TestCategories.Clear();
            var existingDatabaseTestCategories = _zigNetEntities.TestCategories.OrderBy(c => c.TestCategoryID).ToList();
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

            var suiteResult = _zigNetEntities.SuiteResults
                .AsNoTracking()
                .Single(sr => sr.SuiteResultID == testResult.SuiteResult.SuiteResultID);
            if (!databaseTestResult.Test.Suites.Any(s => s.SuiteID == suiteResult.SuiteId))
            {
                var localSuite = _zigNetEntities.Suites.Single(s => s.SuiteID == suiteResult.SuiteId);                
                databaseTestResult.Test.Suites.Add(localSuite);
            }

            _zigNetEntities.TestResults.Add(databaseTestResult);
            _zigNetEntities.SaveChanges();
            
            _zigNetEntities.TemporaryTestResults.Add(new TemporaryTestResult
            {
                TestResultId = databaseTestResult.TestResultID,
                SuiteResultId = testResult.SuiteResult.SuiteResultID,
                SuiteId = suiteResult.SuiteId,
                TestResultTypeId = databaseTestResult.TestResultTypeId
            });
            _zigNetEntities.SaveChanges();

            var databaseLatestTestResult = _zigNetEntities.LatestTestResults
                .SingleOrDefault(ltr =>
                    ltr.SuiteId == suiteResult.SuiteId &&
                    ltr.TestId == databaseTestResult.Test.TestID
                );
            var suite = _zigNetEntities.Suites
                .AsNoTracking()
                .Single(s => s.SuiteID == suiteResult.SuiteId);

            var suiteNameChanged = false;
            if (databaseLatestTestResult == null)
                databaseLatestTestResult = new DbLatestTestResult
                {
                    SuiteId = suiteResult.SuiteId,
                    TestId = databaseTestResult.Test.TestID,
                    TestName = testResult.Test.Name,
                    SuiteName = suite.SuiteName
                };
            else if (databaseLatestTestResult.SuiteName != suite.SuiteName)
            {
                databaseLatestTestResult.SuiteName = suite.SuiteName;
                suiteNameChanged = true;
            }

            var utcNow = DateTime.UtcNow;
            if (testResult.ResultType == DomainTestResultType.Pass && databaseLatestTestResult.PassingFromDateTime == null)
            {
                databaseLatestTestResult.TestResultId = databaseTestResult.TestResultID;
                databaseLatestTestResult.PassingFromDateTime = utcNow;
                databaseLatestTestResult.FailingFromDateTime = null;
                SaveLatestTestResult(databaseLatestTestResult);
            }
            else if ((testResult.ResultType == DomainTestResultType.Fail || testResult.ResultType == DomainTestResultType.Inconclusive)
                      && databaseLatestTestResult.FailingFromDateTime == null)
            {
                databaseLatestTestResult.TestResultId = databaseTestResult.TestResultID;
                databaseLatestTestResult.FailingFromDateTime = utcNow;
                databaseLatestTestResult.PassingFromDateTime = null;
                SaveLatestTestResult(databaseLatestTestResult);
            }
            else if (suiteNameChanged)
                SaveLatestTestResult(databaseLatestTestResult);

            var latestDatabaseTestFailedDuration = _zigNetEntities.TestFailureDurations
                .OrderByDescending(tfd => tfd.FailureStartDateTime)
                .FirstOrDefault(tfd =>
                    tfd.SuiteId == suiteResult.SuiteId &&
                    tfd.TestId == databaseTestResult.Test.TestID
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
                        TestId = databaseTestResult.Test.TestID,
                        TestResultId = databaseTestResult.TestResultID,
                        FailureStartDateTime = utcNow
                    };
                    SaveTestFailedDuration(newTestFailedDuration);
                }
            }
        }


        // todo: find a way to get rid of/add to unit tests or another class
        public DomainTest GetMappedTestWithCategoriesOrDefault(string testName)
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
