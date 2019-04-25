using System;
using RestSharp;

namespace ekin.restsharp.wrapper
{
    public class RestSharpProxy : IRestSharpProxy
    {
        private readonly IRestClient _restClient;
        private readonly IRestRequest _request;

        public RestSharpProxy()
        {
            _restClient = new RestClient();
            _request = new RestRequest();
        }

        public RestSharpProxy(IRestClient restClient)
        {
            _restClient = restClient;
            _request = new RestRequest();

        }

        public TResponse Execute<TRequest, TResponse>(RestSharpProxyRequest<TRequest> restSharpProxyRequest) where TResponse : class, new()
        {
            if (string.IsNullOrEmpty(restSharpProxyRequest.Url))
                throw new ArgumentNullException("restSharpProxyRequest.Url is empty");

            _restClient.BaseUrl = new Uri(restSharpProxyRequest.Url);

            if (!string.IsNullOrEmpty(restSharpProxyRequest.Resource))
            {
                _request.Resource = restSharpProxyRequest.Resource;
            }

            if (restSharpProxyRequest.Headers.Count > 0)
            {
                foreach (var header in restSharpProxyRequest.Headers)
                {
                    _request.AddHeader(header.Key, header.Value);
                }
            }

            if (restSharpProxyRequest.Parameters.Count > 0)
            {
                foreach (var parameter in restSharpProxyRequest.Parameters)
                {
                    _request.AddParameter(parameter.Key, parameter.Value);
                }
            }

            if (restSharpProxyRequest.Body != null)
            {
                _request.AddJsonBody(restSharpProxyRequest.Body);
            }


            IRestResponse<TResponse> response = _restClient.Execute<TResponse>(_request, (Method)restSharpProxyRequest.Method);

            if (response.Data == null)
            {
                throw new ArgumentNullException("Response data is null");
            }

            return response.Data;
        }
    }
}
