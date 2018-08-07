using ZigNet.Database.DTOs;

namespace ZigNet.Services
{
    public interface ISuiteService
    {
        int GetId(string applicationName, string suiteName, string environmentName);
        SuiteName GetName(int suiteId);
        SuiteName GetNameGrouped(int suiteId);
    }
}
