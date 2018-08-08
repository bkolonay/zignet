using System.Collections.Generic;
using ZigNet.Domain.Suite;

namespace ZigNet.Database
{
    public interface IZigNetDatabase
    {
        IEnumerable<Suite> GetMappedSuites();
        IEnumerable<SuiteCategory> GetSuiteCategoriesForSuite(int suiteId);
        int SaveSuite(Suite suite);
    }
}
