using System.Linq;
using DbSuiteResult = ZigNet.Database.EntityFramework.SuiteResult;
using DomainSuiteResult = ZigNet.Domain.Suite.SuiteResult;
using ZigNet.Services.EntityFramework.Mapping;
using ZigNet.Database.EntityFramework;

namespace ZigNet.Services.EntityFramework
{
    public class SuiteResultService : ISuiteResultService
    {
        private ZigNetEntities _zigNetEntities;
        private ISuiteResultMapper _suiteResultMapper;

        public SuiteResultService(IDbContext zigNetEntitiesWrapper, ISuiteResultMapper suiteResultMapper)
        {
            _zigNetEntities = zigNetEntitiesWrapper.Get();
            _suiteResultMapper = suiteResultMapper;
        }

        public DomainSuiteResult Get(int suiteResultId)
        {
            var dbSuite = _zigNetEntities.SuiteResults.Single(sr => sr.SuiteResultID == suiteResultId);
            return _suiteResultMapper.Map(dbSuite);
        }

        public int SaveSuiteResult(DomainSuiteResult domainSuiteResult)
        {
            DbSuiteResult dbSuiteResult = null;
            if (domainSuiteResult.SuiteResultID == 0)
            {
                dbSuiteResult = _suiteResultMapper.Map(domainSuiteResult);
                _zigNetEntities.SuiteResults.Add(dbSuiteResult);
            }
            else
            {
                dbSuiteResult = _zigNetEntities.SuiteResults.Single(s => s.SuiteResultID == domainSuiteResult.SuiteResultID);
                _suiteResultMapper.Map(dbSuiteResult, domainSuiteResult);
            }
                
            _zigNetEntities.SaveChanges();
            return dbSuiteResult.SuiteResultID;
        }
    }
}
