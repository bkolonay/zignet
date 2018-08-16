using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using ZigNet.Database.EntityFramework;
using ZigNet.Services.DTOs;
using ZigNet.Services.EntityFramework.Tests.Helpers;
using DbTestFailureDuration = ZigNet.Database.EntityFramework.TestFailureDuration;
using TestResultType = ZigNet.Domain.Test.TestResultType;

namespace ZigNet.Services.EntityFramework.Tests
{
    public class TestFailureDurationServiceTests
    {
        [TestClass]
        public class Save
        {
            [TestMethod]
            public void IgnoresIfAlwaysPassed()
            {
                var utcNow = DateTime.UtcNow;

                var mockTestFailureDurations = new List<DbTestFailureDuration>().ToDbSetMock();

                var mockContext = new Mock<ZigNetEntities>();
                mockContext.Setup(m => m.TestFailureDurations).Returns(mockTestFailureDurations.Object);

                var mockZignetEntitiesWrapper = new Mock<IDbContext>();
                mockZignetEntitiesWrapper.Setup(z => z.Get()).Returns(mockContext.Object);

                var testFailureDurationDto = new TestFailureDurationDto
                {
                    SuiteId = 2,
                    TestId = 1,
                    TestResultId = 3
                };

                var testFailureDurationService = new TestFailureDurationService(mockZignetEntitiesWrapper.Object);
                var savedTestFailureDurationDto = testFailureDurationService.Save(testFailureDurationDto, TestResultType.Pass, utcNow);

                Assert.IsNull(savedTestFailureDurationDto);
            }

            [TestMethod]
            public void IgnoresTestFailedDurationIfPassedBefore()
            {
                var utcNow = DateTime.UtcNow;

                var mockTestFailureDurations = new List<DbTestFailureDuration>
                {
                    new DbTestFailureDuration {
                        TestFailureDurationID = 4,
                        SuiteId = 2,
                        TestId = 1,
                        TestResultId = 3,
                        FailureStartDateTime = utcNow.AddHours(-10),
                        FailureEndDateTime = utcNow.AddHours(-9)
                    }
                }.ToDbSetMock();

                var mockContext = new Mock<ZigNetEntities>();
                mockContext.Setup(m => m.TestFailureDurations).Returns(mockTestFailureDurations.Object);

                var mockZignetEntitiesWrapper = new Mock<IDbContext>();
                mockZignetEntitiesWrapper.Setup(z => z.Get()).Returns(mockContext.Object);

                var testFailureDurationDto = new TestFailureDurationDto
                {
                    SuiteId = 2,
                    TestId = 1,
                    TestResultId = 3
                };

                var testFailureDurationService = new TestFailureDurationService(mockZignetEntitiesWrapper.Object);
                var savedTestFailureDurationDto = testFailureDurationService.Save(testFailureDurationDto, TestResultType.Pass, utcNow);

                Assert.IsNull(savedTestFailureDurationDto);
            }

            [TestMethod]
            public void UpdatesTestFailedDurationEndTimeWhenNewlyPassing()
            {
                var utcNow = DateTime.UtcNow;

                var mockTestFailureDurations = new List<DbTestFailureDuration>
                {
                    new DbTestFailureDuration {
                        TestFailureDurationID = 4,
                        SuiteId = 2,
                        TestId = 1,
                        TestResultId = 3,
                        FailureStartDateTime = utcNow.AddHours(-10)
                    }
                }.ToDbSetMock();

                var mockContext = new Mock<ZigNetEntities>();
                mockContext.Setup(m => m.TestFailureDurations).Returns(mockTestFailureDurations.Object);

                var mockZignetEntitiesWrapper = new Mock<IDbContext>();
                mockZignetEntitiesWrapper.Setup(z => z.Get()).Returns(mockContext.Object);

                var testFailureDurationDto = new TestFailureDurationDto
                {
                    SuiteId = 2,
                    TestId = 1,
                    TestResultId = 5
                };

                var testFailureDurationService = new TestFailureDurationService(mockZignetEntitiesWrapper.Object);
                var savedTestFailureDurationDto = testFailureDurationService.Save(testFailureDurationDto, TestResultType.Pass, utcNow);

                Assert.AreEqual(4, savedTestFailureDurationDto.TestFailureDurationID);
                Assert.AreEqual(2, savedTestFailureDurationDto.SuiteId);
                Assert.AreEqual(1, savedTestFailureDurationDto.TestId);
                Assert.AreEqual(5, savedTestFailureDurationDto.TestResultId);
                Assert.AreEqual(utcNow.AddHours(-10), savedTestFailureDurationDto.FailureStart);
                Assert.AreEqual(utcNow, savedTestFailureDurationDto.FailureEnd);
            }

