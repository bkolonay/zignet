using System.Collections.Generic;
using ZigNet.Database.DTOs;

namespace ZigNet.Services
{
    public interface ITestResultService
    {
        IEnumerable<LatestTestResult> GetLatestResults(int suiteId, bool group);
    }
}
