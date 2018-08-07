using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ZigNet.Database;
using ZigNet.Domain.Suite;
using System.Collections.Generic;
using ZigNet.Domain.Test;
using ZigNetTestResult = ZigNet.Domain.Test.TestResult;
using ZigNet.Database.DTOs;

namespace ZigNet.Business.Tests
{
    public class ZigNetBusinessTests
    {
        [TestClass]
        public class CreateSuiteMethod
        {
            [TestMethod]
            public void CreatesSuite()
            {
                var zignetDatabase = new Mock<IZigNetDatabase>();
                zignetDatabase.Setup(zd => zd.SaveSuite(It.IsAny<Suite>())).Returns(1);

                var zigNetBusiness = new ZigNetBusiness(zignetDatabase.Object);
                var suiteId = zigNetBusiness.CreateSuite(new Suite());

                Assert.AreEqual(1, suiteId);
            }

            [TestMethod]
            [ExpectedException(typeof(InvalidOperationException))]
            public void ThrowsWhenSuiteHasSameName()
            {
                var zignetDatabase = new Mock<IZigNetDatabase>();
                zignetDatabase.Setup(zd => zd.GetMappedSuites()).Returns(new List<Suite> { new Suite { Name = "suite 1" } });

                var suite = new Suite
                {
                    Name = "suite 1"
                };

                var zigNetBusiness = new ZigNetBusiness(zignetDatabase.Object);
                var suiteId = zigNetBusiness.CreateSuite(suite);
            }
        }

        [TestClass]
        public class AddSuiteCategoryMethod
        {
            [TestMethod]
            public void DoesNotThrowWhenSuiteCategoryAlreadyAssignedToSuite()
            {
                var zignetDatabase = new Mock<IZigNetDatabase>();
                zignetDatabase.Setup(zd => zd.GetSuiteCategoriesForSuite(1)).Returns(new List<SuiteCategory> {
                    new SuiteCategory { Name = "suite category 1" }
                });

                var zigNetBusiness = new ZigNetBusiness(zignetDatabase.Object);
                zigNetBusiness.AddSuiteCategory(1, "suite category 1");
            }

            [TestMethod]
            public void DoesNotThrowWhenSuiteCategoryAssignedToSuite()
            {
                var zignetDatabase = new Mock<IZigNetDatabase>();
                zignetDatabase.Setup(zd => zd.GetMappedSuites()).Returns(new List<Suite> {
                    new Suite { SuiteID = 1, Categories = new List<SuiteCategory>() }
                });

                var zigNetBusiness = new ZigNetBusiness(zignetDatabase.Object);
                zigNetBusiness.AddSuiteCategory(1, "suite category 2");
            }

            [TestMethod]
            [ExpectedException(typeof(InvalidOperationException))]
            public void ThrowsWhenSuiteIdDoesNotExist()
            {
                var zignetDatabase = new Mock<IZigNetDatabase>();

                var zigNetBusiness = new ZigNetBusiness(zignetDatabase.Object);
                zigNetBusiness.AddSuiteCategory(99, "suite category 1");
            }

            [TestMethod]
            [ExpectedException(typeof(InvalidOperationException))]
            public void ThrowsWhenSuiteCategoryNull()
            {
                var zignetDatabase = new Mock<IZigNetDatabase>();

                var zigNetBusiness = new ZigNetBusiness(zignetDatabase.Object);
                zigNetBusiness.AddSuiteCategory(1, null);
            }
        }

        [TestClass]
        public class DeleteSuiteCategoryMethod
        {
            [TestMethod]
            public void DoesNotThrowWhenSuiteCategoryNotAssignedToSuite()
            {
                var zignetDatabase = new Mock<IZigNetDatabase>();
                zignetDatabase.Setup(zd => zd.GetSuiteCategoriesForSuite(1)).Returns(new List<SuiteCategory> {
                    new SuiteCategory { Name = "suite category 1" }
                });

                var zigNetBusiness = new ZigNetBusiness(zignetDatabase.Object);
                zigNetBusiness.DeleteSuiteCategory(1, "suite category 2");
            }

