using System;
using System.Collections.Generic;
using System.Linq;
using ZigNetSuite = ZigNet.Domain.Suite.Suite;
using ZigNetSuiteCategory = ZigNet.Domain.Suite.SuiteCategory;
using ZigNetSuiteResultType = ZigNet.Domain.Suite.SuiteResultType;
using ZigNetTestResultType = ZigNet.Domain.Test.TestResultType;
using ZigNetTestFailureType = ZigNet.Domain.Test.TestFailureType;

namespace ZigNet.Database.EntityFramework
{
    public class ZigNetEntityFrameworkDatabase : IZigNetDatabase
    {
        private IZigNetEntitiesWriter _zigNetEntitiesWriter;
        private IZigNetEntitiesReadOnly _zigNetEntitiesReadOnly;

        public ZigNetEntityFrameworkDatabase(IZigNetEntitiesWriter zigNetEntitiesWriter, IZigNetEntitiesReadOnly zigNetEntitiesReadOnly)
        {
            _zigNetEntitiesWriter = zigNetEntitiesWriter;
            _zigNetEntitiesReadOnly = zigNetEntitiesReadOnly;
        }

        public IEnumerable<ZigNetSuite> GetMappedSuites()
        {
            var databaseSuites = _zigNetEntitiesWriter.GetSuites();

            var suites = new List<ZigNetSuite>();
            foreach (var databaseSuite in databaseSuites)
                suites.Add(MapDatabaseSuite(databaseSuite));

            return suites;
        }
        public IEnumerable<ZigNetSuiteCategory> GetSuiteCategoriesForSuite(int suiteId)
        {
            return MapDatabaseSuiteCategories(_zigNetEntitiesWriter.GetSuite(suiteId).SuiteCategories);
        }
        public int SaveSuite(ZigNetSuite suite)
        {
            Suite databaseSuite;
            if (suite.SuiteID == 0)
                databaseSuite = new Suite { SuiteName = suite.Name, SuiteCategories = new List<SuiteCategory>() };
            else
                databaseSuite = _zigNetEntitiesWriter.GetSuite(suite.SuiteID);

            databaseSuite.SuiteCategories.Clear();
            foreach (var suiteCategory in suite.Categories)
            {
                var existingDatabaseSuiteCategory = _zigNetEntitiesWriter.GetSuiteCategories().SingleOrDefault(sc => sc.CategoryName == suiteCategory.Name);
                if (existingDatabaseSuiteCategory != null)
                    databaseSuite.SuiteCategories.Add(existingDatabaseSuiteCategory);
                else
                    databaseSuite.SuiteCategories.Add(new SuiteCategory { CategoryName = suiteCategory.Name });
            }

            return _zigNetEntitiesWriter.SaveSuite(databaseSuite);
        }

        private TestFailureType GetTestFailureType(ZigNetTestFailureType zigNetTestFailureType)
        {
            switch (zigNetTestFailureType)
            {
                case ZigNetTestFailureType.Exception:
                    return _zigNetEntitiesWriter.GetTestFailureType(2);
                case ZigNetTestFailureType.Assertion:
                    return _zigNetEntitiesWriter.GetTestFailureType(1);
                default:
                    throw new InvalidOperationException("Test failure type not recognized");
            }
        }
        private int MapSuiteResultType(ZigNetSuiteResultType zigNetSuiteResultType)
        {
            switch (zigNetSuiteResultType)
            {
                case ZigNetSuiteResultType.Fail:
                    return 1;
                case ZigNetSuiteResultType.Inconclusive:
                    return 2;
                case ZigNetSuiteResultType.Pass:
                    return 3;
                default:
                    throw new InvalidOperationException("Suite result type not recognized");
            }
        }
        private int MapTestResultType(ZigNetTestResultType zigNetTestResultType)
        {
            switch (zigNetTestResultType)
            {
                case ZigNetTestResultType.Fail:
                    return 1;
                case ZigNetTestResultType.Inconclusive:
                    return 2;
                case ZigNetTestResultType.Pass:
                    return 3;
                default:
                    throw new InvalidOperationException("Test result type not recognized");
            }
        }

        private ICollection<ZigNetSuiteCategory> MapDatabaseSuiteCategories(Suite suite)
        {
            var databaseSuiteCategories = suite.SuiteCategories;

            var suiteCategories = new List<ZigNetSuiteCategory>();
            foreach (var databaseSuiteCategory in databaseSuiteCategories)
                suiteCategories.Add(MapDatabaseSuiteCategory(databaseSuiteCategory));

            return suiteCategories;
        }
        private ZigNetSuite MapDatabaseSuite(Suite databaseSuite)
        {
            return new ZigNetSuite
            {
                SuiteID = databaseSuite.SuiteID,
                Name = databaseSuite.SuiteName,
                Categories = MapDatabaseSuiteCategories(databaseSuite)
            };
        }
        private ZigNetSuiteCategory MapDatabaseSuiteCategory(SuiteCategory databaseSuiteCategory)
        {
            return new ZigNetSuiteCategory
            {
                SuiteCategoryID = databaseSuiteCategory.SuiteCategoryID,
                Name = databaseSuiteCategory.CategoryName
            };
        }
        private IEnumerable<ZigNetSuiteCategory> MapDatabaseSuiteCategories(IEnumerable<SuiteCategory> databaseSuiteCategories)
        {
            var suiteCategories = new List<ZigNetSuiteCategory>();
            foreach (var databaseSuiteCategory in databaseSuiteCategories)
                suiteCategories.Add(MapDatabaseSuiteCategory(databaseSuiteCategory));
            return suiteCategories;
        }
    }
}
