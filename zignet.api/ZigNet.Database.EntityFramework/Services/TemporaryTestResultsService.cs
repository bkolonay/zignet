using System.Linq;

namespace ZigNet.Database.EntityFramework
{
    public class TemporaryTestResultsService : ITemporaryTestResultsService
    {
        private ZigNetEntities _zigNetEntities;

        public TemporaryTestResultsService(IZigNetEntitiesWrapper zigNetEntitiesWrapper)
        {
            _zigNetEntities = zigNetEntitiesWrapper.Get();
        }

        public void DeleteAll(int suiteId)
        {
            var temporaryTestResultsToDelete = _zigNetEntities.TemporaryTestResults.Where(ttr => ttr.SuiteId == suiteId);
            _zigNetEntities.TemporaryTestResults.RemoveRange(temporaryTestResultsToDelete);
            _zigNetEntities.SaveChanges();
        }
    }
}
