using ZigNet.Adapter.Http.DataObjects;

namespace ZigNet.Adapter.Http
{
    public interface IHttpRequestSender
    {
        HttpResponse GetResponse();
        void SetHttpRequest(HttpRequest httpRequest);
    }
}
