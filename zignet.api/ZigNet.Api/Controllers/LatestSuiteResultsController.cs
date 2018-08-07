using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using ZigNet.Business;
using ZigNet.Database.DTOs;

namespace ZigNet.Api.Controllers
{
    public class LatestSuiteResultsController : ApiController
    {
        private ILatestSuiteResultsBusinessProvider _latestSuiteResultsBusinessProvider;

        public LatestSuiteResultsController(ILatestSuiteResultsBusinessProvider latestSuiteResultsBusinessProvider)
        {
            _latestSuiteResultsBusinessProvider = latestSuiteResultsBusinessProvider;
        }

        public IEnumerable<SuiteSummary> Get(bool group = false, bool debug = false)
        {
            return _latestSuiteResultsBusinessProvider.GetLatest(group, debug);
        }
    }
}
