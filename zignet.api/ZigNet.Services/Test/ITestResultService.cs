using System.Collections.Generic;
using ZigNet.Services.DTOs;

namespace ZigNet.Services
{
    public interface ITestResultService
    {
        IEnumerable<LatestTestResultDto> GetLatest(int suiteId);
        IEnumerable<LatestTestResultDto> GetLatestGrouped(int suiteId);
    }
}