            [TestMethod]
            public void CreatesTestFailedDurationIfFirstFailure()
            {
                var utcNow = DateTime.UtcNow;

                var mockTestFailureDurations = new List<DbTestFailureDuration>().ToDbSetMock();

                var mockContext = new Mock<ZigNetEntities>();
                mockContext.Setup(m => m.TestFailureDurations).Returns(mockTestFailureDurations.Object);

                var mockZignetEntitiesWrapper = new Mock<IDbContext>();
                mockZignetEntitiesWrapper.Setup(z => z.Get()).Returns(mockContext.Object);

                var testFailureDurationDto = new TestFailureDurationDto
                {
                    SuiteId = 2,
                    TestId = 1,
                    TestResultId = 3
                };

                var testFailureDurationService = new TestFailureDurationService(mockZignetEntitiesWrapper.Object);
                var savedTestFailureDurationDto = testFailureDurationService.Save(testFailureDurationDto, TestResultType.Fail, utcNow);

                Assert.AreEqual(0, savedTestFailureDurationDto.TestFailureDurationID);
                Assert.AreEqual(2, savedTestFailureDurationDto.SuiteId);
                Assert.AreEqual(1, savedTestFailureDurationDto.TestId);
                Assert.AreEqual(3, savedTestFailureDurationDto.TestResultId);
                Assert.AreEqual(utcNow, savedTestFailureDurationDto.FailureStart);
                Assert.AreEqual(null, savedTestFailureDurationDto.FailureEnd);
            }

            [TestMethod]
            public void CreatesTestFailedDurationIfNewFailure()
            {
                var utcNow = DateTime.UtcNow;

                var mockTestFailureDurations = new List<DbTestFailureDuration>
                {
                    new DbTestFailureDuration {
                        TestFailureDurationID = 4,
                        SuiteId = 2,
                        TestId = 1,
                        TestResultId = 3,
                        FailureStartDateTime = utcNow.AddHours(-10),
                        FailureEndDateTime = utcNow.AddHours(-9)
                    }
                }.ToDbSetMock();

                var mockContext = new Mock<ZigNetEntities>();
                mockContext.Setup(m => m.TestFailureDurations).Returns(mockTestFailureDurations.Object);

                var mockZignetEntitiesWrapper = new Mock<IDbContext>();
                mockZignetEntitiesWrapper.Setup(z => z.Get()).Returns(mockContext.Object);

                var testFailureDurationDto = new TestFailureDurationDto
                {
                    SuiteId = 2,
                    TestId = 1,
                    TestResultId = 3
                };

                var testFailureDurationService = new TestFailureDurationService(mockZignetEntitiesWrapper.Object);
                var savedTestFailureDurationDto = testFailureDurationService.Save(testFailureDurationDto, TestResultType.Fail, utcNow);

                Assert.AreEqual(0, savedTestFailureDurationDto.TestFailureDurationID);
                Assert.AreEqual(2, savedTestFailureDurationDto.SuiteId);
                Assert.AreEqual(1, savedTestFailureDurationDto.TestId);
                Assert.AreEqual(3, savedTestFailureDurationDto.TestResultId);
                Assert.AreEqual(utcNow, savedTestFailureDurationDto.FailureStart);
                Assert.AreEqual(null, savedTestFailureDurationDto.FailureEnd);
            }

