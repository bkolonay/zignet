using ZigNet.Domain.Test;

namespace ZigNet.Services.EntityFramework.Mapping
{
    public interface ITestResultMapper
    {
        int Map(TestResultType testResultType);
    }
}
