using System.Linq;
using ZigNet.Database.EntityFramework;
using ZigNet.Services.DTOs;

namespace ZigNet.Services.EntityFramework
{
    public class TemporaryTestResultsService : ITemporaryTestResultsService
    {
        private ZigNetEntities _db;

        public TemporaryTestResultsService(IDbContext dbContext)
        {
            _db = dbContext.Get();
        }

        // todo: unit test public interfaces
        public void DeleteAll(int suiteId)
        {
            var temporaryTestResultsToDelete = _db.TemporaryTestResults.Where(ttr => ttr.SuiteId == suiteId);
            _db.TemporaryTestResults.RemoveRange(temporaryTestResultsToDelete);
            _db.SaveChanges();
        }

        public TemporaryTestResultDto Save(TemporaryTestResultDto temporaryTestResultDto)
        {
            _db.TemporaryTestResults.Add(new TemporaryTestResult
            {
                TestResultId = temporaryTestResultDto.TestResultId,
                SuiteResultId = temporaryTestResultDto.SuiteResultId,
                SuiteId = temporaryTestResultDto.SuiteId,
                TestResultTypeId = temporaryTestResultDto.TestResultTypeId
            });
            _db.SaveChanges();

            // temporary, needs updated with data from saved record when unit tested
            return new TemporaryTestResultDto();
        }
    }
}