            [TestMethod]
            public void IgnoresTestFailedDurationIfSecondFailure()
            {
                var utcNow = DateTime.UtcNow;

                var mockTestFailureDurations = new List<DbTestFailureDuration>
                {
                    new DbTestFailureDuration {
                        TestFailureDurationID = 4,
                        SuiteId = 2,
                        TestId = 1,
                        TestResultId = 3,
                        FailureStartDateTime = utcNow.AddHours(-1)
                    }
                }.ToDbSetMock();

                var mockContext = new Mock<ZigNetEntities>();
                mockContext.Setup(m => m.TestFailureDurations).Returns(mockTestFailureDurations.Object);

                var mockZignetEntitiesWrapper = new Mock<IDbContext>();
                mockZignetEntitiesWrapper.Setup(z => z.Get()).Returns(mockContext.Object);

                var testFailureDurationDto = new TestFailureDurationDto
                {
                    SuiteId = 2,
                    TestId = 1,
                    TestResultId = 3
                };

                var testFailureDurationService = new TestFailureDurationService(mockZignetEntitiesWrapper.Object);
                var savedTestFailureDurationDto = testFailureDurationService.Save(testFailureDurationDto, TestResultType.Fail, utcNow);

                Assert.IsNull(savedTestFailureDurationDto);
            }

            [TestMethod]
            public void UpdatesLatestTestFailureDurationRecord()
            {
                var utcNow = DateTime.UtcNow;

                var mockTestFailureDurations = new List<DbTestFailureDuration>
                {
                    new DbTestFailureDuration {
                        TestFailureDurationID = 5,
                        SuiteId = 2,
                        TestId = 1,
                        TestResultId = 8,
                        FailureStartDateTime = utcNow.AddHours(-10)
                    },
                    new DbTestFailureDuration {
                        TestFailureDurationID = 6,
                        SuiteId = 2,
                        TestId = 1,
                        TestResultId = 9,
                        FailureStartDateTime = utcNow.AddHours(-1)
                    },
                    new DbTestFailureDuration {
                        TestFailureDurationID = 4,
                        SuiteId = 2,
                        TestId = 1,
                        TestResultId = 7,
                        FailureStartDateTime = utcNow.AddHours(-5)
                    }
                }.ToDbSetMock();

                var mockContext = new Mock<ZigNetEntities>();
                mockContext.Setup(m => m.TestFailureDurations).Returns(mockTestFailureDurations.Object);

                var mockZignetEntitiesWrapper = new Mock<IDbContext>();
                mockZignetEntitiesWrapper.Setup(z => z.Get()).Returns(mockContext.Object);

                var testFailureDurationDto = new TestFailureDurationDto
                {
                    SuiteId = 2,
                    TestId = 1,
                    TestResultId = 10
                };

                var testFailureDurationService = new TestFailureDurationService(mockZignetEntitiesWrapper.Object);
                var savedTestFailureDurationDto = testFailureDurationService.Save(testFailureDurationDto, TestResultType.Pass, utcNow);

                Assert.AreEqual(6, savedTestFailureDurationDto.TestFailureDurationID);
                Assert.AreEqual(2, savedTestFailureDurationDto.SuiteId);
                Assert.AreEqual(1, savedTestFailureDurationDto.TestId);
                Assert.AreEqual(10, savedTestFailureDurationDto.TestResultId);
                Assert.AreEqual(utcNow.AddHours(-1), savedTestFailureDurationDto.FailureStart);
                Assert.AreEqual(utcNow, savedTestFailureDurationDto.FailureEnd);
            }

