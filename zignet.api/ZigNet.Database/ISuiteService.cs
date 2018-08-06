namespace ZigNet.Database
{
    public interface ISuiteService
    {
        int GetSuiteId(string applicationName, string suiteName, string environmentName);
    }
}
