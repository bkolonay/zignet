using System.Linq;
using ZigNet.Database.EntityFramework;
using ZigNet.Services.DTOs;

namespace ZigNet.Services.EntityFramework
{
    public class TemporaryTestResultsService : ITemporaryTestResultsService
    {
        private ZigNetEntities _zigNetEntities;

        public TemporaryTestResultsService(IDbContext zigNetEntitiesWrapper)
        {
            _zigNetEntities = zigNetEntitiesWrapper.Get();
        }

        // todo: unit test public interfaces
        public void DeleteAll(int suiteId)
        {
            var temporaryTestResultsToDelete = _zigNetEntities.TemporaryTestResults.Where(ttr => ttr.SuiteId == suiteId);
            _zigNetEntities.TemporaryTestResults.RemoveRange(temporaryTestResultsToDelete);
            _zigNetEntities.SaveChanges();
        }

        public TemporaryTestResultDto Save(TemporaryTestResultDto temporaryTestResultDto)
        {
            _zigNetEntities.TemporaryTestResults.Add(new TemporaryTestResult
            {
                TestResultId = temporaryTestResultDto.TestResultId,
                SuiteResultId = temporaryTestResultDto.SuiteResultId,
                SuiteId = temporaryTestResultDto.SuiteId,
                TestResultTypeId = temporaryTestResultDto.TestResultTypeId
            });
            _zigNetEntities.SaveChanges();

            // temporary, needs updated with data from saved record when unit tested
            return new TemporaryTestResultDto();
        }
    }
}