            [TestMethod]
            public void IgnoresTestFailureDurationWithoutSuiteId()
            {
                var utcNow = DateTime.UtcNow;

                var mockTestFailureDurations = new List<DbTestFailureDuration>
                {
                    new DbTestFailureDuration {
                        TestFailureDurationID = 4,
                        SuiteId = 99,
                        TestId = 1,
                        TestResultId = 6,
                        FailureStartDateTime = utcNow.AddHours(-1)
                    },
                    new DbTestFailureDuration {
                        TestFailureDurationID = 5,
                        SuiteId = 2,
                        TestId = 1,
                        TestResultId = 7,
                        FailureStartDateTime = utcNow.AddHours(-10)
                    }
                }.ToDbSetMock();

                var mockContext = new Mock<ZigNetEntities>();
                mockContext.Setup(m => m.TestFailureDurations).Returns(mockTestFailureDurations.Object);

                var mockZignetEntitiesWrapper = new Mock<IDbContext>();
                mockZignetEntitiesWrapper.Setup(z => z.Get()).Returns(mockContext.Object);

                var testFailureDurationDto = new TestFailureDurationDto
                {
                    SuiteId = 2,
                    TestId = 1,
                    TestResultId = 8
                };

                var testFailureDurationService = new TestFailureDurationService(mockZignetEntitiesWrapper.Object);
                var savedTestFailureDurationDto = testFailureDurationService.Save(testFailureDurationDto, TestResultType.Pass, utcNow);

                Assert.AreEqual(5, savedTestFailureDurationDto.TestFailureDurationID);
                Assert.AreEqual(2, savedTestFailureDurationDto.SuiteId);
                Assert.AreEqual(1, savedTestFailureDurationDto.TestId);
                Assert.AreEqual(8, savedTestFailureDurationDto.TestResultId);
                Assert.AreEqual(utcNow.AddHours(-10), savedTestFailureDurationDto.FailureStart);
                Assert.AreEqual(utcNow, savedTestFailureDurationDto.FailureEnd);
            }

            [TestMethod]
            public void IgnoresTestFailureDurationWithoutTestId()
            {
                var utcNow = DateTime.UtcNow;

                var mockTestFailureDurations = new List<DbTestFailureDuration>
                {
                    new DbTestFailureDuration {
                        TestFailureDurationID = 4,
                        SuiteId = 2,
                        TestId = 99,
                        TestResultId = 6,
                        FailureStartDateTime = utcNow.AddHours(-1)
                    },
                    new DbTestFailureDuration {
                        TestFailureDurationID = 5,
                        SuiteId = 2,
                        TestId = 1,
                        TestResultId = 7,
                        FailureStartDateTime = utcNow.AddHours(-10)
                    }
                }.ToDbSetMock();

                var mockContext = new Mock<ZigNetEntities>();
                mockContext.Setup(m => m.TestFailureDurations).Returns(mockTestFailureDurations.Object);

                var mockZignetEntitiesWrapper = new Mock<IDbContext>();
                mockZignetEntitiesWrapper.Setup(z => z.Get()).Returns(mockContext.Object);

                var testFailureDurationDto = new TestFailureDurationDto
                {
                    SuiteId = 2,
                    TestId = 1,
                    TestResultId = 8
                };

                var testFailureDurationService = new TestFailureDurationService(mockZignetEntitiesWrapper.Object);
                var savedTestFailureDurationDto = testFailureDurationService.Save(testFailureDurationDto, TestResultType.Pass, utcNow);

                Assert.AreEqual(5, savedTestFailureDurationDto.TestFailureDurationID);
                Assert.AreEqual(2, savedTestFailureDurationDto.SuiteId);
                Assert.AreEqual(1, savedTestFailureDurationDto.TestId);
                Assert.AreEqual(8, savedTestFailureDurationDto.TestResultId);
                Assert.AreEqual(utcNow.AddHours(-10), savedTestFailureDurationDto.FailureStart);
                Assert.AreEqual(utcNow, savedTestFailureDurationDto.FailureEnd);
            }
        }
    }
}
