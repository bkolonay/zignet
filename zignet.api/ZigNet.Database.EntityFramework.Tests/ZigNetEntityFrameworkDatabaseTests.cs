using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Linq;
using ZigNetSuite = ZigNet.Domain.Suite.Suite;
using ZigNetSuiteCategory = ZigNet.Domain.Suite.SuiteCategory;

namespace ZigNet.Database.EntityFramework.Tests
{
    public class ZigNetEntityFrameworkDatabaseTests
    {
        [TestClass]
        public class SaveSuiteMethod
        {
            [TestMethod]
            public void SavesNewSuiteWithNoCategories()
            {
                var zigNetEntitiesWriterMock = new Mock<IZigNetEntitiesWriter>();
                zigNetEntitiesWriterMock.Setup(zewm => zewm.SaveSuite(It.IsAny<Suite>())).Returns(1);
                var zigNetEntitiesReadOnlyMock = new Mock<IZigNetEntitiesReadOnly>();

                var suite = new ZigNetSuite { Name = "new suite", Categories = new List<ZigNetSuiteCategory>() };

                var zigNetEntityFrameworkDatabase = new ZigNetEntityFrameworkDatabase(zigNetEntitiesWriterMock.Object, zigNetEntitiesReadOnlyMock.Object);
                var suiteId = zigNetEntityFrameworkDatabase.SaveSuite(suite);

                Assert.AreEqual(1, suiteId);
            }

            [TestMethod]
            public void SavesNewSuiteWithEmptyCategoryList()
            {
                var zigNetEntitiesWriterMock = new Mock<IZigNetEntitiesWriter>();
                zigNetEntitiesWriterMock.Setup(zewm => zewm.SaveSuite(It.IsAny<Suite>())).Returns(1);
                var zigNetEntitiesReadOnlyMock = new Mock<IZigNetEntitiesReadOnly>();

                var suite = new ZigNetSuite { Name = "new suite", Categories = new List<ZigNetSuiteCategory>() };

                var zigNetEntityFrameworkDatabase = new ZigNetEntityFrameworkDatabase(zigNetEntitiesWriterMock.Object, zigNetEntitiesReadOnlyMock.Object);
                var suiteId = zigNetEntityFrameworkDatabase.SaveSuite(suite);

                Assert.AreEqual(1, suiteId);
            }

            [TestMethod]
            public void SavesNewSuiteWithOnlyNewCategories()
            {
                var zigNetEntitiesWriterMock = new Mock<IZigNetEntitiesWriter>();
                zigNetEntitiesWriterMock.Setup(zewm => zewm.SaveSuite(It.IsAny<Suite>())).Returns(1);
                zigNetEntitiesWriterMock.Setup(zewm => zewm.GetSuiteCategories()).Returns(new List<SuiteCategory>().AsQueryable());
                var zigNetEntitiesReadOnlyMock = new Mock<IZigNetEntitiesReadOnly>();

                var suite = new ZigNetSuite
                {
                    Name = "new suite",
                    Categories = new List<ZigNetSuiteCategory> {
                        new ZigNetSuiteCategory { Name = "suite category 1" }
                    }
                };

                var zigNetEntityFrameworkDatabase = new ZigNetEntityFrameworkDatabase(zigNetEntitiesWriterMock.Object, zigNetEntitiesReadOnlyMock.Object);
                var suiteId = zigNetEntityFrameworkDatabase.SaveSuite(suite);

                Assert.AreEqual(1, suiteId);
            }

            [TestMethod]
            public void SavesNewSuiteWithOnlyExistingCategories()
            {
                var zigNetEntitiesWriterMock = new Mock<IZigNetEntitiesWriter>();
                zigNetEntitiesWriterMock.Setup(zewm => zewm.SaveSuite(It.IsAny<Suite>())).Returns(1);
                zigNetEntitiesWriterMock.Setup(zewm => zewm.GetSuiteCategories()).Returns(new List<SuiteCategory> {
                    new SuiteCategory { CategoryName = "suite category 1" }
                }.AsQueryable());
                var zigNetEntitiesReadOnlyMock = new Mock<IZigNetEntitiesReadOnly>();

                var suite = new ZigNetSuite
                {
                    Name = "new suite",
                    Categories = new List<ZigNetSuiteCategory> {
                        new ZigNetSuiteCategory { Name = "suite category 1" }
                    }
                };

                var zigNetEntityFrameworkDatabase = new ZigNetEntityFrameworkDatabase(zigNetEntitiesWriterMock.Object, zigNetEntitiesReadOnlyMock.Object);
                var suiteId = zigNetEntityFrameworkDatabase.SaveSuite(suite);

                Assert.AreEqual(1, suiteId);
            }

