using ZigNet.Adapter.Http;
using ZigNet.Adapter.SpecFlow.Utility;

namespace ZigNet.Adapter.SpecFlow
{
    public class ZigNetSpecFlowAdapter : ZigNetSpecFlowAdapterBase, IZigNetAdapter
    {
        public ZigNetSpecFlowAdapter(string zigNetApiUrl) : 
            base(new ZigNetApiHandler(new HttpRequestSender(), zigNetApiUrl), new DirectoryService(), new FileService(), new SpecFlowContextWrapper())
        { }
    }
}
