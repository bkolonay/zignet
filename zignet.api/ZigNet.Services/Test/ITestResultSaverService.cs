using ZigNet.Domain.Test;

namespace ZigNet.Services
{
    public interface ITestResultSaverService
    {
        TestResult Save(TestResult testResult);
    }
}
