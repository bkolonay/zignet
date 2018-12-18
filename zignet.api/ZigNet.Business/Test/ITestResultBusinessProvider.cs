using System.Collections.Generic;
using ZigNet.Services.DTOs;
using ZigNet.Domain.Test;
using ZigNet.Services;

namespace ZigNet.Business
{
    public interface ITestResultBusinessProvider
    {
        IEnumerable<LatestTestResultDto> GetLatest(SuiteResultsFilter suiteResultsFilter);
        void Save(TestResult testResult);
    }
}
