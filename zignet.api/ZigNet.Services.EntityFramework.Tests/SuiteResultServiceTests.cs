using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using ZigNet.Database.EntityFramework;
using ZigNet.Services.EntityFramework.Mapping;
using ZigNet.Services.EntityFramework.Tests.Helpers;
using DbSuiteResult = ZigNet.Database.EntityFramework.SuiteResult;
using DomainSuiteResult = ZigNet.Domain.Suite.SuiteResult;

namespace ZigNet.Services.EntityFramework.Tests
{
    public class SuiteResultServiceTests
    {
        [TestClass]
        public class SaveSuiteResult
        {
            [TestMethod]
            public void SavesNew()
            {
                var domainSuiteResult = new DomainSuiteResult();

                var dbSuiteResults = new List<DbSuiteResult>().ToDbSetMock();

                var zigNetEntitiesMock = new Mock<ZigNetEntities>();
                zigNetEntitiesMock.Setup(z => z.SuiteResults).Returns(dbSuiteResults.Object);

                var zigNetEntitiesWrapperMock = new Mock<IZigNetEntitiesWrapper>();
                zigNetEntitiesWrapperMock.Setup(z => z.Get()).Returns(zigNetEntitiesMock.Object);

                var suiteResultMapperMock = new Mock<ISuiteResultMapper>();
                suiteResultMapperMock.Setup(s => s.Map(domainSuiteResult)).Returns(new DbSuiteResult());

                var suiteResultService = new SuiteResultService(zigNetEntitiesWrapperMock.Object, suiteResultMapperMock.Object);
                var suiteId = suiteResultService.SaveSuiteResult(domainSuiteResult);

                Assert.AreEqual(0, suiteId);
            }

            [TestMethod]
            public void SavesExisting()
            {
                var domainSuiteResult = new DomainSuiteResult { SuiteResultID = 1 };
                var dbSuiteResult = new DbSuiteResult { SuiteResultID = 1 };
                var dbSuiteResults = new List<DbSuiteResult> { dbSuiteResult }.ToDbSetMock();

                var zigNetEntitiesMock = new Mock<ZigNetEntities>();
                zigNetEntitiesMock.Setup(z => z.SuiteResults).Returns(dbSuiteResults.Object);

                var zigNetEntitiesWrapperMock = new Mock<IZigNetEntitiesWrapper>();
                zigNetEntitiesWrapperMock.Setup(z => z.Get()).Returns(zigNetEntitiesMock.Object);

                var suiteResultMapperMock = new Mock<ISuiteResultMapper>();

                var suiteResultService = new SuiteResultService(zigNetEntitiesWrapperMock.Object, suiteResultMapperMock.Object);
                var suiteId = suiteResultService.SaveSuiteResult(domainSuiteResult);

                Assert.AreEqual(1, suiteId);
            }
        }
    }
}
