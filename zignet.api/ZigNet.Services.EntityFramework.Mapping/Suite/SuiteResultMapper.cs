using System;
using DbSuiteResult = ZigNet.Database.EntityFramework.SuiteResult;
using SuiteResult = ZigNet.Domain.Suite.SuiteResult;
using SuiteResultType = ZigNet.Domain.Suite.SuiteResultType;

namespace ZigNet.Services.EntityFramework.Mapping
{
    public class SuiteResultMapper : ISuiteResultMapper
    {
        public SuiteResult Map(DbSuiteResult dbSuiteResult)
        {
            return new SuiteResult
            {
                SuiteResultID = dbSuiteResult.SuiteResultID
            };
        }

        public DbSuiteResult Map(SuiteResult suiteResult)
        {
            return new DbSuiteResult
            {
                SuiteId = suiteResult.Suite.SuiteID,
                SuiteResultStartDateTime = suiteResult.StartTime,
                SuiteResultTypeId = Map(suiteResult.ResultType)
            };
        }

        public DbSuiteResult Map(DbSuiteResult dbSuiteResult, SuiteResult suiteResult)
        {
            dbSuiteResult.SuiteResultEndDateTime = suiteResult.EndTime;
            dbSuiteResult.SuiteResultTypeId = Map(suiteResult.ResultType);
            return dbSuiteResult;
        }

        private int Map(SuiteResultType suiteResultType)
        {
            switch (suiteResultType)
            {
                case SuiteResultType.Fail:
                    return 1;
                case SuiteResultType.Inconclusive:
                    return 2;
                case SuiteResultType.Pass:
                    return 3;
                default:
                    throw new InvalidOperationException("Suite result type not recognized");
            }
        }
    }
}
