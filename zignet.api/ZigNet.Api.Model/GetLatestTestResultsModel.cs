using System.Collections.Generic;
using ZigNet.Services.DTOs;

namespace ZigNet.Api.Model
{
    public class GetLatestTestResultsModel
    {
        public string SuiteName { get; set; }
        public IEnumerable<LatestTestResult> LatestTestResults { get; set; }
    }
}
