using System.Linq;
using ZigNet.Database.EntityFramework;

namespace ZigNet.Services.EntityFramework
{
    public class SuiteService : ISuiteService
    {
        private ZigNetEntities _zigNetEntities;

        public SuiteService(IZigNetEntitiesWrapper zigNetEntitiesWrapper)
        {
            _zigNetEntities = zigNetEntitiesWrapper.Get();
        }

        public int GetSuiteId(string applicationName, string suiteName, string environmentName)
        {
            return _zigNetEntities.Suites
                .AsNoTracking()
                .Single(s =>
                    s.Application.ApplicationName == applicationName &&
                    s.SuiteName == suiteName &&
                    s.Environment.EnvironmentName == environmentName)
                .SuiteID;
        }
    }
}
