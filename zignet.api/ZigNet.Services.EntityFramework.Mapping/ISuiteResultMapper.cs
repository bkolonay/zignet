using ZigNet.Database.EntityFramework;

namespace ZigNet.Services.EntityFramework.Mapping
{
    public interface ISuiteResultMapper
    {
        SuiteResult Map(Domain.Suite.SuiteResult suiteResult);
    }
}
