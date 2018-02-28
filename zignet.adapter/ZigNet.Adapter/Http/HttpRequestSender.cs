using System.IO;
using System.Net;
using System.Text;
using ZigNet.Adapter.Http.DataObjects;

namespace ZigNet.Adapter.Http
{
    public class HttpRequestSender : IHttpRequestSender
    {
        private HttpRequest _httpRequest;

        public void SetHttpRequest(HttpRequest httpRequest)
        {
            _httpRequest = httpRequest;
        }

        public HttpResponse GetResponse()
        {
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(_httpRequest.Url);
            httpWebRequest.Method = _httpRequest.RequestType;
            httpWebRequest.ContentType = "application/json";

            // create request body
            if (_httpRequest.RequestBody != null)
            {
                byte[] requestBody = Encoding.UTF8.GetBytes(_httpRequest.RequestBody);
                httpWebRequest.ContentLength = requestBody.Length;
                using (var requestStream = httpWebRequest.GetRequestStream())
                    requestStream.Write(requestBody, 0, requestBody.Length);
            }

            // get response
            var httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            string responseBody;
            using (var responseStream = httpWebResponse.GetResponseStream())
            using (var streamReader = new StreamReader(responseStream))
                responseBody = streamReader.ReadToEnd();

            return new HttpResponse
            {
                StatusCode = (int)httpWebResponse.StatusCode,
                Body = responseBody
            };
        }
    }
}
