using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using ZigNet.Domain.Suite;
using DbSuiteResult = ZigNet.Database.EntityFramework.SuiteResult;
using DomainSuiteResult = ZigNet.Domain.Suite.SuiteResult;
using DomainSuiteResultType = ZigNet.Domain.Suite.SuiteResultType;

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

                var domainSuiteResult = new DomainSuiteResult {
                    Suite = new Suite { SuiteID = 1 },
                    StartTime = now,
                    ResultType = DomainSuiteResultType.Inconclusive
                };

                var suiteResultMapper = new SuiteResultMapper();
                var dbSuiteResult = suiteResultMapper.Map(domainSuiteResult);

                Assert.AreEqual(1, dbSuiteResult.SuiteId);
                Assert.AreEqual(now, dbSuiteResult.SuiteResultStartDateTime);
                Assert.AreEqual(2, dbSuiteResult.SuiteResultTypeId);
            }
        }

        [TestClass]
        public class MapExistingDbToDomain
        {
            [TestMethod]
            public void CorrectlyMaps()
            {
                var now = DateTime.Now;

                var domainSuiteResult = new DomainSuiteResult
                {
                    Suite = new Suite { SuiteID = 1 },
                    EndTime = now,
                    ResultType = DomainSuiteResultType.Pass
                };

                var dbSuiteResult = new DbSuiteResult
                {
                    SuiteResultID = 2,
                    SuiteId = 7
                };

                var suiteResultMapper = new SuiteResultMapper();
                var mappedDbSuiteResult = suiteResultMapper.Map(dbSuiteResult, domainSuiteResult);

                Assert.AreEqual(2, mappedDbSuiteResult.SuiteResultID);
                Assert.AreEqual(7, mappedDbSuiteResult.SuiteId);
                Assert.AreEqual(now, mappedDbSuiteResult.SuiteResultEndDateTime);
                Assert.AreEqual(3, dbSuiteResult.SuiteResultTypeId);
            }
        }
    }
}
