using System.Collections.Generic;
using ZigNet.Services.DTOs;

namespace ZigNet.Services
{
    public interface ILatestTestResultsService
    {
        IEnumerable<LatestTestResultDto> Get(int suiteId);
        IEnumerable<LatestTestResultDto> Get(int[] suiteIds);
    }
}
