using System;
using System.Collections.Generic;
using ZigNet.Domain.Test;
using ZigNet.Services.DTOs;

namespace ZigNet.Services
{
    public interface ITestFailureDurationService
    {
        IEnumerable<TestFailureDurationDto> GetAll();
        TestFailureDurationDto Save(TestFailureDurationDto testFailureDurationDto, TestResultType testResultType, DateTime utcNow);
    }
}
