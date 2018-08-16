using System.Collections.Generic;
using ZigNet.Services.DTOs;

namespace ZigNet.Services
{
    public interface ITestResultService
    {
        IEnumerable<LatestTestResultDto> GetLatestResults(int suiteId);
        IEnumerable<LatestTestResultDto> GetLatestResultsGrouped(int suiteId);
    }
}
