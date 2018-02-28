using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using ZigNet.Domain.Test;
using ZigNet.Api.Model;

namespace ZigNet.Api.Mapping.Tests
{
    [TestClass]
    public class ZigNetApiMapperTests
    {
        [TestClass]
        public class MapCreateSuiteModelMethod
        {
            [TestMethod]
            public void MapsCreateSuiteModelWithCategories()
            {
                var createSuiteModel = new CreateSuiteModel
                {
                    SuiteName = "suite 1",
                    SuiteCategories = new string[] { "suite category 1", "suite category 2" }
                };

                var zigNetApiMapper = new ZigNetApiMapper();
                var suite = zigNetApiMapper.MapCreateSuiteModel(createSuiteModel);

                Assert.AreEqual("suite 1", suite.Name);
                Assert.AreEqual("suite category 1", suite.Categories.ToList()[0].Name);
                Assert.AreEqual("suite category 2", suite.Categories.ToList()[1].Name);
            }

            [TestMethod]
            public void MapsCreateSuiteModelWithoutCategories()
            {
                var createSuiteModel = new CreateSuiteModel
                {
                    SuiteName = "suite 1"
                };

                var zigNetApiMapper = new ZigNetApiMapper();
                var suite = zigNetApiMapper.MapCreateSuiteModel(createSuiteModel);

                Assert.AreEqual("suite 1", suite.Name);
                Assert.AreEqual(0, suite.Categories.Count);
            }

            [TestMethod]
            public void MapsCreateSuiteModelWhenCategoriesIsEmptyList()
            {
                var createSuiteModel = new CreateSuiteModel
                {
                    SuiteName = "suite 1",
                    SuiteCategories = new string[0]
                };

                var zigNetApiMapper = new ZigNetApiMapper();
                var suite = zigNetApiMapper.MapCreateSuiteModel(createSuiteModel);

                Assert.AreEqual("suite 1", suite.Name);
                Assert.AreEqual(0, suite.Categories.ToList().Count);
            }
        }

        [TestClass]
        public class MapCreateTestResultModelMethod
        {
            [TestMethod]
            public void MapsAllData()
            {
                var startTime = DateTime.UtcNow;
                var endTime = DateTime.UtcNow;

                var createTestResultModel = new CreateTestResultModel
                {
                    TestName = "test 1",
                    SuiteResultId = 2,
                    TestCategories = new string[] { "test category 1", "test category 2" },
                    StartTime = startTime,
                    EndTime = endTime,
                    TestResultType = TestResultType.Pass,
                    TestFailureType = TestFailureType.Exception,
                    TestFailureDetails = "failed because of exception at line 5"
                };

                var zigNetApiMapper = new ZigNetApiMapper();
                var testResult = zigNetApiMapper.MapCreateTestResultModel(createTestResultModel);

                Assert.AreEqual("test 1", testResult.Test.Name);
                Assert.AreEqual(2, testResult.SuiteResult.SuiteResultID);
                Assert.AreEqual("test category 1", testResult.Test.Categories.ToList()[0].Name);
                Assert.AreEqual("test category 2", testResult.Test.Categories.ToList()[1].Name);
                Assert.AreEqual(startTime, testResult.StartTime);
                Assert.AreEqual(endTime, testResult.EndTime);
                Assert.AreEqual(TestResultType.Pass, testResult.ResultType);
                Assert.AreEqual(TestFailureType.Exception, testResult.TestFailureDetails.FailureType);
                Assert.AreEqual("failed because of exception at line 5", testResult.TestFailureDetails.FailureDetailMessage);
            }

            [TestMethod]
            public void MapsEmptyModelExceptStartEndDateTime()
            {
                var startTime = DateTime.UtcNow;
                var endTime = DateTime.UtcNow;

                var createTestResultModel = new CreateTestResultModel { StartTime = startTime, EndTime = endTime };

                var zigNetApiMapper = new ZigNetApiMapper();
                var testResult = zigNetApiMapper.MapCreateTestResultModel(createTestResultModel);

                Assert.IsNull(testResult.Test.Name);
                Assert.AreEqual(0, testResult.SuiteResult.SuiteResultID);
                Assert.AreEqual(0, testResult.Test.Categories.Count);
                Assert.AreEqual(startTime, testResult.StartTime);
                Assert.AreEqual(endTime, testResult.EndTime);
                Assert.AreEqual(TestResultType.Inconclusive, testResult.ResultType);
                Assert.AreEqual(TestFailureType.Exception, testResult.TestFailureDetails.FailureType);
                Assert.IsNull(testResult.TestFailureDetails.FailureDetailMessage);
            }

            [TestMethod]
            [ExpectedException(typeof(ArgumentNullException))]
            public void ThrowsWhenStartTimeNull()
            {
                var createTestResultModel = new CreateTestResultModel { EndTime = DateTime.UtcNow };

                var zigNetApiMapper = new ZigNetApiMapper();
                var testResult = zigNetApiMapper.MapCreateTestResultModel(createTestResultModel);
            }

            [TestMethod]
            [ExpectedException(typeof(ArgumentNullException))]
            public void ThrowsWhenEndTimeNull()
            {
                var createTestResultModel = new CreateTestResultModel { StartTime = DateTime.UtcNow };

                var zigNetApiMapper = new ZigNetApiMapper();
                var testResult = zigNetApiMapper.MapCreateTestResultModel(createTestResultModel);
            }
        }
    }
}
