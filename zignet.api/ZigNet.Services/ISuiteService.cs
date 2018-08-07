namespace ZigNet.Services
{
    public interface ISuiteService
    {
        int GetId(string applicationName, string suiteName, string environmentName);
        string GetName(int suiteId);
        string GetNameGrouped(int suiteId);
    }
}
