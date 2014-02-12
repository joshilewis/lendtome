namespace Lending.Core
{
    public abstract class RequestHandler<TRequest, TResponse> : IRequestHandler<TRequest, TResponse>
    {
        public abstract TResponse HandleRequest(TRequest request);
    }
}
