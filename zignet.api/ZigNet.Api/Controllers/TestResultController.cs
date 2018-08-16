﻿using System.Net;
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
        private IZigNetApiMapper _zigNetApiMapper;        

        public TestResultController(ITestResultBusinessProvider testResultBusinessProvider, IZigNetApiMapper zigNetApiMapper)
        {
            _testResultBusinessProvider = testResultBusinessProvider;
            _zigNetApiMapper = zigNetApiMapper;
        }

        public HttpResponseMessage Post([FromBody]CreateTestResultModel createTestResultModel)
        {
            _testResultBusinessProvider.Save(
                _zigNetApiMapper.MapCreateTestResultModel(createTestResultModel));
            return Request.CreateResponse(HttpStatusCode.OK);
        }
    }
}
