using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using ZigNet.Database.EntityFramework;
using ZigNet.Services.EntityFramework.Tests.Helpers;

namespace ZigNet.Services.EntityFramework.Tests
{
    public class SuiteServiceTests
    {
        [TestClass]
        public class GetIdByName
        {
            [TestMethod]
            public void GetsWithName()
            {
                const string suiteName = "Services";
                const string appName = "LoopNet";
                const string envName = "TestMain";

                var suites = new List<Suite>
                {
                    new Suite {
                        SuiteName = suiteName,
                        Application = new Application { ApplicationName = appName },
                        Environment = new Database.EntityFramework.Environment { EnvironmentName = envName },
                        SuiteID = 1
                    }
                };
                var mockSuites = suites.ToDbSetMock();
                mockSuites.Setup(m => m.AsNoTracking()).Returns(mockSuites.Object);

                var mockContext = new Mock<ZigNetEntities>();
                mockContext.Setup(m => m.Suites).Returns(mockSuites.Object);

                var zignetEntitiesWrapperMock = new Mock<IZigNetEntitiesWrapper>();
                zignetEntitiesWrapperMock.Setup(z => z.Get()).Returns(mockContext.Object);

                var suiteService = new SuiteService(zignetEntitiesWrapperMock.Object);
                var suiteId = suiteService.GetId(appName, suiteName, envName);

                Assert.AreEqual(1, suiteId);
            }

            [TestMethod]
            [ExpectedException(typeof(InvalidOperationException))]
            public void ThrowsWithoutMatch()
            {
                const string suiteName = "Services";
                const string appName = "LoopNet";
                const string envName = "TestMain";

                var suites = new List<Suite>
                {
                    new Suite {
                        SuiteName = suiteName,
                        Application = new Application { ApplicationName = appName },
                        Environment = new Database.EntityFramework.Environment { EnvironmentName = envName },
                        SuiteID = 1
                    }
                };
                var mockSuites = suites.ToDbSetMock();
                mockSuites.Setup(m => m.AsNoTracking()).Returns(mockSuites.Object);

                var mockContext = new Mock<ZigNetEntities>();
                mockContext.Setup(m => m.Suites).Returns(mockSuites.Object);

                var zignetEntitiesWrapperMock = new Mock<IZigNetEntitiesWrapper>();
                zignetEntitiesWrapperMock.Setup(z => z.Get()).Returns(mockContext.Object);

                var suiteService = new SuiteService(zignetEntitiesWrapperMock.Object);
                suiteService.GetId(appName, suiteName, "bad-env");
            }
        }

        [TestClass]
        public class GetName
        {
            [TestMethod]
            public void GetsWithName()
            {
                const string suiteName = "Services";
                const string appNameAbbreviation = "LN";
                const string envNameAbbreviation = "TSM";

                var suites = new List<Suite>
                {
                    new Suite {
                        SuiteID = 1,
                        SuiteName = suiteName,
                        Application = new Application { ApplicationNameAbbreviation = appNameAbbreviation },
                        Environment = new Database.EntityFramework.Environment { EnvironmentNameAbbreviation = envNameAbbreviation }
                        
                    }
                };
                var mockSuites = suites.ToDbSetMock();
                mockSuites.Setup(m => m.AsNoTracking()).Returns(mockSuites.Object);
                mockSuites.Setup(m => m.Include(It.IsAny<string>())).Returns(mockSuites.Object);

                var mockContext = new Mock<ZigNetEntities>();
                mockContext.Setup(m => m.Suites).Returns(mockSuites.Object);

                var zignetEntitiesWrapperMock = new Mock<IZigNetEntitiesWrapper>();
                zignetEntitiesWrapperMock.Setup(z => z.Get()).Returns(mockContext.Object);

                var suiteService = new SuiteService(zignetEntitiesWrapperMock.Object);
                var actualSuiteName = suiteService.GetName(1);

                var expectedSuiteName = string.Format("{0} {1} ({2})", appNameAbbreviation, suiteName, envNameAbbreviation);
                Assert.AreEqual(expectedSuiteName, actualSuiteName);
            }

