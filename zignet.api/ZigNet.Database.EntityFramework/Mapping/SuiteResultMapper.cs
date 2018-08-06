using System;

namespace ZigNet.Database.EntityFramework.Mapping
{
    public class SuiteResultMapper : ISuiteResultMapper
    {
        public SuiteResult Map(Domain.Suite.SuiteResult suiteResult)
        {
            return new SuiteResult
            {
                SuiteId = suiteResult.Suite.SuiteID,
                SuiteResultStartDateTime = suiteResult.StartTime,
                SuiteResultTypeId = Map(suiteResult.ResultType)
            };
        }

        private int Map(Domain.Suite.SuiteResultType suiteResultType)
        {
            switch (suiteResultType)
            {
                case Domain.Suite.SuiteResultType.Fail:
                    return 1;
                case Domain.Suite.SuiteResultType.Inconclusive:
                    return 2;
                case Domain.Suite.SuiteResultType.Pass:
                    return 3;
                default:
                    throw new InvalidOperationException("Suite result type not recognized");
            }
        }
    }
}
