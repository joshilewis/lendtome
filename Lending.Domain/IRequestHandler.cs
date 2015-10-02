namespace Lending.Domain
{
    public interface IRequestHandler<in TRequest, out TResponse>
    {
        TResponse HandleRequest(TRequest request);
    }
}
