using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using ZigNet.Domain.Suite;

namespace ZigNet.Services.EntityFramework.Mapping.Tests
{
    public class SuiteResultMapperTests
    {
        [TestClass]
        public class MapDomainToDb
        {
            [TestMethod]
            public void CorrectlyMaps()
            {
                var now = DateTime.Now;

                var suiteResult = new SuiteResult {
                    Suite = new Suite { SuiteID = 1 },
                    StartTime = now,
                    ResultType = SuiteResultType.Inconclusive
                };

                var suiteResultMapper = new SuiteResultMapper();
                var dbSuiteResult = suiteResultMapper.Map(suiteResult);

                Assert.AreEqual(1, dbSuiteResult.SuiteId);
                Assert.AreEqual(now, dbSuiteResult.SuiteResultStartDateTime);
                Assert.AreEqual(2, dbSuiteResult.SuiteResultTypeId);
            }
        }
    }
}
