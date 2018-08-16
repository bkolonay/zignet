using System.Net;
using System.Net.Http;
using System.Web.Http;
using ZigNet.Api.Mapping;
using ZigNet.Api.Model;
using ZigNet.Business;

namespace ZigNet.Api.Controllers
{
    public class SuiteController : ApiController
    {
        private ISuiteBusinessProvider _suiteBusinessProvider;
        private ITestResultBusinessProvider _testResultBusinessProvider;
        private IZigNetApiMapper _zigNetApiMapper;

        public SuiteController(ISuiteBusinessProvider suiteBusinessProvider, ITestResultBusinessProvider testResultBusinessProvider,
            IZigNetApiMapper zigNetApiMapper)
        {
            _suiteBusinessProvider = suiteBusinessProvider;
            _testResultBusinessProvider = testResultBusinessProvider;
            _zigNetApiMapper = zigNetApiMapper;
        }

        [Route("api/Suite/StartById")]
        public int StartById([FromBody]int suiteId)
        {
            return _suiteBusinessProvider.StartSuite(suiteId);
        }

        [Route("api/Suite/Start")]
        public int Start([FromBody]StartSuiteByNameModel startSuiteByNameModel)
        {
            return _suiteBusinessProvider.StartSuite(startSuiteByNameModel.ApplicationName,
                startSuiteByNameModel.SuiteName, startSuiteByNameModel.EnvironmentName);
        }

        [Route("api/Suite/End")]
        public HttpResponseMessage End([FromBody]EndSuiteModel endSuiteModel)
        {
            _suiteBusinessProvider.StopSuite(endSuiteModel.SuiteResultId, endSuiteModel.SuiteResultType);
            return Request.CreateResponse(HttpStatusCode.OK);
        }

        [Route("api/Suite/LatestTestResults")]
        public GetLatestTestResultsModel LatestTestResults([FromBody] int suiteId, bool group = false)
        {
            return new GetLatestTestResultsModel
            {
                 SuiteName = _suiteBusinessProvider.GetSuiteName(suiteId, group),
                 LatestTestResults = _testResultBusinessProvider.GetLatest(suiteId, group)
            };
        }
    }
}
