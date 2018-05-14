using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using ZigNet.Business;
using ZigNet.Database.DTOs;

namespace ZigNet.Api.Controllers
{
    public class LatestSuiteResultsController : ApiController
    {
        private IZigNetBusiness _zigNetBusiness;

        public LatestSuiteResultsController(IZigNetBusiness zignetBusiness)
        {
            _zigNetBusiness = zignetBusiness;
        }

        public IEnumerable<SuiteSummary> Get(bool group = false)
        {
            var latestSuiteResults = _zigNetBusiness.GetLatestSuiteResults(group);
            var debugSuiteResults = latestSuiteResults.Where(sr => sr.SuiteName.Contains("(D)"));
            var releaseSuiteResults = latestSuiteResults.Where(sr => !sr.SuiteName.Contains("(D)")).ToList();
            releaseSuiteResults.AddRange(debugSuiteResults);
            return releaseSuiteResults;
        }
    }
}
