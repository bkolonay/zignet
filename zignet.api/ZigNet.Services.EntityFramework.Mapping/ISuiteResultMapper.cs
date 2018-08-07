using DbSuiteResult = ZigNet.Database.EntityFramework.SuiteResult;
using DomainSuiteResult = ZigNet.Domain.Suite.SuiteResult;

namespace ZigNet.Services.EntityFramework.Mapping
{
    public interface ISuiteResultMapper
    {
        DbSuiteResult Map(DomainSuiteResult domainSuiteResult);
        DomainSuiteResult Map(DbSuiteResult dbSuiteResult);
        DbSuiteResult Map(DbSuiteResult dbSuiteResult, DomainSuiteResult domainSuiteResult);
    }
}
