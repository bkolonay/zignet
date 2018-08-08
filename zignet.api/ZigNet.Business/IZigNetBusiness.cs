using ZigNet.Domain.Suite;

namespace ZigNet.Business
{
    public interface IZigNetBusiness
    {
        int CreateSuite(Suite suite);
        void AddSuiteCategory(int suiteId, string suiteCategoryName);
        void DeleteSuiteCategory(int suiteId, string suiteCategoryName);
    }
}
