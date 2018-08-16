using ZigNet.Services.DTOs;

namespace ZigNet.Services
{
    public interface ITemporaryTestResultsService
    {
        TemporaryTestResultDto Save(TemporaryTestResultDto temporaryTestResultDto);
        void DeleteAll(int suiteId);
    }
}
