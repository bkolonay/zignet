using System.Collections.Generic;
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

        public IEnumerable<SuiteSummary> Get()
        {
            return _zigNetBusiness.GetLatestSuiteResults();
        }
    }
}
