using System.Net;
using System.Net.Http;
using System.Web.Http;
using ZigNet.Api.Mapping;
using ZigNet.Api.Model;
using ZigNet.Business;

namespace ZigNet.Api.Controllers
{
    public class TestResultController : ApiController
    {
        private ITestResultBusinessProvider _testResultBusinessProvider;
        private ISuiteBusinessProvider _suiteBusinessProvider;
        private IZigNetApiMapper _zigNetApiMapper;        

        public TestResultController(ITestResultBusinessProvider testResultBusinessProvider, ISuiteBusinessProvider suiteBusinessProvider,
            IZigNetApiMapper zigNetApiMapper)
        {
            _testResultBusinessProvider = testResultBusinessProvider;
            _suiteBusinessProvider = suiteBusinessProvider;
            _zigNetApiMapper = zigNetApiMapper;
        }

        public HttpResponseMessage Post([FromBody]CreateTestResultModel createTestResultModel)
        {
            _testResultBusinessProvider.Save(
                _zigNetApiMapper.MapCreateTestResultModel(createTestResultModel));
            return Request.CreateResponse(HttpStatusCode.OK);
        }

        [Route("api/TestResult/Latest")]
        public GetLatestTestResultsModel Latest([FromBody] int suiteId, bool group = false)
        {
            return new GetLatestTestResultsModel
            {
                SuiteName = _suiteBusinessProvider.GetSuiteName(suiteId, group),
                LatestTestResults = _testResultBusinessProvider.GetLatest(suiteId, group)
            };
        }
    }
}
