using System.Collections.Generic;
using ZigNet.Services.DTOs;

namespace ZigNet.Services
{
    public interface ITestFailureDurationService
    {
        IEnumerable<TestFailureDurationDto> GetAll();
    }
}
