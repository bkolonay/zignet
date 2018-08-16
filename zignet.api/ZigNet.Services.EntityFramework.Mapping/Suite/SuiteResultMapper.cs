using System;
using DbSuiteResult = ZigNet.Database.EntityFramework.SuiteResult;
using DomainSuiteResult = ZigNet.Domain.Suite.SuiteResult;
using DomainSuiteResultType = ZigNet.Domain.Suite.SuiteResultType;

namespace ZigNet.Services.EntityFramework.Mapping
{
    public class SuiteResultMapper : ISuiteResultMapper
    {
        public DomainSuiteResult Map(DbSuiteResult dbSuiteResult)
        {
            return new DomainSuiteResult
            {
                SuiteResultID = dbSuiteResult.SuiteResultID
            };
        }

        public DbSuiteResult Map(DomainSuiteResult domainSuiteResult)
        {
            return new DbSuiteResult
            {
                SuiteId = domainSuiteResult.Suite.SuiteID,
                SuiteResultStartDateTime = domainSuiteResult.StartTime,
                SuiteResultTypeId = Map(domainSuiteResult.ResultType)
            };
        }

        public DbSuiteResult Map(DbSuiteResult dbSuiteResult, DomainSuiteResult domainSuiteResult)
        {
            dbSuiteResult.SuiteResultEndDateTime = domainSuiteResult.EndTime;
            dbSuiteResult.SuiteResultTypeId = Map(domainSuiteResult.ResultType);
            return dbSuiteResult;
        }

        private int Map(DomainSuiteResultType suiteResultType)
        {
            switch (suiteResultType)
            {
                case DomainSuiteResultType.Fail:
                    return 1;
                case DomainSuiteResultType.Inconclusive:
                    return 2;
                case DomainSuiteResultType.Pass:
                    return 3;
                default:
                    throw new InvalidOperationException("Suite result type not recognized");
            }
        }
    }
}
