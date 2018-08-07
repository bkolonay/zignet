using System.Collections.Generic;
using ZigNet.Database.DTOs;

namespace ZigNet.Business
{
    public interface ITestResultBusinessProvider
    {
        IEnumerable<LatestTestResult> GetLatestResults(int suiteId, bool group);
    }
}
