using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ZigNet.Api.Mapping;
using ZigNet.Api.Model;
using ZigNet.Business;
using ZigNet.Services;
using ZigNet.Services.DTOs;

namespace ZigNet.Api.Controllers
{
    public class SuiteController : ApiController
    {
        private ISuiteBusinessProvider _suiteBusinessProvider;
        private IZigNetApiMapper _zigNetApiMapper;

        public SuiteController(ISuiteBusinessProvider suiteBusinessProvider, IZigNetApiMapper zigNetApiMapper)
        {
            _suiteBusinessProvider = suiteBusinessProvider;
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

        [Route("api/Suite/Latest")]
        public IEnumerable<SuiteSummary> GetLatest([FromUri]SuiteResultsFilter suiteResultsFilter)
        {
            return _suiteBusinessProvider.GetLatest(suiteResultsFilter);
        }
    }
}
