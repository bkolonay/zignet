using System.Collections.Generic;
using ZigNet.Services.DTOs;

namespace ZigNet.Services
{
    public interface ISuiteService
    {
        int GetId(string applicationName, string suiteName, string environmentName);
        IEnumerable<SuiteDto> GetAll();
        SuiteDto Get(int suiteId);
    }
}
