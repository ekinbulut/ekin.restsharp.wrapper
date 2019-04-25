using System.Collections.Generic;

namespace ekin.restsharp.wrapper
{
    public class RestSharpProxyRequest<TRequest>
    {
        public string Url { get; set; }
        public string Resource { get; set; }
        public Dictionary<string, string> Parameters { get; set; }
        public Dictionary<string, string> Headers { get; set; }
        public TRequest Body { get; set; }
        public EnumHttpMethod Method { get; set; }

    }
}