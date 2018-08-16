using System.Linq;
using DbSuiteResult = ZigNet.Database.EntityFramework.SuiteResult;
using DomainSuiteResult = ZigNet.Domain.Suite.SuiteResult;
using ZigNet.Services.EntityFramework.Mapping;
using ZigNet.Database.EntityFramework;

namespace ZigNet.Services.EntityFramework
{
    public class SuiteResultService : ISuiteResultService
    {
        private ZigNetEntities _db;
        private ISuiteResultMapper _suiteResultMapper;

        public SuiteResultService(IDbContext dbContext, ISuiteResultMapper suiteResultMapper)
        {
            _db = dbContext.Get();
            _suiteResultMapper = suiteResultMapper;
        }

        public DomainSuiteResult Get(int suiteResultId)
        {
            var dbSuite = _db.SuiteResults.Single(sr => sr.SuiteResultID == suiteResultId);
            return _suiteResultMapper.Map(dbSuite);
        }

        public int SaveSuiteResult(DomainSuiteResult domainSuiteResult)
        {
            DbSuiteResult dbSuiteResult = null;
            if (domainSuiteResult.SuiteResultID == 0)
            {
                dbSuiteResult = _suiteResultMapper.Map(domainSuiteResult);
                _db.SuiteResults.Add(dbSuiteResult);
            }
            else
            {
                dbSuiteResult = _db.SuiteResults.Single(s => s.SuiteResultID == domainSuiteResult.SuiteResultID);
                _suiteResultMapper.Map(dbSuiteResult, domainSuiteResult);
            }
                
            _db.SaveChanges();
            return dbSuiteResult.SuiteResultID;
        }
    }
}
