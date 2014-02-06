namespace Lending.Core
{
    public interface IRequestHandler<in TRequest, out TResponse>
    {
        TResponse HandleRequest(TRequest userAuthIdString);
    }
}
