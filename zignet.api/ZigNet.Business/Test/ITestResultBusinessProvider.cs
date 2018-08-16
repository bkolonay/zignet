using System.Collections.Generic;
using ZigNet.Services.DTOs;
using ZigNet.Domain.Test;

namespace ZigNet.Business
{
    public interface ITestResultBusinessProvider
    {
        IEnumerable<LatestTestResultDto> GetLatest(int suiteId, bool group);
        void Save(TestResult testResult);
    }
}
