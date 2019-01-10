using System;
using System.Collections.Generic;
using ZigNet.Database.EntityFramework;
using Test = ZigNet.Domain.Test.Test;
using TestStep = ZigNet.Domain.Test.TestStep.TestStep;
using TestResult = ZigNet.Domain.Test.TestResult;
using TestStepResult = ZigNet.Domain.Test.TestStep.TestStepResult;
using TestStepResultType = ZigNet.Domain.Test.TestStep.TestStepResultType;
using DbTest = ZigNet.Database.EntityFramework.Test;
using DbTestStepResult = ZigNet.Database.EntityFramework.TestStepResult;
using DbTestStep = ZigNet.Database.EntityFramework.TestStep;
using System.Linq;
using System.Data.Entity;

namespace ZigNet.Services.EntityFramework
{
    public class TestStepService : ITestStepService
    {
        private ZigNetEntities _db;

        public TestStepService(IDbContext dbContext)
        {
            _db = dbContext.Get();
        }

        public ICollection<TestStepResult> Save(int testId, int testResultId, IEnumerable<TestStepResult> testStepResults)
        {
            var dbTestSteps = _db.TestSteps.AsNoTracking().ToList();
            var dbTest = _db.Tests
                .Include(t => t.TestSteps)
                .Single(t => t.TestID == testId);

            var savedTestStepResults = new List<TestStepResult>();
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
                _db.SaveChanges();

                if (!dbTest.TestSteps.Any(t => t.TestStepID == dbTestStepResult.TestStepId))
                    dbTest.TestSteps.Add(
                        _db.TestSteps
                           .Single(ts => ts.TestStepID == dbTestStepResult.TestStepId));

                savedTestStepResults.Add(Map(dbTestStepResult, dbTest));
            }

            _db.SaveChanges();

            return savedTestStepResults;
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

        private TestStepResultType Map(int testStepResultType)
        {
            switch (testStepResultType)
            {
                case 1:
                    return TestStepResultType.Fail;
                case 2:
                    return TestStepResultType.Inconclusive;
                case 3:
                    return TestStepResultType.Pass;
                default:
                    throw new InvalidOperationException("Test Step Result Type not recognized");
            }
        }

        private TestStepResult Map(DbTestStepResult dbTestStepResult, DbTest dbTest)
        {
            var testStepResult = new TestStepResult
            {
                TestStep = new TestStep { TestStepID = dbTestStepResult.TestStepId, Tests = new List<Test>() },
                TestResult = new TestResult { TestResultID = dbTestStepResult.TestResultId },
                StartTime = dbTestStepResult.TestStepResultStartDateTime,
                EndTime = dbTestStepResult.TestStepResultEndDateTime,
                ResultType = Map(dbTestStepResult.TestStepResultTypeId)
            };

            testStepResult.TestStep.Tests.Add(
                new Test {
                    TestID = dbTest.TestID, Name = dbTest.TestName,
                    TestSteps = new List<TestStep>()
                });

            foreach (var testStep in dbTest.TestSteps)
                testStepResult.TestStep.Tests.First().TestSteps.Add(
                    new TestStep { TestStepID = testStep.TestStepID, Name = testStep.TestStepName });

            if (dbTestStepResult.TestStep != null)
                testStepResult.TestStep.Name = dbTestStepResult.TestStep.TestStepName;

            return testStepResult;
        }
    }
}
