using System.Linq;
using System.Data.Entity;
using ZigNet.Database.EntityFramework;
using ZigNet.Services.DTOs;
using System.Collections.Generic;

namespace ZigNet.Services.EntityFramework
{
    public class SuiteService : ISuiteService
    {
        private ZigNetEntities _db;

        public SuiteService(IDbContext dbContext)
        {
            _db = dbContext.Get();
        }

        public int GetId(string applicationName, string suiteName, string environmentName)
        {
            return _db.Suites
                .AsNoTracking()
                .Single(s =>
                    s.Application.ApplicationName == applicationName &&
                    s.SuiteName == suiteName &&
                    s.Environment.EnvironmentName == environmentName)
                .SuiteID;
        }

        public IEnumerable<SuiteDto> GetAll()
        {
            return _db.Suites
                .AsNoTracking()
                .Include(s => s.Application.ApplicationName)
                .Include(s => s.Environment.EnvironmentName)
                .Select(s => new SuiteDto
                {
                    SuiteID = s.SuiteID,
                    Name = s.SuiteName,
                    ApplicationId = s.ApplicationId,
                    EnvironmentId = s.EnvironmentId,
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
