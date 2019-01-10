using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestStep = ZigNet.Domain.Test.TestStep.TestStep;
using TestStepResult = ZigNet.Domain.Test.TestStep.TestStepResult;
using TestStepResultType = ZigNet.Domain.Test.TestStep.TestStepResultType;
using DbTest = ZigNet.Database.EntityFramework.Test;
using DbTestStepResult = ZigNet.Database.EntityFramework.TestStepResult;
using DbTestStep = ZigNet.Database.EntityFramework.TestStep;
using System.Collections.Generic;
using System;
using Moq;
using ZigNet.Database.EntityFramework;
using ZigNet.Services.EntityFramework.Tests.Helpers;
using System.Linq;

namespace ZigNet.Services.EntityFramework.Tests
{
    public class TestStepServiceTests
    {
        [TestClass]
        public class Save
        {
            [TestMethod]
            public void SavesNewTestStep()
            {
                var now = DateTime.Now;

                var testId = 1;
                var testResultId = 2;
                var testStepResults = new List<TestStepResult>
                {
                    new TestStepResult {
                        StartTime = now, EndTime = now.AddDays(1),
                        ResultType = TestStepResultType.Pass,
                        TestStep = new TestStep { Name = "new-test-step" }
                    }
                };

                var mockTestSteps = new List<DbTestStep> {
                    new DbTestStep { TestStepID = 0, TestStepName = "saved-new-test-step" }
                }.ToDbSetMock();
                mockTestSteps.Setup(t => t.AsNoTracking()).Returns(mockTestSteps.Object);

                var mockTests = new List<DbTest> {
                    new DbTest {
                        TestID = testId, TestName = "test-name",
                        TestSteps = new List<DbTestStep>()
                    }
                }.ToDbSetMock();
                mockTests.Setup(m => m.Include(It.IsAny<string>())).Returns(mockTests.Object);

                var mockTestStepResults = new List<DbTestStepResult>().ToDbSetMock();

                var mockContext = new Mock<ZigNetEntities>();
                mockContext.Setup(m => m.TestSteps).Returns(mockTestSteps.Object);
                mockContext.Setup(m => m.Tests).Returns(mockTests.Object);
                mockContext.Setup(m => m.TestStepResults).Returns(mockTestStepResults.Object);

                var mockDbContext = new Mock<IDbContext>();
                mockDbContext.Setup(d => d.Get()).Returns(mockContext.Object);

                var testStepService = new TestStepService(mockDbContext.Object);
                var testStep = testStepService.Save(testId, testResultId, testStepResults).Single();

                Assert.AreEqual(testResultId, testStep.TestResult.TestResultID);
                Assert.AreEqual(now, testStep.StartTime);
                Assert.AreEqual(now.AddDays(1), testStep.EndTime);
                Assert.AreEqual(TestStepResultType.Pass, testStep.ResultType);
                Assert.AreEqual(0, testStep.TestStep.TestStepID);
                Assert.AreEqual("new-test-step", testStep.TestStep.Name);
                Assert.AreEqual(0, testStep.TestStep.TestStepID);
                Assert.AreEqual(1, testStep.TestStep.Tests.Single().TestID);
                Assert.AreEqual("test-name", testStep.TestStep.Tests.Single().Name);
                Assert.AreEqual(0, testStep.TestStep.Tests.Single().TestSteps.Single().TestStepID);
                Assert.AreEqual("saved-new-test-step", testStep.TestStep.Tests.Single().TestSteps.Single().Name);
            }

            [TestMethod]
            public void SavesExistingTestStep()
            {
                const int testId = 1;

                var testStepResults = new List<TestStepResult>
                {
                    new TestStepResult {
                        ResultType = TestStepResultType.Fail,
                        TestStep = new TestStep { Name = "existing-test-step" }
                    }
                };

                var mockTestSteps = new List<DbTestStep> {
                    new DbTestStep { TestStepID = 3, TestStepName = "existing-test-step" }
                }.ToDbSetMock();
                mockTestSteps.Setup(t => t.AsNoTracking()).Returns(mockTestSteps.Object);

                var mockTests = new List<DbTest> {
                    new DbTest {
                        TestID = testId,
                        TestSteps = new List<DbTestStep>()
                    }
                }.ToDbSetMock();
                mockTests.Setup(m => m.Include(It.IsAny<string>())).Returns(mockTests.Object);

                var mockTestStepResults = new List<DbTestStepResult>().ToDbSetMock();

                var mockContext = new Mock<ZigNetEntities>();
                mockContext.Setup(m => m.TestSteps).Returns(mockTestSteps.Object);
                mockContext.Setup(m => m.Tests).Returns(mockTests.Object);
                mockContext.Setup(m => m.TestStepResults).Returns(mockTestStepResults.Object);

                var mockDbContext = new Mock<IDbContext>();
                mockDbContext.Setup(d => d.Get()).Returns(mockContext.Object);

                var testStepService = new TestStepService(mockDbContext.Object);
                var testStep = testStepService.Save(testId, 2, testStepResults).Single();

                Assert.AreEqual(TestStepResultType.Fail, testStep.ResultType);
                Assert.AreEqual(3, testStep.TestStep.TestStepID);
                Assert.IsNull(testStep.TestStep.Name);
                Assert.AreEqual(3, testStep.TestStep.Tests.Single().TestSteps.Single().TestStepID);
                Assert.AreEqual("existing-test-step", testStep.TestStep.Tests.Single().TestSteps.Single().Name);
            }

            [TestMethod]
            public void AssignsExistingTestStep()
            {
                const int testId = 1;

                var testStepResults = new List<TestStepResult> {
                    new TestStepResult { TestStep = new TestStep { Name = "existing-test-step" } }
                };

                var mockTestSteps = new List<DbTestStep> {
                    new DbTestStep { TestStepID = 3, TestStepName = "existing-test-step" }
                }.ToDbSetMock();
                mockTestSteps.Setup(t => t.AsNoTracking()).Returns(mockTestSteps.Object);

                var mockTests = new List<DbTest> {
                    new DbTest {
                        TestID = testId,
                        TestSteps = new List<DbTestStep> { new DbTestStep {  TestStepID = 3, TestStepName = "test-step-already-assigned"} }
                    }
                }.ToDbSetMock();
                mockTests.Setup(m => m.Include(It.IsAny<string>())).Returns(mockTests.Object);

                var mockTestStepResults = new List<DbTestStepResult>().ToDbSetMock();

                var mockContext = new Mock<ZigNetEntities>();
                mockContext.Setup(m => m.TestSteps).Returns(mockTestSteps.Object);
                mockContext.Setup(m => m.Tests).Returns(mockTests.Object);
                mockContext.Setup(m => m.TestStepResults).Returns(mockTestStepResults.Object);

                var mockDbContext = new Mock<IDbContext>();
                mockDbContext.Setup(d => d.Get()).Returns(mockContext.Object);

                var testStepService = new TestStepService(mockDbContext.Object);
                var testStep = testStepService.Save(testId, 2, testStepResults).Single();

                Assert.AreEqual(3, testStep.TestStep.TestStepID);
                Assert.IsNull(testStep.TestStep.Name);
                Assert.AreEqual(3, testStep.TestStep.Tests.Single().TestSteps.Single().TestStepID);
                Assert.AreEqual("test-step-already-assigned", testStep.TestStep.Tests.Single().TestSteps.Single().Name);
            }
        }
    }
}
