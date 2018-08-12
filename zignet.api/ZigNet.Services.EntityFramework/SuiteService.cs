using System.Linq;
using System.Data.Entity;
using ZigNet.Database.EntityFramework;
using ZigNet.Services.DTOs;
using System.Collections.Generic;

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

        public IEnumerable<SuiteDto> GetAll()
        {
            return _zigNetEntities.Suites
                .AsNoTracking()
                .Include(s => s.Application.ApplicationName)
                .Include(s => s.Environment.EnvironmentName)
                .Select(s => new SuiteDto
                {
                    SuiteID = s.SuiteID,
                    Name = s.SuiteName,
                    ApplicationNameAbbreviation = s.Application.ApplicationNameAbbreviation,
                    EnvironmentNameAbbreviation = s.Environment.EnvironmentNameAbbreviation
                });
        }

        public SuiteDto Get(int suiteId)
        {
            return GetAll().Single(s => s.SuiteID == suiteId);
        }
    }
}