            [TestMethod]
            [ExpectedException(typeof(InvalidOperationException))]
            public void ThrowsWithoutMatch()
            {
                const string suiteName = "Services";
                const string appNameAbbreviation = "LN";
                const string envNameAbbreviation = "TSM";

                var suites = new List<Suite>
                {
                    new Suite {
                        SuiteID = 2,
                        SuiteName = suiteName,
                        Application = new Application { ApplicationNameAbbreviation = appNameAbbreviation },
                        Environment = new Database.EntityFramework.Environment { EnvironmentNameAbbreviation = envNameAbbreviation }

                    }
                };
                var mockSuites = suites.ToDbSetMock();
                mockSuites.Setup(m => m.AsNoTracking()).Returns(mockSuites.Object);
                mockSuites.Setup(m => m.Include(It.IsAny<string>())).Returns(mockSuites.Object);

                var mockContext = new Mock<ZigNetEntities>();
                mockContext.Setup(m => m.Suites).Returns(mockSuites.Object);

                var zignetEntitiesWrapperMock = new Mock<IZigNetEntitiesWrapper>();
                zignetEntitiesWrapperMock.Setup(z => z.Get()).Returns(mockContext.Object);

                var suiteService = new SuiteService(zignetEntitiesWrapperMock.Object);
                var actualSuiteName = suiteService.GetName(1);
            }
        }

        [TestClass]
        public class GetNameGrouped
        {
            [TestMethod]
            public void GetsWithName()
            {
                const string appNameAbbreviation = "LN";
                const string envNameAbbreviation = "TSM";

                var suites = new List<Suite>
                {
                    new Suite {
                        SuiteID = 1,
                        Application = new Application { ApplicationNameAbbreviation = appNameAbbreviation },
                        Environment = new Database.EntityFramework.Environment { EnvironmentNameAbbreviation = envNameAbbreviation }

                    }
                };
                var mockSuites = suites.ToDbSetMock();
                mockSuites.Setup(m => m.AsNoTracking()).Returns(mockSuites.Object);

                var mockContext = new Mock<ZigNetEntities>();
                mockContext.Setup(m => m.Suites).Returns(mockSuites.Object);

                var zignetEntitiesWrapperMock = new Mock<IZigNetEntitiesWrapper>();
                zignetEntitiesWrapperMock.Setup(z => z.Get()).Returns(mockContext.Object);

                var suiteService = new SuiteService(zignetEntitiesWrapperMock.Object);
                var actualSuiteName = suiteService.GetNameGrouped(1);

                var expectedSuiteName = string.Format("{0} {1}", appNameAbbreviation, envNameAbbreviation);
                Assert.AreEqual(expectedSuiteName, actualSuiteName);
            }

            [TestMethod]
            [ExpectedException(typeof(InvalidOperationException))]
            public void ThrowsWithoutMatch()
            {
                const string appNameAbbreviation = "LN";
                const string envNameAbbreviation = "TSM";

                var suites = new List<Suite>
                {
                    new Suite {
                        SuiteID = 2,
                        Application = new Application { ApplicationNameAbbreviation = appNameAbbreviation },
                        Environment = new Database.EntityFramework.Environment { EnvironmentNameAbbreviation = envNameAbbreviation }

                    }
                };
                var mockSuites = suites.ToDbSetMock();
                mockSuites.Setup(m => m.AsNoTracking()).Returns(mockSuites.Object);

                var mockContext = new Mock<ZigNetEntities>();
                mockContext.Setup(m => m.Suites).Returns(mockSuites.Object);

                var zignetEntitiesWrapperMock = new Mock<IZigNetEntitiesWrapper>();
                zignetEntitiesWrapperMock.Setup(z => z.Get()).Returns(mockContext.Object);

                var suiteService = new SuiteService(zignetEntitiesWrapperMock.Object);
                var actualSuiteName = suiteService.GetNameGrouped(1);
            }
        }
    }
}
