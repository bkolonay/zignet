using System;
using System.Linq;
using ZigNet.Database;
using ZigNet.Domain.Suite;

namespace ZigNet.Business
{
    public class ZigNetBusiness : IZigNetBusiness
    {
        private IZigNetDatabase _zignetDatabase;

        public ZigNetBusiness(IZigNetDatabase zigNetDatabase)
        {
            _zignetDatabase = zigNetDatabase;
        }

        public int CreateSuite(Suite suite)
        {
            if (_zignetDatabase.GetMappedSuites().Any(s => s.Name == suite.Name))
                throw new InvalidOperationException("Suite with name already exists: " + suite.Name);

            return _zignetDatabase.SaveSuite(suite);
        }
        public void AddSuiteCategory(int suiteId, string suiteCategoryName)
        {
            if (string.IsNullOrWhiteSpace(suiteCategoryName))
                throw new InvalidOperationException("Suite category name cannot be null or empty string");

            var currentSuiteCategories = _zignetDatabase.GetSuiteCategoriesForSuite(suiteId);
            if (currentSuiteCategories.Any(sc => sc.Name == suiteCategoryName))
                return;

            var suite = _zignetDatabase.GetMappedSuites().Single(s => s.SuiteID == suiteId);
            suite.Categories.Add(new SuiteCategory { Name = suiteCategoryName });
            _zignetDatabase.SaveSuite(suite);
        }
        public void DeleteSuiteCategory(int suiteId, string suiteCategoryName)
        {
            if (string.IsNullOrWhiteSpace(suiteCategoryName))
                throw new InvalidOperationException("Suite category name cannot be null or empty string");

            var currentSuiteCategories = _zignetDatabase.GetSuiteCategoriesForSuite(suiteId);
            if (!currentSuiteCategories.Any(sc => sc.Name == suiteCategoryName))
                return;

            var suite = _zignetDatabase.GetMappedSuites().Single(s => s.SuiteID == suiteId);
            suite.Categories.Remove(suite.Categories.Single(sc => sc.Name == suiteCategoryName));
            _zignetDatabase.SaveSuite(suite);
        }
    }
}
