using ZigNet.Api.Model;
using ZigNet.Domain.Suite;
using ZigNet.Domain.Test;

namespace ZigNet.Api.Mapping
{
    public interface IZigNetApiMapper
    {
        Suite MapCreateSuiteModel(CreateSuiteModel createSuiteModel);
        TestResult MapCreateTestResultModel(CreateTestResultModel createTestResultModel);
    }
}
