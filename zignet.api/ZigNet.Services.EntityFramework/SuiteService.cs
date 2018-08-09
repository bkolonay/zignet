using System.Linq;
using System.Data.Entity;
using ZigNet.Database.EntityFramework;
using ZigNet.Services.DTOs;

namespace ZigNet.Services.EntityFramework
{
    public class SuiteService : ISuiteService
    {
        private ZigNetEntities _zigNetEntities;

        public SuiteService(IZigNetEntitiesWrapper zigNetEntitiesWrapper)
        {
            _zigNetEntities = zigNetEntitiesWrapper.Get();
        }

        public int GetId(string applicationName, string suiteName, string environmentName)
        {
            return _zigNetEntities.Suites
                .AsNoTracking()
                .Single(s =>
                    s.Application.ApplicationName == applicationName &&
                    s.SuiteName == suiteName &&
                    s.Environment.EnvironmentName == environmentName)
                .SuiteID;
        }

        public SuiteName GetName(int suiteId)
        {
            var suite = _zigNetEntities.Suites
                .AsNoTracking()
                .Include(s => s.Application.ApplicationName)
                .Include(s => s.Environment.EnvironmentName)
                .Select(s => new
                {
                    s.SuiteID,
                    s.SuiteName,
                    s.Application.ApplicationNameAbbreviation,
                    s.Environment.EnvironmentNameAbbreviation
                })
                .Single(s => s.SuiteID == suiteId);

            return new SuiteName
            {
                ApplicationNameAbbreviation = suite.ApplicationNameAbbreviation,
                Name = suite.SuiteName,
                EnvironmentNameAbbreviation = suite.EnvironmentNameAbbreviation
            };
        }

        public SuiteName GetNameGrouped(int suiteId)
        {
            var suite = _zigNetEntities.Suites
                .AsNoTracking()
                .Select(s => new { s.SuiteID, s.Application.ApplicationNameAbbreviation, s.Environment.EnvironmentNameAbbreviation })
                .Single(s => s.SuiteID == suiteId);

            return new SuiteName
            {
                ApplicationNameAbbreviation = suite.ApplicationNameAbbreviation,
                EnvironmentNameAbbreviation = suite.EnvironmentNameAbbreviation
            };
        }
    }
}
