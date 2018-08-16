using System;
using System.Collections.Generic;
using ZigNet.Domain.Test;
using ZigNet.Services.DTOs;

namespace ZigNet.Services
{
    public interface ILatestTestResultService
    {
        LatestTestResultDto Save(LatestTestResultDto latestTestResultDto, TestResultType testResultType, DateTime utcNow);
        IEnumerable<LatestTestResultDto> Get(int suiteId);
        IEnumerable<LatestTestResultDto> Get(int[] suiteIds);
    }
}
