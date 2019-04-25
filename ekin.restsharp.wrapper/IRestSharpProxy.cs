namespace ekin.restsharp.wrapper
{
    public interface IRestSharpProxy
    {
        TResponse Execute<TRequest, TResponse>(RestSharpProxyRequest<TRequest> restSharpProxyRequest) where TResponse : class, new();
    }
}