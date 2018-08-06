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
        private IZigNetBusiness _zigNetBusiness;
        private IZigNetApiMapper _zigNetApiMapper;
        private ISuiteBusinessProvider _suiteBusinessProvider;

        public SuiteController(IZigNetBusiness zignetBusiness, IZigNetApiMapper zigNetApiMapper, ISuiteBusinessProvider suiteBusinessProvider)
        {
            _zigNetBusiness = zignetBusiness;
            _zigNetApiMapper = zigNetApiMapper;
            _suiteBusinessProvider = suiteBusinessProvider;
        }

        public int Post([FromBody]CreateSuiteModel createSuiteModel)
        {
            return _zigNetBusiness.CreateSuite(_zigNetApiMapper.MapCreateSuiteModel(createSuiteModel));
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
            _zigNetBusiness.StopSuite(endSuiteModel.SuiteResultId, endSuiteModel.SuiteResultType);
            return Request.CreateResponse(HttpStatusCode.OK);
        }

        [Route("api/Suite/LatestTestResults")]
        public GetLatestTestResultsModel LatestTestResults([FromBody] int suiteId, bool group = false)
        {
            return new GetLatestTestResultsModel
            {
                 SuiteName = _zigNetBusiness.GetSuiteName(suiteId, group),
                 LatestTestResults = _zigNetBusiness.GetLatestTestResults(suiteId, group)
            };
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
    }
}