            [TestMethod]
            public void SavesNewSuiteWithNewAndExistingCategories()
            {
                var zigNetEntitiesWriterMock = new Mock<IZigNetEntitiesWriter>();
                zigNetEntitiesWriterMock.Setup(zewm => zewm.SaveSuite(It.IsAny<Suite>())).Returns(1);
                zigNetEntitiesWriterMock.Setup(zewm => zewm.GetSuiteCategories()).Returns(new List<SuiteCategory> {
                    new SuiteCategory { CategoryName = "suite category 1" },
                    new SuiteCategory { CategoryName = "suite category 2" },
                }.AsQueryable());
                var zigNetEntitiesReadOnlyMock = new Mock<IZigNetEntitiesReadOnly>();

                var suite = new ZigNetSuite
                {
                    Name = "new suite",
                    Categories = new List<ZigNetSuiteCategory> {
                        new ZigNetSuiteCategory { Name = "suite category 1" },
                        new ZigNetSuiteCategory { Name = "suite category 3" }
                    }
                };

                var zigNetEntityFrameworkDatabase = new ZigNetEntityFrameworkDatabase(zigNetEntitiesWriterMock.Object, zigNetEntitiesReadOnlyMock.Object);
                var suiteId = zigNetEntityFrameworkDatabase.SaveSuite(suite);

                Assert.AreEqual(1, suiteId);
            }

            [TestMethod]
            public void SavesExistingSuiteWithExistingCategory()
            {
                var zigNetEntitiesWriterMock = new Mock<IZigNetEntitiesWriter>();
                zigNetEntitiesWriterMock.Setup(zewm => zewm.GetSuite(1)).Returns(new Suite { SuiteID = 1 });
                zigNetEntitiesWriterMock.Setup(zewm => zewm.GetSuiteCategories()).Returns(new List<SuiteCategory> {
                    new SuiteCategory { SuiteCategoryID = 1, CategoryName = "suite category 1" }
                }.AsQueryable());
                zigNetEntitiesWriterMock.Setup(zewm => zewm.SaveSuite(It.IsAny<Suite>())).Returns(1);
                var zigNetEntitiesReadOnlyMock = new Mock<IZigNetEntitiesReadOnly>();

                var suite = new ZigNetSuite
                {
                    SuiteID = 1,
                    Name = "existing suite",
                    Categories = new List<ZigNetSuiteCategory> {
                        new ZigNetSuiteCategory { Name = "suite category 1" }
                    }
                };

                var zigNetEntityFrameworkDatabase = new ZigNetEntityFrameworkDatabase(zigNetEntitiesWriterMock.Object, zigNetEntitiesReadOnlyMock.Object);
                var suiteId = zigNetEntityFrameworkDatabase.SaveSuite(suite);

                Assert.AreEqual(1, suiteId);
            }

            [TestMethod]
            public void SavesExistingSuiteWithNewCategory()
            {
                var zigNetEntitiesWriterMock = new Mock<IZigNetEntitiesWriter>();
                zigNetEntitiesWriterMock.Setup(zewm => zewm.GetSuite(1)).Returns(new Suite { SuiteID = 1 });
                zigNetEntitiesWriterMock.Setup(zewm => zewm.GetSuiteCategories()).Returns(new List<SuiteCategory> {
                    new SuiteCategory { SuiteCategoryID = 1, CategoryName = "suite category 1" }
                }.AsQueryable());
                zigNetEntitiesWriterMock.Setup(zewm => zewm.SaveSuite(It.IsAny<Suite>())).Returns(1);
                var zigNetEntitiesReadOnlyMock = new Mock<IZigNetEntitiesReadOnly>();

                var suite = new ZigNetSuite
                {
                    SuiteID = 1,
                    Name = "existing suite",
                    Categories = new List<ZigNetSuiteCategory> {
                        new ZigNetSuiteCategory { Name = "suite category 2" }
                    }
                };

                var zigNetEntityFrameworkDatabase = new ZigNetEntityFrameworkDatabase(zigNetEntitiesWriterMock.Object, zigNetEntitiesReadOnlyMock.Object);
                var suiteId = zigNetEntityFrameworkDatabase.SaveSuite(suite);

                Assert.AreEqual(1, suiteId);
            }
        }
    }
}
