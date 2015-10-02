namespace Lending.Domain
{
    public abstract class RequestHandler<TRequest, TResponse> : IRequestHandler<TRequest, TResponse>
    {
        public abstract TResponse HandleRequest(TRequest request);
    }
}
