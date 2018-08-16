using DbSuiteResult = ZigNet.Database.EntityFramework.SuiteResult;
using SuiteResult = ZigNet.Domain.Suite.SuiteResult;

namespace ZigNet.Services.EntityFramework.Mapping
{
    public interface ISuiteResultMapper
    {
        DbSuiteResult Map(SuiteResult suiteResult);
        SuiteResult Map(DbSuiteResult dbSuiteResult);
        DbSuiteResult Map(DbSuiteResult dbSuiteResult, SuiteResult suiteResult);
    }
}
