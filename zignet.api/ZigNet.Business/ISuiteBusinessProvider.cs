namespace ZigNet.Business
{
    public interface ISuiteBusinessProvider
    {
        int StartSuite(int suiteId);
        int StartSuite(string applicationName, string suiteName, string environmentName);
    }
}
