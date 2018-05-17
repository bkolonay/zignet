using Newtonsoft.Json;
using ZigNet.Adapter.Http;
using ZigNet.Adapter.Http.DataObjects;
using ZigNet.Api.Model;
using ZigNet.Domain.Suite;

namespace ZigNet.Adapter
{
    public class ZigNetApiHandler : IZigNetApiHandler
    {
        private IHttpRequestSender _httpRequestSender { get; set; }
        private string _zigNetApiUrl;

        public ZigNetApiHandler(IHttpRequestSender httpRequestSender, string zigNetApiUrl)
        {
            _httpRequestSender = httpRequestSender;
            _zigNetApiUrl = zigNetApiUrl;
        }

        public int StartSuite(int suiteId)
        {
            _httpRequestSender.SetHttpRequest(
                new HttpRequest
                {
                    RequestType = "POST",
                    Url = _zigNetApiUrl + "api/Suite/StartById",
                    RequestBody = suiteId.ToString()
                });
            return int.Parse(_httpRequestSender.GetResponse().Body);
        }

        public int StartSuite(string applicationName, string suiteName, string environmentName)
        {
            var startSuiteByNameModel = new StartSuiteByNameModel
            {
                ApplicationName = applicationName,
                SuiteName = suiteName,
                EnvironmentName = environmentName
            };

            _httpRequestSender.SetHttpRequest(
                new HttpRequest
                {
                    RequestType = "POST",
                    Url = _zigNetApiUrl + "api/Suite/Start",
                    RequestBody = JsonConvert.SerializeObject(startSuiteByNameModel)
                });
            return int.Parse(_httpRequestSender.GetResponse().Body);
        }

        public void SaveTestResult(CreateTestResultModel createTestResultModel)
        {
            _httpRequestSender.SetHttpRequest(
                new HttpRequest
                {
                    RequestType = "POST",
                    Url = _zigNetApiUrl + "api/TestResult",
                    RequestBody = JsonConvert.SerializeObject(createTestResultModel)
                });
            _httpRequestSender.GetResponse();
        }

        public void StopSuite(int suiteResultId, SuiteResultType suiteResultType)
        {
            var endSuiteModel = new EndSuiteModel { SuiteResultId = suiteResultId, SuiteResultType = suiteResultType };

            _httpRequestSender.SetHttpRequest(
                new HttpRequest
                {
                    RequestType = "POST",
                    Url = _zigNetApiUrl + "api/Suite/End",
                    RequestBody = JsonConvert.SerializeObject(endSuiteModel)
                });
            _httpRequestSender.GetResponse();
        }
    }
}
