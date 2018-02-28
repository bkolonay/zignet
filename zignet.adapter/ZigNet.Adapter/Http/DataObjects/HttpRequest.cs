namespace ZigNet.Adapter.Http.DataObjects
{
    public class HttpRequest
    {
        public HttpRequest()
        {
            HttpResponse = new HttpResponse();
        }

        public string Url { get; set; }
        public string RequestType { get; set; }
        public string RequestBody { get; set; }
        public HttpResponse HttpResponse { get; set; }
    }
}
