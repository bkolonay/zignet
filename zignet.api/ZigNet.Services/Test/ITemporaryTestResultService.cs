using ZigNet.Services.DTOs;

namespace ZigNet.Services
{
    public interface ITemporaryTestResultService
    {
        TemporaryTestResultDto Save(TemporaryTestResultDto temporaryTestResultDto);
        void DeleteAll(int suiteId);
    }
}
