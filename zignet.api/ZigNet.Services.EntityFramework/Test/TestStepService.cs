using System;
using System.Collections.Generic;
using ZigNet.Database.EntityFramework;
using TestStepResult = ZigNet.Domain.Test.TestStep.TestStepResult;
using TestStepResultType = ZigNet.Domain.Test.TestStep.TestStepResultType;
using DbTestStepResult = ZigNet.Database.EntityFramework.TestStepResult;
using DbTestStep = ZigNet.Database.EntityFramework.TestStep;
using System.Linq;

namespace ZigNet.Services.EntityFramework
{
    public class TestStepService : ITestStepService
    {
        private ZigNetEntities _db;

        public TestStepService(IDbContext dbContext)
        {
            _db = dbContext.Get();
        }

        public void Save(int testResultId, IEnumerable<TestStepResult> testStepResults)
        {
            var dbTestSteps = _db.TestSteps.AsNoTracking().ToList();

            foreach (var testStepResult in testStepResults)
            {
                var dbTestStepResult = new DbTestStepResult
                {
                    TestResultId = testResultId,
                    TestStepResultStartDateTime = testStepResult.StartTime,
                    TestStepResultEndDateTime = testStepResult.EndTime,
                    TestStepResultTypeId = Map(testStepResult.ResultType)
                };

                // use FirstOrDefault instead of SingleOrDefault because first-run multi-threaded tests can end up inserting duplicate step names
                // (before the check for duplicates happens)
                var existingTestStep = dbTestSteps
                    .FirstOrDefault(t => t.TestStepName == testStepResult.TestStep.Name);
                if (existingTestStep != null)
                    dbTestStepResult.TestStepId = existingTestStep.TestStepID;
                else
                    dbTestStepResult.TestStep = new DbTestStep { TestStepName = testStepResult.TestStep.Name };

                _db.TestStepResults.Add(dbTestStepResult);
            }

            _db.SaveChanges();
        }

        private int Map(TestStepResultType testStepResultType)
        {
            switch (testStepResultType)
            {
                case TestStepResultType.Fail:
                    return 1;
                case TestStepResultType.Inconclusive:
                    return 2;
                case TestStepResultType.Pass:
                    return 3;
                default:
                    throw new InvalidOperationException("Test Step Result Type not recognized");
            }
        }
    }
}
