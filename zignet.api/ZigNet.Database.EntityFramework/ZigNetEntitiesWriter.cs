using System.Linq;
using System.Data.Entity;

namespace ZigNet.Database.EntityFramework
{
    public class ZigNetEntitiesWriter : IZigNetEntitiesWriter
    {
        private ZigNetEntities _zigNetEntities;

        public ZigNetEntitiesWriter(IZigNetEntitiesWrapper zigNetEntitiesWrapper)
        {
            _zigNetEntities = zigNetEntitiesWrapper.Get();
        }

        public Suite GetSuite(int suiteId)
        {
            return _zigNetEntities.Suites.Single(s => s.SuiteID == suiteId);
        }
        public SuiteResult GetSuiteResult(int suiteResultId)
        {
            return _zigNetEntities.SuiteResults.Single(sr => sr.SuiteResultID == suiteResultId);
        }
        public Test GetTestWithSuites(int testId)
        {
            return _zigNetEntities.Tests
                .Include(t => t.Suites)
                .Single(t => t.TestID == testId);
        }
        public TestFailureType GetTestFailureType(int testFailureTypeId)
        {
            return _zigNetEntities.TestFailureTypes
                .Single(tft => tft.TestFailureTypeID == testFailureTypeId);
        }
        public IQueryable<TestCategory> GetTestCategories()
        {
            return _zigNetEntities.TestCategories;
        }
        public IQueryable<LatestTestResult> GetLatestTestResults()
        {
            return _zigNetEntities.LatestTestResults;
        }
        public IQueryable<TestFailureDuration> GetTestFailureDurations()
        {
            return _zigNetEntities.TestFailureDurations;
        }
        public int SaveSuiteResult(SuiteResult suiteResult)
        {
            if (suiteResult.SuiteResultID == 0)
                _zigNetEntities.SuiteResults.Add(suiteResult);
            _zigNetEntities.SaveChanges();
            return suiteResult.SuiteResultID;
        }
        public void SaveLatestTestResult(LatestTestResult latestTestResult)
        {
            if (latestTestResult.LatestTestResultID == 0)
                _zigNetEntities.LatestTestResults.Add(latestTestResult);
            _zigNetEntities.SaveChanges();
        }
        public void SaveTestFailedDuration(TestFailureDuration testFailedDuration)
        {
            if (testFailedDuration.TestFailureDurationID == 0)
                _zigNetEntities.TestFailureDurations.Add(testFailedDuration);
            _zigNetEntities.SaveChanges();
        }
        public TestResult SaveTestResult(TestResult testResult)
        {
            _zigNetEntities.TestResults.Add(testResult);
            _zigNetEntities.SaveChanges();
            return testResult;
        }
        public void SaveTemporaryTestResult(TemporaryTestResult temporaryTestResult)
        {
            _zigNetEntities.TemporaryTestResults.Add(temporaryTestResult);
            _zigNetEntities.SaveChanges();
        }
        public void DeleteAllTemporaryTestResultsForSuite(int suiteId)
        {
            var temporaryTestResultsToDelete = _zigNetEntities.TemporaryTestResults.Where(ttr => ttr.SuiteId == suiteId);
            _zigNetEntities.TemporaryTestResults.RemoveRange(temporaryTestResultsToDelete);
            _zigNetEntities.SaveChanges();
        }

        public IQueryable<Suite> GetSuites()
        {
            return _zigNetEntities.Suites;
        }
        public IQueryable<SuiteCategory> GetSuiteCategories()
        {
            return _zigNetEntities.SuiteCategories;
        }
        public int SaveSuite(Suite suite)
        {
            if (suite.SuiteID == 0)
                _zigNetEntities.Suites.Add(suite);
            _zigNetEntities.SaveChanges();
            return suite.SuiteID;
        }
    }
}
