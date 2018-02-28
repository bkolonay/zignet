using ZigNet.Adapter.Http;

namespace ZigNet.Adapter.SpecFlow
{
    public class ZigNetSpecFlowAdapter : ZigNetSpecFlowAdapterBase, IZigNetAdapter
    {
        public ZigNetSpecFlowAdapter(string zigNetApiUrl) : base(new ZigNetApiHandler(new HttpRequestSender(), zigNetApiUrl))
        { }
    }
}