            [TestMethod]
            public void DoesNotThrowWhenSuiteCategoryDeletedFromSuite()
            {
                var zignetDatabase = new Mock<IZigNetDatabase>();
                zignetDatabase.Setup(zd => zd.GetSuiteCategoriesForSuite(1)).Returns(new List<SuiteCategory> {
                    new SuiteCategory { Name = "suite category 1" }
                });
                zignetDatabase.Setup(zd => zd.GetMappedSuites()).Returns(new List<Suite> {
                    new Suite {
                        SuiteID = 1,
                        Categories = new List<SuiteCategory> {
                            new SuiteCategory { Name = "suite category 1" }
                        }

                    }
                });

                var zigNetBusiness = new ZigNetBusiness(zignetDatabase.Object);
                zigNetBusiness.DeleteSuiteCategory(1, "suite category 1");
            }

            [TestMethod]
            [ExpectedException(typeof(InvalidOperationException))]
            public void ThrowsWhenSuiteIdDoesNotExist()
            {
                var zignetDatabase = new Mock<IZigNetDatabase>();
                zignetDatabase.Setup(zd => zd.GetSuiteCategoriesForSuite(99)).Returns(new List<SuiteCategory> {
                    new SuiteCategory { Name = "suite category 1" }
                });

                var zigNetBusiness = new ZigNetBusiness(zignetDatabase.Object);
                zigNetBusiness.DeleteSuiteCategory(99, "suite category 1");
            }

            [TestMethod]
            [ExpectedException(typeof(InvalidOperationException))]
            public void ThrowsWhenSuiteCategoryNull()
            {
                var zignetDatabase = new Mock<IZigNetDatabase>();

                var zigNetBusiness = new ZigNetBusiness(zignetDatabase.Object);
                zigNetBusiness.DeleteSuiteCategory(1, null);
            }
        }

        [TestClass]
        public class SaveTestResultMethod
        {
            [TestMethod]
            [ExpectedException(typeof(ArgumentNullException))]
            public void ThrowsWhenTestNameNull()
            {
                var zignetDatabase = new Mock<IZigNetDatabase>();

                var testResult = new ZigNetTestResult { Test = new Test() };

                var zigNetBusiness = new ZigNetBusiness(zignetDatabase.Object);
                zigNetBusiness.SaveTestResult(testResult);
            }
        }

        [TestClass]
        public class GetLatestTestResultsMethod
        {
            [TestMethod]
            public void MapsLatestTestResults()
            {
                var utcNow = DateTime.UtcNow;

                var zignetDatabase = new Mock<IZigNetDatabase>();
                zignetDatabase.Setup(zdm => zdm.GetLatestTestResults(1, false)).Returns(
                    new List<LatestTestResult>{ 
                        new LatestTestResult { TestName = "test1", FailingFromDate = utcNow, TestResultID = 2 } 
                    }
                );

                var zigNetBusiness = new ZigNetBusiness(zignetDatabase.Object);
                var latestTestResults = zigNetBusiness.GetLatestTestResults(1, false).ToList();

                Assert.AreEqual(1, latestTestResults.Count);
                Assert.AreEqual("test1", latestTestResults[0].TestName);
                Assert.AreEqual(utcNow, latestTestResults[0].FailingFromDate);
                Assert.AreEqual(2, latestTestResults[0].TestResultID);
                Assert.IsNull(latestTestResults[0].PassingFromDate);
            }

            [TestMethod]
            public void DoesNotThrowWhenZeroLatestTestResults()
            {
                var utcNow = DateTime.UtcNow;

                var zignetDatabase = new Mock<IZigNetDatabase>();
                zignetDatabase.Setup(zdm => zdm.GetLatestTestResults(1, false)).Returns(new List<LatestTestResult>());

                var zigNetBusiness = new ZigNetBusiness(zignetDatabase.Object);
                var latestTestResults = zigNetBusiness.GetLatestTestResults(1, false).ToList();

                Assert.AreEqual(0, latestTestResults.Count);
            }
        }
    }
}
