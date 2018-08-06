using ZigNet.Database.EntityFramework;
using ZigNet.Services.EntityFramework.Mapping;

namespace ZigNet.Services.EntityFramework
{
    public class SuiteResultService : ISuiteResultService
    {
        private ZigNetEntities _zigNetEntities;
        private ISuiteResultMapper _suiteResultMapper;

        public SuiteResultService(IZigNetEntitiesWrapper zigNetEntitiesWrapper, ISuiteResultMapper suiteResultMapper)
        {
            _zigNetEntities = zigNetEntitiesWrapper.Get();
            _suiteResultMapper = suiteResultMapper;
        }

        public int SaveSuiteResult(Domain.Suite.SuiteResult suiteResult)
        {
            var dbSuiteResult = _suiteResultMapper.Map(suiteResult);

            if (dbSuiteResult.SuiteResultID == 0)
                _zigNetEntities.SuiteResults.Add(dbSuiteResult);
            _zigNetEntities.SaveChanges();
            return dbSuiteResult.SuiteResultID;
        }
    }
}
