using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ZigNet.Api.Mapping;
using ZigNet.Api.Model;
using ZigNet.Business;
using ZigNet.Services;

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
        public GetLatestTestResultsModel GetLatest([FromUri]SuiteResultsFilter suiteResultsFilter)
        {
            if (suiteResultsFilter == null)
                suiteResultsFilter = new SuiteResultsFilter();

            var suiteName = "";
            //if (suiteResultsFilter.Applications == null || suiteResultsFilter.Applications.Length == 0)
            //    suiteName = "All Tests";

            return new GetLatestTestResultsModel
            {
                SuiteName = suiteName,
                LatestTestResults = _testResultBusinessProvider.GetLatest(suiteResultsFilter)
            };
        }
    }
}
