using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ZigNet.Api.Mapping;
using ZigNet.Api.Model;
using ZigNet.Business;
using ZigNet.Business.Models;

namespace ZigNet.Api.Controllers
{
    public class SuiteController : ApiController
    {
        private IZigNetBusiness _zigNetBusiness;
        private IZigNetApiMapper _zigNetApiMapper;

        public SuiteController(IZigNetBusiness zignetBusiness, IZigNetApiMapper zigNetApiMapper)
        {
            _zigNetBusiness = zignetBusiness;
            _zigNetApiMapper = zigNetApiMapper;
        }

        public int Post([FromBody]CreateSuiteModel createSuiteModel)
        {
            return _zigNetBusiness.CreateSuite(_zigNetApiMapper.MapCreateSuiteModel(createSuiteModel));
        }

        [Route("api/Suite/AddCategory")]
        public HttpResponseMessage AddCategory([FromBody]AddDeleteSuiteCategoryModel addSuiteCategoryModel)
        {
            _zigNetBusiness.AddSuiteCategory(addSuiteCategoryModel.SuiteID, addSuiteCategoryModel.SuiteCategoryName);
            return Request.CreateResponse(HttpStatusCode.OK);
        }

        [Route("api/Suite/DeleteCategory")]
        [HttpPost]
        public HttpResponseMessage DeleteCategory([FromBody]AddDeleteSuiteCategoryModel deleteSuiteCategoryModel)
        {
            _zigNetBusiness.DeleteSuiteCategory(deleteSuiteCategoryModel.SuiteID, deleteSuiteCategoryModel.SuiteCategoryName);
            return Request.CreateResponse(HttpStatusCode.OK);
        }

        [Route("api/Suite/StartById")]
        public int StartById([FromBody]int suiteId)
        {
            return _zigNetBusiness.StartSuite(suiteId);
        }

        [Route("api/Suite/Start")]
        public int Start([FromBody]StartSuiteByNameModel startSuiteByNameModel)
        {
            return _zigNetBusiness.StartSuite(startSuiteByNameModel.SuiteName);
        }

        [Route("api/Suite/End")]
        public HttpResponseMessage End([FromBody]EndSuiteModel endSuiteModel)
        {
            _zigNetBusiness.EndSuite(endSuiteModel.SuiteResultId, endSuiteModel.SuiteResultType);
            return Request.CreateResponse(HttpStatusCode.OK);
        }

        [Route("api/Suite/LatestTestResults")]
        public IEnumerable<LatestTestResult> LatestTestResults([FromBody] int suiteId)
        {
            return _zigNetBusiness.GetLatestTestResults(suiteId);
        }
    }
}
