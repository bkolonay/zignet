namespace ZigNet.Services
{
    public interface ISuiteService
    {
        int GetSuiteId(string applicationName, string suiteName, string environmentName);
    }
}
