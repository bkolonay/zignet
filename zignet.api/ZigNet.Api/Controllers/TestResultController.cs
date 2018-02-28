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
        private IZigNetBusiness _zigNetBusiness;
        private IZigNetApiMapper _zigNetApiMapper;

        public TestResultController(IZigNetBusiness zignetBusiness, IZigNetApiMapper zigNetApiMapper)
        {
            _zigNetBusiness = zignetBusiness;
            _zigNetApiMapper = zigNetApiMapper;
        }

        public HttpResponseMessage Post([FromBody]CreateTestResultModel createTestResultModel)
        {
            _zigNetBusiness.SaveTestResult(_zigNetApiMapper.MapCreateTestResultModel(createTestResultModel));
            return Request.CreateResponse(HttpStatusCode.OK);
        }
    }
}
